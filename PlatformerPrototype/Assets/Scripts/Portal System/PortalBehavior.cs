using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{

    private HashSet<GameObject> portalAstonauts = new HashSet<GameObject>();  //Stores the Objects traversing through the portal

    [SerializeField]
    private Transform dest;

    //An object triggers the portal
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if an object was already in portal (in portal hashset) do not traverse it again
        if (portalAstonauts.Contains(collision.gameObject)) 
        {
            return;
        }

        if (dest.TryGetComponent(out PortalBehavior endOfJourney)) 
        {
            endOfJourney.portalAstonauts.Add(collision.gameObject); //Add the the hash set of the destination portal the traveller game object 
        }

        collision.transform.position = dest.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        portalAstonauts.Remove(collision.gameObject); //Astonaut has left the starting portal
    }

}
