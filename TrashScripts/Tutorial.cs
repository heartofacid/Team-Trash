using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{

    public GameObject[] trash;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            trash[0].SetActive(false);// checks player 1
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            trash[1].SetActive(false); //checks player 2
        }
        if (!trash[0].activeSelf && !trash[1].activeSelf)
        {
            SceneManager.LoadScene(0);// go to start screen
        }
    }

}
