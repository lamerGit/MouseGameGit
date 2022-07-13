using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour
{
    public GameObject LevelUpPanel;
    public void Stage1()
    {
        SceneManager.LoadScene(0);
    }

    public void OnLevelUpPanel()
    {
        LevelUpPanel.SetActive(true);
    }
    
}
