using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

namespace PierreMizzi.Useful.PoolingObjects
{

    // TODO : 🟥 Released objects are put back into their container

    public delegate void GameObjectDelegate(GameObject value);

    public class PoolingManager : MonoBehaviour
    {

        #region MonoBehaviour

        private void Awake()
        {
            Subscribe();
            InitiliazePools();
        }


        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            if (m_poolingChannel != null)
            {
                m_poolingChannel.onGetFromPool += CallbackGetFromPool;
                m_poolingChannel.onReleaseToPool += CallbackReleaseToPool;
            }
        }

        private void Unsubscribe()
        {
            if (m_poolingChannel != null)
            {
                m_poolingChannel.onGetFromPool -= CallbackGetFromPool;
                m_poolingChannel.onReleaseToPool -= CallbackReleaseToPool;
            }
        }

        #endregion

        #region Pooling Channel

        [SerializeField] private PoolingChannel m_poolingChannel = null;

        private void CallbackReleaseToPool(GameObject gameObject)
        {
            if (m_objectPools.ContainsKey(gameObject.name))
            {
                m_objectPools[gameObject.name].Release(gameObject);
            }
        }

        private GameObject CallbackGetFromPool(GameObject gameObject)
        {
            if (m_objectPools.ContainsKey(gameObject.name))
                return m_objectPools[gameObject.name].Get();
            else
                return null;
        }

        private void CreatePoolFromConfig(PoolConfig config)
        {
            if (!m_objectPools.ContainsKey(config.prefab.name))
            {
                // Creates a container
                GameObject container = new GameObject();
                container.name = $"PoolContainer_{config.prefab.name}";
                container.transform.parent = m_poolsContainer;

                // Creates a special method for creating pool instances
                System.Func<GameObject> create = () =>
                {
                    GameObject gameObject = Instantiate(
                        config.prefab,
                        Vector3.zero,
                        Quaternion.identity,
                        container.transform
                    );
                    gameObject.name = config.prefab.name;
                    return gameObject;
                };

                // Create a new pool
                ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
                    create,
                    config.onGetFromPool ?? GetFromPool,
                    config.onReleaseToPool ?? ReleaseToPool,
                    config.onDestroyInPool ?? DestroyInPool,
                    true,
                    config.defaultSize,
                    config.maxSize
                );

                // Add the pool in the dictionnary
                m_objectPools.Add(config.prefab.name, pool);
            }
        }

        #endregion

        #region PoolingBehaviour

        [SerializeField] private Transform m_poolsContainer = null;

        private Dictionary<string, ObjectPool<GameObject>> m_objectPools =
            new Dictionary<string, ObjectPool<GameObject>>();

        private void InitiliazePools()
        {
            foreach (PoolConfig config in m_poolingChannel.pools)
            {
                CreatePoolFromConfig(config);
            }
        }

        private void ReleaseToPool(GameObject poolObject)
        {
            poolObject.SetActive(false);
        }

        private void GetFromPool(GameObject poolObject)
        {
            poolObject.SetActive(true);
        }

        private void DestroyInPool(GameObject poolObject)
        {
            Destroy(poolObject);
        }

        #endregion



    }
}
