using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Fields")]
    public List<GameObject> items = new List<GameObject>();// list items pickup

     public bool isOpen;

    [Header("UI items section")]
    public GameObject UI_Window;//cửa sổ Inventory
    public Image[] items_images;

    [Header(" items description")]
    public GameObject UI_descriptionWindow;
    public Image descriptionImage;
    public Text descriptionName;
    public Text descriptionTitle;
    public GameObject ButtonMenu;


    private void Start()
    {
        HideAll();
    }
    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.B))
        {
            ToggleInventory();
        }
    }


    // bật tắt túi đồ
    public void ToggleInventory()
    {
        isOpen = !isOpen; //lật
        UI_Window.SetActive(isOpen);
        ButtonMenu.SetActive(!isOpen);//khi bật túi đồ thì tắt nút menu
        Update_UI();
    }


    public void pickup(GameObject item)
    {
       items.Add(item); //pickup khai báo  gameobject item sẽ nhận thêm items và truyền vào list(kho)
        Update_UI();
    }


    //làm mới các phần tử UI trong cửa sổ kiểm tra
    //hiển thị nó với vị trí tương ứng trong list items
    //xếp vị trí cho vật phẩm
    void Update_UI()
    {
        HideAll();
        for (int i = 0 ; i < items.Count; i++)
        {
            // items_images[i] vị trí hình ảnh đc khai báo trống đc gán ảnh từ của vật phẩm thứ tự list items list items
            items_images[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
            items_images[i].gameObject.SetActive(true);
        }
       
    }

    //ẩn các mục hình ảnh có sẵn nhưng chưa đcn nhận item
    void HideAll()
    {
        foreach(var i in  items_images)     
        {
            i.gameObject.SetActive(false);
        }
        HideDescription();//Đảm bảo rằng khi tắt túi đồ đột ngột thì sẽ tắt luôn description
    }

    //hiển thị hình ảnh và thông tin vật phẩm mỗi khiđưa chuột vào
    public  void ShowDescription( int id)  //khi đc gọi nhận một số id tương ứng với vị trí trong list item hiển thị hình ảnh và title vật phẩm đó
    {
        descriptionImage.sprite = items_images[id].sprite;
        descriptionName.text = items[id].name;//gọi text name của object của id
        descriptionTitle.text = items[id].GetComponent<Item>().descriptionText;//gọi component của của id
        descriptionImage.gameObject.SetActive(true);
        descriptionName.gameObject.SetActive(true);
        descriptionTitle.gameObject.SetActive(true);

    }

    //ẩn hình ảnh và thông tin vật phẩm bên mô tả sản phẩm 
    //dùng khi đưa chuột ra khỏi vật phẩm trong kho
    public void HideDescription()
    {
        descriptionImage.gameObject.SetActive(false);
        descriptionName.gameObject.SetActive(false);
        descriptionTitle.gameObject.SetActive(false);
    }


    //hàm sử dụng vật phẩm
    //sau khi dùng xóa ảnh và xóa vật phẩm khỏi list items
     public void consume(int id)
    {
       // if (GetComponent<HealthBar>().healht < 100 )//đầy hp k dùng đc đùi gà
        
            if (items[id].GetComponent<Item>().Type == Item.ItemType.consumable)
            {
                Debug.Log($"consume + {items[id].name}");
                //gọi sư kiện
                items[id].GetComponent<Item>().consumeEvent.Invoke();
                //delete item in time
                Destroy(items[id], 0.1f);
                //xóa item khỏi list
                items.RemoveAt(id);
                //cập nhật lại UI
                Update_UI();
            }
        
       // Debug.Log("đã đầy hp");
     }
}
