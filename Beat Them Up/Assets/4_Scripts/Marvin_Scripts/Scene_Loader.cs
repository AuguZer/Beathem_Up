using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Loader : MonoBehaviour
{
    public static Scene_Loader instance;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Il y a plus d'une instance dans la Scène");
            return;
        }
        instance = this;
    }
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

    public void GameOver()
    {
        SceneManager.LoadScene(2);
    }
}
