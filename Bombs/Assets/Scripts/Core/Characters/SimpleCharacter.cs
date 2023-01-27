using UnityEngine;

namespace Bombs.Core.Characters
{
    public class SimpleCharacter : BaseCharacter
    {
        private const float START_HEALTH = 100f;

        private float _health;

        private void Awake() => _health = START_HEALTH;

        public override void Damage(float damage)
        {
            _health -= damage;

            if (_health > 0f)
                return;

            gameObject.SetActive(false);
            OnDie.Invoke();
        }

        public override Vector3 GetPosition() => transform.position;
    }
}