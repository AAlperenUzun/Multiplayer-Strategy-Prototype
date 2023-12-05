using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public NavMeshAgent agent;
    private GameManager gameManager;
    private MeshRenderer _mesh;
    private Color startColor;
    public Color selectColor;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.RegisterPlayer(this);
        agent = GetComponent<NavMeshAgent>();
        _mesh = GetComponent<MeshRenderer>();
        startColor = _mesh.material.color;
    }

    public void MoveTo(Vector3 point)
    {
        if (photonView.IsMine)
        {
            agent.SetDestination(point);
        }
    }

    public void Select()
    {
        _mesh.material.color = selectColor;
    }

    public void Deselect()
    {
        _mesh.material.color = startColor;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (agent.isStopped)return;
            stream.SendNext(transform.position);
        }
        else
        {
            agent.SetDestination((Vector3)stream.ReceiveNext());
        }
    }
}