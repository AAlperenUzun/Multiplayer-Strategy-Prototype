using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public NavMeshAgent agent;
    private GameManager _gameManager;
    private MeshRenderer _mesh;
    private Color _startColor;
    public Color selectColor;
    private Wood _targetWood;
    private bool _isMovingToWood = false;
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _gameManager.RegisterPlayer(this);
        agent = GetComponent<NavMeshAgent>();
        _mesh = GetComponent<MeshRenderer>();
        _startColor = _mesh.material.color;
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
        _mesh.material.color = _startColor;
    }

    public void CollectWood(Wood woodResource)
    {
            _targetWood = woodResource;
            _isMovingToWood = true;
            Debug.LogError("collect");
    }
    private void Update()
    {
        if (_isMovingToWood && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                if (_targetWood != null)
                {
                    OnCollectWoodCompleted();
                    _targetWood.Collect();
                    _targetWood = null;
                }
                _isMovingToWood = false;
            }
        }
    }

    private void OnCollectWoodCompleted()
    {
        if (photonView.IsMine)
        {
            var value= _gameManager.AddWoodToTeam(_targetWood.woodValue);
            bool isBlue = false || PhotonNetwork.IsMasterClient;

            photonView.RPC("UpdateWoodScore", RpcTarget.All, isBlue, value);
        }
    }
    [PunRPC]
    public void UpdateWoodScore(bool isBlue, int woodAmount)
    {
        _gameManager.UpdateWoodScore(isBlue, woodAmount);
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