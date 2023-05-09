using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        accountName.text = "Logged in as: " + AccountManager.Instance.Username;
        if (AccountManager.Instance.CharacterDatas != null)
        {
            if(AccountManager.Instance.CharacterDatas.Count > 0)
            {
                string characterName = "Selected character: " + AccountManager.Instance.CharacterDatas[AccountManager.Instance.SelectedCharacter].Name + "(" + AccountManager.Instance.CharacterDatas[AccountManager.Instance.SelectedCharacter].ClassName + ")";
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

    }
}
