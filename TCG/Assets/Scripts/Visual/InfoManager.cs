using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public Text MessageText;
    public Text TipText;
    public GameObject MessagePanel;
    public GameObject TipPanel;

    public static InfoManager Instance;


    void Awake()
    {
        Instance = this;
        MessagePanel.SetActive(false);
        TipPanel.SetActive(false);
    }

    private IEnumerator ShowMessageCoroutine(string Message, float Duration)
    {
        MessageText.text = Message;
        MessagePanel.SetActive(true);

        yield return new WaitForSeconds(Duration);

        MessagePanel.SetActive(false);
    }

    public void ShowMessage(string Message, float Duration)
    {
        StartCoroutine(ShowMessageCoroutine(Message, Duration));
    }

    public void ShowTip(string Tip)
    {
        TipText.text = Tip;
        TipPanel.SetActive(true);
    }

    public void HideTip()
    {
        TipPanel.SetActive(false);
    }
}
