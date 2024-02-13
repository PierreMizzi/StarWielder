## Pooling Manager Extension

### Setup : 

1. Create a ``PoolingChannel`` ScriptableObject
2. Create an Empty GameObject as ``PoolingManager``
3. Assign it the PoolingChannel ScriptableObject
4. Now in any given script, create a PoolConfig property and set it up.
5. Invoke PoolingChannel ``onCreatePool`` to initialize the pool
6. Now you can call  ``onGetFromPool`` and ``onReleaseToPool`` to access the pool 