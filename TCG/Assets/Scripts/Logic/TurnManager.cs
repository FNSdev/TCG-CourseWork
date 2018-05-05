using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

// этот класс занимается предоставлением хода игрокам поочередно и отсчитывает время до конца хода
public class TurnManager : MonoBehaviour {

    private RopeTimer timer;

    // сущность для реализации паттерна singleton
    public static TurnManager Instance;

    private Player _whoseTurn;
    public Player WhoseTurn
    {
        get
        {
            return _whoseTurn;
        }

        set
        {
            _whoseTurn = value;
            timer.StartTimer();

            GlobalSettings.Instance.EnableEndTurnButtonOnStart(_whoseTurn);

            TurnMaker tm = WhoseTurn.GetComponent<TurnMaker>();
            // player`s method OnTurnStart() will be called in tm.OnTurnStart();
            tm.OnTurnStart();
            if (tm is PlayerTurnMaker)
            {
                WhoseTurn.HighlightPlayableCards();
            }
            // remove highlights for opponent.
            WhoseTurn.otherPlayer.HighlightPlayableCards(true);
                
        }
    }

    void Awake()
    {
        Instance = this;
        timer = GetComponent<RopeTimer>();
    }

    void Start()
    {
        OnGameStart();
    }

    public void OnGameStart()
    {
        //Debug.Log("In TurnManager.OnGameStart()");

        CardLogic.CardsCreatedThisGame.Clear();
        CreatureLogic.CreaturesCreatedThisGame.Clear();

        foreach (Player p in Player.Players)
        {
            p.ManaThisTurn = 0;
            p.ManaLeft = 0;
            p.LoadCharacterInfoFromAsset();
            p.TransmitInfoAboutPlayerToVisual();
            p.PArea.PDeck.CardsInDeck = p.deck.cards.Count;
            p.PArea.Portrait.transform.position = p.StartingPosition.position;
        }

        Sequence punch = DOTween.Sequence();
        punch.Append(Player.Players[0].PArea.Portrait.transform.DOMove(new Vector3(0f, 1f, -4.5f), 1.5f).SetEase(Ease.InBounce).SetDelay(1f));
        punch.Insert(0f, Player.Players[1].PArea.Portrait.transform.DOMove(new Vector3(0f, -1f, -4.5f), 1.5f).SetEase(Ease.InBounce).SetDelay(1f));
        punch.Append(Player.Players[0].PArea.Portrait.transform.DOShakePosition(1.5f, 1.5f, 7, 60, false, true));
        punch.Insert(2.5f, Player.Players[1].PArea.Portrait.transform.DOShakePosition(1.5f, 1.5f, 7, 60, false, true));
        
        Sequence s = DOTween.Sequence();
        s.Append(Player.Players[0].PArea.Portrait.transform.DOMove(Player.Players[0].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.Insert(0f, Player.Players[1].PArea.Portrait.transform.DOMove(Player.Players[1].PArea.PortraitPosition.position, 1f).SetEase(Ease.InQuad));
        s.PrependInterval(4f);
        s.OnComplete(() =>
            {
                int rnd = Random.Range(0,2);  
                Player whoGoesFirst = Player.Players[rnd];
                Player whoGoesSecond = whoGoesFirst.otherPlayer;
                int initDraw = 4;
                for (int i = 0; i < initDraw; i++)
                {            
                    whoGoesSecond.DrawACard(true);
                    whoGoesFirst.DrawACard(true);
                }
                whoGoesSecond.DrawACard(true);
                whoGoesSecond.DrawACoin();
                new StartATurnCommand(whoGoesFirst).AddToQueue();
            });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EndTurn();
    }

    public void EndTurn()
    {
        // stop timer
        timer.StopTimer();
        // send all commands in the end of current player`s turn
        WhoseTurn.OnTurnEnd();

        new StartATurnCommand(WhoseTurn.otherPlayer).AddToQueue();
    }

    public void StopTheTimer()
    {
        timer.StopTimer();
    }

    public void EndTurnTest()
    {
        timer.StopTimer();
        timer.StartTimer();
    }
}

