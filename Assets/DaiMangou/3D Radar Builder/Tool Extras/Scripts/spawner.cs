using System;
using System.Collections;
using System.Collections.Generic;
using DaiMangou.RadarBuilder3D;
using UnityEngine;
using Random = UnityEngine.Random;

public class spawner : MonoBehaviour
{
    [TextArea] public string info =
        "This example script show how to trigger a manual search for new objects for the radar to track. You should also call this when you decide to add more objects to a pool then you told the radar would be there at start.  Simply call 'DaiMangou.ProRadarBuilder._3DRadar.doInstanceObjectCheck();'";

    public List<Transform> ObjectsToSpawn = new List<Transform>();

    public bool SearchAtEndOfSpawning;
    public bool SearchNow;
    public int SpawnCount;
    public int SpawnLimit;
    public float SpawnRadius = 100;
    public float SpawnTime = 2f;

    private IEnumerator Start()
    {
        while (true)
        {
            while (SpawnCount < SpawnLimit)
                yield return StartCoroutine(DropBomb());

            yield return null;
        }
    }

    private IEnumerator DropBomb()
    {
        yield return new WaitForSeconds(SpawnTime);

        foreach (var o in ObjectsToSpawn)
        {
            Instantiate(o,
                new Vector3(Random.Range(-SpawnRadius, SpawnRadius), Random.Range(-SpawnRadius, SpawnRadius),
                    Random.Range(-SpawnRadius, SpawnRadius)), Quaternion.identity);
            SpawnCount += 1;
        }
    }

    private void Update()
    {
        if (SearchAtEndOfSpawning && SpawnLimit == SpawnCount)
        {
            try
            {
                _3DRadar.doInstanceObjectCheck();
            }
            catch
            {
               // none
            }

            SearchAtEndOfSpawning = false;
        }


        if (!SearchNow) return;
        try
        {
            _3DRadar.doInstanceObjectCheck();
        }
        catch
        {
            // none
        }


        SearchNow = false;
    }
}