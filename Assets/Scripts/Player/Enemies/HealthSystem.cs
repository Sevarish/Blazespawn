using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{   [SerializeField] private float maxHealth = 100, health;
    [SerializeField] private bool atFullHealth = true;
    [SerializeField] Slider healthBarSlider;
    float transparency = 0,
          holdTimer = 0;
    Image backgroundColor, //RED
          foregroundColor; //GREEN
    Slider healthBar;
    Camera cam;
    TargetSystem tgSystem;
    [SerializeField] GameObject damageText;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        healthBar = Instantiate(healthBarSlider, transform.position, Quaternion.identity);
        healthBar.transform.SetParent(GameObject.Find("Canvas").gameObject.transform);
        tgSystem = GameObject.FindGameObjectWithTag("PlayerChar").GetComponent<TargetSystem>();
        backgroundColor = healthBar.transform.GetChild(0).GetComponent<Image>();
        foregroundColor = healthBar.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();

        if (atFullHealth)
        {
            health = maxHealth;
        }
        healthBar.value = CalculatePercentageHealth();
    }

    void Update()
    {
        if (IsInCameraView() && tgSystem.GetTarget() == this.gameObject)
        {
            FollowUI();
            if (transparency < 250) {
                transparency += 50;
            }
            holdTimer = 0;
        }
        else
        {
            holdTimer += Time.deltaTime;
            FollowUI();
            if (holdTimer > 0.5f)
            {
                if (transparency > 0)
                {
                    transparency -= 10;
                }
            }
        }

        backgroundColor.color =  new Color32(255, 21, 0, (byte)transparency);
        foregroundColor.color = new Color32(20, 255, 0, (byte)transparency);
    }

    //Is called from other scripts that intend to deal damage to the script's holder.
    public void TakeDamage(int damage, string typeDmg = "Normal")
    {
        health -= damage;
        healthBar.value = CalculatePercentageHealth();
        transparency = 200;
        holdTimer = 0;

        var dmgObjectText = Instantiate(damageText, cam.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z)), Quaternion.identity);
        dmgObjectText.transform.SetParent(GameObject.Find("Canvas").transform);
        Text TextComponent = dmgObjectText.GetComponent<Text>();
        dmgObjectText.GetComponent<DamageTextBehaviour>().Target = this.gameObject;
        TextComponent.text = "" + damage;
        TextComponent.color = TypeColor(typeDmg);
        Destroy(dmgObjectText, 0.5f);

        if (health < 1)
        {
            Destroy(healthBar.gameObject);
            Destroy(this.gameObject);
            return;
        }
    }

    public Color32 TypeColor(string type)
    {
        Color32 color = new Color32(255, 255, 255, 255);
        switch (type)
        {
            case "Fire":
                color = new Color32(240, 140, 10, 255);
                break;
            case "Ice":
                color = new Color32(7, 242, 242, 255);
                break;
            case "Nature":
                color = new Color32(10, 207, 27, 255);
                break;
            case "Psychic":
                color = new Color32(145, 13, 222, 255);
                break;
            case "Dark":
                color = new Color32(69, 51, 79, 255);
                break;
            case "Poison":
                color = new Color32(13, 255, 0, 255);
                break;
            case "Normal":
                color = new Color32(255, 255, 255, 255);
                break;
        }
        return color;
    }

    private float CalculatePercentageHealth()
    {
        float resultHealth = health / maxHealth;
        return resultHealth;
    }

    //Makes the healthbar follow the Camera.
    private void FollowUI()
    {
        //float capHeight = (Screen.height / 100) * 80;
        //Vector3 healthBarToWorldPos = cam.ScreenToWorldPoint(healthBar.transform.position);
        //Vector3 healthBarPos = cam.WorldToScreenPoint(healthBarToWorldPos);
        //Debug.Log(healthBarPos.y);
        //if (healthBarToWorldPos.y > capHeight)
        //{
        //    healthBar.transform.position = cam.WorldToScreenPoint(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z));
        //}
        //else
        //{ 
        //}
        if (IsInCameraView())
        {
            healthBar.transform.position = cam.WorldToScreenPoint(new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + (this.gameObject.transform.localScale.y / 2 + 1), this.gameObject.transform.position.z));
        }
    }

    //Checks if healthbar is in view.
    private bool IsInCameraView()
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(this.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }
}
