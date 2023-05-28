using UnityEngine;
using UnityEngine.UI;

/*******************************************************************************

   Nom du fichier: PopUpBoxRegister.cs
   
   Contexte: Cette classe sert a gérer les popups UI pour s'enregistrer
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class PopUpBoxRegister : MonoBehaviour
{
    [SerializeField]
    private Button okButton;

    public static PopUpBoxRegister Instance {get; private set;}

    private void Awake()
    {
        Instance = this;
        HideUI();
        okButton.onClick.AddListener(GoNext);
    }

    private void GoNext()
    {
        HideUI();
        LoginUI.Instance.ShowUI();
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
