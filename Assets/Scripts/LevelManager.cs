using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{

   public Vector2 playerInitPosition;

    private void Start()
    {
        playerInitPosition = FindObjectOfType<playerController>().transform.position;
    }
    public void Restart()
    {
        // 1 restart scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // đặt lại vị trí player
        //khi người chơi hồi sinh đặt người chơi ở vị trí đó
        FindObjectOfType<playerController>().transform.position = playerInitPosition;
        FindObjectOfType<HealthBar>().DiePanel.SetActive(false);//tắt panel die khi hồi sinh
    }
}
