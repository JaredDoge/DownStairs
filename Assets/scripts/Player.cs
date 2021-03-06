using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
   
    [SerializeField] GameObject HpBar;
    [SerializeField] Text scoreText;
    int score;
    float scoreTime;
    GameObject currentFloor;
    Animator anim;
    SpriteRenderer sr;
    AudioSource deathSound;
    [SerializeField] GameObject replayButton;

    bool isStop;
    private int Hp;

    // Start is called before the first frame update
    void Start()
    {
        init();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isStop) return;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            sr.flipX = false;
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            anim.SetBool("run", true);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            sr.flipX = true;
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }

        UpdateScore();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {


        switch (other.gameObject.tag)
        {

            case "Normal":

                if (IsFloorTop(other))
                {
                    currentFloor = other.gameObject;
                    ModifyHp(1);
                    other.gameObject.GetComponent<AudioSource>().Play();
                }

                break;

            case "Nails":

                if (IsFloorTop(other))
                {
                    currentFloor = other.gameObject;
                    ModifyHp(-3);
                    anim.SetTrigger("hurt");
                    other.gameObject.GetComponent<AudioSource>().Play();
                }

                break;

            case "Trampoline":

                if (IsFloorTop(other))
                {
                    currentFloor = other.gameObject;
                    ModifyHp(1);
                    other.gameObject.GetComponent<Animator>().SetTrigger("jump");
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    other.gameObject.GetComponent<AudioSource>().Play();
                }

                break;

            case "ConveyorRight":
                if (IsFloorTop(other))
                {

                    currentFloor = other.gameObject;
                    ModifyHp(1);
                    other.gameObject.GetComponent<AudioSource>().Play();
                }

                break;

            case "ConveyorLeft":
                if (IsFloorTop(other))
                {

                    currentFloor = other.gameObject;
                    ModifyHp(1);
                    other.gameObject.GetComponent<AudioSource>().Play();
                }

                break;


            case "Fake":
                if (IsFloorTop(other))
                {
                    StartCoroutine(DelayToFake(other.gameObject));
                    currentFloor = other.gameObject;
                    ModifyHp(1);
                   
                }

                break;

            //????????????
            case "Ceilling":

                if (currentFloor != null)
                {
                    currentFloor.GetComponent<BoxCollider2D>().enabled = false;
                }

                ModifyHp(-3);
                anim.SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();

                break;





        }

    }

    private IEnumerator DelayToFake(GameObject obj)
   {

      yield return new WaitForSeconds(0.5f);
      obj.GetComponent<Animator>().SetTrigger("flip");
      currentFloor.GetComponent<BoxCollider2D>().enabled = false;
      obj.gameObject.GetComponent<AudioSource>().Play();

   }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "DeathLine")
        {
            Death();
        }
    }


    //?????????????????????
    private bool IsFloorTop(Collision2D other)
    {
        return other.contacts[0].normal == new Vector2(0f, 1f);
    }

    private void ModifyHp(int num)
    {
        Hp += num;

        if (Hp > 10)
        {
            Hp = 10;
        }
        else if (Hp < 0)
        {
            Hp = 0;
            Death();
        }
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        for (int i = 0; i < HpBar.transform.childCount; i++)
        {
            HpBar.transform.GetChild(i).gameObject.SetActive(Hp > i);
        }

    }

    private void UpdateScore()
    {
        scoreTime += Time.deltaTime;
        if (scoreTime > 2f)
        {
            score++;
            scoreTime = 0f;
            scoreText.text = "??????" + score + "???";
            Floor.moveSpeed = (float)(score * 0.05) + 1;
        }
    }

    private void init()
    {
        isStop = false;
        Hp = 10;
        score = 0;
        scoreTime = 0f;
        scoreText.text = "??????" + score + "???";
    }

    private void Death()
    {
        isStop = true;
        deathSound.Play();
        Time.timeScale = 0;
        replayButton.SetActive(true);
    }

    public void Replay()
    {

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }



}
