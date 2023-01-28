using Cards;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private EPlayers _playerType;
    
    private int _maxHealth = 30;
    private const int _damage = 0;
    [SerializeField] private int _currentHealth;
    

    [SerializeField] private int _currentMana;
    [SerializeField] private int _maxMana = 3;

    public void Init(EPlayers playerType)
    {
        _playerType = playerType;
        _currentHealth = _maxHealth;
        _currentMana = _maxMana;
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
        Debug.Log($"У игрока {_playerType} осталось {_currentMana}/{_maxMana} маны.");
    }

    public void ManaLog()
    {
        Debug.Log($"У игрока {_playerType} {_currentMana}/{_maxMana} маны");
    }
}