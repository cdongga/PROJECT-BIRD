using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class spawnCoin : MonoBehaviour
{
    public GameObject coinPrefab;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Vector3 randomPosition=new Vector3(Random.Range(10,20),0, Random.Range(10,20));
            Instantiate(coinPrefab,randomPosition,Quaternion.identity);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bird")
        {
            Vector3 randomPosition = new Vector3(Random.Range(10, 20), 0, Random.Range(10, 20));
            Instantiate(coinPrefab, randomPosition, Quaternion.identity);
        }
    }
}
