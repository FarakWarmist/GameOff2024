using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private List<Transform> _keySpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> _safeBoxSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> _specialItemSpawnPoints = new List<Transform>();
    [SerializeField] private List<Transform> _itemForNPCSpawnPoints = new List<Transform>();
    [SerializeField] private Transform _exitKeySpawnPoint;
    [Header("Prefabs")]
    [SerializeField] private GameObject _key1;
    [SerializeField] private GameObject _key2;
    [SerializeField] private GameObject _safeBox;
    [SerializeField] private GameObject _specialItem;
    [SerializeField] private GameObject _exitKey;
    [SerializeField] private GameObject _itemForNPC;

    void Start()
    {
        GetTheSpawnPoints();
        SpawnItems();
    }

    private void SpawnItems()
    {
        SpawnKeys();

        int rand = Random.Range(0, _safeBoxSpawnPoints.Count);
        Instantiate(_safeBox, _safeBoxSpawnPoints[rand].position, _safeBoxSpawnPoints[rand].rotation);

        foreach(Transform t in _specialItemSpawnPoints)
            Instantiate(_specialItem, t.position, Quaternion.identity);

        rand = Random.Range(0, _itemForNPCSpawnPoints.Count);
        Instantiate(_itemForNPC, _itemForNPCSpawnPoints[rand].position, Quaternion.identity);
    }

    private void SpawnKeys()
    {
        int rand1 = Random.Range(0, _keySpawnPoints.Count);
        int rand2 = Random.Range(0, _keySpawnPoints.Count);

        int i = 0;
        while(rand1 == rand2 && i < 30)
        {
            rand2 = Random.Range(0, _keySpawnPoints.Count);
            i++;
        }

        Instantiate(_key1, _keySpawnPoints[rand1].position, Quaternion.identity);
        Instantiate(_key2, _keySpawnPoints[rand2].position, Quaternion.identity);

        Instantiate(_exitKey, _exitKeySpawnPoint.position, Quaternion.identity);
    }

    private void GetTheSpawnPoints()
    {
        Transform keys = transform.GetChild(0);
        foreach(Transform t in keys)
            _keySpawnPoints.Add(t);

        Transform safeBox = transform.GetChild(1);
        foreach(Transform t in safeBox)
            _safeBoxSpawnPoints.Add(t);

        Transform keyFragments = transform.GetChild(2);
        foreach(Transform t in keyFragments)
            _specialItemSpawnPoints.Add(t);

        Transform itemForNPC = transform.GetChild(3);
        foreach(Transform t in itemForNPC)
            _itemForNPCSpawnPoints.Add(t);

        if(_exitKeySpawnPoint == null) Debug.LogError("Exit Key Spawn Point is NULL");
    }
}
