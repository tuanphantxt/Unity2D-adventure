using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("HP")]
    public Image fillHPBar;
    public float healht;
    public float fullHelth = 100f;
    public Text healthText;
 

    [Header("Mana")]
    public Image fillMNBar;
    public float Mana;
    public float fullMana = 100f;
    public Text ManaText;

    [Header("die Panel")]
    public GameObject DiePanel;
    public bool isDiepanel;



    [Header("SKill")]
    public Image fillKame;
    public Image fillMelle;
    public Image fillDash;

    private void Start()
    {
        healht = fullHelth;
        Mana = fullMana;
    }

    private void Update()
    {
        controlHPMana();
        FillBar();
    }



    void FillBar()
    {
        fillHPBar.fillAmount = healht / fullHelth;
        healthText.text = healht + "/" + fullHelth;

        fillMNBar.fillAmount = Mana / fullMana;
        ManaText.text = Mana + "/" + fullMana;


        fillKame.fillAmount = playerController.instance.KameTime / (playerController.instance.KameTimeLife);
        if (fillKame.fillAmount == 1)
        {
            fillKame.fillAmount = 0;
        }
        fillMelle.fillAmount = playerController.instance.MelleTime / (playerController.instance.MelleTimeLife);
        if (fillMelle.fillAmount == 1)
        {
            fillMelle.fillAmount = 0;
        }
        fillDash.fillAmount = playerController.instance.DashTime / (playerController.instance.dashingTime);
        if (fillDash.fillAmount == 1)
        {
            fillDash.fillAmount = 0;
        }
    }
    void controlHPMana()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            loseHeadth(20);
        }
        if (Input.GetKeyDown(KeyCode.O) && healht <= 100)
        {
            buffHeadth(20);
        }

        if (healht > fullHelth)
        {
            healht = fullHelth;
        }
        if (healht < 1)
        {
            healht = 0;
        }

        //////mana
        if (Input.GetKeyDown(KeyCode.I))
        {
            loseMana(19);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            buffMana(20);
        }
        if (Mana > fullMana)
        {
            Mana = fullMana;
        }
        if (Mana < 0)
        {
            Mana = 0;
        }
    }

    public void loseHeadth(int value)
    {
        if (healht < 0) return;
        healht -= value;
        fillHPBar.fillAmount = healht / fullHelth;
        if (healht <= 0)
        {
            FindObjectOfType<playerController>().Die();
        }

    }

    public void buffHeadth(int value)
    {
        if(healht >= 1)
        {
            if (healht > fullHelth) return;
            healht += value;
            //fillHPBar.fillAmount = healht / fullHelth;
            if (healht >= 100)
            {
                Debug.Log("đầy rồi");
            }
        }
    }

    public void loseMana(int value)
    {
        //if (Mana < 0) return;
        Mana -= value;
       // fillMNBar.fillAmount = Mana / fullMana;
    }

    public void buffMana(int value)
    {
        //if (Mana > 100) return;
        Mana += value;
    }
}
