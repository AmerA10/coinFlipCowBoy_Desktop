using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinThrow : MonoBehaviour
{

    public GameObject coin;
    [SerializeField]
    private int coinAmt;
    [SerializeField]
    private int maxCoinAmt;
    private bool onThrow;
    public List<GameObject> coins;

    public Transform coinThrowPos;
    // Start is called before the first frame update
    void Start()
    {
 
        coinAmt = maxCoinAmt;
       

    }

    // Update is called once per frame
    void Update()
    {

        onThrow = Input.GetButtonDown("Fire2");
        ThrowCoin();
    }

    private void ThrowCoin ()
    {
        if (coinAmt > 0 && onThrow)
        {
            GameObject throwingCoin = Instantiate(coin, coinThrowPos.position, Quaternion.identity);
            coinAmt--;
            coins.Add(throwingCoin);
            
        }

       
    }





}
