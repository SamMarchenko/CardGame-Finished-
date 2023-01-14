using System;
using System.Collections.Generic;
using Cards;
using Cards.ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

// public class CardManager : MonoBehaviour
// {
//     private Material _baseMat;
//     private List<CardPropertiesData> _allCards;
//     private CardView[] _playerDeck1;
//     private CardView[] _playerDeck2;
//     [SerializeField] private CardPackConfiguration[] _packs;
//     [SerializeField] private CardView cardViewPrefab;
//
//     [Space, SerializeField, Range(1f, 100f)]
//     private int _maxNumberCardInDeck = 30;
//
//     [SerializeField, Space] private Transform _deck1Parent;
//     [SerializeField] private Transform _deck2Parent;
//     [SerializeField] private PlayerHand _playerHand1;
//     [SerializeField] private PlayerHand _playerHand2;
//
//     private void Awake()
//     {
//         IEnumerable<CardPropertiesData> cards = new List<CardPropertiesData>();
//         foreach (var pack in _packs)
//         {
//             cards = pack.UnionProperties(cards);
//         }
//         _allCards = new List<CardPropertiesData>(cards);
//         _baseMat = new Material(Shader.Find("TextMeshPro/Sprite"));
//         _baseMat.renderQueue = 2997;
//     }
//
//     private void Start()
//     {
//         _playerDeck1 = CreateDeck(_deck1Parent);
//         _playerDeck2 = CreateDeck(_deck2Parent);
//     }
//
//     private void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             for (int i = _playerDeck1.Length-1; i >=0; i--)
//             {
//                 if (_playerDeck1[i] == null) continue;
//                 _playerHand1.SetNewCard(_playerDeck1[i]);
//                 _playerDeck1[i] = null;
//                 break;
//             }
//         }
//     }
//
//     private CardView[] CreateDeck(Transform parent)
//     {
//         var deck = new CardView[_maxNumberCardInDeck];
//         var offset = 0.8f;
//
//         for (int i = 0; i < _maxNumberCardInDeck; i++)
//         {
//             deck[i] = Instantiate(cardViewPrefab, parent);
//             deck[i].transform.localPosition = new Vector3(0f,offset,0f);
//             deck[i].transform.eulerAngles = new Vector3(0,0,180f);
//             deck[i].SwitchVisual();
//             offset += 0.8f;
//             
//             //todo: тут заполняется рандомом, надо будет переделать по конкретную колоду
//             var random = _allCards[Random.Range(0, _allCards.Count)];
//             
//             var newMat = new Material(_baseMat);
//             newMat.mainTexture = random.Texture;
//             deck[i].Configuration(random, CardUtility.GetDescriptionById(random.Id), newMat);
//         }
//         
//         return deck;
//     }
// }