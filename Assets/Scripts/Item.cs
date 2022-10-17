using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PolygonCollider2D))]
public class Item : MonoBehaviour
{

    public enum InteractionType { None, pickup, examine, key}
    public enum ItemType { Static, consumable }
   

    [Header("attributes")]
    public InteractionType InteractType;
    public ItemType Type;
    

    [Header("Examine")]
    public string descriptionText;//văn bản mô tả
    public Sprite image;//hình ảnh item

    [Header("CustomEvent")]
    public UnityEvent customEvent; //event
    public UnityEvent consumeEvent;
    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true; //set trigget thành true khi gắn script vào object
    }




    public void Interact()
    {
        switch(InteractType)
        {
            case InteractionType.pickup://pick vật phẩm trong biến enum
                FindObjectOfType<Inventory>().pickup(gameObject);//gọi hàm pickup và truyền chính gameobject này cho nó
                FindObjectOfType<InteractionSystem>().ExamineItemPickup(this);//truyền chính thẻ này vào hàm ,gọi hàm hiển thị ảnh và text
                gameObject.SetActive(false);
                Debug.Log("Nhặt được " +gameObject);
                break;

            case InteractionType.examine:
                //hiển thị cửa sổ kiểm tra vật phẩm
                //hiển thị hình ảnh
                //hiển thị văn bản 
                FindObjectOfType<InteractionSystem>().ExamineItem(this);//truyền chính thẻ này vào hàm ,gọi hàm hiển thị ảnh và text
                Debug.Log("Check : " +gameObject);
                break;
            case InteractionType.key:
                //hiển thị cửa sổ kiểm tra vật phẩm
                //hiển thị hình ảnh
                //hiển thị văn bản 
                FindObjectOfType<InteractionSystem>().ExamineItem(this);//truyền chính thẻ này vào hàm ,gọi hàm hiển thị ảnh và text
                gameObject.SetActive(false);
                Debug.Log("Check : " + gameObject);
                break;

            default:
                Debug.Log("null item");
                break;
        }
        //gọi event hành động
        customEvent.Invoke();
    }
}
