using UnityEngine;
using System.Collections.Generic;

namespace PierreMizzi.Useful.PoolingObjects
{

    // TODO : 🟧 EnemyGroups -> Difficult, lots of setup in Editor (cf LinkID : 10)
    // TODO : 🟥 HealthFlower

    public delegate GameObject GetFromPool(GameObject gameObject);

    public delegate void ReleaseFromPool(GameObject gameObject);

    [CreateAssetMenu(fileName = "PoolingChannel", menuName = "Bitrost/PoolingChannel", order = 0)]
    public class PoolingChannel : ScriptableObject
    {
        public List<PoolConfig> pools = new List<PoolConfig>();

        public GetFromPool onGetFromPool = null;

        public ReleaseFromPool onReleaseToPool = null;

        private void OnEnable()
        {
            onGetFromPool = (GameObject gameObject) =>
            {
                return null;
            };
            onReleaseToPool = (GameObject gameObject) => { };
        }
    }
}
