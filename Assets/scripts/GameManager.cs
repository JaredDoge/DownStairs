using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Tooltip("Prefab- 玩家的角色")]
    public GameObject playerPrefab;

    private GameObject player1;

    private GameObject player2;

    private Game.NetPlayer nPlayer1;

    private Game.NetPlayer nPlayer2;


    [SerializeField]
    GameObject HpBar1;

    [SerializeField]
    GameObject HpBar2;

    [SerializeField]
    Text scoreText;

    int score;
    float scoreTime;

    private int hp_p1;
    private int hp_p2;

    void Start()
    {
        Init();
        CreatePlayer();
    }

    void Update()
    {
        UpdateScore();
    }



    void Init()
    {
        score = 0;
        scoreTime = 0f;
        scoreText.text = "地下" + score + "層";
    }

    void CreatePlayer()
    {
        // int index = 0;
        // foreach (Player p in PhotonNetwork.PlayerList)
        // {

           // index++;
              int index=PhotonNetwork.LocalPlayer.ActorNumber;
            if (index == 1)
            {
                player1 = PhotonNetwork.Instantiate(this.playerPrefab.name,
                                                     new Vector3(-3.15f, 3.71f, 1f), Quaternion.identity, 0);
                nPlayer1 = player1.GetComponent<Game.NetPlayer>();

                nPlayer1.PlayerHurt += this.OnP1Hurted;

            }
            else if (index == 2)
            {

                player2 = PhotonNetwork.Instantiate(this.playerPrefab.name,
                                         new Vector3(-2.15f, 3.71f, 1f), Quaternion.identity, 0);
                nPlayer2 = player2.GetComponent<Game.NetPlayer>();

                nPlayer2.PlayerHurt += this.OnP2Hurted;
            }

       // }
    }

    void OnP1Hurted(int num)
    {
        hp_p1 += num;

        if (hp_p1 > 10)
        {
            hp_p1 = 10;
        }
        else if (hp_p1 < 0)
        {
            hp_p1 = 0;
            nPlayer1.Death();
        }
        UpdateHpBar(HpBar1, hp_p1);
    }


    void OnP2Hurted(int num)
    {
        hp_p2 += num;

        if (hp_p2 > 10)
        {
            hp_p2 = 10;
        }
        else if (hp_p2 < 0)
        {
            hp_p2 = 0;
            nPlayer2.Death();
        }
        UpdateHpBar(HpBar2, hp_p2);
    }

    // Update is called once per frame


    private void UpdateHpBar(GameObject hpBar, int hp)
    {
        for (int i = 0; i < hpBar.transform.childCount; i++)
        {
            hpBar.transform.GetChild(i).gameObject.SetActive(hp > i);
        }

    }

    private void UpdateScore()
    {
        scoreTime += Time.deltaTime;
        if (scoreTime > 2f)
        {
            score++;
            scoreTime = 0f;
            scoreText.text = "地下" + score + "層";
            Floor.moveSpeed = (float)(score * 0.02) + 1;
        }
    }



}
