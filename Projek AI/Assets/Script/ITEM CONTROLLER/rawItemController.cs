using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rawItemController : MonoBehaviour
{
    public string name;
    public GameObject ri;
    public static int id;


    // Start is called before the first frame update
    void Awake()
    {
        id++;
        if(GameObject.Find("PF Batrei") != null)
        {
            ri = GameObject.Find("PF Batrei");
            ri.name = "Batrei " + id;
        }
        else if (GameObject.Find("PF Alkohol") != null)
        {
            ri = GameObject.Find("PF Alkohol");
            ri.name = "Alkohol " + id;
        }
        else if (GameObject.Find("PF Cloth") != null)
        {
            ri = GameObject.Find("PF Cloth");
            ri.name = "Cloth " + id;
        }
        else if (GameObject.Find("PF Kabel") != null)
        {
            ri = GameObject.Find("PF Kabel");
            ri.name = "Kabel " + id;
        }
        else if (GameObject.Find("PF Besi") != null)
        {
            ri = GameObject.Find("PF Besi");
            ri.name = "Besi " + id;
        }
        else if (GameObject.Find("PF Botol") != null)
        {
            ri = GameObject.Find("PF Botol");
            ri.name = "Botol " + id;
        }
        else
        {
            ri.name = "-";
        }
    }
}
