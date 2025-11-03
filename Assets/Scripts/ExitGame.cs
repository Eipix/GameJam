using UnityEngine;

public class ExitGame : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            // В редакторе Unity просто останавливаем Play Mode
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // В сборке — выходим из приложения
            Application.Quit();
#endif
        }
    }
}
