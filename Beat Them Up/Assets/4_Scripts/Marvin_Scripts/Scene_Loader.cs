using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Loader : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChargerUneScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
     }

    // Update is called once per frame
    public void NextLevel()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(index + 1);
    }

    public void TryAgain()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(index);
    }
}
