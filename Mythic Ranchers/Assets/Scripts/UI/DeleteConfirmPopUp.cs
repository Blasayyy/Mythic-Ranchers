using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteConfirmPopUp : MonoBehaviour
{
    public static DeleteConfirmPopUp Instance { get; private set; }

    [SerializeField]
    private Button yesButton;

    [SerializeField]
    private Button noButton;


    private void Awake()
    {
        Instance = this;
        HideUI();

        noButton.onClick.AddListener(HideUI);
        yesButton.onClick.AddListener(DeleteCharacter);
    }

    private void DeleteCharacter()
    {
        CharacterSelectManager.Instance.DeleteCharacter();
        HideUI();
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
