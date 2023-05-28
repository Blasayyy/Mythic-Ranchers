using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/*******************************************************************************

   Nom du fichier: NetworkedSprite.cs
   
   Contexte: Cette classe représente un sprite qui sera synchroniser sur le server
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class NetworkedSprite : NetworkBehaviour
{
    [SerializeField]
    private List<Sprite> sprites = new List<Sprite>();

    public bool isNonMoveable = false;

    private NetworkVariable<int> chosenSpriteIndex = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            chosenSpriteIndex.Value = Random.Range(0, sprites.Count);
        }

        GetComponent<SpriteRenderer>().sprite = sprites[chosenSpriteIndex.Value];
        chosenSpriteIndex.OnValueChanged += SyncSprite;
        SyncSprite(0, chosenSpriteIndex.Value); // Initial Sync
    }

    private void SyncSprite(int oldIndex, int newIndex)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[newIndex];

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        Vector2 originalSize = spriteRenderer.sprite.bounds.size;
        boxCollider.size = new Vector2(originalSize.x * 0.6f, originalSize.y * 0.4f);
        boxCollider.offset = spriteRenderer.sprite.bounds.center;

        if (!isNonMoveable)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            }
        }
    }
}
