using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ===============================
// AUTHOR: Emmy Berg
// CONTRIBUTORS: ---
// DESC: Spawns enemies in an area
// and identifies proper layers
// DATE MODIFIED: 5/27/2021
// ===============================

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyArray;

    public int minLimit = 0;
    public int maxLimit = 3;

    RectTransform rt;

    void Start()
    {
        int numOfEnemies = Random.Range(minLimit, maxLimit);

        rt = (RectTransform)transform;

        for (int i = 0; i < numOfEnemies; i++)
        {
            Invoke("Spawning", i + 0.5f);
        }
    }

    void Spawning()
    {

        int typeIndex = Random.Range(0, enemyArray.Length);

        Vector3 positionOffset = new Vector3(Random.Range(rt.rect.xMin, rt.rect.xMax), Random.Range(rt.rect.yMin, rt.rect.yMax), 0);

        Debug.Log($"Spawning enemy {typeIndex} at offset {positionOffset}");

        var enemy = Instantiate(enemyArray[typeIndex], transform.position + positionOffset, Quaternion.identity, transform.parent);

        enemy.layer = this.gameObject.layer;

        enemy.GetComponent<SpriteRenderer>().sortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;

    }
    private void OnDrawGizmos()
    {
        rt = (RectTransform)transform;
        Vector3 size = new Vector3(rt.rect.width, rt.rect.height, 1);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
