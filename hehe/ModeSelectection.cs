using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;// Đảm bảo bạn đã import thư viện này

public class ModeSelection : MonoBehaviour
{
    private string mode;
    // Hàm được gọi khi người chơi chọn chế độ
    public void OnModeSelected(string mode)
    {
        // Lưu chế độ chơi đã chọn vào PlayerPrefs
        PlayerPrefs.SetString("GameMode", mode);

        // Sau khi chọn chế độ, chuyển sang scene DoGame
        SceneManager.LoadScene("DoGame");
    }
}
