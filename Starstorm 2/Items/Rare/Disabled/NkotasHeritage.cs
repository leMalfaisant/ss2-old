﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace Starstorm2Unofficial.Cores.Items
{
    class NkotasHeritage : SS2Item<NkotasHeritage>
    {
        public override string NameInternal => "SS2U_ItemOnLevelUp";
        public override string Name => "Nkota's Heritage";
        public override string Pickup => "Receive an item on level up or starting the Teleporter event.";
        public override string Description => "<style=cIsUtility>Receive an item</style> on <style=cIsUtility>level up</style> or starting the <style=cIsUtility>Teleporter event</style>. Rerolls for a higher item tier <style=cIsUtility>0</style> <style=cStack>(+1 per stack)</style> times. <style=cIsUtility>Unaffected by luck</style>.";
        public override string Lore => "\"My friends. I have gathered you all here today to share some important news.\n\nFirst, it pains my heart to announce that Nkota, the last of our great leaders, our kind matriarch, has sadly passed on from this life. As far as we know, she took her last breath peacefully in rest last night, full of years and with all business here finished. With that said, there shall be no one to succeed her in leadership. She was the last of her line, with no living brothers, sisters, or children.\n\nInstead, in her final testament, her wisdom has created a new beginning. For us. For all of us. Her wish is that the remaining members of this community spread out, either together or separately, into the world. It is perhaps a drastic option, but one that I, personally, also believe to be the best for this community.\n\nNkota shall be buried at our final destination, as is her will. You all may grieve for her and this community for a time, but do not tarry. A group of pilgrims will travel with myself to the great grey path, fifty miles west, after a week's time. You may follow, or go on your own way. It is your choice entirely. If you do not think that you can make the journey, do not fret. Nkota may have had no children, yes, but her heritage is in spirit, not in blood. You are all her children, and she will protect you until you reach your final goal.\n\nThank you all for your time. I would suggest you begin to pack your possessions for the journey ahead.\"\n\n - Eulogy for Nkota and the Village of Dago";
        public override ItemTier Tier => ItemTier.Tier3;
        public override ItemTag[] Tags => new ItemTag[]
        {
            ItemTag.Utility,
            ItemTag.AIBlacklist,
            ItemTag.CannotCopy
        };
        public override string PickupIconPath => "NkotasHeritage_Icon";
        public override string PickupModelPath => "MDLNkotasHeritage";

        public override void Init()
        {
            base.Init();
            networkSound = Modules.Assets.CreateNetworkSoundEventDef("SS2UDiaryLevelUp");
        }

        public override void RegisterHooks()
        {
            On.RoR2.GlobalEventManager.OnTeamLevelUp += GlobalEventManager_OnTeamLevelUp;
            On.RoR2.TeleporterInteraction.ChargingState.OnEnter += ChargingState_OnEnter;
        }

        private static NetworkSoundEventDef networkSound;
        private static GameObject effectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/SurvivorPod/PodGroundImpact.prefab").WaitForCompletion();
        public void ProcNkotaServer(TeamIndex teamIndex)
        {
            if (!NetworkServer.active) return;
            ReadOnlyCollection<TeamComponent> teamMembers = TeamComponent.GetTeamMembers(teamIndex);
            foreach(TeamComponent teamComponent in teamMembers)
            {
                if (!teamComponent.body || !teamComponent.body.isPlayerControlled || !teamComponent.body.inventory) continue;

                int itemCount = teamComponent.body.inventory.GetItemCount(itemDef);
                if (itemCount <= 0) continue;

                //ItemCore.DropShipCall(teamComponent.body.transform, itemCount, TeamManager.instance.GetTeamLevel(teamIndex));
                ItemCore.RollNkota(teamComponent.body.transform, itemCount);
                EffectManager.SpawnEffect(effectPrefab, new EffectData
                {
                    origin = teamComponent.body.transform.position,
                    scale = 15
                }, true);
                EffectManager.SimpleSoundEffect(networkSound.index, teamComponent.body.transform.position, true);
            }
        }

        private void ChargingState_OnEnter(On.RoR2.TeleporterInteraction.ChargingState.orig_OnEnter orig, EntityStates.BaseState self)
        {
            orig(self);
            if (NetworkServer.active) ProcNkotaServer(TeamIndex.Player);
        }

        public void GlobalEventManager_OnTeamLevelUp(On.RoR2.GlobalEventManager.orig_OnTeamLevelUp orig, TeamIndex teamIndex)
        {
            orig(teamIndex);
            if (NetworkServer.active) ProcNkotaServer(teamIndex);
        }

        public override ItemDisplayRuleDict CreateDisplayRules()
        {
            displayPrefab = LegacyResourcesAPI.Load<GameObject>(PickupModelPath);
            var disp = displayPrefab.AddComponent<ItemDisplay>();
            disp.rendererInfos = Utils.SetupRendererInfos(displayPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new ItemDisplayRule[]
            {
                new ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "ThighL",
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
                    followerPrefab = displayPrefab,
                    childName = "ThighL",
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
                    followerPrefab = displayPrefab,
                    childName = "ThighL",
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
                    followerPrefab = displayPrefab,
                    childName = "ThighL",
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
                    followerPrefab = displayPrefab,
                    childName = "ThighL",
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
                    followerPrefab = displayPrefab,
                    childName = "ThighL",
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
                    followerPrefab = displayPrefab,
                    childName = "ThighL",
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
                    followerPrefab = displayPrefab,
                    childName = "ThighL",
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
                    followerPrefab = displayPrefab,
                    childName = "ThighL",
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
                    followerPrefab = displayPrefab,
                    childName = "ThighL",
                    localPos = new Vector3(0.11f, 0.29f, 0.24f),
                    localAngles = new Vector3(90f, 30f, 0f),
                    localScale = new Vector3(0.1f, 0.1f, 0.1f)
                }
            });

            return rules;
        }
    }
}

