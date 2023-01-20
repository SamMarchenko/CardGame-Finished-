using System;
using System.Collections;
using Cards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler,
    IDragHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _frontCard;
    [Space, SerializeField] private MeshRenderer _icon;
    [SerializeField] private TextMeshPro _cosText;
    [SerializeField] private TextMeshPro _nameText;
    public TextMeshPro NameText => _nameText;
    [SerializeField] private TextMeshPro _descriptionText;
    [SerializeField] private TextMeshPro _typeText;
    [SerializeField] private TextMeshPro _attackText;
    [SerializeField] private TextMeshPro _healthText;

    private CardClickSignalHandler _cardClickSignalHandler;
    private CardPointerSignalHandler _cardPointerSignalHandler;
    private CardDragSignalHandler _cardDragSignalHandler;

    private Vector3 _stepPosition = new Vector3(0, 0.5f, 0);
    public Vector3 StepPosition => _stepPosition;
    private const float _scale = 1.7f;
    public float Scale => _scale;
    private Transform _startPosition;
    private Transform _endPosition;

    public bool IsEnable
    {
        get => _icon.enabled;
        set
        {
            _icon.enabled = value;
            _frontCard.SetActive(value);
        }
    }

    public ECardStateType StateType { get; set; } = ECardStateType.InDeck;

    public void Configuration(CardPropertiesData data, string description, Material icon)
    {
        _icon.sharedMaterial = icon;
        _cosText.text = data.Cost.ToString();
        _nameText.text = data.Name;
        _descriptionText.text = description;
        _typeText.text = data.Type == ECardUnitType.None ? string.Empty : data.Type.ToString();
        _attackText.text = data.Attack.ToString();
        _healthText.text = data.Health.ToString();
    }

    public void Init(CardClickSignalHandler cardClickSignalHandlerHandler,
        CardPointerSignalHandler cardPointerSignalHandler, CardDragSignalHandler cardDragSignalHandler)
    {
        _cardClickSignalHandler = cardClickSignalHandlerHandler;
        _cardPointerSignalHandler = cardPointerSignalHandler;
        _cardDragSignalHandler = cardDragSignalHandler;
    }

    [ContextMenu("Switch Visual")]
    public void SwitchVisual()
    {
        IsEnable = !IsEnable;
    }

    public Transform GetStartCardPosition()
    {
        return _startPosition;
    }
    
    public Transform GetEndCardPosition()
    {
        return _endPosition;
    }

    public void SetStartCardPosition(Transform startPosition)
    {
        _startPosition = startPosition;
    }
    
    public void SetEndCardPosition(Transform endPosition)
    {
        _endPosition = endPosition;
    }

    public void MoveAnimation(Transform endPosition)
    {
        StartCoroutine(MoveInHand(this, endPosition));
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    private IEnumerator MoveInHand(CardView cardView, Transform parent)
    {
        var time = 0f;
        var startPos = cardView.transform.position;
        var endPos = parent.position;
        cardView.SwitchVisual();
        cardView.transform.eulerAngles = new Vector3(0, 0, 180);
        while (time < 1f)
        {
            cardView.transform.position = Vector3.Lerp(startPos, endPos, time);
            time += Time.deltaTime;
            yield return null;
        }

        cardView.transform.parent = parent;
        cardView.StateType = ECardStateType.InHand;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _cardPointerSignalHandler.FirePointerOn(new CardPointerSignal(this));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cardPointerSignalHandler.FirePointerOff(new CardPointerSignal(this));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _cardDragSignalHandler.FireDragStart(new CardDragSignal(this));
        Debug.Log("OnBeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _cardDragSignalHandler.FireDragEnd(new CardDragSignal(this));
        Debug.Log("OnEndDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        _cardDragSignalHandler.FireDragging(new CardDragSignal(this));
        Debug.Log("OnDrag");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _cardClickSignalHandler.Fire(new CardClickSignal(this));
    }
}