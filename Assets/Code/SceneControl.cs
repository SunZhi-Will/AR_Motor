using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public void EnterARMode(){
        SceneManager.LoadScene("AR");
    }
    public void Exit(){
        Application.Quit();
    }
}
