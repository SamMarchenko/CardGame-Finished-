using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.Managers;
using Cards.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    [SerializeField] private Player _firstPlayer;
    [SerializeField] private Player _secondPlayer;
    [SerializeField] private CardPackConfiguration[] _commonPacks;
    [SerializeField] private CardPackConfiguration[] _classPacks;
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private HandSlotsHandler _handSlotsHandler;
    [SerializeField] private StageController _stageController;

    [Space, SerializeField, Range(1f, 100f)]
    private int _maxNumberCardInDeck = 30;

    [SerializeField] private StartHandPreviewHandler _startHandPreviewHandler;
    private Material _baseMat;
    private List<CardPropertiesData> _allCommonCards;
    private List<CardPropertiesData> _firstPlayerClassCards;
    private List<CardPropertiesData> _secondPlayerClassCards;
    private EGameStage _gameStage;
    private Player _whoseMove;

    private void OnEnable()
    {
        _stageController.SetCurrentGameStage += SetCurrentGameStage;
        _stageController.SetTurnMoving += SetTurnMoving;
    }

    private void Awake()
    {
        PlayersInit();
        IEnumerable<CardPropertiesData> cards = new List<CardPropertiesData>();
        foreach (var pack in _commonPacks)
        {
            cards = pack.UnionProperties(cards);
        }

        _firstPlayer.Turn = ETurn.First;
        _secondPlayer.Turn = ETurn.Second;
        _allCommonCards = new List<CardPropertiesData>(cards);
        _baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
        _baseMat.renderQueue = 2997;

        _firstPlayerClassCards = new List<CardPropertiesData>(SetClassCardsData(_firstPlayer.Type));
        _secondPlayerClassCards = new List<CardPropertiesData>(SetClassCardsData(_secondPlayer.Type));
        CreateDeck(_firstPlayer);
        CreateDeck(_secondPlayer);
    }

    private void Start()
    {
        AllActionsSubscribe();
    }

    private void AllActionsSubscribe()
    {
        foreach (var card in _firstPlayer.Deck.GetCards())
        {
            card.WantStartDrag += WantStartDrag;
            card.WantChangePosition += WantChangePosition;
            card.OnCardClick += OnCardClick;
        }

        foreach (var card in _secondPlayer.Deck.GetCards())
        {
            card.WantStartDrag += WantStartDrag;
            card.WantChangePosition += WantChangePosition;
            card.OnCardClick += OnCardClick;
        }
    }

    private void OnCardClick(Card card)
    {
        if (_gameStage == EGameStage.ChooseStartHand)
        {
            if (card.Owner != _whoseMove)
                return;
            _whoseMove.SwapCardInStartHand(_whoseMove,card);
        }
    }
    

    private void SetTurnMoving(Player player)
    {
        _whoseMove = player;
        if (_gameStage == EGameStage.ChooseStartHand)
        {
            var deck = _whoseMove.Deck.GetCards();
            //todo: переписать
            TakeCardInHandFromDeck(deck, 3);
        }
    }

    private void SetCurrentGameStage(EGameStage stage)
    {
        _gameStage = stage;
    }

    private void Update()
    {
    }

    private void ChangeMove()
    {
        // switch (_stageController.WhoseMove)
        // {
        //     case ETurn.First:
        //         FlipCards(_firstPlayerHand.Cards);
        //         FlipCards(_secondPlayerHand.Cards);
        //         _firstPlayer.IncreaseMaxManaPool();
        //         _firstPlayer.RecoverManaPool();
        //         break;
        //     case ETurn.Second:
        //         FlipCards(_secondPlayerHand.Cards);
        //         FlipCards(_firstPlayerHand.Cards);
        //         _secondPlayer.IncreaseMaxManaPool();
        //         _secondPlayer.RecoverManaPool();
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException();
        // }
        //
        // _stageController.WhoseMove = _stageController.WhoseMove == ETurn.First ? ETurn.Second : ETurn.First;
    }

    private void FlipCards(List<Card> cards)
    {
        if (cards is null)
        {
            return;
        }

        foreach (var card in cards)
        {
            card?.SwitchVisual();
        }
    }

    private void TakeCardInHandFromDeck(List<Card> deck, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            for (int j = deck.Count - 1; j >= 0; j--)
            {
                if (deck[j] == null) continue;
                _whoseMove.Hand.SetNewCard(_whoseMove, deck[j]);
                deck.Remove(deck[j]);
                break;
            }
        }
    }

    private void TakeCardInSlot(Card[] deck)
    {
        for (int i = deck.Length - 1; i >= 0; i--)
        {
            
        }
    }

    private void WantStartDrag(Card card)
    {
        // if (card.Turn != _stageController.WhoseMove)
        // {
        //     Debug.Log("!!!Не твой ход!!!");
        //     card.CanDrag = false;
        //     return;
        // }
        //
        // card.CanDrag = true;
    }

    private void WantChangePosition(Card card)
    {
        if (!IsEnoughMana(card))
        {
            Debug.Log("Недостаточно маны!");
            ReturnCardInHand(card);
            return;
        }

        Player player = new Player();
        switch (_stageController.WhoseMove)
        {
            // case ETurn.First:
            //     player = _firstPlayer;
            //     break;
            // case ETurn.Second:
            //     player = _secondPlayer;
            //     break;
            // default:
            //     throw new ArgumentOutOfRangeException();
        }

        if (player.Table.HasNearestFreePosition(card.transform, out Transform position))
        {
            player.SpendManaPool(card.Cost);
            var slot = _handSlotsHandler.FindSlotByCard(player.Turn, card);
            _handSlotsHandler.RefreshSlot(slot);
            StartCoroutine(MoveCard(card, position.transform.position));
            player.Table.RemovePosition(position);
            player.Hand.RemoveCardFromHand(card);
            card.StateType = ECardStateType.OnTable;
        }
        else
        {
            ReturnCardInHand(card);
        }
    }

    private void ReturnCardInHand(Card card)
    {
        StartCoroutine(MoveCard(card, card.PreviousPosition));
        card.StateType = ECardStateType.InHand;
    }

    private bool IsEnoughMana(Card card)
    {
        bool result = false;
        switch (card.Turn)
        {
            case ETurn.First:
                result = _firstPlayer.CurrentManaPool - card.Cost >= 0;
                break;
            case ETurn.Second:
                result = _secondPlayer.CurrentManaPool - card.Cost >= 0;
                break;
        }

        return result;
    }

    private IEnumerator MoveCard(Card card, Vector3 parent)
    {
        var time = 0f;
        var startPos = card.transform.position;
        var endPos = parent;
        while (time < 0.1f)
        {
            card.transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }

        card.transform.position = parent;
        card.CurrentPosition = card.transform.position;
    }

    private void CreateDeck(Player player)
    {
        var deck = new List<Card>(new Card[_maxNumberCardInDeck]);
        var data = new List<CardPropertiesData>();
        data.AddRange(player.Turn == ETurn.First ? _firstPlayerClassCards : _secondPlayerClassCards);

        // Заполнение классовыми картами
        for (var i = 0; i < data.Count; i++)
        {
            deck[i] = Instantiate(_cardPrefab, player.Deck.DeckPosition);
            deck[i].SwitchVisual();
            var newMat = new Material(_baseMat);
            newMat.mainTexture = data[i].Texture;
            deck[i].Configuration(data[i], CardUtility.GetDescriptionById(data[i].Id), newMat);
            deck[i].CanSwap = true;
            deck[i].Owner = player;
        }

        //Заполнение общими картами
        for (int i = data.Count; i < deck.Count; i++)
        {
            deck[i] = Instantiate(_cardPrefab, player.Deck.DeckPosition);
            deck[i].SwitchVisual();
            var random = _allCommonCards[Random.Range(0, _allCommonCards.Count)];
            var newMat = new Material(_baseMat);
            newMat.mainTexture = random.Texture;
            deck[i].Configuration(random, CardUtility.GetDescriptionById(random.Id), newMat);
            deck[i].CanSwap = true;
            deck[i].Owner = player;
        }

        player.Deck.SetCards(deck);
        player.Deck.MixDeck();
        foreach (var card in deck)
        {
            card.Turn = player.Turn;
        }
    }

    private List<CardPropertiesData> SetClassCardsData(ESideType classType)
    {
        var cardsData = new List<CardPropertiesData>();
        foreach (var pack in _classPacks)
        {
            if (pack.ESideType != classType)
                continue;
            cardsData.AddRange(pack.Cards);
            break;
        }

        return cardsData;
    }

    private void PlayersInit()
    {
        _firstPlayer.Init(_handSlotsHandler);
        _secondPlayer.Init(_handSlotsHandler);
        _stageController.Init(_firstPlayer, _secondPlayer);
    }
}