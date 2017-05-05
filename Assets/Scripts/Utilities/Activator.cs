using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Activator : MonoBehaviour
{
    // Objects to activate
    public List<GameObject> ActivateObjects;
    
    public bool ActivateChildren;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (ActivateObjects.Count > 0)
            {
                foreach (var currentObject in ActivateObjects)
                {
                    currentObject.SetActive(true);
                }
            }

            if (ActivateChildren)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        
    }
}
