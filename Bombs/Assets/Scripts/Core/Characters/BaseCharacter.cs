using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Bombs.Core.Characters 
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class BaseCharacter : MonoBehaviour, ICharacter
    {
        private const float DESTINATION_DONE_DISTANCE = 1f;
        private const float NEW_DESTINATION_RADIUS = 10f;

        public static Action OnDie { get; set; }

        private NavMeshAgent _navAgent;
        private NavMeshAgent NavAgent
        {
            get 
            { 
                if (_navAgent == null)
                    _navAgent = GetComponent<NavMeshAgent>();
                return _navAgent;
            }
        }

        public static Dictionary<int, ICharacter> ICharacters { get; private set; } = new();

        protected virtual void OnEnable() => ICharacters[gameObject.GetInstanceID()] = this;
        protected virtual void OnDisable() => ICharacters.Remove(gameObject.GetInstanceID());

        private void MoveTo() => MoveTo(transform.position + Random.insideUnitSphere * NEW_DESTINATION_RADIUS);

        public void MoveTo(Vector3 destination)
        {
            if (!NavMesh.SamplePosition(destination, out var hit, float.MaxValue, Physics.AllLayers))
                return;

            NavAgent.SetDestination(hit.position);
        }

        private void Update()
        {
            if (NavAgent.remainingDistance < DESTINATION_DONE_DISTANCE)
                MoveTo();
        }

        public abstract void Damage(float damage);

        public abstract Vector3 GetPosition();
    }
}