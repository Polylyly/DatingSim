using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Keybinds : MonoBehaviour
{
    private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public TextMeshProUGUI jumpText, pauseText, barrageText, blockText, heavyText, healText, parryText;
    private GameObject currentKey;
    Player playerScript;
    public GameObject keybindCanvas, settingsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Jump Key")) PlayerPrefs.SetString("Jump Key", "Space");
        if (!PlayerPrefs.HasKey("Pause Key")) PlayerPrefs.SetString("Pause Key", "Escape");
        if (!PlayerPrefs.HasKey("Barrage Key")) PlayerPrefs.SetString("Barrage Key", "E");
        if (!PlayerPrefs.HasKey("Block Key")) PlayerPrefs.SetString("Block Key", "F");
        if (!PlayerPrefs.HasKey("Heavy Key")) PlayerPrefs.SetString("Heavy Key", "R");
        if (!PlayerPrefs.HasKey("Heal Key")) PlayerPrefs.SetString("Heal Key", "H");
        if (!PlayerPrefs.HasKey("Parry Key")) PlayerPrefs.SetString("Parry Key", "G");

        keys.Add("Jump", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump Key")));
        keys.Add("Pause", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause Key")));
        keys.Add("Barrage", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Barrage Key")));
        keys.Add("Block", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Block Key")));
        keys.Add("Heavy", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Heavy Key")));
        keys.Add("Heal", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Heal Key")));
        keys.Add("Parry", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Parry Key")));

        jumpText.SetText(keys["Jump"].ToString());
        pauseText.SetText(keys["Pause"].ToString());
        barrageText.SetText(keys["Barrage"].ToString());
        blockText.SetText(keys["Block"].ToString());
        heavyText.SetText(keys["Heavy"].ToString());
        healText.SetText(keys["Heal"].ToString());
        parryText.SetText(keys["Parry"].ToString());
    }

    public void OpenPage()
    {
        keys["Jump"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump Key"));
        keys["Pause"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Pause Key"));
        keys["Barrage"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Barrage Key"));
        keys["Block"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Block Key"));
        keys["Heavy"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Heavy Key"));
        keys["Heal"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Heal Key"));
        keys["Parry"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Parry Key"));

        jumpText.SetText(keys["Jump"].ToString());
        pauseText.SetText(keys["Pause"].ToString());
        barrageText.SetText(keys["Barrage"].ToString());
        blockText.SetText(keys["Block"].ToString());
        heavyText.SetText(keys["Heavy"].ToString());
        healText.SetText(keys["Heal"].ToString());
        parryText.SetText(keys["Parry"].ToString());
    }

    // Update is called once per frame
    public void Back()
    {
        keys["Jump"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), jumpText.GetParsedText());
        keys["Pause"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), pauseText.GetParsedText());
        keys["Barrage"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), barrageText.GetParsedText());
        keys["Block"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), blockText.GetParsedText());
        keys["Heavy"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), heavyText.GetParsedText());
        keys["Heal"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), healText.GetParsedText());
        keys["Parry"] = (KeyCode)System.Enum.Parse(typeof(KeyCode), parryText.GetParsedText());

        PlayerPrefs.SetString("Jump Key", keys["Jump"].ToString());
        PlayerPrefs.SetString("Pause Key", keys["Pause"].ToString());
        PlayerPrefs.SetString("Barrage Key", keys["Barrage"].ToString());
        PlayerPrefs.SetString("Block Key", keys["Block"].ToString());
        PlayerPrefs.SetString("Heavy Key", keys["Heavy"].ToString());
        PlayerPrefs.SetString("Heal Key", keys["Heal"].ToString());
        PlayerPrefs.SetString("Parry Key", keys["Parry"].ToString());

        settingsCanvas.SetActive(true);
        keybindCanvas.SetActive(false);
    }

    private void OnGUI()
    {
        if (currentKey != null)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                keys[currentKey.name] = e.keyCode;
                currentKey.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(e.keyCode.ToString());
                currentKey = null;
            }
        }
    }
    public void ChangeKey(GameObject clicked)
    {
        currentKey = clicked;
    }
}
