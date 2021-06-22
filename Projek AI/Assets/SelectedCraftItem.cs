using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedCraftItem : MonoBehaviour
{
    string[] items = new string[] { "Burning Cloth", "Bandage", "Decoy Bottle" };
    string[] desc = new string[] { "Placed to brighten surroundings for 30 seconds.", "Heals you. Straightforward.", "Thrown to distract nearby enemies for 10 seconds." };
    public int selected = 0;
    public GameObject[] options;
    public Image[] itemImages;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private GameObject findChild(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}
