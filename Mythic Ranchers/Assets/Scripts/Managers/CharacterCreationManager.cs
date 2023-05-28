using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*******************************************************************************

   Nom du fichier: CharacterCreationManager.cs
   
   Contexte: Cette classe sert a gérer la création de nouveau character
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class CharacterCreationManager : MonoBehaviour
{
    public static CharacterCreationManager Instance { get; private set; }

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
    private TextMeshProUGUI classDecription;

    [SerializeField]
    private Button createButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button logOutButton;

    [SerializeField]
    private TextMeshProUGUI errorText;

    public Sprite BerzerkerSprite;
    public Sprite MageSprite;
    public Sprite NecromancerSprite;

    private List<string> classes;
    private int currentIndex = 0;

    private void Awake()
    {
        Instance = this;

        errorText.gameObject.SetActive(false);

        classes = new List<string>();
        classes.Add("Berzerker");
        classes.Add("Mage");
        classes.Add("Necromancer");

        leftButton.onClick.AddListener(GoLeft);
        rightButton.onClick.AddListener(GoRight);
        createButton.onClick.AddListener(CreateCharacter);
        backButton.onClick.AddListener(Back);
        logOutButton.onClick.AddListener(LogOut);

        if(AccountManager.Instance.CharacterDatas == null || AccountManager.Instance.CharacterDatas.Count <= 0)
        {
            backButton.gameObject.SetActive(false);
            logOutButton.gameObject.SetActive(true);
        }
        else
        {
            backButton.gameObject.SetActive(true);
            logOutButton.gameObject.SetActive(false);
        }
    }

    private void GoLeft()
    {
        currentIndex--;

        if(currentIndex < 0)
        {
            currentIndex = classes.Count - 1;
        }

        SetInfo(currentIndex);
    }

    private void GoRight()
    {
        currentIndex++;

        if (currentIndex >= classes.Count)
        {
            currentIndex = 0;
        }

        SetInfo(currentIndex);
    }

    private async void CreateCharacter()
    {
        List<EquipmentData> emptyEquipment = new List<EquipmentData>();
        if(await DatabaseManager.Instance.CreateCharacter(AccountManager.Instance.Username, nameText.text, classes[currentIndex] ,1, 0, 1, emptyEquipment, "00000000000"))
        {
            PopUpBoxCharacterCreation.Instance.ShowUI();
            AccountManager.Instance.GetUserData(AccountManager.Instance.Username);
        }
        else
        {
            errorText.gameObject.SetActive(true);
        }
    }

    private void SetInfo(int currentIndex)
    {
        string className = classes[currentIndex];
        classNameText.text = className;

        string description = "";

        if(className == "Berzerker")
        {
            characterImage.sprite = BerzerkerSprite;
            description = "The berserker class is a fearsome group of warriors who are known for their mastery of rage and ferocity in battle. With a deep understanding of their" +
                " inner fury, berserkers are able to tap into this power and unleash devastating attacks upon their enemies. Their strength and endurance are unparalleled on the battlefield," +
                " as they are able to push themselves beyond their physical limits. Some view the berserker's rage as a gift, while others see it as a curse that they must learn to control. " +
                "Regardless, all berserkers are respected for their unmatched power and are feared by enemies who face them in combat.";
        }
        else if(className == "Mage")
        {
            characterImage.sprite = MageSprite;
            description = "The mage class is a group of powerful spellcasters who wield the magical energies of the universe. Through intense study and meditation, mages are able to manipulate the" +
                " elements, control the minds of their enemies, and call forth devastating displays of arcane power. Mages are often seen as enigmatic and mysterious, as their abilities are beyond the" +
                " understanding of most mortals. Despite this, they are highly respected for their immense knowledge and mastery of the arcane arts. Many seek to learn from mages, as their abilities offer" +
                " a path to great power and influence. However, the path to becoming a mage is a difficult one, as it requires great discipline and dedication to unlock the secrets of the universe.";
        }
        else if(className == "Necromancer")
        {
            characterImage.sprite = NecromancerSprite;
            description = "The necromancer class is a group of dark magic users who have learned to harness the power of death and decay. Necromancers are feared and reviled by many, as their abilities " +
                "allow them to manipulate the souls and bodies of the dead. They are known for their ability to summon undead minions, drain the life force of their enemies, and even raise the dead to fight" +
                " on their behalf. Necromancers are often seen as dangerous and unstable, as their powers can corrupt both their bodies and their souls. However, those who are able to master the dark arts of" +
                " necromancy can become immensely powerful, wielding abilities that are unmatched by any other class. Despite their fearsome reputation, there are some who seek to learn from necromancers, hoping" +
                " to unlock the secrets of life and death.";
        }
        classDecription.text = description;
    }

    private void Back()
    {
        SceneManager.LoadScene("CharacterSelectScene");
    }

    private void LogOut()
    {
        MainMenu.Instance.LogOut();
        SceneManager.LoadScene("MenuScene");
    }
}
