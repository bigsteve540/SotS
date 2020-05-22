using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private const int MAX_POOL_SIZE = 50;

    private static Dictionary<IPoolable, ObjectPooler> pools = new Dictionary<IPoolable, ObjectPooler>();

    public static ObjectPooler GetPool(IPoolable _prefab)
    {
        if (pools.ContainsKey(_prefab))
        {
            if (pools[_prefab] != null)
                return pools[_prefab];
            else
                pools.Remove(_prefab);
        }

        var pool = new GameObject("Pool-" + (_prefab as Component).name).AddComponent<ObjectPooler>();
        pool.Initialize(_prefab);
        pools.Add(_prefab, pool);
        return pool;
    }

    public static ObjectPooler Prewarm(IPoolable _prefab, int _initialSize)
    {
        if (pools.ContainsKey(_prefab))
        {
            if (pools[_prefab] != null)
            {
                Debug.LogError("Pool already created, can't prewarm");
                return pools[_prefab];
            }
            else
                pools.Remove(_prefab);
        }

        var pool = new GameObject("Pool-" + (_prefab as Component).name).AddComponent<ObjectPooler>();
        pool.Initialize(_prefab, _initialSize);
        pools.Add(_prefab, pool);
        return pool;
    }

    private GameObject prefab;

    private Queue<IPoolable> objects = new Queue<IPoolable>();
    private List<IPoolable> disabledObjects = new List<IPoolable>();

    private void Initialize(IPoolable _poolablePrefab, int _initialSize = MAX_POOL_SIZE)
    {
        prefab = (_poolablePrefab as Component).gameObject;
        for (int i = 0; i < _initialSize; i++)
        {
            var pooledObject = (Instantiate(prefab) as GameObject).GetComponent<IPoolable>();
            (pooledObject as Component).gameObject.name += " " + i;

            pooledObject.OnDestroyPoolObject += () => AddObjectToAvailable(pooledObject);

            (pooledObject as Component).gameObject.SetActive(false);
        }
    }

    private void AddObjectToAvailable(IPoolable _pooledObject)
    {
        disabledObjects.Add(_pooledObject);
        objects.Enqueue(_pooledObject);
    }

    private IPoolable Get()
    {
        lock (this)
        {
            if (objects.Count == 0)
            {
                int amountToGrowPool = Mathf.Max((disabledObjects.Count / 10), 1);
                Initialize(prefab.GetComponent<IPoolable>(), amountToGrowPool);
            }

            var pooledObject = objects.Dequeue();

            return pooledObject;
        }
    }

    public IPoolable Get(Vector3 _position, Quaternion _rotation)
    {
        var pooledObject = Get();

        (pooledObject as Component).transform.position = _position;
        (pooledObject as Component).transform.rotation = _rotation;
        (pooledObject as Component).gameObject.SetActive(true);

        return pooledObject;
    }

    private void Update()
    {
        MakeDisabledObjectsChildren();
    }

    private void MakeDisabledObjectsChildren()
    {
        if (disabledObjects.Count > 0)
        {
            foreach (var pooledObject in disabledObjects)
            {
                (pooledObject as Component).transform.SetParent(transform);
            }

            disabledObjects.Clear();
        }
    }
}
