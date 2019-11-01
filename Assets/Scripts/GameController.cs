using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public UnityEngine.UI.Text scoreLabel;
    public GameObject Reticle;
    public GameObject winnerLabelObject;
    public GameObject RetryButton;

    public void Update ()
    {
        int count = GameObject.FindGameObjectsWithTag ("Enemy").Length;
        scoreLabel.text = count.ToString ();

        if (OVRInput.Get(OVRInput.Button.Four))
        {
            // 現在のシーン番号を取得
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            // 現在のシーンを再読み込みする
            SceneManager.LoadScene(sceneIndex);
        }

        /* ゲームリセットボタン、最初のキー入力で誤爆する恐れがあるのでＯＦＦ
        if (OVRInput.Get(OVRInput.Button.Three) || Input.GetKey(KeyCode.K))
        {
            EnemySpawner es = GetComponent<EnemySpawner>();
            es.SpawnEnemies(EnemiesJSON);
        }
        */
        /*
        if (count == 0) {
            // オブジェクトをアクティブにする
            winnerLabelObject.SetActive(true);
            Reticle.SetActive(false);
        }
        */

    }
    
    public void OnRetry ()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
    }
}
