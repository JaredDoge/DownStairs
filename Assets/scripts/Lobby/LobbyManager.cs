using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
public class LobbyManager : MonoBehaviour
{
    public Button roomButton;

    public Button backButton;

    public GameObject nameInput;

    public GameObject roomIdInput;
    

    private PlayState state;

    void Start()
    {

        state=SceneData.Pop<PlayState>("state");
        initView();



        // button.onClick.AddListener(delegate
        // {
        //         SceneManager.LoadScene(ScenceBuildIndex.SCENCE_MAIN);
        // });
    }

    void initView()
    {
        switch(state)
        {
            case PlayState.Create:

            roomButton.GetComponentInChildren<Text>().text="創建房間";
            roomIdInput.SetActive(false);

            break;

            case PlayState.Join:
            
            roomButton.GetComponentInChildren<Text>().text="加入房間";
            roomIdInput.SetActive(true);

            break;
        }    
    }
    void Update()
    {
        

    }
}
