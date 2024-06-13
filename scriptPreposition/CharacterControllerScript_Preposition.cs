using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prepostion
{

    public class CharacterControllerScript_Preposition : MonoBehaviour
    {
        Rigidbody2D character;
        Animator characterani;
        // Start is called before the first frame update
        void Awake()
        {
            if (UIManager_Preposition.instance.IsBoyCharacter)
                transform.GetChild(0).transform.SetAsFirstSibling();

            else
                transform.GetChild(1).transform.SetAsFirstSibling();


            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            // Level2Manager_Preposition.instance.character = transform.GetChild(0).GetComponent<Animator>();
            character = GetComponent<Rigidbody2D>();
            characterani = transform.GetChild(0).GetComponent<Animator>();

        }


        public void ResetCharacterAnimtion()
        {
            if (characterani == null) return;
            characterani.ResetTrigger("IsRun");
            characterani.ResetTrigger("IsJump");
            characterani.ResetTrigger("Isresetjump");
            characterani.ResetTrigger("IsSlide");
        }
        // Update is called once per frame
        void Update()
        {
            IsPlayerRunning();
        }
        void Run()
        {

            characterani.SetTrigger("IsRun");
        }
        void Jump()
        {

            characterani.SetTrigger("IsJump");
            character.AddForce(Vector2.up * 250, ForceMode2D.Force);
            Level2Manager_Preposition.instance.Speed = 4;
            //StartCoroutine("WaitForJump");

        }

        IEnumerator WaitForJump()
        {



            yield return new WaitForSeconds(1f);
            characterani.Play("characterJump", -1, 0f);
            if (Level2Manager_Preposition.instance.ISGameOver) yield break;
            Level2Manager_Preposition.instance.Speed = 4;
            character.AddForce(Vector2.up * 250, ForceMode2D.Force);
            yield return new WaitForSeconds(.3f);
            Level2Manager_Preposition.instance.IsRunning = true;
            yield return new WaitForSeconds(.7f);
            characterani.SetTrigger("Isresetjump");
            characterani.speed = 1f;
            yield return new WaitForSeconds(.1f);
            characterani.ResetTrigger("IsJump");

            Level2Manager_Preposition.instance.Speed = 6;
            run();


        }


        IEnumerator runafterjump()
        {

            characterani.ResetTrigger("IsJump");
            characterani.SetTrigger("Isresetjump");
            yield return new WaitForSeconds(.19f);
            if (Level2Manager_Preposition.instance.ISGameOver) yield break;
            characterani.SetTrigger("idle");
            Level2Manager_Preposition.instance.Speed = 6;
            run();
        }

        bool IsOnlyOnce;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            IsOnlyOnce = true;

            if (collision.transform.name == "On_jump")
            {
                //print("force");
                //Level2Manager_Preposition.instance.PrepositionButt_Animation(true);
                //Level2Manager_Preposition.instance.Speed = 3;
                characterani.ResetTrigger("IsRun");
                characterani.SetTrigger("IsJump");
                // StartCoroutine("runafterjump");
                Level2Manager_Preposition.instance.Speed = 4;
                character.AddForce(Vector2.up * 250, ForceMode2D.Force);


            }
            //if (collision.transform.name == "Under")
            //{

            //    Level2Manager_Preposition.instance.PrepositionButt_Animation(true);
            //    Level2Manager_Preposition.instance.NearByPreposition_object = collision.transform.name;
            //    Level2Manager_Preposition.instance.IsTriggerCoin = true;

            //    Level2Manager_Preposition.instance.NearByPreposition_coin = collision.transform.GetComponentInParent<GroundMoveScript>().coin;

            //}
            if (collision.transform.name == "StopJump")
            {


                Jump();
                // StartCoroutine(waitforRunning());


            }
            if (collision.transform.name == "Of_jump")
            {


                StartCoroutine("waitforRunning");


            }
            if (collision.transform.name == "Slide")
            {
                characterani.ResetTrigger("IsRun");
                characterani.SetTrigger("IsSlide");

                StartCoroutine("waitSlideToRunning");


            }
            if (collision.transform.name == "sign")
            {

                characterani.SetTrigger("idle");
                Level2Manager_Preposition.instance.IsRunning = false;

                StartCoroutine("waitforAppearbutton");


            }

            if (collision.transform.name == "Above")
            {

                characterani.SetTrigger("IsJump");
                character.AddForce(Vector2.up * 250, ForceMode2D.Force);
                Level2Manager_Preposition.instance.Speed = 5;

            }
            if (collision.transform.name == "Limit")
            {

                characterani.SetTrigger("Isresetjump");
                Level2Manager_Preposition.instance.IsRunning = false;
                StartCoroutine("WaitForJump");



            }

            if (collision.transform.name == "LimitOn")
            {


                StartCoroutine("runafterjump");


            }

            if (collision.transform.name == "LimitAbove")
            {
                print("call");
                characterani.ResetTrigger("Isresetjump");
                characterani.ResetTrigger("IsJump");
                characterani.SetTrigger("Isresetjump");
                StartCoroutine("AfterAbovewaitforRunning");


            }
            //if (collision.transform.name == "OnTopOf")
            //{

            //    Level2Manager_Preposition.instance.PrepositionButt_Animation(true);
            //    Level2Manager_Preposition.instance.NearByPreposition_object = collision.transform.name;
            //    Level2Manager_Preposition.instance.IsTriggerCoin = true;

            //    Level2Manager_Preposition.instance.NearByPreposition_coin = collision.transform.GetComponentInParent<GroundMoveScript>().coin;

            //}
            //if (collision.transform.name == "InForntOf")
            //{

            //    Level2Manager_Preposition.instance.PrepositionButt_Animation(true);
            //    Level2Manager_Preposition.instance.NearByPreposition_object = collision.transform.name;
            //    Level2Manager_Preposition.instance.IsTriggerCoin = true;

            //    Level2Manager_Preposition.instance.NearByPreposition_coin = collision.transform.GetComponentInParent<GroundMoveScript>().coin;

            //}
            //if (collision.transform.name == "Behind")
            //{

            //    Level2Manager_Preposition.instance.PrepositionButt_Animation(true);
            //    Level2Manager_Preposition.instance.NearByPreposition_object = collision.transform.name;
            //    Level2Manager_Preposition.instance.IsTriggerCoin = true;

            //    Level2Manager_Preposition.instance.NearByPreposition_coin = collision.transform.GetComponentInParent<GroundMoveScript>().coin;

            //}
            //if (collision.transform.name == "Beneath")
            //{

            //    Level2Manager_Preposition.instance.PrepositionButt_Animation(true);
            //    Level2Manager_Preposition.instance.NearByPreposition_object = collision.transform.name;
            //    Level2Manager_Preposition.instance.IsTriggerCoin = true;

            //    Level2Manager_Preposition.instance.NearByPreposition_coin = collision.transform.GetComponentInParent<GroundMoveScript>().coin;

            //}




        }

        IEnumerator waitforAppearbutton()
        {
            yield return new WaitForSeconds(2f);

            characterani.SetTrigger("IsRun");
            Level2Manager_Preposition.instance.IsRunning = true;
            yield return new WaitForSeconds(2f);
            Level2Manager_Preposition.instance.waitSomeTime();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {

            if (Level2Manager_Preposition.instance.ISGameOver) return;

            if (collision.transform.name == "Coin")
            {

                // Level2Manager_Preposition.instance.PrepositionButt_Animation(true);
                if (Level2Manager_Preposition.instance.LastPress_button != null)
                {
                    if ((collision.transform.parent.transform.name == Level2Manager_Preposition.instance.LastPress_button))
                    {
                      //  if (Level2Manager_Preposition.instance.iscomplete) return;
                       print("stay");
                        collision.transform.gameObject.SetActive(false);
                        Level2Manager_Preposition.instance.coin_object.transform.position = collision.transform.position;
                        Level2Manager_Preposition.instance.coin_object.transform.localScale = collision.transform.localScale;
                        Level2Manager_Preposition.instance.EnterCoin(collision.transform.parent.transform.name, collision.transform.gameObject);
                    }

                }

            }
        }

        IEnumerator waitSlideToRunning()
        {
            yield return new WaitForSeconds(.1f);
            Level2Manager_Preposition.instance.IsSlide = true;
            Level2Manager_Preposition.instance.Speed = 7.5f;
            yield return new WaitForSeconds(1.2f);

            characterani.ResetTrigger("IsSlide");
            run();

            Level2Manager_Preposition.instance.IsSlide = false;
            Level2Manager_Preposition.instance.Speed = 6;
        }

        IEnumerator waitforRunning()
        {

            characterani.SetTrigger("IsJump");
            character.AddForce(Vector2.up * 250, ForceMode2D.Force);
            yield return new WaitForSeconds(.5f);
            characterani.SetTrigger("Isresetjump");
            yield return new WaitForSeconds(.3f);
            if (Level2Manager_Preposition.instance.ISGameOver) yield break;

            Level2Manager_Preposition.instance.IsRunning = false;
            characterani.ResetTrigger("IsJump");
            run();

            Level2Manager_Preposition.instance.IsRunning = true;
        }


        public bool IsPlayerRunning()
        {
            // print((characterani.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun")));
            return characterani.GetCurrentAnimatorStateInfo(0).IsName("CharacterRun");
        }
        public bool IsPlayerIdle()
        {
            //print((characterani.GetCurrentAnimatorStateInfo(0).IsName("idle")));
            return characterani.GetCurrentAnimatorStateInfo(0).IsName("idle");
        }

        IEnumerator AfterAbovewaitforRunning()
        {




            yield return new WaitForSeconds(.15f);
            if (Level2Manager_Preposition.instance.ISGameOver) yield break;
            Level2Manager_Preposition.instance.IsRunning = false;
            characterani.ResetTrigger("Isresetjump");
            yield return new WaitForSeconds(.15f);
            run();
            Level2Manager_Preposition.instance.Speed = 6;
            Level2Manager_Preposition.instance.IsRunning = true;
        }
        private void OnTriggerExit2D(Collider2D collision)
        {

            if (Level2Manager_Preposition.instance.ISGameOver) return;
            if (collision.transform.tag == "HeightExit")
            {
                // print("exit");
                // Level2Manager_Preposition.instance.PrepositionButt_Animation(false);
                //character.GetComponent<Animator>().SetTrigger("IsRun");   

            }
            if (collision.transform.name == "Coin")
            {
                print("exit");
                // Level2Manager_Preposition.instance.PrepositionButt_Animation(true);

                if ((collision.transform.parent.transform.name != Level2Manager_Preposition.instance.LastPress_button))
                {
                    Level2Manager_Preposition.instance.coin_object.transform.position = collision.transform.position;
                    Level2Manager_Preposition.instance.coin_object.transform.localScale = collision.transform.localScale;
                    Level2Manager_Preposition.instance.EnterCoin(collision.transform.parent.transform.name, collision.transform.gameObject);
                }
            }
        }


        public void Idle()
        {
            characterani.SetTrigger("idle");
            characterani.Play("idle");
        }
        public void run()
        {
            if (!Level2Manager_Preposition.instance.ISGameOver)
            {
                characterani.ResetTrigger("idle");
                characterani.ResetTrigger("IsJump");
                characterani.SetTrigger("IsRun");

            }

        }
    }
}