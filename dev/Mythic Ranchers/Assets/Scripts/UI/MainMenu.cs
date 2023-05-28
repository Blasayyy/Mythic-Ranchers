using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/*******************************************************************************

   Nom du fichier: MainMenuUI.cs
   
   Contexte: Cette classe sert a gérer le UI du menu principal
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI accountName;

    [SerializeField]
    private TextMeshProUGUI characterNameText;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        RegisterUI.Instance.HideUI();

        if(PlayerPrefs.GetString("username") != null && PlayerPrefs.GetString("username") != "" && PlayerPrefs.GetString("password") != null)
        {
            LoginUI.Instance.HideUI();
            AccountManager.Instance.GetUserData(PlayerPrefs.GetString("username"));
        }

        if(AccountManager.Instance.Username != null && AccountManager.Instance.Username != "")
        {
            LoginUI.Instance.HideUI();
            if (AccountManager.Instance.CharacterDatas == null || AccountManager.Instance.CharacterDatas.Count <= 0)
            {
                SceneManager.LoadScene("CharacterCreationScene");
            }
        }
        else
        {
            LoginUI.Instance.ShowUI();
        }

        accountName.text = "Logged in as: " + AccountManager.Instance.Username;
    }

    void Update()
    {
        accountName.text = "Logged in as: " + AccountManager.Instance.Username;
        if (AccountManager.Instance.CharacterDatas != null)
        {
            if(AccountManager.Instance.CharacterDatas.Count > 0)
            {
                string characterName = "Selected character: " + AccountManager.Instance.CharacterDatas[AccountManager.Instance.SelectedCharacter].Name +
                                       "(" + AccountManager.Instance.CharacterDatas[AccountManager.Instance.SelectedCharacter].ClassName + ")";
                characterNameText.text = characterName;
            }            
        }                
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToCharacterScreen()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    public void LogOut()
    {
        PlayerPrefs.SetString("username", null);
        PlayerPrefs.SetString("password", null);
        AccountManager.Instance.Username = "";
        AccountManager.Instance.GetUserData("");

        if(LoginUI.Instance != null)
        {
            LoginUI.Instance.ShowUI();
        }
    }
}
