using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
	public enum ECardUnitType : byte
	{
		All = 0,
		Murloc = 1,
		Beast = 2,
		Elemental = 3,
		Mech = 4,
		Self = 5
	}

	public enum EAbility
	{
		None,
		Taunt,
		Charge,
		Battlecry,
		IncreaseStats,
		SetBuff,
		SetDebuff,
		DrawCard
	}

	public enum ECurrentStageType
	{
		StartWaiting,
		ChooseStartHandStage,
		MoveStage
	}
	

	public enum EHeroType : byte
	{
		Common = 0,
		Mage = 1,
		Warrior = 2,
		Priest = 3,
		Warlock = 4,
		Hunter = 5,
	}

	public enum ECardStateType
	{
		InDeck,
		InHand,
		OnTable,
		Discard
	}

	public enum EPlayers
	{
		FirstPlayer,
		SecondPlayer
	}

	public enum EScaleType
	{
		Increase,
		Decrease
	}
}
