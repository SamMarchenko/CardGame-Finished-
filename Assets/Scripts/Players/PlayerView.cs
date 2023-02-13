using System;
using Cards;
using DefaultNamespace;
using DG.Tweening;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerView : MonoBehaviour, IPointerClickHandler, IDamageable
{
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _currentMana;
    [SerializeField] private int _maxMana = 3;
    [SerializeField] private TextMeshProUGUI _healthValueText;
    [SerializeField] private TextMeshProUGUI _manaValuesText;
    
    private PlayerSignalBus _playerSignalBus;
    private int _maxHealth = 30;
    private const int _damage = 0;
    
    public EPlayers PlayerType;
    public bool CanAttack = false;



    public void Init(EPlayers playerType, PlayerSignalBus playerSignalBus)
    {
        _playerSignalBus = playerSignalBus;
        PlayerType = playerType;
        _currentHealth = _maxHealth;
        _currentMana = _maxMana;

        UpdateHealthUI();
        UpdateManaUI();
    }

    
    public int GetCurrentMana()
    {
        return _currentMana;
    }

    public void SetCurrentMana(int value)
    {
        _currentMana = value;
        UpdateManaUI();
    }

    public int GetMaxMana()
    {
        return _maxMana;
    }

    public void SetMaxMana(int value)
    {
        _maxMana = value;
        UpdateManaUI();
    }

    public void ManaUse(CardView card)
    {
        _currentMana -= card.GetCost();
        UpdateManaUI();
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
        
        transform.DOShakeRotation(0.5f, new Vector3(0, 5, 0), 10, 5f).OnComplete(() => SetHealth(health));
        
        UpdateHealthUI();
        
    }

    public void RestoreHealth(int value)
    {
        var health = GetHealth();
        if (health + value > _maxHealth)
        {
            SetHealth(_maxHealth);
            return;
        }
        SetHealth(health + value);
    }

    public void SetCoolDownAttack(bool value)
    {
        //в данном прототипе у игрока нет оружия, он никогда не сможет атаковать. Если оружие появится, то будет присваиваться value
        CanAttack = false;
    }

    private void SetHealth(int health)
    {
        _currentHealth = health;
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Debug.Log($"GAME OVER. {PlayerType} проиграл!!!");
            
            _playerSignalBus.PlayerDeathFire(new PlayerDeathSignal(this));
        }
        UpdateHealthUI();
    }
    
    private void UpdateHealthUI()
    {
        _healthValueText.text = "HP:" + _currentHealth;
    }

    private void UpdateManaUI()
    {
        _manaValuesText.text = "MP:" + _currentMana + "/" + _maxMana; 
    }

    public int GetHealth()
    {
        return _currentHealth;
    }
}