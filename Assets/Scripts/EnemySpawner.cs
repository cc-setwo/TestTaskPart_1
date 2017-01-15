using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int MaxEnemyCount;

    private GameObject _prefab;
    private GameObject _enemies;
    private Vector3[] _enemiesPositions;
    private const float _timeToSpawn = 2.0f;
    private float _timeToSpawncopy;
    private static EnemySpawner _forCoroutine;
    // Use this for initialization
    void Start ()
	{
	    _timeToSpawncopy = _timeToSpawn;
	    _prefab = Resources.Load("Enemy") as GameObject;
        _enemies = gameObject;
        _enemiesPositions = new Vector3[_enemies.transform.childCount];
        for (int i = 0; i < _enemies.transform.childCount; i++)
        {
            _enemiesPositions[i] = _enemies.transform.GetChild(i).position;
        }
        _forCoroutine = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    //if (_enemies.transform.childCount < MaxEnemyCount)
	    //{
	    //    _timeToSpawncopy -= Time.deltaTime;
	    //    if (_timeToSpawncopy <= _timeToSpawn)
	    //    {
	    //        _timeToSpawncopy = _timeToSpawn;
     //           SpawnEnemy();
	    //    }
	    //}
	}

    private void SpawnEnemy()
    {
        Debug.Log(Time.deltaTime);
        for (int i = 0; i < _enemiesPositions.Length; i++)
        {
            if (_enemies.transform.FindChild("Enemy" + i) == null)
            {
                GameObject SpawnChild = Instantiate(_prefab, _enemiesPositions[i], Quaternion.identity);
                SpawnChild.transform.parent = _enemies.transform;
                SpawnChild.name = "Enemy" + i;
                break;
            }
        }
    }

    public static void SpawnEnemyStatic(int index)
    {
        _forCoroutine.StartCoroutine(_forCoroutine.SpawnEnemys(index));
    }

    public IEnumerator SpawnEnemys(int index)
    {
        yield return new WaitForSeconds(_timeToSpawn);
        GameObject SpawnedChild = Instantiate(_prefab, _enemiesPositions[index], Quaternion.identity);
        SpawnedChild.transform.parent = gameObject.transform;
        SpawnedChild.name = "Enemy" + index;
    }
}
