using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //static
    public static GameManager instance;

    //変数の宣言（UI、PlayerController）
    [SerializeField]
    private Slider hpSlider;//hpSliderに入れたSlider型の情報をとってくる
    [SerializeField]
    private PlayerController player;

    //STスライダー
    [SerializeField]
    private Slider stSlider;


    private void Awake()
    {
        if(instance==null)
        {
            instance = this;//インスタンスに何もなかったらGMをinstanceに入れる
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthUI()//PlayerControllerのHPをUIに適用
    {
        hpSlider.maxValue = player.maxHealth;//hpSlider.maxValueはSliderの中の値
        hpSlider.value = player.currentHealth;
    }

     //UI更新用の関数作成, PlayerControllerをUIの橋渡しとしてGMが機能している
    public void UpdateStaminaUI()//PlayerControllerのSTをUIに適用
    {
        stSlider.maxValue = player.totalStamina;//stSlider.maxValueはSliderの中の値
        stSlider.value = player.currentStamina;
    }
}
