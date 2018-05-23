using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Command
{
    public static Queue<Command> CommandQueue = new Queue<Command>();
    public static bool playingQueue = false;
    public Player CommandSender = GlobalSettings.Instance.LowPlayer;

    public virtual void AddToQueue()
    {
        CommandQueue.Enqueue(this);
        if (!playingQueue)
            PlayFirstCommandFromQueue();
    }

    public virtual void StartCommandExecution()
    {
        // list of everything that we have to do with this command (draw a card, play a card, play spell effect, etc...)
        // there are 2 options of timing : 
        // 1) use tween sequences and call CommandExecutionComplete in OnComplete()
        // 2) use coroutines (IEnumerator) and WaitFor... to introduce delays, call CommandExecutionComplete() in the end of coroutine
    }

    public static void CommandExecutionComplete()
    {
        CommandQueue.Dequeue();
        if (CommandQueue.Count > 0)
            PlayFirstCommandFromQueue();
        else
            playingQueue = false;
        if (TurnManager.Instance.WhoseTurn != null)
            TurnManager.Instance.WhoseTurn.HighlightPlayableCards();
    }

    public static void PlayFirstCommandFromQueue()
    {
        playingQueue = true;
        CommandQueue.Peek().StartCommandExecution();
    }

    public static bool CardDrawPending()
    {
        foreach (Command c in CommandQueue)
        {
            if (c is DrawACardCommand || c is GetACardCommand)
            {
                Debug.Log("Drags not allowed!");
                return true;
            }
                
        }
        return false;
    }
}
