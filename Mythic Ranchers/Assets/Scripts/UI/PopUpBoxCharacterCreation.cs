using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        SceneManager.LoadScene("MenuScene");
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
