using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour {

    public float health;

    public List<Drop> drops;
    public float dropRadius;

    public void TakeDamage(float _damage)
    {
        health -= _damage;
        CheckHealth();
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        foreach (var drop in drops)
        {
            for (int i = 0; i < Random.Range(drop.minAmount, drop.maxAmount); i++)
            {
                Vector3 dropPos = Random.insideUnitCircle * dropRadius;
                dropPos.z += dropPos.y;
                dropPos.y = 0.25f;
                Instantiate(drop.item.droppedItemPrefab, dropPos + transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            }
        }
        Destroy(gameObject);
    }

}

[System.Serializable]
public class Drop
{
    public Item item;
    public int minAmount;
    public int maxAmount;
}