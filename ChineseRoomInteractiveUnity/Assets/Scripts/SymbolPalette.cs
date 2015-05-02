using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SymbolPalette : MonoBehaviour 
{
    public string[] symbols;
    public Button symbol_button_prefab;
    public Button done_button_prefab;

    private HandController hand;
    private Paper write_target;


    public void Awake()
    {
        // get HandController reference
        hand = FindObjectOfType<HandController>();
        if (hand == null) Debug.LogError("HandController object not found");
    }
    public void Start()
    {
        // create symbol buttons
        for (int i = 0; i < symbols.Length; ++i)
        {
            string symbol = symbols[i];

            Button symbol_bttn = Instantiate(symbol_button_prefab);
            symbol_bttn.transform.SetParent(transform, false);
            symbol_bttn.GetComponentInChildren<Text>().text = symbol;
            symbol_bttn.onClick.AddListener(() => OnSymbolButtonClick(symbol));
        }

        // create done button
        Button done_bttn = Instantiate(done_button_prefab);
        done_bttn.transform.SetParent(transform, false);
        done_bttn.onClick.AddListener(() => OnDoneButtonClick());


        // deactivate the palette
        gameObject.SetActive(false);
    }
    public void CreatePalette(Paper write_target)
    {
        this.write_target = write_target;

        gameObject.SetActive(true);
        transform.parent.position = (Vector2)hand.transform.position + Vector2.up * 2.5f;
    }

    private void OnSymbolButtonClick(string symbol)
    {
        write_target.AddText(symbol);
    }
    private void OnDoneButtonClick()
    {
        gameObject.SetActive(false);
    }
}
