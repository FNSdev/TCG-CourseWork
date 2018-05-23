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
            new StartTheTimerCommand(timer).AddToQueue();

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
        //OnGameStart();
        // OnPVPGameStartTest();
    }
    
    public void Test()
    {
        CardLogic.CardsCreatedThisGame.Clear();
        CreatureLogic.CreaturesCreatedThisGame.Clear();

        Player you = GlobalSettings.Instance.LowPlayer;
        string[] assets1 = new string[you.deck.cards.Count];
        for (int i = 0; i < assets1.Length; i++)
        {
            assets1[i] = you.deck.cards[i].name;
        }
        PlayerConnection.SendDeckInfoOnServer((byte)you.ID, assets1);

        Player opponent = GlobalSettings.Instance.TopPlayer;
        string[] assets2 = new string[opponent.deck.cards.Count];
        for (int i = 0; i < assets2.Length; i++)
        {
            assets2[i] = opponent.deck.cards[i].name;
        }
        PlayerConnection.SendDeckInfoOnServer((byte)opponent.ID, assets2);

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
    }

    public void OnPVPGameStartTest()
    {
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
            int rnd = Random.Range(0, 2);
            Player whoGoesFirst = Player.Players[rnd];
            Player whoGoesSecond = whoGoesFirst.otherPlayer;
            int initDraw = 4;
            for (int i = 0; i < initDraw; i++)
            {
                whoGoesSecond.DrawACard(true);
                PlayerConnection.SendCommandOnServer((byte)whoGoesSecond.ID, CommandType.DrawACard, "1");
                whoGoesFirst.DrawACard(true);
                PlayerConnection.SendCommandOnServer((byte)whoGoesFirst.ID, CommandType.DrawACard, "1");
            }
            whoGoesSecond.DrawACard(true);
            PlayerConnection.SendCommandOnServer((byte)whoGoesSecond.ID, CommandType.DrawACard, "1");
            whoGoesSecond.DrawACoin();
            PlayerConnection.SendCommandOnServer((byte)whoGoesSecond.ID, CommandType.GetACard, "Coin");

            new StartATurnCommand(whoGoesFirst).AddToQueue();
            PlayerConnection.SendCommandOnServer((byte)whoGoesFirst.ID, CommandType.StartATurn);

        });
        
    }

    public void OnGameStart()
    {
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
        new StopTheTimerCommand(timer).AddToQueue();
        WhoseTurn.OnTurnEnd();
        PlayerConnection.SendCommandOnServer((byte)WhoseTurn.otherPlayer.ID, CommandType.StartATurn);
        new StartATurnCommand(WhoseTurn.otherPlayer).AddToQueue();
    }

    public void RopeEndTurn()
    {
        new StopTheTimerCommand(timer).AddToQueue();
        if(WhoseTurn == GlobalSettings.Instance.LowPlayer)
            WhoseTurn.OnTurnEnd();
        new StartATurnCommand(WhoseTurn.otherPlayer).AddToQueue();
    }

    public void StopTheTimer()
    {
        new StopTheTimerCommand(timer).AddToQueue();
    }

    public void EndTurnTest()
    {
        timer.StopTimer();
        timer.StartTimer();
    }
}

