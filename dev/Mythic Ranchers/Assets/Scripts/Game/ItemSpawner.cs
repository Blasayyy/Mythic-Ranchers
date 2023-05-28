using UnityEngine;

/*******************************************************************************

   Nom du fichier: ItemSpawner.cs
   
   Contexte: Cette classe sert a faire apparaître du loot sur demande
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;

    void Start()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0f);
        GameObject loot = Instantiate(itemPrefab, randomPosition, Quaternion.identity);
    }
}
