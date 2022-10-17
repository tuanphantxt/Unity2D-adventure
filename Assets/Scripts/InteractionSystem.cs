using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{

    public Transform detectCheckPoint; //gameobject dò
    private const float detectCheckRadius = 0.7f;//vùng check items
    public LayerMask detectLayer;// lớp item va chạm

    public GameObject detechdObject;//object tìm thấy

    [Header("Examine")]
    //examine Window
    public GameObject examineWindow;
    public Image examineImage;
    public Text examineText;
    public bool isExamine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DetectObject()) 
        { 
            if( InteractInput())
            {
                // sau khi tìm đc vật thể
                // cho đối tượng đó chạy script items hàm Interact của nó
                detechdObject.GetComponent<Item>().Interact();
            }
        }


        if(Input.GetKeyDown(KeyCode.RightArrow) || 
            (Input.GetKeyDown(KeyCode.LeftArrow))||
            (Input.GetKeyDown(KeyCode.UpArrow))|| 
            (Input.GetKeyDown(KeyCode.DownArrow)) ||
            (Input.GetKeyDown(KeyCode.W)) ||
            (Input.GetKeyDown(KeyCode.A)) ||
            (Input.GetKeyDown(KeyCode.S)) ||
            (Input.GetKeyDown(KeyCode.D)) ||
            (Input.GetKeyDown(KeyCode.Space)))
        {
            examineWindow.SetActive(false);
            isExamine = false;
        }

    }


    bool InteractInput ()
    {
        return Input.GetKeyDown(KeyCode.E);
    }


    //hàm check va chạm với 1 đối tượng
    bool DetectObject()
    {
        //gameobject dò
        //vùng check detectCheckPoint.position
        //vật thể dc check detectLayer
        Collider2D obj = Physics2D.OverlapCircle(detectCheckPoint.position, detectCheckRadius,detectLayer);
        if (obj == null)
        {
            detechdObject = null;
            return false;//nếu không tìm ra đối tượng trả về false
        }
        else
        {
            detechdObject = obj.gameObject; // detechdObject ban đầu là 1 đối tượng rỗng nếu có va chạm lấy đối tượng đó gán cho detechdObject
            return true;
        }
    }


    //hàm nhận item để vào window thông tin
    //bảng hiển thị giao tiếp với item --KHÔNG-- nhặt đc
    public void ExamineItem( Item item)//gameoject được truyền vào item
    {
        examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite; //lấy sprite từ item đc truyền vào và gán
        examineText.text = item.descriptionText;
        examineWindow.SetActive(true);//bật cửa sổ xem thông tin vật phẩm lên
        isExamine = true;
    }


    //bảng hiển thị giao tiếp với item nhặt đc
    public void ExamineItemPickup(Item item)//gameoject được truyền vào item
    {
        examineImage.sprite = item.GetComponent<SpriteRenderer>().sprite; //lấy sprite từ item đc truyền vào và gán
        examineText.text = item.name;
        examineWindow.SetActive(true);//bật cửa sổ xem thông tin vật phẩm lên
        isExamine = true;
    }




    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(detectCheckPoint.position, detectCheckRadius);
    }
}
