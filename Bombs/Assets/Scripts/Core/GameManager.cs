using Bombs.Core.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bombs.Core
{
    public class GameManager : MonoBehaviour
    {
        private void OnEnable() => BaseCharacter.OnDie += TryRestart;
        private void OnDisable() => BaseCharacter.OnDie -= TryRestart;

        private void TryRestart()
        {
            if (BaseCharacter.ICharacters.Count > 0)
                return;

            SceneManager.LoadScene(0);
        }
    }
}