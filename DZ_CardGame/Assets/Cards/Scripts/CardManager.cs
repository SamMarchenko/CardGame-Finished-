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
    private Card[] _playerDeck1;
    private Card[] _playerDeck2;
    [SerializeField] private CardPackConfiguration[] _commonPacks;
    [SerializeField] private CardPackConfiguration[] _classPacks;
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private ETurn _whoseMove = ETurn.First;

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
        _playerDeck1 = CreateDeck(_player1Hero.Turn,_deck1Parent);
        _playerDeck2 = CreateDeck(_player2Hero.Turn,_deck2Parent);
        foreach (var card in _playerDeck1)
        {
            card.WantStartDrag += WantStartDrag;
            card.WantChangePosition += WantChangePosition;
        }
        foreach (var card in _playerDeck2)
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
                    TakeCardsInHandFromDeck(_playerDeck1);
                    break;
                case ETurn.Second:
                    TakeCardsInHandFromDeck(_playerDeck2);
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
                _playerHand1.SetNewCard(deck[i]); 
            }
            else
            {
                _playerHand2.SetNewCard(deck[i]); 
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
        
        PlayerTable playerTable = new PlayerTable();
        Player player = new Player();
        switch (_whoseMove)
        {
            case ETurn.First:
                playerTable = _playerTable1;
                player = _player1Hero;
                break;
            case ETurn.Second:
                playerTable = _playerTable2;
                player = _player2Hero;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        if (playerTable.HasNearestFreePosition(card.transform, out DrawCardSlots position))
        {
            player.SpendManaPool(card.Cost);
            StartCoroutine(MoveCard(card, position.transform.position));
            playerTable.RemovePosition(position);
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
    
    private Card[] CreateDeck(ETurn turn,Transform parent)
    {
        var deck = new Card[_maxNumberCardInDeck];
        var data = new List<CardPropertiesData>();
        data.AddRange(turn == ETurn.First ? _classCardsPlayer1 : _classCardsPlayer2);
        
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
        
        MixCardsInDeck(deck);
        //Выстраивание стопки карт
        LineUpCardsStack(deck);
        
        foreach (var card in deck)
        {
            card.Turn = turn;
        }
        return deck;
    }

    private void LineUpCardsStack(Card[] deck)
    {
        var offset = 0.8f;
        foreach (var card in deck)
        {
            card.transform.localPosition = new Vector3(0f, offset, 0f);
            card.transform.eulerAngles = new Vector3(0, 0, 180f);
            offset += 0.8f;
        }
    }

    private void MixCardsInDeck(Card[] cards)
    {
        for (int i = cards.Length - 1; i >= 1; i--)
        {
            var j = Random.Range(0, i + 1);
            var temp = cards[j];
            cards[j] = cards[i];
            cards[i] = temp;
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
}