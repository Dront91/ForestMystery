using System;
using UnityEngine;

namespace MysteryForest
{
    public class GodMode : MonoBehaviour
    {
        public Action<bool> OnGodModeEnable;
        public void GodModeEnable(bool state)
        {
            OnGodModeEnable?.Invoke(state);
        }
    }
}
