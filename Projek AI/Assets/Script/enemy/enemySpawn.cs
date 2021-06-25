using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySpawn : MonoBehaviour
{
    GameObject[] hiddenEnemys;
    GameObject[] hiddenPaths;

    public void unhide()
    {
        foreach (var item in hiddenEnemys)
        {
            item.SetActive(true);
        }
        foreach (var item in hiddenPaths)
        {
            item.SetActive(true);
        }
    }
}
