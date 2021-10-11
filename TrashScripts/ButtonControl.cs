using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonControl : MonoBehaviour
{
    //public GameObject helpPanel;

    public Button start;
    public Button help;
    public Button exit;

    public Text endPhrase;

    // Start is called before the first frame update
    void Start()
    {
        //EventSystem.current.SetSelectedGameObject(null);
    }

    //Update is called once per frame
    void Update()
    {
        // how we handle button control
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            print("hello?");
            start.onClick.Invoke();
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            print("hello???");
            help.onClick.Invoke();
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            exit.onClick.Invoke();
        }
        endPhrase.text = PlayerPrefs.GetString("endPhrase"); //retrieves player pref of phrasing to use on end screen
    }

    //Button Management
    public void Startbutton()
    {
        SceneManager.LoadScene(1);//starts game
    }

    public void HelpButton()
    {
        SceneManager.LoadScene(2);//tutorial screen
    }

    public void ExitButton()
    {
        SceneManager.LoadScene(0);//start screen
    }
}
