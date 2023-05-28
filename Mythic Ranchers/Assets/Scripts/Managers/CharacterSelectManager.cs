using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*******************************************************************************

   Nom du fichier: CharacterSelectManager.cs
   
   Contexte: Cette classe sert a gérer la sélection du character que le joueur 
             souhaite jouer
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI classNameText;

    [SerializeField]
    private Button leftButton;

    [SerializeField]
    private Button rightButton;

    [SerializeField]
    private Image characterImage;

    [SerializeField]
    private Button talentsButton;

    [SerializeField]
    private Button equipmentButton;

    [SerializeField]
    private Button selectButton;

    [SerializeField]
    private Button createButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button deleteButton;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI keyLevelText;

    [SerializeField]
    private TextMeshProUGUI staminaText;

    [SerializeField]
    private TextMeshProUGUI strengthText;

    [SerializeField]
    private TextMeshProUGUI intellectText;

    [SerializeField]
    private TextMeshProUGUI agilityText;

    [SerializeField]
    private TextMeshProUGUI armorText;

    [SerializeField]
    private TextMeshProUGUI leechText;

    public Sprite BerzerkerSprite;
    public Sprite MageSprite;
    public Sprite NecromancerSprite;

    private int currentIndex = 0;

    private void Awake()
    {
        Instance = this;

        
        createButton.onClick.AddListener(GoToCharacterCreation);
        backButton.onClick.AddListener(Back);
        selectButton.onClick.AddListener(SelectCharacter);
        deleteButton.onClick.AddListener(DeleteCharacterConfirm);

        SetInfo(currentIndex);
    }


    public void GoLeft()
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = AccountManager.Instance.CharacterDatas.Count - 1;
        }

        SetInfo(currentIndex);
    }

    public void GoRight()
    {
        currentIndex++;

        if (currentIndex >= AccountManager.Instance.CharacterDatas.Count)
        {
            currentIndex = 0;
        }

        SetInfo(currentIndex);
    }

    private void SetInfo(int index)
    {
        nameText.text = AccountManager.Instance.CharacterDatas[index].Name;
        classNameText.text = AccountManager.Instance.CharacterDatas[index].ClassName;
        levelText.text = "level: " + AccountManager.Instance.CharacterDatas[index].Level.ToString();
        keyLevelText.text = "key level: " +  AccountManager.Instance.CharacterDatas[index].Current_key.ToString();
        staminaText.text = "stamina: " + AccountManager.Instance.CharacterDatas[index].Stats["stamina"].ToString();
        strengthText.text = "strength: " + AccountManager.Instance.CharacterDatas[index].Stats["strength"].ToString();
        intellectText.text = "intellect: " + AccountManager.Instance.CharacterDatas[index].Stats["intellect"].ToString();
        agilityText.text = "agility: " + AccountManager.Instance.CharacterDatas[index].Stats["agility"].ToString();
        armorText.text = "armor: " + AccountManager.Instance.CharacterDatas[index].Stats["armor"].ToString();
        leechText.text = "leech: " + AccountManager.Instance.CharacterDatas[index].Stats["leech"].ToString();

        if (classNameText.text == "Berzerker")
        {
            characterImage.sprite = BerzerkerSprite;
        }
        else if (classNameText.text == "Mage")
        {
            characterImage.sprite = MageSprite;
        }
        else if (classNameText.text == "Necromancer")
        {
            characterImage.sprite = NecromancerSprite;
        }

        if(currentIndex == AccountManager.Instance.SelectedCharacter)
        {
            nameText.color = Color.green;
        }
        else
        {
            nameText.color = Color.white;
        }
    }

    private void SelectCharacter()
    {
        AccountManager.Instance.SelectedCharacter = currentIndex;
        SetInfo(currentIndex);
    }

    private void GoToCharacterCreation()
    {
        SceneManager.LoadScene("CharacterCreationScene");
    }


    private void Back()
    {
        SceneManager.LoadScene("MenuScene");
    }

    private void DeleteCharacterConfirm()
    {
        DeleteConfirmPopUp.Instance.ShowUI();
    }

    public async void DeleteCharacter()
    {
        if(await DatabaseManager.Instance.DeleteCharacter(AccountManager.Instance.CharacterDatas[currentIndex].Name))
        {
            AccountManager.Instance.GetUserData(AccountManager.Instance.Username);
            if(AccountManager.Instance.SelectedCharacter == currentIndex)
            {
                AccountManager.Instance.SelectedCharacter = 0;
            }
            currentIndex = 0;
            SetInfo(currentIndex);            
        }
    }
}
