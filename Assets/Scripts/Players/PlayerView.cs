using Cards;
using DefaultNamespace;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerView : MonoBehaviour, IPointerClickHandler, IDamageable
{
    public EPlayers PlayerType;
    private PlayerSignalBus _playerSignalBus;

    private int _maxHealth = 30;
    private const int _damage = 0;
    [SerializeField] private int _currentHealth;


    [SerializeField] private int _currentMana;
    [SerializeField] private int _maxMana = 3;
    //[SerializeField] private TextMeshPro _healthText;

    public void Init(EPlayers playerType, PlayerSignalBus playerSignalBus)
    {
        _playerSignalBus = playerSignalBus;
        PlayerType = playerType;
        _currentHealth = _maxHealth;
        _currentMana = _maxMana;

        //_healthText.text = _maxHealth.ToString();
    }

    public int GetCurrentMana()
    {
        return _currentMana;
    }

    public void SetCurrentMana(int value)
    {
        _currentMana = value;
    }

    public int GetMaxMana()
    {
        return _maxMana;
    }

    public void SetMaxMana(int value)
    {
        _maxMana = value;
    }

    public void ManaUse(CardView card)
    {
        _currentMana -= card.GetCost();
        Debug.Log($"У игрока {PlayerType} осталось {_currentMana}/{_maxMana} маны.");
    }

    public void ManaLog()
    {
        Debug.Log($"У игрока {PlayerType} {_currentMana}/{_maxMana} маны");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _playerSignalBus.PlayerClickFire(new PlayerClickSignal(this));
    }

    public int GetDamage()
    {
        return _damage;
    }

    public void ApplyDamage(int damage)
    {
        var health = GetHealth();
        health -= damage;
        SetHealth(health);
    }

    private void SetHealth(int health)
    {
        _currentHealth = _maxHealth;
        // _healthText.text = health.ToString();
    }

    public int GetHealth()
    {
        return _currentHealth;
        //return int.Parse(_healthText.text);
    }
}