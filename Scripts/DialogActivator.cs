using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{

    //会話文章、ダイアログの表示判定
    [SerializeField,Header("会話文章"),Multiline(3)]
    private string[] lines;

    private bool canActivator;





    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && canActivator && !GameManager.instance.dialogBox.activeInHierarchy)//右クリックかつアクティブダイアログかつヒエラルキー上にダイアログが表示されていない時
        {
            GameManager.instance.ShowDialog(lines);//コンポーネントにある文字列を引数へ
        }
    }

    //衝突判定
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            canActivator = true;//クリックされたら上のupdateのifに入る
        }
    }
    //離れた時
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            canActivator = false;
            GameManager.instance.ShowDialogChange(canActivator);//すぐに消す
        }
    }
}
