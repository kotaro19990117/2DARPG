using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    //変数
    //アイテム回復良。アイテムが消えるまでの時間、取得できるようになるまでの時間
    public int HealthItemRecoveryValue;
    [SerializeField]
    private float lifeTime;
    public float waitTime;//Itemが生成されるときにPlayerが重なってると一瞬で取ってしまう、判別のため
    // Start is called before the first frame update
    void Start()
    {
        //時間経過で削除
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
    }
}
