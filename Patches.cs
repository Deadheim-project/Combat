using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace Combat
{
    public class Patches
    {
        [HarmonyPatch(typeof(Character), nameof(Character.ApplyDamage))]
        public static class ApplyDamage
        {
            public static int combatHashCode = "Combat".GetStableHashCode();

            public static void Postfix(Character __instance, HitData hit)
            {
                if (!hit.GetAttacker()) return;

                StatusEffect debuff = ObjectDB.m_instance.GetStatusEffect(combatHashCode);
                debuff.m_ttl = Combat.BuffSeconds.Value;

                if (Combat.ActivateOnlyForPVP.Value)
                {
                    if (!hit.GetAttacker().IsPlayer()) return;
                    if (!__instance.IsPlayer()) return;
                }


                if (hit.GetAttacker().IsPlayer())
                {
                    Player attacker = (Player)hit.GetAttacker();
                    if (attacker.GetSEMan().HaveStatusEffect("Combat")) attacker.GetSEMan().RemoveStatusEffect(combatHashCode);
                    attacker.GetSEMan().AddStatusEffect(debuff);
                }

                if (__instance.IsPlayer())
                {
                    Player damaged = (Player)__instance;
                    if (damaged.GetSEMan().HaveStatusEffect("Combat")) damaged.GetSEMan().RemoveStatusEffect(combatHashCode);
                    damaged.GetSEMan().AddStatusEffect(debuff);
                }

            }
        }

        [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.Awake))]
        public static class ConsumeItem
        {
            private static void Postfix(ObjectDB __instance)
            {
                __instance.m_StatusEffects.Add(StatusEffects.CreateStatusEffect("Combat", "Combat", Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(x => x.name == "mapicon_player_32")));
            }
        }

        [HarmonyPatch(typeof(Humanoid), nameof(Humanoid.IsTeleportable))]
        public static class IsTeleportable
        {
            private static bool Prefix(Humanoid __instance)
            {
                if (!Combat.PreventTeleport.Value) return true;
                if (!__instance.GetSEMan().HaveStatusEffect("Combat")) return true;

                __instance.Message(MessageHud.MessageType.Center, "You can't teleport in Combat");
                return false;
            }
        }
    }
}
