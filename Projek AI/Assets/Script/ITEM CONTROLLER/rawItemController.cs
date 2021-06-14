using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rawItemController : MonoBehaviour
{
    public GameObject ri;
    public static int id;


    // Start is called before the first frame update
    void Awake()
    {
        id++;
        ri.name = ri.name + id;
        /*if (GameObject.Find("PF Burning Cloth G") != null)
        {
            ri = GameObject.Find("PF Burning Cloth G");
            ri.name = "Burning Cloth G" + id;
        }
        else if (GameObject.Find("PF Decoy Bottle G") != null)
        {
            ri = GameObject.Find("PF Decoy Bottle G");
            ri.name = "Decoy Bottle G" + id;
        }
        else if (GameObject.Find("PF Bandage G") != null)
        {
            ri = GameObject.Find("PF Bandage G");
            ri.name = "Bandage G" + id;
        }
        else if (GameObject.Find("PF Batrei") != null)
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
        }*/
    }
}
