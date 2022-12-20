using System.Collections;
using System.Collections.Generic;
using Cards;
using Cards.ScriptableObjects;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Zenject;

public class GameCircle : IInitializable, ITickable
{
    private readonly DeckFactory _deckFactory;
    private Material _baseMat;
    private List<CardPropertiesData> _allCards;
    private CardView[] _playerDeck1;
    private CardView[] _playerDeck2;
    [SerializeField] private CardView _cardViewPrefab;
    
    
    
    [SerializeField, Space] private Transform _deck1Parent;
    [SerializeField] private Transform _deck2Parent;
    [SerializeField] private PlayerHand _playerHand1;
    [SerializeField] private PlayerHand _playerHand2;

    public GameCircle(DeckFactory deckFactory)
    {
        _deckFactory = deckFactory;
    }
    
    // private void Awake()
    // {
    //     IEnumerable<CardPropertiesData> cards = new List<CardPropertiesData>();
    //     foreach (var pack in _packs)
    //     {
    //         cards = pack.UnionProperties(cards);
    //     }
    //     _allCards = new List<CardPropertiesData>(cards);
    //     _baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
    //     _baseMat.renderQueue = 2997;
    // }
    
    public void Initialize()
    {
        _playerDeck1 = _deckFactory.CreateDeck(EPlayers.First);
        _playerDeck2 = _deckFactory.CreateDeck(EPlayers.Second);
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = _playerDeck1.Length-1; i >=0; i--)
            {
                if (_playerDeck1[i] == null) continue;
                _playerHand1.SetNewCard(_playerDeck1[i]);
                _playerDeck1[i] = null;
                break;
            }
        }
    }
}
