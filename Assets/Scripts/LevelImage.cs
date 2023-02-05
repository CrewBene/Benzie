using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelImage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}