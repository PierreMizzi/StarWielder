using UnityEngine;
using System.Collections.Generic;

namespace PierreMizzi.Useful.PoolingObjects
{
    public delegate GameObject GetFromPool(GameObject gameObject);

    public delegate void ReleaseFromPool(GameObject gameObject);

    public delegate void PoolConfigDelegate(PoolConfig config);

    [CreateAssetMenu(fileName = "PoolingChannel", menuName = "Bitrost/PoolingChannel", order = 0)]
    public class PoolingChannel : ScriptableObject
    {
        public List<PoolConfig> pools = new List<PoolConfig>();

        public PoolConfigDelegate onCreatePool = (PoolConfig config) => { };

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
