using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.AI;

public class GameManager : MonoBehaviourPun
{
    public Camera cam;
    private List<PlayerController> _players = new List<PlayerController>();
    private PlayerController _selectedPlayer;

    private int blueTeamWood = 0;
    private int redTeamWood = 0;

    public TMP_Text blueTeamWoodText;
    public TMP_Text redTeamWoodText;
    
    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var player = hit.collider.GetComponent<PlayerController>();
                var wood = hit.collider.GetComponent<Wood>();
                if (player != null && player.photonView.IsMine)
                {
                    if (_selectedPlayer != null)
                    {
                        _selectedPlayer.Deselect();
                    }

                    _selectedPlayer = player;
                    _selectedPlayer.Select();
                }
                else if (_selectedPlayer != null)
                {
                    if (wood != null)
                    {
                        _selectedPlayer.CollectWood(wood);
                    }
                    _selectedPlayer.MoveTo(hit.point);
                }
            }
        }
    }
    
    
    public void UpdateWoodScore(bool isBlue, int woodAmount)
    {
        if (isBlue)
        {
            blueTeamWood = woodAmount;
            blueTeamWoodText.text = "" + blueTeamWood;
        }
        else
        {
            redTeamWood = woodAmount;
            redTeamWoodText.text = "" + redTeamWood;
        }
    }

    public int AddWoodToTeam(int value)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            blueTeamWood+=value;
            blueTeamWoodText.text =""+ blueTeamWood;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "BlueTeamWood", blueTeamWood } });
            return blueTeamWood;
        }
        else
        {
            redTeamWood+=value;
            redTeamWoodText.text = "" + redTeamWood;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "RedTeamWood", redTeamWood } });
            return redTeamWood;
        }
    }
    
    public void RegisterPlayer(PlayerController player)
    {
        _players.Add(player);
    }
}