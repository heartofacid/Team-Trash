using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Manager : MonoBehaviour
{
    public GameObject[] trashArray;
    public GameObject[] recycleArray;

    private GameObject[] wasteArray;

    public GameObject cans;

    private GameObject currentItem = null;

    public Text player1Text;
    public Text player2Text;
    public Text wasteName;
    public Text winner;

    private ArrayList wasteList = new ArrayList();

    private bool isItemSorted = false;
    private bool hasPlayer1Answered = false;
    private bool hasPlayer2Answered = false;

    private int playerScore1 = 0;
    private int playerScore2 = 0;

    private float currentTime;
    private float delayTimer;
    
    private Animator currentItemAnimation;

    private enum animStates {
        none = -1,
        playerOneTrash = 0,
        playerOneRecycle = 1,
        playerTwoTrash = 2,
        playerTwoRecycle= 3
    }

    private animStates p1State = animStates.none;
    private animStates p2State = animStates.none;

    public AudioSource audioManager;
    public AudioSource music;

    public AudioClip[] canSounds;

    public AudioClip wrong;

    PlayerPrefs endScreen;

    // Start is called before the first frame update
    void Start()
    {
        InitWasteList();
        SelectItem();
        wasteInvisble();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(wasteList.Count <= 0)
        {
            if (playerScore1 > playerScore2)
            {
                PlayerPrefs.SetString("endPhrase", "Player 1 Wins with " + playerScore1 + " points");
            }
            else if(playerScore2>playerScore1)
            {
                PlayerPrefs.SetString("endPhrase","Player 2 Wins with " + playerScore2 + " points");
            }
            else
            {
                PlayerPrefs.SetString("endPhrase","Tied Game, both players scored " + playerScore1 + " points");
            }
            SceneManager.LoadScene(3);
            music.Stop();
            
        }
        else
        {
            Game();
        }

    }
    // InitWasteList function puts items in from trashArray
    // and recyleArray into wasteArray. Then wasteArray is
    // converted into an ArrayList called wasteList.
    private void InitWasteList()
    {
        wasteArray = recycleArray.Concat(trashArray).ToArray();
        for (int i = 0; i < wasteArray.Length; i++)
        {
            wasteList.Add(wasteArray[i]);
        }
    }
    // SelectItem removes a random item from wasteList
    private void SelectItem()
    {
        if(wasteList.Count > 0)
        {
            int item = Random.Range(0, wasteList.Count);
            currentItem = (GameObject)wasteList[item];
            wasteList.RemoveAt(item);
            isItemSorted = false;
            currentItem.SetActive(true);
            wasteName.text = currentItem.name; //sets name on screen
            hasPlayer1Answered = false;
            hasPlayer2Answered = false;
            // turn cans back to normal color
            currentItemAnimation = currentItem.GetComponent<Animator>();
            for (int i = 0; i < 4; i++)
            {
                cans.GetComponentsInChildren<Renderer>()[i].material.color = Color.white;
            }
        }
    }

    private void Game()
    {
        if (currentTime <= 5 && !isItemSorted)
        { 
            //Player 1
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && !hasPlayer1Answered)
            {
                if (IsItemRecycleable() && Input.GetKeyDown(KeyCode.D) || !IsItemRecycleable() && Input.GetKeyDown(KeyCode.A))
                {
                    playerScore1++;
                    isItemSorted = true;
                    hasPlayer2Answered = true;
                    hasPlayer1Answered = true;
                    player1Text.text = "" + playerScore1; //updates score
                    audioManager.clip = canSounds[Random.Range(0, canSounds.Length)]; //randomizes sounds produced by cans
                    audioManager.Play();
                    if (IsItemRecycleable())// plays correct trash animation
                    {
                        p1State = animStates.playerOneRecycle;
                    } else
                    {
                        p1State = animStates.playerOneTrash;

                    }
                    currentItemAnimation.SetInteger("animationNum", (int)p1State);
                }
                else
                {
                    audioManager.clip = wrong;
                    audioManager.Play();
                    cans.GetComponentsInChildren<Renderer>()[0].material.color = Color.red;// color shift cans to red if incorrect
                    cans.GetComponentsInChildren<Renderer>()[3].material.color = Color.red;
                }
                hasPlayer1Answered = true;
            }
            //Player 2
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W)) && !hasPlayer2Answered)
            {
                if (IsItemRecycleable() && Input.GetKeyDown(KeyCode.W) || !IsItemRecycleable() && Input.GetKeyDown(KeyCode.S))
                {
                    playerScore2++;
                    isItemSorted = true;
                    hasPlayer2Answered = true;
                    hasPlayer1Answered = true;
                    player2Text.text = "" + playerScore2;
                    audioManager.clip = canSounds[Random.Range(0, canSounds.Length)];
                    audioManager.Play();
                    if (IsItemRecycleable())
                    {
                        p2State = animStates.playerTwoRecycle;
                    }
                    else
                    {
                        p2State = animStates.playerTwoTrash;

                    }
                    currentItemAnimation.SetInteger("animationNum", (int)p2State);
                    }
                else
                {
                    audioManager.clip = wrong;
                    audioManager.Play();
                    cans.GetComponentsInChildren<Renderer>()[1].material.color = Color.red;
                    cans.GetComponentsInChildren<Renderer>()[2].material.color = Color.red;
                }

                hasPlayer2Answered = true;
            }
        }
            
        else
        {
            // when no one sorts an item on time

            //start next round code
            currentTime = 0;
            isItemSorted = false;
            
            StartCoroutine(animTimer());

        }
    }
    
    private bool IsItemRecycleable()
    {
        return recycleArray.Contains(currentItem);

    }

    private void wasteInvisble()
    {
        foreach(GameObject item in wasteList)
        {
            item.SetActive(false);
        }
    }

    //The IEnumerator allow us to wait for the animation of the items to complete before proceeding to spawn the next one
    //also resets animationstates
    private IEnumerator animTimer()
    {
        yield return new WaitForSeconds(1f);
        currentItem.SetActive(false);
        SelectItem();
        currentItemAnimation.SetInteger("animationNum", (int)animStates.none);
    }
    public void ExitButton2()
    {
        SceneManager.LoadScene(0);
    }

}
