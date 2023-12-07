using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{

    private List<int> playerTaskList = new List<int>();
    private List<int> playerSequenceList = new List<int>();

    public List<AudioClip> buttonSoundList = new List<AudioClip>();

    //color32 gives range of colors from 1-255
    public List<List<Color32>> buttonColors = new List<List<Color32>>();

    public List<Button> clickableButtons;

    public AudioClip loseSound;

    public AudioSource audioSource;

    public CanvasGroup buttons;

    public GameObject startButton;

    public int count = 0;

    public void Begin_Game()
    {
        //setting button colors that will be used
        buttonColors.Add(new List<Color32> { new Color32(255, 200, 200, 100), new Color32(255, 0, 0, 255) });//red
        buttonColors.Add(new List<Color32> { new Color32(255, 187, 109, 100), new Color32(255, 136, 0, 255) });//orange
        buttonColors.Add(new List<Color32> { new Color32(162, 255, 124, 100), new Color32(72, 248, 0, 255) });//green
        buttonColors.Add(new List<Color32> { new Color32(57, 111, 255, 100), new Color32(0, 70, 255, 255) });//blue

        //clickableButtons[0].GetComponent<Image>().color = buttonColors[0][0];
        
        //setting images to initial color
        for (int i = 0; i < 4; i++)
        {
            clickableButtons[i].GetComponent<Image>().color = buttonColors[i][0];
        }
    }

    public void AddPlayerSequenceList(int buttonId)
    {
        playerSequenceList.Add(buttonId);
        StartCoroutine(HighlightButton(buttonId));
        for (int i = 0; i < playerSequenceList.Count; i++)
        {
            Debug.Log("add player sequence");
            if (playerTaskList[i] == playerSequenceList[i])
            {
                continue;
            }
            else
            {
                StartCoroutine(PlayerLost());
                return;
            }
        }


        if (playerSequenceList.Count == playerTaskList.Count)
        {
            count++;
            StartCoroutine(StartNextRound());
            Debug.Log(count);
            //checking if player won game
            if (count == 5)
            {
                StartCoroutine(PlayerWon());
            }



        }
    }

    public void StartGame()
    {
        StartCoroutine(StartNextRound());
        startButton.SetActive(false);

    }

    public IEnumerator HighlightButton(int buttonId)
    {
        //Debug.Log("Highlighting");
        clickableButtons[buttonId].GetComponent<Image>().color = buttonColors[buttonId][1];
        //audioSource.PlayOneShot(buttonSoundList[buttonId]);
        yield return new WaitForSeconds(.5f);
        clickableButtons[buttonId].GetComponent<Image>().color = buttonColors[buttonId][0];
    }

    public IEnumerator PlayerLost()
    {
        Debug.Log("Player lost");
        //audioSource.PlayOneShot(loseSound);
        playerSequenceList.Clear();
        playerTaskList.Clear();
        yield return new WaitForSeconds(2f);
        startButton.SetActive(true);
        count = 0;
    }

    public IEnumerator PlayerWon()
    {
        Debug.Log("Player Won");
        playerSequenceList.Clear();
        playerTaskList.Clear();
        yield return new WaitForSeconds(2f);
        startButton.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public IEnumerator StartNextRound()
    {
        //clears list for next round
        playerSequenceList.Clear();
        //disable buttons to stop player from keep clicking
        buttons.interactable = false;
        yield return new WaitForSeconds(1f);
        playerTaskList.Add(Random.Range(0, 4));
        foreach (int index in playerTaskList)
        {
            //Debug.Log("InHere");
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(HighlightButton(index));
        }
        buttons.interactable = true;
        yield return null;
    }

}
