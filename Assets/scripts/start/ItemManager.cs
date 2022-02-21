using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    private int index = 0;
    // Start is called before the first frame update
    private int maxIndex;

    void Start()
    {
        maxIndex = transform.childCount - 1;
        SelectItem(index);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Vertical"))
        {
            float vertial = Input.GetAxisRaw("Vertical");

            SelectItem(ModifyIndex(vertial));
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            next();
        }
    }

    private int ModifyIndex(float vertial)
    {
        if (vertial == -1)
        {
            index++;
        }
        else if (vertial == 1)
        {
            index--;
        }

        if (index < 0)
        {
            index = 0;
        }
        else if (index > maxIndex)
        {
            index = maxIndex;
        }
        return index;
    }

    private void SelectItem(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i).GetChild(0);
            Animator anim = t.GetComponent<Animator>();
            t.gameObject.SetActive(index == i);
            //選中就跑動畫，沒選到隱藏
            anim.SetBool("run", index == i);

        }
    }

    private void next()
    {
        switch(index)
        {
            //start
            case 0:
                SceneManager.LoadScene(ScenceBuildIndex.SCENCE_MAIN);
            break;

            case 1:
                SceneData.Set("state",PlayState.Create);
                SceneManager.LoadScene(ScenceBuildIndex.SCENCE_LOBBY);
            break;

             case 2:
                SceneData.Set("state",PlayState.Join);
                SceneManager.LoadScene(ScenceBuildIndex.SCENCE_LOBBY);
            break;


            case 3:
                Application.Quit();
            break;

        }
    }
}
