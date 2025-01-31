﻿using RoR2;
using UnityEngine;
using EntityStates;
using Starstorm2Unofficial.Survivors.Executioner.Components;

//TODO: should check that secondary is ion gun before attempting to store/load charges

namespace EntityStates.SS2UStates.Executioner
{
    public class ExecutionerMain : GenericCharacterMain
    {
        private Animator animator;
        private GenericSkill ionGunSkill;
        private IonGunChargeComponent storedChargeComp;
        private MasterIonStockComponent masterIonStock;

        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();

            //set up ion gun stock system
            ionGunSkill = skillLocator.secondary;
            storedChargeComp = base.GetComponent<IonGunChargeComponent>();

            if (base.characterBody && base.characterBody.master)
            {
                masterIonStock = base.characterBody.master.GetComponent<MasterIonStockComponent>();
                if (!masterIonStock) masterIonStock = base.characterBody.master.AddComponent<MasterIonStockComponent>();
            }

            //GlobalEventManager.onCharacterDeathGlobal += OnKillHandler;
        }

        public override void OnExit()
        {
            //GlobalEventManager.onCharacterDeathGlobal -= OnKillHandler;

            if (ionGunSkill && masterIonStock)
            {
                masterIonStock.stocks = ionGunSkill.stock;
            }

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // rest idle!!
            if (this.animator) this.animator.SetBool("inCombat", (!base.characterBody.outOfCombat || !base.characterBody.outOfDanger));
        }

        public override void Update()
        {
            base.Update();

            if (base.isAuthority && base.characterMotor.isGrounded)
            {
                if (Starstorm2Unofficial.Modules.Config.GetKeyPressed(Starstorm2Unofficial.Modules.Config.RestKeybind))
                {
                    this.outer.SetInterruptState(new EntityStates.SS2UStates.Common.Emotes.RestEmote(), InterruptPriority.Any);
                    return;
                }
                else if (Starstorm2Unofficial.Modules.Config.GetKeyPressed(Starstorm2Unofficial.Modules.Config.TauntKeybind))
                {
                    this.outer.SetInterruptState(new EntityStates.SS2UStates.Common.Emotes.TauntEmote(), InterruptPriority.Any);
                    return;
                }
            }
        }

        private void OnKillHandler(DamageReport report)
        {
            if (report != null && report.attacker)
            {
                if (report.attacker == base.gameObject)
                {
                    storedChargeComp.RpcAddIonCharge();
                }
            }
        }
    }
}