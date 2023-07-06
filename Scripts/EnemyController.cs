using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{


    //変数
    //（剛体、アニメーション(Editorから取得)、動く速度、待ち時間、動く時間。タイマー、動く方向、移送範囲）
    private Rigidbody2D rb;
    private Animator enemyAnim;

    [SerializeField]
    private float moveSpeed, waitTime, walkTime;

    private float waitCounter, moveCounter;

    private Vector2 moveDir;

    [SerializeField]
    private BoxCollider2D area;

    //追いかける判定、追いかけている判定、追いかける速度、気づく範囲、プレイヤーの位置
    [SerializeField, Tooltip("プレイヤーを追いかける？")]
    private bool chase;
    private bool isChasing;
    [SerializeField]
    private float chaseSpeed, rangeToChase;
    private Transform target;

    //攻撃間隔、攻撃力
    [SerializeField]
    private float waitAfterHitting;
    [SerializeField]
    private int attackDamage;

    //現在のHP、ノックバック中判定、吹き飛び時間、吹き飛び力、タイマー、吹き飛ぶ方向
    [SerializeField]
    private int currentHealth;
    private bool isKnockingback;
    [SerializeField]
    private float knockbackTime, knockbackForce;
    private float knockbackCounter;
    private Vector2 knockDir;

    //ドロップアイテム、ドロップ率
    [SerializeField]
    private GameObject portion;
    [SerializeField]
    private float healthDropChance;


    // Start is called before the first frame update
    void Start()
    {
        //Startでコンポーネント取得、タイマーの設定
        rb = GetComponent<Rigidbody2D>();//Startと同時にEnemyContorollerについているrbを取得
        enemyAnim = GetComponent<Animator>();

        waitCounter = waitTime;

        //プレイヤーの位置を取得
        target = GameObject.FindGameObjectWithTag("Player").transform;//GameObjectをtagで探す＝時間がかかる
    }

    // Update is called once per frame
    void Update()
    {
        //Update:ノックバック関係のコード
        if(isKnockingback)
        {
            if(knockbackCounter > 0)
            {
                knockbackCounter -= Time.deltaTime;
                rb.velocity = knockbackForce * knockDir;
            }
            else//吹っ飛びが終わった時
            {
                rb.velocity = Vector2.zero;
                isKnockingback = false;
            }

            return;//これ以降の処理が行われずにUpdateから抜け出す＝さまよえない
        }

        //追いかける判定次第ではプレイヤーを追いかけるように
        if(!isChasing)
        {
            //さまよう
            //Updateでタイマーを減らし、移動先を決めるコード記述
            if(waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
                rb.velocity = Vector2.zero;//動きを止める

                if(waitCounter <= 0)
                {
                    moveCounter = walkTime;

                    enemyAnim.SetBool("moving", true);

                    moveDir = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
                    moveDir.Normalize();//正規化して返す
                }
            }
            else
            {
                moveCounter -= Time.deltaTime;

                rb.velocity = moveDir * moveSpeed;

                if(moveCounter <= 0)
                {
                    enemyAnim.SetBool("moving", false);

                    waitCounter = waitTime;
                }
            }

            //待ってる間に追いかけている判定をつける
            if(chase)
            {
                if(Vector3.Distance(transform.position, target.transform.position) < rangeToChase)//EnemyとPlayerの距離＜範囲
                {
                    isChasing = true;
                }
            }
        }
        else//isChasing = true
        {
            if(waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
                rb.velocity = Vector2.zero;

                if(waitCounter <= 0)
                {
                    enemyAnim.SetBool("moving", true);
                }
            }
            else
            {
                moveDir = target.transform.position - transform.position;//動く方向を決めている
                moveDir.Normalize();

                rb.velocity = moveDir * chaseSpeed;
            }

            if(Vector3.Distance(transform.position, target.transform.position) > rangeToChase)//EnemyとPlayerの距離が十分離れたら追いかけるのをやめる
                {
                    isChasing = false;
                    waitCounter = waitTime;
                    enemyAnim.SetBool("moving", false);
                }
        }
        
        

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, area.bounds.min.x + 1, area.bounds.max.x - 1),
            Mathf.Clamp(transform.position.y, area.bounds.min.y + 1, area.bounds.max.y - 1),
            transform.position.z);//Clampは数を丸める,→現在地が指定範囲外であればx,yをエリアの最小の+1~最大の-1に丸める
    }


    //衝突判定の追加
    private void OnCollisionEnter2D(Collision2D collision)//Unityのcollider同士が衝突したら呼び出される関数
    {
        if(collision.gameObject.tag == "Player")
        {
            if(isChasing)//追いかけている時に当たったのなら攻撃
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                player.KnockBack(transform.position);
                player.DamagePlayer(attackDamage);

                waitCounter = waitAfterHitting;//攻撃間隔
                enemyAnim.SetBool("moving", false);
            }
        }
    }

    //関数：ノックバック、ダメージ
    public void KnockBack(Vector3 position)//weapon.transform.positionが入る
    {
        isKnockingback = true;
        knockbackCounter = knockbackTime;

        knockDir = transform.position - position;//enemy-weapon
        knockDir.Normalize();

        enemyAnim.SetBool("moving", false);
    }

    public void TakeDamage(int damage, Vector3 position)//weaponの攻撃力
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            //攻撃関数にドロップ判定を追加する
            if(Random.Range(0,100) < healthDropChance && portion != null)//ポーションが設定されている時
            {
                Instantiate(portion, transform.position, transform.rotation);//GOを生成する(もの,座標位置,回転位置)
            }

            Destroy(gameObject);//enemycontrollerがアタッチされているgameobjectを壊すことができる
        }

        KnockBack(position);//上の関数に入ってKnockBackがtrueになりUpdateでknockbackの動きが起きる(velocity)

    }
}
