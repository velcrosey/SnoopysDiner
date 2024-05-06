using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string level;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MoveScene(level);
        }
    }

    public void ButtonMoveScene(string levelName)
    {
        MoveScene(levelName);
    }

    private void MoveScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
