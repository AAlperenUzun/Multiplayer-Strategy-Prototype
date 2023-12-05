using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class GameManager : MonoBehaviourPun
{
    public Camera cam;
    private List<PlayerController> players = new List<PlayerController>();
    private PlayerController selectedPlayer;

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
                // Kontrol edilen bir karaktere tıklandı mı kontrol et
                var player = hit.collider.GetComponent<PlayerController>();
                if (player != null && player.photonView.IsMine)
                {
                    if (selectedPlayer != null)
                    {
                        selectedPlayer.Deselect();
                    }

                    selectedPlayer = player;
                    selectedPlayer.Select();
                }
                else if (selectedPlayer != null)
                {
                    // Seçilen karaktere hareket emri gönder
                    selectedPlayer.MoveTo(hit.point);
                }
            }
        }
    }

    public void RegisterPlayer(PlayerController player)
    {
        players.Add(player);
    }
}