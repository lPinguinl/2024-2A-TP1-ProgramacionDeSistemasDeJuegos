using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] private List<EnemyData> enemyDataList; // List of enemy data Scriptable Objects

        public EnemyData GetEnemyData(string enemyType)
        {
            foreach (var data in enemyDataList)
            {
                if (data.enemyType == enemyType)
                {
                    return data; // Return the matching enemy data
                }
            }

            Debug.LogError($"Enemy type {enemyType} not found!");
            return null; // Return null if no matching type found
        }
    }
}