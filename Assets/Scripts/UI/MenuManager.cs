using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public levelSelector LS = null;

    //Load Screen Game Objects
    public GameObject loadingScene;
    public Slider Loadslider;

    //Pause Screen Game Objects
    public GameObject PausePanel;
    public Button PauseButton;
    public Button UnPauseButton;

    //Audio Systems
    public SoundManager SoundManager;
    public Slider AudioSlider;

    //Tutorial Objects
    public GameObject TutorialPanel;

    private void Awake()
    {
        //Hide Pause Menu Assets
        PausePanel.SetActive(false);
        AudioSlider.gameObject.SetActive(false);
        TutorialPanel.SetActive(false);

        //Set the audio to the last saved slider value
        SoundManager.ChangeVolume(AudioSlider.value);
    }


    #region Scene Swap method
    //This method will take an input of a scene name then take you to that scene
    public void ToScene(string scene)
    {
        LS.changeScenes(this);
        //SceneManager.LoadScene(scene);
    }
    #endregion


    #region LoadingScreen Method
    //This scene can be called to pull up the loading screen while loading a scene called from the "ToScene" method
    public IEnumerator LoadAsynchronously(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        while (!operation.isDone)
        {
            loadingScene.SetActive(true);


            float progress = Mathf.Clamp01(operation.progress/ 0.9f);
            Loadslider.value = progress;

            //yield return new WaitForSeconds(3f); // Small Waiting Delay for levels
            yield return null;
        }
    }
    #endregion


    #region Pause Game Method
    //Pause the game bet setting the time scale to 0 so all time activites 
    //Activates the pause menu pannel to pull up a new UI
    public void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        PauseButton.gameObject.SetActive(false);
        AudioSlider.gameObject.SetActive(true);
    }
    #endregion


    #region Un Pause Method
    public void UnPauseGame()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        PauseButton.gameObject.SetActive(true);
        AudioSlider.gameObject.SetActive(false);
    }
    #endregion

    public void GetAudioSliderValue()
    {
        SoundManager.ChangeVolume(AudioSlider.value);
        Debug.Log("Volume Changed");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadTutorial()
    {
        TutorialPanel.SetActive(true);
    }
    public void HideTutorial()
    {
        TutorialPanel.SetActive(false);
    }
}
