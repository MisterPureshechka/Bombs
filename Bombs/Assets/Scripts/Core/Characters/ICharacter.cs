using System;
using UnityEngine;

namespace Bombs.Core.Characters
{
    public interface ICharacter: IDamageable
    {
        Vector3 GetPosition();
        void MoveTo(Vector3 destination);
    }
}