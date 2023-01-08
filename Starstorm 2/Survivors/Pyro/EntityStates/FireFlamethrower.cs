﻿using RoR2;
using Starstorm2.Survivors.Pyro.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EntityStates.SS2UStates.Pyro
{
    public class FireFlamethrower : BaseState
    {
        public static float baseDuration = 0.16f;
        public static float baseSelfForce = 450f;
        public static float maxDistance = 20f;
        public static float heatFractionPerTick = 0.025f;
		public static float procCoefficient = 0.7f;
		public static float damageCoefficient = 0.6f;
		public static GameObject impactEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/missileexplosionvfx");

		private float duration;
		private HeatController heatController;
		private FlamethrowerController flameController;

		public override void OnEnter()
        {
            base.OnEnter();

            duration = FireFlamethrower.baseDuration / this.attackSpeedStat;
			heatController = base.GetComponent<HeatController>();
			if (heatController)
            {
				heatController.AddHeatAuthority(FireFlamethrower.heatFractionPerTick);
            }

			flameController = base.GetComponent<FlamethrowerController>();
			if (flameController)
            {
				flameController.FireFlamethrower(0.2f);
            }

			ShootFlame();
		}

        public override void FixedUpdate()
        {
            base.FixedUpdate();

			if (base.isAuthority && base.fixedAge >= duration)
            {
				this.outer.SetNextStateToMain();
				return;
            }
        }

        private void ShootFlame()
		{
			if (base.characterBody)
			{
				base.characterBody.SetAimTimer(2f);
			}
			Ray aimRay = base.GetAimRay();
			if (base.isAuthority)
			{
				new BulletAttack
				{
					owner = base.gameObject,
					weapon = base.gameObject,
					origin = aimRay.origin,
					aimVector = aimRay.direction,
					minSpread = 0f,
					damage = FireFlamethrower.damageCoefficient * this.damageStat,
					force = 0f,
					muzzleName = "Muzzle",
					hitEffectPrefab = FireFlamethrower.impactEffectPrefab,
					isCrit = base.RollCrit(),
					radius = 2.4f,
					falloffModel = BulletAttack.FalloffModel.None,
					stopperMask = LayerIndex.world.mask,
					procCoefficient = FireFlamethrower.procCoefficient,
					maxDistance = FireFlamethrower.maxDistance,
					smartCollision = true,
					damageType = (heatController && heatController.IsHighHeat()? DamageType.IgniteOnHit : DamageType.Generic)
				}.Fire();
				heatController.AddHeatAuthority(FireFlamethrower.heatFractionPerTick);

				if (base.characterMotor && !base.characterMotor.isGrounded)
				{
					Vector3 selfForceDirection = base.GetAimRay().direction;
					if (selfForceDirection.y < 0f)
					{
						selfForceDirection.x = 0f;
						selfForceDirection.z = 0f;

						float attackSpeedFactor = Mathf.Sqrt(this.attackSpeedStat);
						selfForceDirection /= attackSpeedFactor;

						base.characterMotor.ApplyForce(selfForceDirection * -FireFlamethrower.baseSelfForce, true, false);
					}
				}
			}
		}

        public override InterruptPriority GetMinimumInterruptPriority()
        {
			return InterruptPriority.Skill;
        }
    }
}
