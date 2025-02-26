using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// pickup脚本用于点击拾取。在当前场景下挂载该脚本后，可以通过点击对应物品存放进入背包
// 拾取逻辑为：根据物品在场景中的名称，在ItemInfo文件中获取物品id来存储
// 使用方法：将需要进行点击拾取的物品按照文件命名，并在场景中挂载本脚本
// 文件中没有的物体点击后返回id为0，不会被拾取
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
        //左键点击
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
                //获取点击到的碰撞体
                Collider2D[] col = GetComponentsInChildren<Collider2D>();
                // Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                // Debug.Log(this.name);
                if(col.Length > 0)
                {
                    // Debug.Log("aa"+onScreenPosition);
                    foreach(Collider2D c in col)
                    {
                        // Debug.Log(c.name);
                        // Debug.Log(c.transform.position);
                        Vector3 mousePosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        mousePosition.z=0;
                        if(c.OverlapPoint(mousePosition)){
                            //这里编写需要特殊拾取的物品的代码
                            //当选中道具栏中的火柴时，才能正确拾取怪火
                            if(c.name=="Fire"){
                                if(Inventory.getInstance().isChecked!=true || Inventory.getInstance().checkedItem!=21){
                                    continue;
                                }
                            }
                            // 点击箱子
                            // if(c.name=="box0"){
                                
                            // }
                            int id = ItemsInfo.getInstance().getId(c.name);
                            if(id!=0){
                                PickedItems.getInstance().pickedItems.Add(id);
                                // Debug.Log(id);
                                int num = PlayerPrefs.GetInt("PickedItemNum");
                                PlayerPrefs.SetInt("picked"+(num+1).ToString(),id);
                                Debug.Log("cunchu:"+num+1+":"+PlayerPrefs.GetInt("picked"+(num+1).ToString()));
                                PlayerPrefs.SetInt("PickedItemNum",num+1);
                                Inventory.getInstance().ItemUpdate();
                            }
                            
                            EventHandler.CallUpdateUIEvent(c.name,0);
                            // Debug.Log("event:"+ItemsInfo.getInstance().getItemName(c.name));
                            c.enabled=false;
                            c.GetComponent<SpriteRenderer>().enabled = false;
                            
                            // Debug.Log(c.name+mousePosition+"    "+c.transform.position);
                        }
                        
                        
                        
                        
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
