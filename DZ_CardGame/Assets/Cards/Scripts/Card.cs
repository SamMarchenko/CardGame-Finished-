using Cards;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private GameObject _frontCard;
    [Space, SerializeField] private MeshRenderer _icon;
    [SerializeField] private TextMeshPro _cost;
    [SerializeField] private TextMeshPro _name;
    [SerializeField] private TextMeshPro _description;
    [SerializeField] private TextMeshPro _type;
    [SerializeField] private TextMeshPro _attack;
    [SerializeField] private TextMeshPro _health;

    public bool IsEnable
    {
        get => _icon.enabled;
        set
        {
            _icon.enabled = value;
            _frontCard.SetActive(value);
        }
    }
    public void Configuration(CardPropertiesData data, string description, Material icon)
    {
        _icon.sharedMaterial = icon;
        _cost.text = data.Cost.ToString();
        _name.text = data.Name;
        _description.text = description;
        _type.text = data.Type == ECardUnitType.None ? string.Empty : data.Type.ToString();
        _attack.text = data.Attack.ToString();
        _health.text = data.Health.ToString();
    }

    [ContextMenu("Switch Visual")]
    public void SwitchVisual()
    {
        IsEnable = !IsEnable;
    }
    
}