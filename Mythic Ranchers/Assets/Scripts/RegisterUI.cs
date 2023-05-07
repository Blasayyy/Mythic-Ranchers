using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterUI : MonoBehaviour
{
    public static RegisterUI Instance { get; private set; }

    [SerializeField]
    private Button registerButton;

    [SerializeField]
    private TextMeshProUGUI usernameText;

    [SerializeField]
    private TMP_InputField passwordInputField;


    private string username;
    private string password;

    private void Awake()
    {
        Instance = this;

        registerButton.onClick.AddListener(() =>
        {
            if (usernameText != null && passwordInputField.text != null)
            {
                username = usernameText.text;
                password = passwordInputField.text;

                DatabaseManager.Instance.CreateUser(username, password);
            }
            
            HideUI();
        });

    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
