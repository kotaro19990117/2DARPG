using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    //変数
    //攻撃力
    [SerializeField]
    private int attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //衝突判定oncolではなく isTrigerにチェックがついてない時oncollider チェックついていたらonTrigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "enemy")
        {
            //攻撃用の関数を呼び出す
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage, transform.position);
        }
    }
}
