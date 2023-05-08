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

    [SerializeField]
    private TextMeshProUGUI errorText;

    private string username;
    private string password;

    private void Awake()
    {
        Instance = this;

        registerButton.onClick.AddListener(RegisterUser);
        errorText.gameObject.SetActive(false);
    }

    private async void RegisterUser()
    {
        if (usernameText != null && passwordInputField.text != null)
        {
            username = usernameText.text;
            string unHashedPassword = passwordInputField.text;

            password = PasswordHasher.Instance.HashPassword(unHashedPassword);

            if (await DatabaseManager.Instance.CreateUser(username, password))
            {
                HideUI();
                PopUpBoxRegister.Instance.ShowUI();
            }
            else
            {
                errorText.gameObject.SetActive(true);
            }
        }
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }
}
