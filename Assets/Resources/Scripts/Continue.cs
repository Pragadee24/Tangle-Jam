using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    public void OnMouseDown()
    {
        int curSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nxtSceneIndex = 0;
        if (curSceneIndex == 0)
        {
            nxtSceneIndex++;
        }
        SceneManager.LoadScene(nxtSceneIndex);
    }
}
