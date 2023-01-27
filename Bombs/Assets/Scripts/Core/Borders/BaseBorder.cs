using System.Collections.Generic;
using UnityEngine;

namespace Bombs.Core.Borders
{
    public abstract class BaseBorder : MonoBehaviour, IBorder
    {
        public static Dictionary<int, IBorder> IBorders { get; private set; } = new();

        private void OnEnable() => IBorders[gameObject.GetInstanceID()] = this;
        private void OnDisable() => IBorders.Remove(gameObject.GetInstanceID());
        public abstract bool Block();
        public abstract void Damage(float damage);
    }
}