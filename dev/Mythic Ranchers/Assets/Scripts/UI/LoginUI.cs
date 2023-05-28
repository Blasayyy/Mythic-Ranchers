using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*******************************************************************************

   Nom du fichier: LoginUI.cs
   
   Contexte: Cette classe sert a gérer le UI lorsqu'on se connecte au programme
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class LoginUI : MonoBehaviour
{
    public static LoginUI Instance { get; private set; }

    [SerializeField]
    public TextMeshProUGUI usernameText;

    [SerializeField]
    public TMP_InputField passwordInputField;

    [SerializeField]
    private TextMeshProUGUI errorText;

    [SerializeField]
    private Button loginButton;

    [SerializeField]
    private Button registerButton;

    [SerializeField]
    private Toggle rememberMeToggle;

    private string username;
    private string password;

    private void Awake()
    {
        Instance = this;

        loginButton.onClick.AddListener(Login);
        registerButton.onClick.AddListener(ShowRegistration);
        errorText.gameObject.SetActive(false);

        DontDestroyOnLoad(this);
    }

    private async void Login()
    {
        if (usernameText != null && passwordInputField.text != null)
        {
            username = usernameText.text;
            password = passwordInputField.text;

            bool loginSuccessful = await DatabaseManager.Instance.LoginUser(username, password);
            if (loginSuccessful)
            {
                if (rememberMeToggle.isOn)
                {
                    PlayerPrefs.SetString("username", username);
                    PlayerPrefs.SetString("password", PasswordHasher.Instance.HashPassword(password));
                }

                AccountManager.Instance.GetUserData(username);
                
                HideUI();
                if (AccountManager.Instance.CharacterDatas == null || AccountManager.Instance.CharacterDatas.Count <= 0)
                {
                    SceneManager.LoadScene("CharacterCreationScene");
                }                    
            }
            else
            {
                errorText.gameObject.SetActive(true);
            }
        }
    }

    private void ShowRegistration()
    {
        HideUI();
        RegisterUI.Instance.ShowUI();
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
