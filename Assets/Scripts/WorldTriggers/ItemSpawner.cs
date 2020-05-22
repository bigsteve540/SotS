using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;
    public static Dictionary<int, ItemDrop> dropsInWorld = new Dictionary<int, ItemDrop>();
    public static ItemPickedUpHandler OnPickedUpItem;
    private static int nextDropID = 1;

    public delegate void ItemPickedUpHandler(int _index);

    public ItemDrop ItemDropPrefab;

    private ObjectPooler dropObjectPool;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        OnPickedUpItem += ItemPickedUp;

        dropObjectPool = ObjectPooler.GetPool(ItemDropPrefab);

        StartCoroutine(SpawnItem());
    }

    public void DropItem(Dimension _d, Vector2 _pos, GeneratorSettings _settings)
    {
        Vector3 worldPos = new Vector3(_pos.x, _pos.y, (int)_d);

        ItemDrop drop = dropObjectPool.Get(worldPos, Quaternion.identity) as ItemDrop;

        Item item = ItemGenerator.GenerateItem(_settings);

        drop.Initialize(nextDropID, item, _d);
        dropsInWorld.Add(nextDropID, drop);

        ServerSend.ItemSpawned(nextDropID, worldPos);

        nextDropID++;
    }

    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(5f);

        DropItem(Dimension.overworld, Vector2.zero, new GeneratorSettings());
    }

    private void ItemPickedUp(int _index)
    {
        dropsInWorld[_index].gameObject.SetActive(false);
        dropsInWorld.Remove(_index);
    }
}
