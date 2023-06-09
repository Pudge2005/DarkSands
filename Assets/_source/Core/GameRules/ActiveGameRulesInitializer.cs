﻿using System;
using Game.Core.TwoDInThreeD;
using UnityEngine;

namespace Game.Core.GameRules
{
    public sealed class ActiveGameRulesInitializer : MonoBehaviour
    {
        [SerializeField] private ArmorCalculationMethod _armorCalculationMethod;
        [SerializeField] private LayerMask _ignoreCharactersLayer;
        [SerializeField] private DirectionsCompositeSo _directionsComposite;


        private void Awake()
        {
            ActiveGameRules.ArmorCalculationMethod = _armorCalculationMethod;
            int layerID = -1;
            int bitMask = _ignoreCharactersLayer;

            for (int i = 0; i < 32; i++)
            {
                if (CheckBit(bitMask, i))
                {
                    layerID = i;
                    break;
                }
            }

            ActiveGameRules.IgnoreCharactersLayer = layerID;
            ActiveGameRules.DirectionsComposite = _directionsComposite;
        }

        private static bool CheckBit(int value, int bitPos)
        {
            int x = value >> bitPos;
            return (x & 1) == 1;
        }
    }
}
