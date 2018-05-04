using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public Text MessageText;
    public GameObject MessagePanel;

    public static MessageManager Instance;

    void Awake()
    {
        Instance = this;
        MessagePanel.SetActive(false);
    }

    private IEnumerator ShowMessageCoroutine(string Message, float Duration, Command command) 
    {
        MessageText.text = Message;
        MessagePanel.SetActive(true);

        yield return new WaitForSeconds(Duration);

        MessagePanel.SetActive(false);
        Command.CommandExecutionComplete();
    }

    public void ShowMessage(string Message, float Duration, Command command) 
    {
        StartCoroutine(ShowMessageCoroutine(Message, Duration, command)); 
    }

}
