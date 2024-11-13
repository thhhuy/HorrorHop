using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import thư viện SceneManager

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private bool isPaused;
    [SerializeField] private MonoBehaviour[] playerControls; // Các thành phần điều khiển của người chơi

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                ActivateMenu();
            }
            else
            {
                DeactivateMenu();
            }
        }
    }

    void ActivateMenu()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor

        // Vô hiệu hóa các thành phần điều khiển người chơi
        foreach (var control in playerControls)
        {
            control.enabled = false;
        }
    }

    void DeactivateMenu()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen

        // Kích hoạt lại các thành phần điều khiển người chơi
        foreach (var control in playerControls)
        {
            control.enabled = true;
        }
    }

    // Phương thức để tiếp tục trò chơi
    public void ResumeGame()
    {
        isPaused = false;
        DeactivateMenu();
    }

    // Phương thức để thoát ra menu chính
    public void ExitToMenu()
    {
        // Khôi phục thời gian trước khi thoát ra menu
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene"); // Đảm bảo rằng bạn có một scene tên là "MainMenu"
    }
}
