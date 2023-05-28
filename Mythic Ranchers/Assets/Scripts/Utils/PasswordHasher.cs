using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/*******************************************************************************

   Nom du fichier: PasswordHasher.cs
   
   Contexte: Cette classe sert à hasher les passwords avant de les envoyer dans la DB
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class PasswordHasher: MonoBehaviour
{
    public static PasswordHasher Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }

    public bool VerifyPassword(string password, string storedHash)
    {
        string passwordHash = HashPassword(password);
        return passwordHash == storedHash;
    }
}