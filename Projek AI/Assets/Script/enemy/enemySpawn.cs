using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    public GameObject[] hiddenEnemys;

    public void unhide()
    {
        foreach (var item in hiddenEnemys)
        {
            item.SetActive(true);
        }
    }
}
