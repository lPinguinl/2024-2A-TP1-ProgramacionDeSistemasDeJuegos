using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public int damage;
        public float attackCooldown;
        public string enemyType; 
    }
}