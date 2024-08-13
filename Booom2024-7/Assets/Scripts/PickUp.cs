using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    private bool a = true;
    float tempTime = 0;
    private void Start(){
        //inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        a = true;
    }

    private void Update(){
        var mouse = Mouse.current;
        if(mouse == null){
            return;
        }
        
        // Debug.Log(a);
        if(mouse.leftButton.wasPressedThisFrame){
            // tempTime+= Time.deltaTime;
            // if(tempTime>0.5f){
            //     tempTime=0f;
                    var onScreenPosition  = mouse.position.ReadValue();
                // var ray = Camera.main.ScreenPointToRay(onScreenPosition);

                // var hit = Physics2D.Raycast(new Vector2(ray.origin.x,ray.origin.y),Vector2.zero,Mathf.Infinity);
                // RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                // if(hit.collider!=null){
                //     Debug.Log("Hit!");
                //     a=false;
                //     AddItem();
                //     //hit.collider.gameObject.transform.position = ray.origin;
                // }
                Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                // Debug.Log(this.name);
                if(col.Length > 0)
                {
                    foreach(Collider2D c in col)
                    {
                        if(c.name=="Fire"){
                            if(Inventory.getInstance().isChecked!=true || Inventory.getInstance().checkedItem!=21){
                                return;
                            }
                        }
                        int id = ItemsInfo.getInstance().getId(c.name);
                        PickedItems.getInstance().pickedItems.Add(id);
                        int num = PlayerPrefs.GetInt("PickedItemNum");
                        PlayerPrefs.SetInt("picked"+(num+1).ToString(),id);
                        Debug.Log("cunchu:"+num+1+":"+PlayerPrefs.GetInt("picked"+(num+1).ToString()));
                        PlayerPrefs.SetInt("PickedItemNum",num+1);
                        Inventory.getInstance().ItemUpdate();

                        c.enabled=false;
                        c.GetComponent<SpriteRenderer>().enabled = false;
                        
                    }

                }
            // }
            
                    


        }
    }

    void AddItem(){
        int id = ItemsInfo.getInstance().getId(this.name);
        Debug.Log(this.name);
        PickedItems.getInstance().pickedItems.Add(id);
        int num = PlayerPrefs.GetInt("PickedItemNum");
        
        Inventory.getInstance().ItemUpdate();
        
        this.GetComponent<SpriteRenderer>().enabled = false;

        //物品已经拾取
    }
}
