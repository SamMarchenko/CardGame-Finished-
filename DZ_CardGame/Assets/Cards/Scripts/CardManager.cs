using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    private Material _baseMat;
    private List<CardPropertiesData> _allCommonCards;
    private List<CardPropertiesData> _classCardsPlayer1;
    private List<CardPropertiesData> _classCardsPlayer2;
    [SerializeField] private PlayerDeck _playerDeck1;

    [SerializeField] private PlayerDeck _playerDeck2;

    private StartHandController _startHandController;
    [SerializeField] private CardPackConfiguration[] _commonPacks;
    [SerializeField] private CardPackConfiguration[] _classPacks;
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private ETurn _whoseMove = ETurn.First;
    [SerializeField] private HandSlotsHandler _handSlotsHandler;

    [Space, SerializeField, Range(1f, 100f)]
    private int _maxNumberCardInDeck = 30;

    [SerializeField, Space] private Transform _deck1Parent;
    [SerializeField] private Transform _deck2Parent;
    [SerializeField] private PlayerHand _playerHand1;
    [SerializeField] private PlayerHand _playerHand2;
    [SerializeField] private PlayerTable _playerTable1;
    [SerializeField] private PlayerTable _playerTable2;
    [SerializeField] private Player _player1Hero;
    [SerializeField] private Player _player2Hero;


    private void Awake()
    {
        IEnumerable<CardPropertiesData> cards = new List<CardPropertiesData>();
        foreach (var pack in _commonPacks)
        {
            cards = pack.UnionProperties(cards);
        }

        _player1Hero.Turn = ETurn.First;
        _player2Hero.Turn = ETurn.Second;
        _allCommonCards = new List<CardPropertiesData>(cards);
        _baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
        _baseMat.renderQueue = 2997;
    }

    private void Start()
    {
        _classCardsPlayer1 = new List<CardPropertiesData>(SetClassCardsData(_player1Hero.Type));
        _classCardsPlayer2 = new List<CardPropertiesData>(SetClassCardsData(_player2Hero.Type));
        PlayersInit();
        CreateDeck(_player1Hero, _deck1Parent);
        CreateDeck(_player2Hero, _deck2Parent);
        _startHandController = new StartHandController(_player1Hero, _player2Hero);


        foreach (var card in _playerDeck1.GetCards())
        {
            card.WantStartDrag += WantStartDrag;
            card.WantChangePosition += WantChangePosition;
        }

        foreach (var card in _playerDeck2.GetCards())
        {
            card.WantStartDrag += WantStartDrag;
            card.WantChangePosition += WantChangePosition;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (_whoseMove)
            {
                case ETurn.First:
                    TakeCardsInHandFromDeck(_playerDeck1.GetCards());
                    break;
                case ETurn.Second:
                    TakeCardsInHandFromDeck(_playerDeck2.GetCards());
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeMove();
        }
    }

    private void ChangeMove()
    {
        switch (_whoseMove)
        {
            case ETurn.First:
                FlipCards(_playerHand1.Cards);
                FlipCards(_playerHand2.Cards);
                _player1Hero.IncreaseMaxManaPool();
                _player1Hero.RecoverManaPool();
                break;
            case ETurn.Second:
                FlipCards(_playerHand2.Cards);
                FlipCards(_playerHand1.Cards);
                _player2Hero.IncreaseMaxManaPool();
                _player2Hero.RecoverManaPool();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _whoseMove = _whoseMove == ETurn.First ? ETurn.Second : ETurn.First;
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

    private void TakeCardsInHandFromDeck(Card[] deck)
    {
        for (int i = deck.Length - 1; i >= 0; i--)
        {
            if (deck[i] == null) continue;

            if (_whoseMove == ETurn.First)
            {
                _playerHand1.SetNewCard(ETurn.First, deck[i]);
            }
            else
            {
                _playerHand2.SetNewCard(ETurn.Second, deck[i]);
            }

            deck[i] = null;
            break;
        }
    }

    private void WantStartDrag(Card card)
    {
        if (card.Turn != _whoseMove)
        {
            Debug.Log("!!!Не твой ход!!!");
            card.CanDrag = false;
            return;
        }

        card.CanDrag = true;
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
        switch (_whoseMove)
        {
            case ETurn.First:
                player = _player1Hero;
                break;
            case ETurn.Second:
                player = _player2Hero;
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
                result = _player1Hero.CurrentManaPool - card.Cost >= 0;
                break;
            case ETurn.Second:
                result = _player2Hero.CurrentManaPool - card.Cost >= 0;
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

    private Card[] CreateDeck(Player player, Transform parent)
    {
        var deck = new Card[_maxNumberCardInDeck];
        var data = new List<CardPropertiesData>();
        data.AddRange(player.Turn == ETurn.First ? _classCardsPlayer1 : _classCardsPlayer2);

        // Заполнение классовыми картами
        for (var i = 0; i < data.Count; i++)
        {
            deck[i] = Instantiate(_cardPrefab, parent);
            deck[i].SwitchVisual();
            var newMat = new Material(_baseMat);
            newMat.mainTexture = data[i].Texture;
            deck[i].Configuration(data[i], CardUtility.GetDescriptionById(data[i].Id), newMat);
        }

        //Заполнение общими картами
        for (int i = data.Count; i < _maxNumberCardInDeck; i++)
        {
            deck[i] = Instantiate(_cardPrefab, parent);
            deck[i].SwitchVisual();
            //todo: тут заполняется рандомом, надо будет переделать по конкретную колоду
            var random = _allCommonCards[Random.Range(0, _allCommonCards.Count)];

            var newMat = new Material(_baseMat);
            newMat.mainTexture = random.Texture;
            deck[i].Configuration(random, CardUtility.GetDescriptionById(random.Id), newMat);
        }

        player.Deck.SetCards(deck);
        player.Deck.MixDeck();
        foreach (var card in deck)
        {
            card.Turn = player.Turn;
        }

        return deck;
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
        _playerHand1.Init(_handSlotsHandler);
        _playerHand2.Init(_handSlotsHandler);
        _player1Hero.Init(_playerHand1, _playerTable1, _playerDeck1);
        _player2Hero.Init(_playerHand2, _playerTable2, _playerDeck2);
    }
}