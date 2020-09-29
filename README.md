# VR-OculusQuest-Socketio-study  
---
2019.10~ 某社様にて、選考課題として2週間常駐させて頂き製作に取り組みました。  

### 要件

- Oclulus Questのtouchコントローラーやヘッドの位置情報を取得してクライアント側で表示出来ていること
- Websocketを使用した技術(Rails, Node.js等から私が任意で選定)を用いて、複数機のクライアント間でお互いの頭、手、姿の位置が正しく確認できること
---
**キャプチャ動画**  
 - 2019-10-28 am  
  高画質↓  
  https://www.youtube.com/watch?v=JubKeeb9RNI  
  Gif画像↓  
  ![VR-Socket-Study](https://user-images.githubusercontent.com/43542677/81929511-1692df00-9622-11ea-9a34-625530357957.gif)
  ---

  基本的に下記掲載の動画のチュートリアルに沿って進める。  
  

**動画との主な相違点(注意点)**  
[ 動画 ] ： [ 今回 ]  
  定点視点 ： 各プレイヤー主観  
  キャラが静的オブジェクトのみで構成 ： 頭部と手が操作で動き回る(基準はプレイヤー本体に追従)  

---

## 技術構成  
クライアント：Unity (2019.2.9f1 Personal)  
サーバサイド：Node.js (v10.16.3), express, Socket.IO  
使用言語：C#, JavaScript  

---

## Reference video tutorials
**Unity 5 Multiplayer Networking Node JS socket.io Pt1**  
 イントロダクション(目標アプリの紹介)  
 https://www.youtube.com/watch?v=tn3cWcSYmHE  

**Pt2 (自キャラプレハブ)**  
 *Prefab : *Player(/*Gun, /*HealthBar), *Canvas  
 https://www.youtube.com/watch?v=sj9MvcJKRZs&t=286s  

**Pt3 (ヘルスバー詳細、敵キャラ、スポーン位置)**  
 Prefab : HealthBar Canvas, *Eneny, *Bullet  
 *cs : Enemy/*SpawnPoint.cs, Bullet/Bullet.cs  
 https://www.youtube.com/watch?v=erDzqBiC2iQ&t=659s  

**Pt4 (ネットワークマネージャ初出、入室フォームパネルの作成、プレイヤー移動の定義)**  
 Pre : *Join Game Canvas, *Network Manager  
 cs : HealthBar Canvas/*Billboard.cs, Network Manager/*Network Manager.cs, Player/*PlayerController.cs  
 https://www.youtube.com/watch?v=Vqdlrky4cTM  

**Pt5 (プレイヤー発砲, 体力値、デス消滅、リスポーン)**  
 cs : Player/*Health.cs  
 https://www.youtube.com/watch?v=4YauRosgN0k  

**Pt6 (Bullet.csの詳細, 敵スポーン位置の詳細)**  
 cs : Enemy/*EnemySpawner.cs  
 https://www.youtube.com/watch?v=jy6kqSQ7IPs  

**Pt7 (ヘルスバーのダメージ挙動調整、体力ゼロ敵の消滅、プレイヤースポーン位置, PlayerJSON)**  
 cs : Player/*PlayerSpawner.cs, Network Manager.cs  
 https://www.youtube.com/watch?v=0jmUQ0ErAyU  

**Pt8 (通信パラメータを格納するJSONの定義)**  
 cs : Network Manager.cs  
   *JSON : *PointJSON, *PlayerJSON, *PositionJSON, *RotationJSON, *UserJSON, *HealthChangeJSON, *EnemiesJSON, *ShootJSON,  *UserHealthJSON  
 https://www.youtube.com/watch?v=oYucBRfjyRE  

**Pt9 (ローカルサーバー、ランダム文字列)**  
 *js : *index.js  
   *Event(js) : *connection, *playerconnect, *play, *enemies  
 https://www.youtube.com/watch?v=HKyl2wSShFM  

**Pt10 (ローカルサーバー続き)**  
 js : index.js  
   Event(js) : *player move, *player turn, *player shoot, *health, *disconnect  
 https://www.youtube.com/watch?v=qx_zEFeMtvA  

 **Pt11 (Asset : "Socket IO for Unity"の導入、URLの設定、Network Manager.cs側でのイベント設定、IEnumerator詳細)**  
 Asset : "Socket IO for Unity"  
 cs : Network Manager.cs  
   Event(cs) : *OnOtherPlayerConnected, OnPlay  
 https://www.youtube.com/watch?v=h6JISqIuVRc  

**Pt12 (敵スポーン位置の通信、参戦ボタン"Button"の設定)**  
cs : EnemySpawner.cs  
  Event(cs) : *SpawnEnemies  
cs : Network Manager.cs  
  Event(cs) : *OnEnemies  
Pre : Join Game Canvas, Network Manager  
 https://www.youtube.com/watch?v=I33fOjqhWIQ  

**Pt13 (PlayerJSON修正、OnPlay修正)**  
cs : Network Manager.cs  
  Event(cs) : OnPlay, OnOtherPlayerDisconnected  
  JSON : PlayerJSON  
https://www.youtube.com/watch?v=CkHACZsrEvc&t=2s  

**Pt14 (通信パラメータ整備)**  
cs : Network Manager.cs  
  Event(cs) : *OnPlayerMove, *OnPlayerTurn, *CommandMove, *CommandTurn, *PlayerShoot  
  JSON : PlayerJSON  
cs : PlayerController.cs(NetworkManagerにpositionやrotatuionの値を送る)  
  Event(cs) : *CmdFire  
https://www.youtube.com/watch?v=WfT_ZbxaKgs10000  

**Pt15 (自ダメージ)**  
cs : Network Manager.cs  
  Event(cs) : *OnHealth, CommandHealthChange  
cs : Health.cs(*TakeDamage, OnChangeHealth)  
https://www.youtube.com/watch?v=fnKp7W-W_o8&t=678s  

**Pt16 (敵スポーン調整)**  
cs : EnemySpawner.cs(SpawnEnemies)  
https://www.youtube.com/watch?v=IxkFFJ6t8OY&t=234s  

---

## Reference articles  
**C#, IEnumeratorとIEnumerableを調べた**  
 https://qiita.com/vc_kusuha/items/2048391d821cb94fa489  

**json parserとは？オンラインパーサやPython、JSでの書き方を解説！**  
 https://www.sejuku.net/blog/79898  

**Unity, JsonUtilityでJsonへシリアライズしたデータをローカルに保存する**  
 https://hiyotama.hatenablog.com/entry/2019/08/30/110000  

**JavaScriptのjson stringifyを完全に理解しよう！**  
 https://www.sejuku.net/blog/79911

**JavaScript入門】pushで配列に要素を追加する方法(連想配列/pop)**  
 https://www.sejuku.net/blog/84475  

**JS Math.random()**  
 https://developer.mozilla.org/ja/docs/Web/JavaScript/Reference/Global_Objects/Math/random  

**JS Math**  
 https://developer.mozilla.org/ja/docs/Web/JavaScript/Reference/Global_Objects/Math  

**Unique id generator in javascript**  
 https://learnersbucket.com/examples/javascript/unique-id-generator-in-javascript/  

**厳密非等価演算子 ---「分かりそう」で「分からない」でも「分かった」気になれるIT用語辞典**  
https://wa3.i-3-i.info/word17940.html  

**Array.prototype.splice()**  
https://developer.mozilla.org/ja/docs/Web/JavaScript/Reference/Global_Objects/Array/splice  

**Unity入門】Instantiateを使いこなそう!使い方・使用例まとめ!**  
https://www.sejuku.net/blog/48180  


