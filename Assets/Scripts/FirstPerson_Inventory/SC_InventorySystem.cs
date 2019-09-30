using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_InventorySystem : MonoBehaviour
{

    public Texture crosshairTexture;
    public SC_CharacterController playerController;
    public SC_PickItem[] availableItems; //List with Prefabs of all the available items

    //Available items slots
    int[] itemSlots = new int[12];
    bool showInventory = false;
    float windowAnimation = 1;
    float animationTimer = 0;

    //UI Drag & Drop
    int hoveringOverIndex = -1;
    int itemIndexToDrag = -1;
    Vector2 dragOffset = Vector2.zero;

    //Item Pick up
    //SC_PickItem detectedItem;
    //int detectedItemIndex;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        //Initialize Item Slots
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i] = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Show/Hide inventory
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showInventory = !showInventory;
            animationTimer = 0;

            //if (showInventory)
            //{
            //    Cursor.visible = true;
            //    Cursor.lockState = CursorLockMode.None;
            //}
            //else
            //{
            //    //Cursor.visible = false;
            //    //Cursor.lockState = CursorLockMode.Locked;
            //}
        }

        if (animationTimer < 1)
        {
            animationTimer += Time.deltaTime;
        }

        if (showInventory)
        {
            windowAnimation = Mathf.Lerp(windowAnimation, 0, animationTimer);
            playerController.canMove = false;

            PopulateCatalog();
        }
        else
        {
            windowAnimation = Mathf.Lerp(windowAnimation, 1f, animationTimer);
            playerController.canMove = true;
        }

        //Begin item drag
        if (Input.GetMouseButtonDown(0) && hoveringOverIndex > -1 && itemSlots[hoveringOverIndex] > -1)
        {
            Debug.Log("Mouse Down: " + hoveringOverIndex);
            itemIndexToDrag = hoveringOverIndex;
        }

        //Release dragged item
        if (Input.GetMouseButtonUp(0) && itemIndexToDrag > -1)
        {
            Debug.Log("Mouse up: ");
            if (hoveringOverIndex < 0)
            {

                Debug.Log("Item index: " + itemSlots[itemIndexToDrag]);
                for (int i = 0; i < availableItems.Length; i++)
                {
                    Debug.Log("available items :  " + availableItems[i].itemName);
                }
                //Drop the item outside
                Instantiate(availableItems[itemSlots[itemIndexToDrag]], playerController.playerCamera.transform.position + (playerController.playerCamera.transform.forward*6), Quaternion.identity);
                //itemSlots[itemIndexToDrag] = -1;
            }
            else
            {
                //Switch items between the selected slot and the one we are hovering on
                int itemIndexTmp = itemSlots[itemIndexToDrag];
                itemSlots[itemIndexToDrag] = itemSlots[hoveringOverIndex];
                itemSlots[hoveringOverIndex] = itemIndexTmp;

            }
            itemIndexToDrag = -1;
        }

        ////Item pick up
        //if (detectedItem && detectedItemIndex > -1)
        //{
        //    if (Input.GetKeyDown(KeyCode.F))
        //    {
        //        //Add the item to inventory
        //        int slotToAddTo = -1;
        //        for (int i = 0; i < itemSlots.Length; i++)
        //        {
        //            if (itemSlots[i] == -1)
        //            {
        //                slotToAddTo = i;
        //                break;
        //            }
        //        }
        //        if (slotToAddTo > -1)
        //        {
        //            itemSlots[slotToAddTo] = detectedItemIndex;
        //            //detectedItem.PickItem();
        //        }
        //    }
        //}
    }

    void FixedUpdate()
    {
        ////Detect if the Player is looking at any item
        //RaycastHit hit;
        //Ray ray = playerController.playerCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        //if (Physics.Raycast(ray, out hit, 2.5f))
        //{
        //    Transform objectHit = hit.transform;

        //    if (objectHit.CompareTag("Respawn"))
        //    {
        //        if ((detectedItem == null || detectedItem.transform != objectHit) && objectHit.GetComponent<SC_PickItem>() != null)
        //        {
        //            SC_PickItem itemTmp = objectHit.GetComponent<SC_PickItem>();

        //            //Check if item is in availableItemsList
        //            for (int i = 0; i < availableItems.Length; i++)
        //            {
        //                if (availableItems[i].itemName == itemTmp.itemName)
        //                {
        //                    detectedItem = itemTmp;
        //                    detectedItemIndex = i;
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        detectedItem = null;
        //    }
        //}
        //else
        //{
        //    detectedItem = null;
        //}
    }

    void PopulateCatalog()
    {
        for (int i = 0; i < availableItems.Length; i++)
        {
            for (int j = 0; j < itemSlots.Length; j++)
            {
                if (itemSlots[j] == i)
                    break;
                if (itemSlots[j] == -1)
                {
                    itemSlots[j] = i;
                    break;
                }
            }
        }
    }
    void OnGUI()
    {
        //Inventory UI
        GUI.Label(new Rect(5, 5, 200, 25), "Press 'Tab' to open Catalog");

        //Inventory window
        if (windowAnimation < 1)
        {
            GUILayout.BeginArea(new Rect(10 - (430 * windowAnimation), Screen.height / 2 - 200, 302, 430), GUI.skin.GetStyle("box"));

            GUILayout.Label("Catalog", GUILayout.Height(25));

            GUILayout.BeginVertical();
            for (int i = 0; i < itemSlots.Length; i += 3)
            {
                GUILayout.BeginHorizontal();
                //Display 3 items in a row
                for (int a = 0; a < 3; a++)
                {
                    if (i + a < itemSlots.Length)
                    {
                        if (itemIndexToDrag == i + a || (itemIndexToDrag > -1 && hoveringOverIndex == i + a))
                        {
                            GUI.enabled = false;
                        }

                        if (itemSlots[i + a] > -1)
                        {
                            if (availableItems[itemSlots[i + a]].itemPreview)
                            {
                                GUILayout.Box(availableItems[itemSlots[i + a]].itemPreview, GUILayout.Width(95), GUILayout.Height(95));
                            }
                            else
                            {
                                GUILayout.Box(availableItems[itemSlots[i + a]].itemName, GUILayout.Width(95), GUILayout.Height(95));
                            }
                        }
                        else
                        {
                            //Empty slot
                            GUILayout.Box("", GUILayout.Width(95), GUILayout.Height(95));
                        }

                        //Detect if the mouse cursor is hovering over item
                        Rect lastRect = GUILayoutUtility.GetLastRect();
                        Vector2 eventMousePositon = Event.current.mousePosition;
                        if (Event.current.type == EventType.Repaint && lastRect.Contains(eventMousePositon))
                        {
                            hoveringOverIndex = i + a;
                            if (itemIndexToDrag < 0)
                            {
                                dragOffset = new Vector2(lastRect.x - eventMousePositon.x, lastRect.y - eventMousePositon.y);
                            }
                        }

                        GUI.enabled = true;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            if (Event.current.type == EventType.Repaint && !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                hoveringOverIndex = -1;
            }

            GUILayout.EndArea();
        }

        //Item dragging
        if (itemIndexToDrag > -1)
        {
            if (availableItems[itemSlots[itemIndexToDrag]].itemPreview)
            {
                GUI.Box(new Rect(Input.mousePosition.x + dragOffset.x, Screen.height - Input.mousePosition.y + dragOffset.y, 95, 95), availableItems[itemSlots[itemIndexToDrag]].itemPreview);
            }
            else
            {
                GUI.Box(new Rect(Input.mousePosition.x + dragOffset.x, Screen.height - Input.mousePosition.y + dragOffset.y, 95, 95), availableItems[itemSlots[itemIndexToDrag]].itemName);
            }
        }

        //Display item name when hovering over it
        if (hoveringOverIndex > -1 && itemSlots[hoveringOverIndex] > -1 && itemIndexToDrag < 0)
        {
            GUI.Box(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y - 30, 100, 25), availableItems[itemSlots[hoveringOverIndex]].itemName);
        }

        if (!showInventory)
        {
            //Player crosshair
            GUI.color = Color.white;//detectedItem ? Color.green : Color.white;
            GUI.DrawTexture(new Rect(Screen.width / 2 - 4, Screen.height / 2 - 4, 8, 8), crosshairTexture);
            GUI.color = Color.white;

            ////Pick up message
            //if (detectedItem)
            //{
            //    GUI.color = new Color(0, 0, 0, 0.84f);
            //    GUI.Label(new Rect(Screen.width / 2 - 75 + 1, Screen.height / 2 - 50 + 1, 150, 20), "Press 'F' to pick '" + detectedItem.itemName + "'");
            //    GUI.color = Color.green;
            //    GUI.Label(new Rect(Screen.width / 2 - 75, Screen.height / 2 - 50, 150, 20), "Press 'F' to pick '" + detectedItem.itemName + "'");
            //}
        }
    }
}
