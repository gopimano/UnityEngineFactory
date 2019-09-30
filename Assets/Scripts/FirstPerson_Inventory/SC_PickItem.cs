using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PickItem : MonoBehaviour
{
    public string itemName = "Unknown Item"; //Each item must have an unique name
    public Texture itemPreview;

    // Start is called before the first frame update
    void Start()
    {
        //Change item tag to Respawn to detect when we look at it
        gameObject.tag = "Respawn";
    }

    public void PickItem()
    {
        Destroy(gameObject);
    }
}
