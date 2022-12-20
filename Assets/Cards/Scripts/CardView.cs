using System;
using Cards;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private GameObject _frontCard;
    [Space, SerializeField] private MeshRenderer _icon;
    [SerializeField] private TextMeshPro _cosText;
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private TextMeshPro _descriptionText;
    [SerializeField] private TextMeshPro _typeText;
    [SerializeField] private TextMeshPro _attackText;
    [SerializeField] private TextMeshPro _healthText;
    
    private Vector3 _stepPosition = new Vector3(0,0.5f,0);
    private const float _scale = 1.7f;

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
                transform.localPosition += _stepPosition;
                transform.localScale *= _scale;
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
                transform.localPosition -= _stepPosition;
                transform.localScale /= _scale;
                break;
            case ECardStateType.OnTable:
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}