using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; private set; }

    public string Username { get; set; }
    public List<CharacterData> CharacterDatas { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void GetUserData(string username)
    {
        Username = username;

    }

}
