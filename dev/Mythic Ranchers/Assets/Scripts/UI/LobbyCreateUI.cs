using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*******************************************************************************

   Nom du fichier: LobbyCreateUI.cs
   
   Contexte: Cette classe sert a gérer le UI lors de la création d'un lobby
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class LobbyCreateUI : MonoBehaviour
{
    public static LobbyCreateUI Instance { get; private set; }

    [SerializeField]
    private Button createButton;

    [SerializeField]
    private Button lobbyNameButton;

    [SerializeField]
    private Button publicPrivateButton;

    [SerializeField]
    private TextMeshProUGUI lobbyNameText;

    [SerializeField]
    private TextMeshProUGUI publicPrivateText;

    private string lobbyName;
    private bool isPrivate;
    private int keyLevel;        

    private void Awake()
    {
        Instance = this;

        createButton.onClick.AddListener(() =>
        {
            if (lobbyNameText != null)
            {
                lobbyName = lobbyNameText.text;
            }
            else
            {
                lobbyName = "Lobby Name";
            }
            LobbyManager.Instance.CreateLobby(lobbyName, isPrivate, AccountManager.Instance.CharacterDatas[AccountManager.Instance.SelectedCharacter].Current_key);
            HideUI();
        });

        publicPrivateButton.onClick.AddListener(() =>
        {
            isPrivate = !isPrivate;
            UpdateText();
        });
    }

    private void UpdateText()
    {
        lobbyNameText.text = lobbyName;
        publicPrivateText.text = isPrivate ? "Private" : "Public";
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
        lobbyName = "Lobby Name";
        isPrivate = false;
        UpdateText();
    }
}
