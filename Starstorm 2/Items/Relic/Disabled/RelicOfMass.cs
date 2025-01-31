﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoR2;
using UnityEngine;
using Starstorm2Unofficial.Cores;
using R2API;

//FIXME: adds health after applying transcendence shields

namespace Starstorm2Unofficial.Cores.Items
{
    class RelicOfMass : SS2Item<RelicOfMass>
    {
        public override string NameInternal => "SS2U_RelicMass";
        public override ItemTier Tier => ItemTier.Lunar;
        public override ItemTag[] Tags => new ItemTag[]
        {
            ItemTag.Utility
        };
        public override string PickupIconPath => "RelicOfMassLunar";
        public override string PickupModelPath => "MDLRelicOfMass";

        public override void RegisterHooks()
        {
            On.RoR2.CharacterBody.RecalculateStats += ModifyAccel;
            SharedHooks.GetStatCoefficients.HandleStatsInventoryActions += HandleStats;
        }

        /*public override ItemDisplayRuleDict CreateDisplayRules()
        {
            var trackerObject = LegacyResourcesAPI.Load<GameObject>("RelicOfMassFollowerObject");
            displayPrefab = LegacyResourcesAPI.Load<GameObject>(PickupModelPath);
            var itemFollower = trackerObject.AddComponent<ItemFollower>();
            itemFollower.itemDisplay = trackerObject.AddComponent<ItemDisplay>();
            itemFollower.itemDisplay.rendererInfos = Utils.SetupRendererInfos(trackerObject);
            itemFollower.followerPrefab = displayPrefab;
            itemFollower.distanceDampTime = 0.25f;
            itemFollower.distanceMaxSpeed = 100f;
            itemFollower.targetObject = trackerObject;
            var disp = displayPrefab.AddComponent<ItemDisplay>();
            disp.rendererInfos = Utils.SetupRendererInfos(displayPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = trackerObject,
                    childName = "Base",
                    //set ^ to "tracketObject" once this shit works LOL
                    localPos = new Vector3(0.12f, 0.16f, 0.2f),
                    localAngles = new Vector3(90f, 30f, 0f),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            });

            rules.Add("mdlHuntress", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = trackerObject,
                    childName = "Base",
                    localPos = new Vector3(0.13f, 0.16f, 0.26f),
                    localAngles = new Vector3(90f, 30f, 0f),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            });

            rules.Add("mdlMage", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = trackerObject,
                    childName = "Base",
                    localPos = new Vector3(0.15f, 0.16f, 0.24f),
                    localAngles = new Vector3(90f, 40f, 0f),
                    localScale = new Vector3(0.08f, 0.08f, 0.08f)
                }
            });

            rules.Add("mdlEngi", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = trackerObject,
                    childName = "Base",
                    localPos = new Vector3(0.12f, 0.16f, 0.23f),
                    localAngles = new Vector3(90f, 30f, 0f),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            });

            rules.Add("mdlMerc", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = trackerObject,
                    childName = "Base",
                    localPos = new Vector3(0.11f, 0.29f, 0.24f),
                    localAngles = new Vector3(90f, 30f, 0f),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            });

            rules.Add("mdlLoader", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = trackerObject,
                    childName = "Base",
                    localPos = new Vector3(0.11f, 0.29f, 0.24f),
                    localAngles = new Vector3(90f, 30f, 0f),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            });

            rules.Add("mdlCaptain", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = trackerObject,
                    childName = "Base",
                    localPos = new Vector3(0.11f, 0.29f, 0.24f),
                    localAngles = new Vector3(90f, 30f, 0f),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            });

            rules.Add("mdlToolbot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = trackerObject,
                    childName = "Base",
                    localPos = new Vector3(0.11f, 0.29f, 0.24f),
                    localAngles = new Vector3(90f, 30f, 0f),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            });

            rules.Add("mdlTreebot", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = trackerObject,
                    childName = "Base",
                    localPos = new Vector3(0.11f, 0.29f, 0.24f),
                    localAngles = new Vector3(90f, 30f, 0f),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            });

            rules.Add("mdlCroco", new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = trackerObject,
                    childName = "Base",
                    localPos = new Vector3(0.11f, 0.29f, 0.24f),
                    localAngles = new Vector3(90f, 30f, 0f),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            });

            return rules;
        }*/


        protected override void SetupMaterials(GameObject modelPrefab) {
            modelPrefab.GetComponentInChildren<Renderer>().material = Modules.Assets.CreateMaterial("matRelicOfMass");
        }

        private void ModifyAccel(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            int rmassCount = GetCount(self);
            if (rmassCount > 0)
            {
                self.acceleration = self.baseAcceleration / (rmassCount * 8f);
            }
        }

        private void HandleStats(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args, Inventory inventory)
        {
            int itemCount = inventory.GetItemCount(itemDef);
            args.healthMultAdd += itemCount;
        }
    }
}

