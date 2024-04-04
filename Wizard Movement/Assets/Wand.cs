using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{

    public Transform spellSpawnPoint;
    public GameObject spellPrefab;
    public float spellSpeed;

    public float mana;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && (mana >= 10)) {
            var bullet = Instantiate(spellPrefab, spellSpawnPoint.position, spellSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = spellSpawnPoint.forward * spellSpeed;
            mana -= 10;
        }
    }
}
