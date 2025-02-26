using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PackCell : MonoBehaviour
{
    public UnityEngine.UI.RawImage _cellRawImage;
    
    public int _goodsId=0;
    public bool isFull;
    public bool isChecked;

    public int getGoodsId(){
        return _goodsId;
    }

    public void selectCell(){
        if(!isFull){
            return;
        }
        if(isFull){
            string id=this.name;
            id=id[6].ToString();
            if(isChecked){
                Debug.Log("check"+id);
                isChecked=false;
                Inventory.getInstance().CancelCheck(int.Parse(id));
            }else{
                Debug.Log("not check"+id);
                isChecked=true;
                Inventory.getInstance().CheckCell(int.Parse(id));
            }
            // gameObject.GetComponent<UnityEngine.UI.Image>().color =Color.black;
            
            // Debug.Log("idddd"+id);
        }
    }

    public void SelectImage(bool c){
        if(c){
            gameObject.GetComponent<UnityEngine.UI.Image>().color =Color.black;
        }else{
            gameObject.GetComponent<UnityEngine.UI.Image>().color =Color.white;
        }
    }

   

    public void setGoodsId(int id){
        _goodsId = id;

    }

    public void updateImage(){
         if(_goodsId==0){
            isFull=false;
            _cellRawImage.texture = Resources.Load<Texture>("Art/Sprites/Items/tou");
        }else{
            isFull=true;
            _cellRawImage.texture = Resources.Load<Texture>(ItemsInfo.getInstance().getSpritePath(_goodsId));
        }

    }

    void Awake(){
        // Debug.Log(ItemsInfo.getInstance().getSpritePath(2));
        isFull=false;
        isChecked = false;
        _cellRawImage = transform.Find("RawImage").GetComponent<UnityEngine.UI.RawImage>();
        // _cellRawImage.texture = Resources.Load<Texture>(ItemsInfo.getInstance().getSpritePath(2));
        
    }
    void Start(){
        //Debug.Log(ItemsInfo.getInstance().getSpritePath(2));
        if(_goodsId==0){
            isFull=false;
            _cellRawImage.texture = Resources.Load<Texture>("Art/Sprites/Items/tou");
        }else{
            isFull=true;
            _cellRawImage.texture = Resources.Load<Texture>(ItemsInfo.getInstance().getSpritePath(_goodsId));
        }
        
    }
}
