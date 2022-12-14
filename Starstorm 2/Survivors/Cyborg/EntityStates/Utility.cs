﻿using RoR2;
using RoR2.Projectile;
using UnityEngine;
using Starstorm2.Cores;
using Starstorm2.Survivors.Cyborg.Components;

namespace EntityStates.Starstorm2States.Cyborg
{
    public class CyborgFireOverheat : BaseSkillState
    {
        public static float damageCoefficient = 6f;
        public static float baseDuration = 0.5f;
        public static float recoil = 1f;
        public static GameObject projectilePrefab;

        private float duration;
        private bool hasFired;
        private Animator animator;
        private string muzzleString;

        private CyborgController cyborgController;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = CyborgFireOverheat.baseDuration / this.attackSpeedStat;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "Muzzle";
            cyborgController = base.GetComponent<CyborgController>();
            if (cyborgController)
            {
                cyborgController.allowJetpack = false;
            }

            Util.PlaySound("CyborgUtility", base.gameObject);
            base.PlayAnimation("Gesture, Override", "FireSpecial", "FireArrow.playbackRate", this.duration);

            //old code
            /*if (base.isAuthority && base.characterBody && base.characterBody.characterMotor)
            {
                float height = base.characterBody.characterMotor.isGrounded ? this.groundKnockbackDistance : this.airKnockbackDistance;
                float num3 = base.characterBody.characterMotor ? base.characterBody.characterMotor.mass : 1f;
                float acceleration2 = base.characterBody.acceleration;
                float num4 = Trajectory.CalculateInitialYSpeedForHeight(height, -acceleration2);
                base.characterBody.characterMotor.ApplyForce(-num4 * num3 * base.GetAimRay().direction, false, false);
            }*/

            //copied from sniper
            /*Vector3 direction = -base.GetAimRay().direction;
            if (base.isAuthority)
            {
                direction.y = Mathf.Max(direction.y, 0.05f);
                Vector3 a = direction.normalized * 4f * 10f;
                Vector3 b = Vector3.up * 7f;
                Vector3 b2 = new Vector3(direction.x, 0f, direction.z).normalized * 3f;


                if (base.characterMotor)
                {
                    base.characterMotor.Motor.ForceUnground();
                    base.characterMotor.velocity = a + b + b2;
                    base.characterMotor.velocity.y *= 0.8f;
                    if (base.characterMotor.velocity.y < 0) base.characterMotor.velocity.y *= 0.1f;
                }
                if (base.characterDirection)
                {

                    base.characterDirection.moveVector = direction;
                }
            }*/
            FireBFG();
        }

        public override void OnExit()
        {
            if (cyborgController)
            {
                cyborgController.allowJetpack = true;
            }
            base.OnExit();
        }

        private void FireBFG()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(0.75f);
                Ray aimRay = base.GetAimRay();
                EffectManager.SimpleMuzzleFlash(Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    ProjectileManager.instance.FireProjectile(CyborgFireOverheat.projectilePrefab,
                        aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction),
                        base.gameObject, this.characterBody.damage * CyborgFireOverheat.damageCoefficient,
                        0f,
                        Util.CheckRoll(this.characterBody.crit,
                        this.characterBody.master),
                        DamageColorIndex.Default, null, -1f);
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}