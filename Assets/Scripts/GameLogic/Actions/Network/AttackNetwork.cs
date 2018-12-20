using System;

using Hexlibrary;
using GameLogic.Actions;
using GameLogic.Actions.Network;
using GameLogic.Units;

[Serializable]
public class AttackNetwork : GenericNetworkAction
{
    private readonly int Damage;
    private readonly int Range;
    private readonly int Chance;
    private readonly DamageType AttackType;
    private readonly int ArmourPenetration;

    private readonly int DOTDamage;
    private readonly int Duration;
    private readonly int ApplicationInterval;

    public AttackNetwork(Hex unitHex_, Hex[] targetHexes_, GenericAttack attack_)
        : base(unitHex_, targetHexes_, attack_.ActionType)
    {
        this.Damage = attack_.Damage;
        this.Range = attack_.Range;
        this.Chance = attack_.Chance;
        this.AttackType = attack_.DamageType;
        this.ArmourPenetration = attack_.ArmourPenetration;

        this.DOTDamage = attack_.DOTDamage;
        this.Duration = attack_.Duration;
        this.ApplicationInterval = attack_.ApplicationInterval;
    }


    private bool IsSuccessfulHit()
    {
        return this.Chance >= new System.Random().Next(0, 101);
    }

    private int CalculatePenetrationDamage()
    {
        return (int)(this.Damage * (this.ArmourPenetration / 100));
    }

    private int CalculateDamageToArmour()
    {
        return (int)(this.Damage * (1 - this.ArmourPenetration / 100));
    }

    private int CalculatePenetrationBonus(GenericUnit target, int damageToArmour)
    {
        if (damageToArmour > target.Current_AP)
        {
            target.AddEffect(new EffectReduceAP(target.Current_AP));
            return damageToArmour - target.Current_AP;
        }
        else
        {
            target.AddEffect(new EffectReduceAP(damageToArmour));
            return 0;
        }
    }

    private int CalculateTotalDamage(ref GenericUnit target)
    {
        if (IsSuccessfulHit())
        {
            return CalculatePenetrationDamage() + CalculatePenetrationBonus(target, CalculateDamageToArmour());
        }
        else
        {
            return 0;  
        }
    }

    public override void executeAction(HexGrid hexGrid)
    {
		base.executeAction (hexGrid);
        foreach (var targetHex in TargetHexes)
        {
            GenericUnit target = hexGrid.units.getByType1(targetHex);
            int totalDamage = CalculateTotalDamage(ref target);
            EffectDamage damage = new EffectDamage(totalDamage, DOTDamage, Duration, ApplicationInterval);

            hexGrid.units.getByType1(targetHex).AddEffect(damage);
        }
    }
}

