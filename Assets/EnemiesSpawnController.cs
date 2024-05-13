using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemiesSpawnController : MonoBehaviour
{
    [SerializeField] private Enemy _enemyPrefab;

    [Header ("Transforms")]
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
    [SerializeField] private Transform _parent;
    [SerializeField] private Transform _enemyTarget;

    [Header("Amount parameters")]
    [SerializeField] private int _maxAtOnce;
    private int _spawned;

    [SerializeField] private int _enemiesDefeatedToWin;
    private int _currentlyDefeated;

    [Header("Time periods")]
    [SerializeField] private float _enemySpawnRate;
    private WaitForSeconds _enemySpawnTime;

    private ObjectPool<Enemy> _enemiesPool;

    private void Start()
    {
        _enemiesPool = new ObjectPool<Enemy>(OnCreateEnemy, null, null, OnEnemyDestroy, false, 10, 40);
        _enemySpawnTime = new WaitForSeconds(_enemySpawnRate);
        StartCoroutine(Spawn());
    }

    #region Methods for pool creation
    private Enemy OnCreateEnemy()
    {
        Enemy newEnemy = Instantiate(_enemyPrefab, _parent);
        newEnemy.Create(_enemyTarget);
        return newEnemy;

    }
    private void OnEnemyDestroy(Enemy enemy)
    {
        _currentlyDefeated++;
        Destroy(enemy.gameObject);
    }
    #endregion

    IEnumerator Spawn()
    {
        while(true)
        {
            yield return _enemySpawnTime;
            if (_spawned >= _maxAtOnce)
            {
                continue;
            }
            var enemy = _enemiesPool.Get();
            enemy.Initialize(KillEnemy);
            //Getting random spawn pont
            int rnd = Random.Range(0, _spawnPoints.Count);
            enemy.transform.position = _spawnPoints[rnd].position;
            enemy.transform.SetParent(_parent);

            if(_currentlyDefeated >= _enemiesDefeatedToWin)
            {
                Debug.Log("Horray, Victory!");
                break;
            }
        }
    }
    private void KillEnemy(Enemy enemy) => _enemiesPool.Release(enemy);
}
