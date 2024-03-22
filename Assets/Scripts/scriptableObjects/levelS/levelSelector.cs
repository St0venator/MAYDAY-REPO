using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/levelSelector", order = 1)]
public class levelSelector : ScriptableObject
{
    public List<string> allScenes;
    public string nextScene;

    int sceneNum;
    public void setup()
    {
        sceneNum = allScenes.Count;

        if(sceneNum != 0)
        {
            nextScene = allScenes[Random.Range(0, sceneNum - 1)];
        }
        else
        {
            nextScene = "WinScene";
        }
    }

    public void changeScenes(MenuManager mngr)
    {
        mngr.StartCoroutine("LoadAsynchronously", nextScene);
        allScenes.Remove(nextScene);
    }

    public void preSetScenes()
    {
        allScenes.Add("blockGame");
        allScenes.Add("goatGame");
        allScenes.Add("lungs");
        allScenes.Add("negaMungus");
        allScenes.Add("quintana");
        allScenes.Add("skullFace");
        allScenes.Add("texus");
        allScenes.Add("yknow");
    }

    private void Awake()
    {
        preSetScenes();
        setup();
    }
}
