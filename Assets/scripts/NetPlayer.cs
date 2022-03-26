using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.Events;

namespace Game
{


    public class NetPlayer : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] float moveSpeed;
        [SerializeField] float jumpForce;


        GameObject currentFloor;
        Animator anim;
        SpriteRenderer sr;
        AudioSource deathSound;
        [SerializeField] GameObject replayButton;

        bool isStop;

        public Action<int> PlayerHurt;


        // Start is called before the first frame update
        void Start()
        {
           // init();
            sr = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            deathSound = GetComponent<AudioSource>();

        }

        private void init()
        {
           // isStop = false;
          //  Hp = 10;
          // score = 0;
          //  scoreTime = 0f;
          // scoreText.text = "地下" + score + "層";
        }


        // Update is called once per frame
        void Update()
        {

            if (isStop) return;

            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.localScale=new Vector3(1,0.6f,1);
                transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
                anim.SetBool("run", true);

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                
                transform.localScale=new Vector3(-1,0.6f,1);
                transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
                anim.SetBool("run", true);
            }
            else
            {
                anim.SetBool("run", false);
            }
        }

        void UpdatePlayerHart(int hp)
        {
            PlayerHurt?.Invoke(hp);    
        }

        private void OnCollisionEnter2D(Collision2D other)
        {

             if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            switch (other.gameObject.tag)
            {

                case "Normal":

                    if (IsFloorTop(other))
                    {
                        currentFloor = other.gameObject;
                        UpdatePlayerHart(1);
                        other.gameObject.GetComponent<AudioSource>().Play();
                    }

                    break;

                case "Nails":

                    if (IsFloorTop(other))
                    {
                        currentFloor = other.gameObject;
                        UpdatePlayerHart(1);
                        anim.SetTrigger("hurt");
                        other.gameObject.GetComponent<AudioSource>().Play();
                    }

                    break;

                case "Trampoline":

                    if (IsFloorTop(other))
                    {
                        currentFloor = other.gameObject;
                        UpdatePlayerHart(1);
                        other.gameObject.GetComponent<Animator>().SetTrigger("jump");
                        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                        other.gameObject.GetComponent<AudioSource>().Play();
                    }

                    break;

                case "ConveyorRight":
                    if (IsFloorTop(other))
                    {

                        currentFloor = other.gameObject;
                        UpdatePlayerHart(1);
                        other.gameObject.GetComponent<AudioSource>().Play();
                    }

                    break;

                case "ConveyorLeft":
                    if (IsFloorTop(other))
                    {

                        currentFloor = other.gameObject;
                        UpdatePlayerHart(1);
                        other.gameObject.GetComponent<AudioSource>().Play();
                    }

                    break;


                case "Fake":
                    if (IsFloorTop(other))
                    {
                        StartCoroutine(DelayToFake(other.gameObject));
                        currentFloor = other.gameObject;
                        UpdatePlayerHart(1);
                        
                         
                    }

                    break;

                //上方尖刺
                case "Ceilling":

                    if (currentFloor != null)
                    {
                        currentFloor.GetComponent<BoxCollider2D>().enabled = false;
                    }

                    UpdatePlayerHart(-3);
                    anim.SetTrigger("hurt");
                    other.gameObject.GetComponent<AudioSource>().Play();

                    break;


            }

        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // 為玩家本人的狀態, 將 IsFiring 的狀態更新給其他玩家

                //是否翻轉
              //  stream.SendNext(sr.flipX);
            }
            else
            {
                // 非為玩家本人的狀態, 單純接收更新的資料
               // this.IsFiring = (bool)stream.ReceiveNext();
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


        //是否碰撞到上方
        private bool IsFloorTop(Collision2D other)
        {
            return other.contacts[0].normal == new Vector2(0f, 1f);
        }

       



        // public void Replay()
        // {

        //     Time.timeScale = 1;
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // }

        public void Death()
        {
            // isStop = true;
            deathSound.Play();
            //   Time.timeScale = 0;
            //  replayButton.SetActive(true);
        }



    }
}
