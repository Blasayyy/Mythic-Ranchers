using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*******************************************************************************

   Nom du fichier: PopUpBoxCharacterCreation.cs
   
   Contexte: Cette classe sert a gérer les popups UI pour la création des characters
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class PopUpBoxCharacterCreation : MonoBehaviour
{
    [SerializeField]
    private Button okButton;

    public static PopUpBoxCharacterCreation Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        HideUI();
        okButton.onClick.AddListener(GoNext);
    }

    private void GoNext()
    {
        HideUI();
        SceneManager.LoadScene("CharacterSelectScene");
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
