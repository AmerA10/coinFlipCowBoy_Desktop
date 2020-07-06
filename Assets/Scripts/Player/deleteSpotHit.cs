using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteSpotHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySpotHit());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator DestroySpotHit()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(this.gameObject);
    }
}
