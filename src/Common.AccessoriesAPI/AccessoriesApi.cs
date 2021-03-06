﻿using HarmonyLib;
using System;

namespace Common
{
    internal static class AccessoriesApi
    {
        private static UnityEngine.Object _moreAccessoriesInstance;
        private static Type _moreAccessoriesType;

        private static Func<ChaControl, int, ChaAccessoryComponent> _getChaAccessoryCmp;

        private static bool MoreAccessoriesInstalled => _moreAccessoriesType != null;
        private static bool _initialized;

        internal static ChaAccessoryComponent GetAccessory(this ChaControl character, int accessoryIndex)
        {
            if (!_initialized)
                Init();

            return _getChaAccessoryCmp(character, accessoryIndex);
        }

        private static void Init()
        {
            DetectMoreAccessories();

            if (MoreAccessoriesInstalled)
            {
                var getAccCmpM = AccessTools.Method(_moreAccessoriesType, "GetChaAccessoryComponent");
                _getChaAccessoryCmp = (control, componentIndex) => (ChaAccessoryComponent)getAccCmpM.Invoke(_moreAccessoriesInstance, new object[] { control, componentIndex });
            }
            else
            {
                _getChaAccessoryCmp = (control, i) => control.cusAcsCmp[i];
            }
            _initialized = true;
        }

        private static void DetectMoreAccessories()
        {
            try
            {
                _moreAccessoriesType = Type.GetType("MoreAccessoriesKOI.MoreAccessories, MoreAccessories, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

                if (_moreAccessoriesType != null)
                    _moreAccessoriesInstance = UnityEngine.Object.FindObjectOfType(_moreAccessoriesType);
            }
            catch
            {
                _moreAccessoriesType = null;
            }
        }
    }
}