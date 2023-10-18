using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Periodically spawns the specified prefab in a random location.
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// Object to spawn
    /// </summary>
    public GameObject Prefab;

    /// <summary>
    /// Seconds between spawn operations
    /// </summary>
    public float SpawnInterval = 20;

    /// <summary>
    /// How many units of free space to try to find around the spawned object
    /// </summary>
    public float FreeRadius = 10;

    [SerializeField]
    private float timeSinceLastSpawn = 0;

    /// <summary>
    /// Check if we need to spawn and if so, do so.
    /// </summary>
    //ReSharper disable once UnusedMember.Local
    void Update()
    {
        if(timeSinceLastSpawn <= 0)
        {
            SpawnEnemy();
            timeSinceLastSpawn = SpawnInterval;
        }
        else
        {
            timeSinceLastSpawn -= Time.deltaTime;
        }
    }

    public void SpawnEnemy()
    {
        var spawnPoint = SpawnUtilities.RandomFreePoint(FreeRadius);
        Instantiate(Prefab, spawnPoint, Quaternion.identity);
    }
}
