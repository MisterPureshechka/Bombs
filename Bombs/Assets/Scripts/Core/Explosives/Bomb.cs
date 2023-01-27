using Bombs.Core.Borders;
using Bombs.Core.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bombs.Core.Explosives
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Bomb : MonoBehaviour
    {
        private const int RAYCAST_BUFFER = 100;

        [SerializeField] private float _blowTimer;
        [SerializeField] private float _radius;
        [SerializeField] private float _damage;
        [SerializeField] private GameObject _body;
        [SerializeField] private GameObject _marker;

        private RaycastHit[] _hitsBuffer = new RaycastHit[RAYCAST_BUFFER];
        private List<IDamageable> _iDamageables = new();

        private Collider _coll;
        private Collider Coll
        {
            get
            {
                if (_coll == null)
                    _coll = GetComponent<Collider>();
                return _coll;
            }
        }

        private void OnEnable() 
        {
            Coll.isTrigger = true;
        } 

        private void OnTriggerEnter(Collider other)
        {
            if (_ticking != null)
                return;

            _ticking = Ticking();
            StartCoroutine(_ticking);
        }

        private IEnumerator _ticking;
        private IEnumerator Ticking()
        {
            _marker.transform.localScale = 2f * _radius * Vector3.one;
            _marker.SetActive(true);

            yield return new WaitForSeconds(_blowTimer);

            _marker.SetActive(false);

            Blow();

            _ticking = null;
            gameObject.SetActive(false);            
        }

        public void Blow()
        {
            _iDamageables.Clear();
            foreach(var target in BaseCharacter.ICharacters)
            {
                var heading = target.Value.GetPosition() - transform.position;
                var distance = heading.magnitude;

                if (distance > _radius)
                    continue;

                var ray = new Ray(transform.position, heading / distance);
                var bufferLenght = Physics.RaycastNonAlloc(ray, _hitsBuffer, distance);
                ICharacter targetCharacter = null;
                for (var  i = 0; i < bufferLenght; i++)
                {
                    var instanceId = _hitsBuffer[i].collider.gameObject.GetInstanceID();

                    if (BaseBorder.IBorders.TryGetValue(instanceId, out var border))
                    {
                        _iDamageables.Add(border);
                        targetCharacter = null;
                        if (border.Block())
                            break;
                    }

                    BaseCharacter.ICharacters.TryGetValue(instanceId, out targetCharacter);
                }

                if (targetCharacter != null)
                    _iDamageables.Add(targetCharacter);
            }

            foreach (var damageable in _iDamageables)
                damageable.Damage(_damage);
        }
    }
}