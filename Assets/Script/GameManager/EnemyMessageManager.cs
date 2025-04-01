using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMessageManager : Singleton<EnemyMessageManager>
{
    [Header("ref")]
    public RectTransform statusMessage;
    [SerializeField] Image enemyImage;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI msText;

    private Queue<EnemyLibraryData> messageQueue = new Queue<EnemyLibraryData>();
    private bool isDisplayingMessage = false;
    private float displayTimer;

    private void Awake()
    {
        //statusMessage.GetComponent<ButtonUI>().ClickFunc = () => HideStatusMessage();
    }

    public void ShowStatusMessage(EnemyLibraryData data)
    {
        messageQueue.Enqueue(data); // Add the message to the queue

        // If no message is currently being displayed, display this one
        if (!isDisplayingMessage)
        {
            DisplayNextMessage();
        }
    }

    private void DisplayNextMessage()
    {
        if (messageQueue.Count > 0)
        {
            var nextMessage = messageQueue.Dequeue(); // Get the next message
            DisplayMessage(nextMessage);
        }
        else
        {
            isDisplayingMessage = false; // No more messages to display
        }
    }

    private void DisplayMessage(EnemyLibraryData enemyData)
    {
        // Set the message text
        enemyImage.sprite = enemyData.sprite;
        hpText.text = enemyData.enemyHealth.ToString();
        msText.text = enemyData.enemyMoveSpeed.ToString();

        statusMessage.anchoredPosition = new Vector2(-1000, -250);
        statusMessage.DOAnchorPos(new Vector2(0, -250), 1f).SetUpdate(false);

        isDisplayingMessage = true; // Mark that a message is being displayed
        displayTimer = 5f;
    }

    public void HideStatusMessage()
    {
        isDisplayingMessage = false;
        var rt = statusMessage.GetComponent<RectTransform>();
        // Hide the current message
        
        rt.anchoredPosition = new Vector2(0, -250);
        rt.DOAnchorPos(new Vector2(-1000, -250),1f).SetUpdate(false);

        // Display the next message, if any
        this.Invoke(() => DisplayNextMessage(), 1f);
    }

    /*public void ForceShowMessage(string message)
    {
        // Clear the queue and stop any ongoing display
        messageQueue.Clear();
        isDisplayingMessage = false;

        // Immediately display the new message
        DisplayMessage(message);
    }*/

    private void Update()
    {
        if (displayTimer > 0)
            displayTimer -= Time.deltaTime;
        if (isDisplayingMessage && displayTimer <= 0)
        {
            HideStatusMessage();
        }
    }
}
