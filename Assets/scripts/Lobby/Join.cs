using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
public class Join : MonoBehaviourPunCallbacks
{

    public Button roomButton;

    public Button backButton;

    public InputField nameInput;

    public InputField roomIdInput;


    public void setState(PlayState state)
    {
         switch (state)
        {
            case PlayState.Create:

                roomButton.GetComponentInChildren<Text>().text = "創建房間";
                roomIdInput.gameObject.SetActive(false);

                break;

            case PlayState.Join:

                roomButton.GetComponentInChildren<Text>().text = "加入房間";
                roomIdInput.gameObject.SetActive(true);

                break;
        }     
    }


}
