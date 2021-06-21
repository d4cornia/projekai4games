using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{
    
    public class SimpleScript : MonoBehaviour
    {
        void Update()
        {
            //m_textMeshPro.SetText(label, m_frame % 1000);
            //m_frame += 1 * Time.deltaTime;
            this.transform.LookAt(GameObject.Find("PF Player").transform);
        }

    }
}
