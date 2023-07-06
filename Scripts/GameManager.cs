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

    //UI2つ,表示する文章、今何行目か、文字送り判定
    public GameObject dialogBox;
    public Text dialogText;

    private string[] dialogLines;//string型の配列

    private int currentLine;

    private bool justStarted;


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
        if(dialogBox.activeInHierarchy)//ダイアログが表示されている時
        {
            if(Input.GetMouseButtonUp(1))//右クリックされると
            {
                if(!justStarted)//2回目以降の文字送りなら
                {
                    currentLine++;//右クリックされると一つ配列が進む

                    if(currentLine >= dialogLines.Length)//配列が終わると
                    {
                        dialogBox.SetActive(false);//非表示になる
                    }
                    else//まだ配列があると
                    {
                        dialogText.text = dialogLines[currentLine];//さっき一つ進んだ配列の文字がテキストに入る
                    }
                }
                else//最初のときは
                {
                    justStarted = false;//最初判定を消す＝最初のボタンクリックで文字送りをしないため
                }
            }
        }
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

    //ダイアログを表示して表示する文章を設定、ダイアログの表示切り替え
    public void ShowDialog(string[] lines)
    {
        dialogLines = lines;//引数をダイアログに入れる
        currentLine = 0;//初期化
        dialogText.text = dialogLines[currentLine];//最初のテキストを入れる
        dialogBox.SetActive(true);//表示されるようになる

        justStarted = true;//1回目のクリックを省くためのダミー
    }

    //ダイアログの表示切り替えができる
    public void ShowDialogChange(bool x)
    {
        dialogBox.SetActive(x);
    }
}
