using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //変数作成
    [SerializeField, Tooltip("移動スピードです")]//privateでもeditorで編集できる, マウスカーソルで補足説明できる
    private int moveSpeed;

    [SerializeField]
    private Animator playerAnim;

    public Rigidbody2D rb;

    [SerializeField]
    private Animator weaponAnim;

    //(現在のHPと最大HP)
    [System.NonSerialized]//Inspectorから消せる
    public int currentHealth;
    public int maxHealth;

    //(現在のSTと最大ST)
    [System.NonSerialized]//Inspectorから消せる
    public float currentStamina;

    //スタミナ回復速度
    public float totalStamina, recovelySpeed;
    
    //ダッシュの速度、長さ、スタミナ消費量
    [SerializeField]
    private float dashSpeed, dashLength, dashCost;
    //タイマー、移動にかけるようの変数
    private float dashCounter, activeMoveSpeed;

    //吹き飛び判定、吹き飛ぶ方向、吹き飛び時間、吹き飛び力、タイマー
    private bool isKnockingback;
    private Vector2 knockDir;
    [SerializeField]
    private float knockbackTime, knockbackForce;
    private float knockbackCounter;
    //無敵時間、タイマー
    [SerializeField]
    private float invincibilityTime;
    private float invincibilityCounter;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        GameManager.instance.UpdateHealthUI();//staticにしたので呼び出せる

        currentStamina = totalStamina;
        GameManager.instance.UpdateStaminaUI();//staticにしたので呼び出せる
        
        activeMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //無敵時間の判定と無敵時のコード取得
        if(invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
        }

        if(isKnockingback)
        {
            knockbackCounter -= Time.deltaTime;
            rb.velocity = knockDir * knockbackForce;

            if(knockbackCounter <= 0)
            {
                isKnockingback = false;
            }
            else
            {
                return;//knockback中はこれ以降のUpdateの処理がなくなる＝Playerの動きが制御できない
            }
        }

        //キー入力を受け取り移動させるコード記述
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") ).normalized * activeMoveSpeed;//unity>-projectsettingで自動設定

        //方向とアニメーションの連動
        if(rb.velocity != Vector2.zero)//物体の速度が、xyの値がゼロではない。という判定
        {
            if(Input.GetAxisRaw("Horizontal") != 0)//横軸の入力を受け取っているとき
            {
                if(Input.GetAxisRaw("Horizontal") > 0)
                {
                    playerAnim.SetFloat("X", 1f);
                    playerAnim.SetFloat("Y", 0);

                    weaponAnim.SetFloat("X", 1f);
                    weaponAnim.SetFloat("Y", 0);

                }
                else
                {
                    playerAnim.SetFloat("X", -1f);
                    playerAnim.SetFloat("Y", 0);

                    weaponAnim.SetFloat("X", -1f);
                    weaponAnim.SetFloat("Y", 0);

                }
            }
            else if(Input.GetAxisRaw("Vertical") > 0)//横軸の入力を受け取らず、縦軸の入力(右)を受け取っている時
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", 1f);

                weaponAnim.SetFloat("X", 0);
                weaponAnim.SetFloat("Y", 1f);

            }
            else
            {
                playerAnim.SetFloat("X", 0);
                playerAnim.SetFloat("Y", -1f);

                weaponAnim.SetFloat("X", 0);
                weaponAnim.SetFloat("Y", -1f);

            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            weaponAnim.SetTrigger("Attack");
        }

        if(dashCounter <= 0)//dashしてないとき
        {
            if(Input.GetKeyDown(KeyCode.Space) && currentStamina > dashCost)//必要最低限のSTがあって
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;

                currentStamina -= dashCost;

                GameManager.instance.UpdateStaminaUI();
            }
        }
        else//dash中のとき
        {
            dashCounter -= Time.deltaTime;
            if(dashCounter <= 0)//dashカウンター切れで
            {
                activeMoveSpeed = moveSpeed;//元に戻る
            }
        }

        //Staminaの自動回復
        currentStamina = Mathf.Clamp(currentStamina + recovelySpeed * Time.deltaTime, 0, totalStamina);
        GameManager.instance.UpdateStaminaUI();

    }
    ///<summary>
    ///吹き飛ばし用関数
    ///</summary>
    ///<program name="position"></target>
    public void KnockBack(Vector3 position)//引数はenemyの現在地
    {
        knockbackCounter = knockbackTime;
        isKnockingback = true;

        knockDir = transform.position - position;//player-enermy
        knockDir.Normalize();
    }

    public void DamagePlayer(int damage)
    {
        if(invincibilityCounter <= 0)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);//HPが負にならないように丸める
            invincibilityCounter = invincibilityTime;
        }
        if(currentHealth == 0)//死んだら
        {
            gameObject.SetActive(false);//Playerを非表示にする
        }
    GameManager.instance.UpdateHealthUI();//UIを更新する
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Portion" && maxHealth > currentHealth && collision.GetComponent<Items>().waitTime <= 0)
        {
            Items items = collision.GetComponent<Items>();
            currentHealth = Mathf.Clamp(currentHealth + items.HealthItemRecoveryValue, 0, maxHealth);
            GameManager.instance.UpdateHealthUI();
            Destroy(collision.gameObject);
        }
    }
}

