using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomUI : MonoBehaviour
{
    // Start is called before the first frame update

    class PlayerModel
    {
        public string name;
        public int index;
    }

    public Button startBtn;

    public Button backBtn;

    public Text PlayerCounter;

    public GameObject playList;

    public Text roomId;

    [SerializeField]
    private GameObject PlayerItem;

    private VerticalLayoutGroup vlg;

    private List<PlayerModel> players = new List<PlayerModel>();



    void Start()
    {
        vlg = playList.GetComponent<VerticalLayoutGroup>();

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ReloadData(Player[] player)
    {

        players.Clear();
        int playerIndex = 0;
        foreach (Player p in player)
        {
            playerIndex++;
            players.Add(new PlayerModel
            {
                name = p.NickName,
                index = playerIndex
            });
        }

        for (int i = 0; i < playList.transform.childCount; i++)
        {
            Destroy(playList.transform.GetChild(i).gameObject);
        }

        foreach (PlayerModel model in players)
        {
            GameObject item = Instantiate(PlayerItem, playList.transform);
            RoomPlayerItem i = item.GetComponent<RoomPlayerItem>();
            i.Name.text = model.name;
            i.Index.text = model.index + "P";
        }
    }
}
