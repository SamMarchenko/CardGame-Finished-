using System;
using Cards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private GameObject _frontCard;
    [Space, SerializeField] private MeshRenderer _icon;
    [SerializeField] private TextMeshPro _cosText;
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private TextMeshPro _descriptionText;
    [SerializeField] private TextMeshPro _typeText;
    [SerializeField] private TextMeshPro _attackText;
    [SerializeField] private TextMeshPro _healthText;
    public Action<Card> WantChangePosition;

    private bool _onDragging;
    private bool _isScaled;
    private Vector3 _currentPosition;
    private Vector3 _previousPosition;
    public Vector3 PreviousPosition => _previousPosition;

    public Vector3 CurrentPosition
    {
        get => _currentPosition;
        set => _currentPosition = value;
    }

    private Vector3 _stepPosition = new Vector3(0,3f,0);
    private const float _scale = 2f;

    private Camera _camera;

    public bool IsEnable
    {
        get => _icon.enabled;
        set
        {
            _icon.enabled = value;
            _frontCard.SetActive(value);
        }
    }

    private void Start()
    {
        _camera = Camera.main;
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

    [ContextMenu("Switch Visual")]
    public void SwitchVisual()
    {
        IsEnable = !IsEnable;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (StateType)
        {
            case ECardStateType.InHand:
                if (_onDragging) return;
                ScaleCard(true);
                break;
            case ECardStateType.OnTable:
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        switch (StateType)
        {
            case ECardStateType.InHand:
                ScaleCard(false);
                break;
            case ECardStateType.OnTable:
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _previousPosition = transform.position;
        ScaleCard(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        WantChangePosition?.Invoke(this);
        transform.position += new Vector3(0,1,0);
        _onDragging = false;
        ScaleCard(false);
        Debug.Log(transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        switch (StateType)
        {
            case ECardStateType.InHand:
                _onDragging = true;
                DragCard();
                break;
        }
    }

    private void DragCard()
    {
        Ray R = _camera.ScreenPointToRay(Input.mousePosition);
        Vector3 PO = transform.position;
        Vector3 PN = -_camera.transform.forward;
        float t = Vector3.Dot(PO - R.origin, PN) / Vector3.Dot(R.direction, PN);
        Vector3 P = R.origin + R.direction * t;
        
        transform.position = P;
    }

    private void ScaleCard(bool upScale)
    {
        if (upScale == _isScaled) return;
       
        if (upScale)
        {
            transform.localPosition += _stepPosition;
            transform.localScale *= _scale;
            _isScaled = true;
        }
        else
        {
            transform.localPosition -= _stepPosition;
            transform.localScale /= _scale;
            _isScaled = false;
        }
    }
}