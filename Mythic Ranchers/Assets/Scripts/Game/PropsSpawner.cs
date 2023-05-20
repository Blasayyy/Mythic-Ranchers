using System.Collections.Generic;
using UnityEngine;

public class PropsSpawner : MonoBehaviour
{

    public static void SpawnSprites(Vector2Int position, GameObject spriteGameObjectPrefab, List<Sprite> sprites)
    {
        GameObject newGameObject = Instantiate(spriteGameObjectPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);

        Sprite chosenSprite = sprites[Random.Range(0, sprites.Count)];

        SpriteRenderer spriteRenderer = newGameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = chosenSprite;
            BoxCollider2D boxCollider = newGameObject.GetComponent<BoxCollider2D>();
            if (boxCollider == null)
            {
                boxCollider = newGameObject.AddComponent<BoxCollider2D>();
            }
            Vector2 originalSize = spriteRenderer.sprite.bounds.size;
            boxCollider.size = new Vector2(originalSize.x * 0.6f, originalSize.y * 0.4f);

            boxCollider.offset = spriteRenderer.sprite.bounds.center;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the prefab. Please add one.");
        }
    }

    public static void SpawnSprites(Vector2Int position, GameObject spriteGameObjectPrefab)
    {
        GameObject newGameObject = Instantiate(spriteGameObjectPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);

    }
}
