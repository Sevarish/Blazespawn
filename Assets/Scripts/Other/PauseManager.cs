using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool paused = false;
    private int currentGrowTime,
                growthAmountX = 1,
                growthAmountY = 1,
                pausingTimer,
                debug;
    [SerializeField] private Image AbilitySelect;
    private Image currentMenu;
    private RectTransform currentRect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        pausingTimer += 1;
        if (Input.GetKeyDown(KeyCode.Joystick1Button7) && pausingTimer > 60)
        {
            pausingTimer = 0;
            paused = !paused;
            SetState(paused);
        }

        if (currentMenu != null)
        {
            currentGrowTime += 1;
            if (currentGrowTime < 10)
            {
                    currentRect.localScale += new Vector3(growthAmountX, growthAmountY, 0);
            }
        }
    }

    public bool GetState()
    {
        return paused;
    }

    public void SetState(bool p)
    {
        paused = p;
        if (paused)
        {
            currentGrowTime = 0;
            InstantiateMenu();
            Time.timeScale = 0;
        }
        if (!paused)
        {
            if (currentMenu != null)
            {
                Destroy(currentMenu);
                currentRect = null;
            }
            Time.timeScale = 1;
        }
    }

    private void InstantiateMenu()
    {
        currentMenu = Instantiate(AbilitySelect, new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.identity);
        currentMenu.transform.SetParent(GameObject.Find("Canvas").transform);
        currentRect = currentMenu.GetComponent<RectTransform>();
    }
}
