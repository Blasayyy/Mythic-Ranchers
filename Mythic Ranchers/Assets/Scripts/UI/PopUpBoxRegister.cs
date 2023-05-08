using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
