using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //シングルトン化
    public static SoundManager instance;//一つしかないですよ〜


    //変数
    //SE格納用の配列
    public AudioSource[] se;
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;//一つしかないですよ
        }
        else if( instance != this)//中身が自分じゃない時＝SMが2つ存在する時
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);//SMだけは画面遷移しても壊れないようにする（画面遷移で崩れるので）
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    ///<summary>
    ///0:攻撃, 1:ダメージ, 2:被弾, 3:回復, 4:全滅
    ///<summary>
    ///<param name="x"></param>
    //SE再生用の関数
    public void PlaySE(int x)
    {
        se[x].Stop();//音を止めて
        se[x].Play();//音を鳴らす
    }
}
