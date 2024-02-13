## Pooling Manager Extension

### Setup : 

1. Create a ``PoolingChannel`` ScriptableObject
2. Create an Empty GameObject as ``PoolingManager``
3. Assign it the ``PoolingChannel`` to ``PoolingManager``
4. Set-up ``PoolConfig`` in ``PoolingChannel``

### Use :

- Reference the same prefab used in a ``PoolConfig`` in any given script
- With the key and the Channel, you can pool any object :

``` c#
	// TODO : 🟥 Améliorer ça
	EnemyBullet bullet = m_poolingChannel.onGetFromPool.Invoke(m_bulletPrefab.gameObject).GetComponent<EnemyBullet>();
	
```
- With the Channel, you can release a pooledObject
``` c#
	// EnemyBullet script, when released
	m_poolingChannel.onReleaseToPool.Invoke(gameObject);
```

### Change log

v0.2 :
- 

- Initiliaze pools from poolChannel
