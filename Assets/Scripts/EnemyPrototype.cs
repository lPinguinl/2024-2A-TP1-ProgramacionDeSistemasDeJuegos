using UnityEngine;

namespace Enemies
{
    public abstract class EnemyPrototype : MonoBehaviour
    {
        public abstract EnemyPrototype Clone(); // Method for cloning the enemy
    }
}