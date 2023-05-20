using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private BoxCollider2D collider;
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Item item;

    public void Initialize(Item item)
    {
        this.item = item;
        sr.sprite = item.image;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bool canAdd = InventoryManager.instance.AddItem(item);
            if (canAdd)
            {
                StartCoroutine(MoveAndCollect(collision.transform));
            }
        }
    }

    private IEnumerator MoveAndCollect(Transform target)
    {
        Destroy(collider);

        while (transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return 0;
        }

        Destroy(gameObject);
    }

    private void OnMouseOver()
    {
        Debug.Log("hovering loot");
        Tooltip.instance.ShowTooltip("Loot");
    }

    private void OnMouseExit()
    {
        Tooltip.instance.HideTooltip();
    }

}
