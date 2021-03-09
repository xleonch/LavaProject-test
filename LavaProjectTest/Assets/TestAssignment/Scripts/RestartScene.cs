using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUILayout.Button("Restart"))
            SceneManager.LoadScene("TestAssignmentScene");
    }
}