﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DamageDeckCardSE
{

    public class DirectHit : GenericDamageCard
    {
        DiceRoll SavedAssignedDamageDiceRoll;

        public DirectHit()
        {
            Name = "Direct Hit";
            Type = CriticalCardType.Ship;
            AiAvoids = true;
            ImageUrl = "https://i.imgur.com/hmybQQt.jpg";
        }

        public override void ApplyEffect(object sender, EventArgs e)
        {
            SufferAdditionalDamage();
        }

        private void SufferAdditionalDamage()
        {
            Messages.ShowInfo("A Direct Hit causes " + Host.PilotInfo.PilotName + " to suffer 1 additional damage");

            DamageSourceEventArgs directhitDamage = new DamageSourceEventArgs()
            {
                Source = "Critical hit card",
                DamageType = DamageTypes.CriticalHitCard
            };

            SavedAssignedDamageDiceRoll = Host.AssignedDamageDiceroll;
            Host.AssignedDamageDiceroll = new DiceRoll(DiceKind.Attack, 0, DiceRollCheckType.Virtual);

            Host.Damage.TryResolveDamage(1, directhitDamage, RepairDirectHit);
        }

        public void RepairDirectHit()
        {
            Host.AssignedDamageDiceroll = SavedAssignedDamageDiceRoll;

            DiscardEffect();
            Triggers.FinishTrigger();
        }

        public override void DiscardEffect()
        {
            base.DiscardEffect();

            // Do nothing;
        }
    }

}