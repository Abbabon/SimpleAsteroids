using UnityEngine;
using UnityEngine.Pool;

public class PrefabObjectPool<T> where T: MonoBehaviour
{
    private readonly ObjectPool<T> _pool;

    public PrefabObjectPool(T prefab, int initialPoolSize = 10, int maxPoolSize = 50)
    {
        _pool = new ObjectPool<T>(
            createFunc: () =>
            {
                var obj = Object.Instantiate(prefab);
                obj.gameObject.SetActive(false);
                return obj;
            },
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: Object.Destroy,
            collectionCheck: false,
            defaultCapacity: initialPoolSize,
            maxSize: maxPoolSize
        );

        // Prepopulate the pool
        var poolObjects = new T[initialPoolSize]; 
        for (var i = 0; i < initialPoolSize; i++)
        {
            poolObjects[i] = _pool.Get();
        }
        for (var i = 0; i < initialPoolSize; i++)
        {
            _pool.Release(poolObjects[i]);
        }
    }

    public T GetObject()
    {
        return _pool.Get();
    }

    public void ReturnObject(T obj)
    {
        _pool.Release(obj);
    }
}