using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Loot : NetworkBehaviour
{

    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private BoxCollider2D collider;
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Item[] itemList;
    [SerializeField]
    private Item item;

    private void Start()
    {
        int rand = Random.Range(0, itemList.Length + 1);
        Initialize(itemList[rand]);
    }

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
        Tooltip.instance.ShowTooltip(item.tooltip);
    }

    private void OnMouseExit()
    {
        Tooltip.instance.HideTooltip();
    }

}
