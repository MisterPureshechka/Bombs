using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Bombs.Core
{
    public class BombThrower : MonoBehaviour
    {
        [SerializeField] private GameObject _bombPrefab;
        [SerializeField] private float _coolDown;
        [SerializeField] private float _spawnHeight;

        private float _lastSpawnTime;

        private Stack<GameObject> _bombs = new ();

        private void Update()
        {
            if (!Input.GetMouseButtonUp(0))
                return;

            if (!Physics.Raycast(Camera.allCameras[0].ScreenPointToRay(Input.mousePosition), out var hit))
                return;

            if ((Time.time - _lastSpawnTime) < _coolDown)
                return;

            if (!NavMesh.SamplePosition(hit.point, out var navHit, float.MaxValue, Physics.AllLayers))
                return;

            var bombPosition = navHit.position + Vector3.up * _spawnHeight;

            _lastSpawnTime = Time.time;

            GameObject bomb = null;
            
            foreach (var tempBomb in _bombs)
            {
                if (tempBomb.activeSelf)
                    continue;

                bomb = tempBomb;
                
                break;
            }
            if (bomb == null)
            {
                bomb = Instantiate(_bombPrefab);
                _bombs.Push(bomb);
            }
            bomb.transform.position = bombPosition;
            bomb.SetActive(true);
        }
    }
}