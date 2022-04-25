using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace Combat
{
    public class StatusEffects
    {
        public static SE_Stats CreateStatusEffect(string effectName, string m_name, Sprite icon)
        {
            SE_Stats effect = ScriptableObject.CreateInstance<SE_Stats>();
            effect.name = effectName;
            effect.m_name = m_name;
            effect.m_tooltip = m_name;
            effect.m_icon = icon;

            return effect;
        }
    }
}
