var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);

server.listen(3000);

var enemySpawnPoints = [];
var wakizashiSpawnPoints = [];
var playerSpawnPoints = [];
var clients = [];

app.get('/', function(req, res) {
    res.send('ホーム');
});

// 通信について
io.on('connection', function(socket){

    var currentPlayer = {};
    currentPlayer.name = 'unknown'; //+ (new Date()).toString();

    // プレイヤーの接続
    socket.on('player connect', function(){
        console.log(currentPlayer.name+' recv: player connect, and clients.length is ' + clients.length);
        console.log("Now cliants info : ", clients);
        // すべてのクライアントについて名前と位置と回転を定義
        for(var i=0; i<clients.length; i++) {
            console.log('clients[', i,'] info : ', clients[i]);
            var playerConnected = {
                name:clients[i].name,
                position:clients[i].position,
                rotation:clients[i].rotation
            };

            var connectedHead = {
                name:clients[i].name,
                headPosition:clients[i].headPosition,
                headRotation:clients[i].headRotation
            };

            var connectedRightHand = {
                name:clients[i].name,
                rightHandPosition:clients[i].rightHandPosition,
                rightHandRotation:clients[i].rightHandRotation
            };

            var connectedLeftHand = {
                name:clients[i].name,
                leftHandPosition:clients[i].leftHandPosition,
                leftHandRotation:clients[i].leftHandRotation
            };

            var othersEnemy = {
                name: clients[i].name,
                position:clients[i].position,
                rotation:clients[i].rotation
            };

            var othersWakizashi = {
                name: clients[i].name,
                position:clients[i].position,
                rotation:clients[i].rotation
            };


            socket.emit('other player connected', playerConnected);
            socket.emit('other player head', connectedHead);
            socket.emit('other player right hand', connectedRightHand);
            socket.emit('other player left hand', connectedLeftHand);
            socket.emit('others enemy', othersEnemy);
            socket.emit('others wakizashi', othersWakizashi);


            // 接続された」プレイヤーの情報を表示
            console.log(currentPlayer.name+' emit: other player connected: '+JSON.stringify(playerConnected));
            console.log(currentPlayer.name+' emit: other player head: '+JSON.stringify(connectedHead));
            console.log(currentPlayer.name+' emit: other player right hand: '+JSON.stringify(connectedRightHand));
            console.log(currentPlayer.name+' emit: other player leftt hand: '+JSON.stringify(connectedLeftHand));
            console.log(currentPlayer.name+' emit: others enemy: '+JSON.stringify(othersEnemy));
            console.log(currentPlayer.name+' emit: other wakizashi : '+JSON.stringify(othersWakizashi));
        }
    });

    // ゲームプレイ中の通信
    socket.on('play', function(data){
        console.log(currentPlayer.name+' recv: play: '+JSON.stringify(data));
        console.log("old player info : ", currentPlayer);
        console.log("data is : ", data);
        currentPlayer = data;
        console.log("new player info : ", currentPlayer);

        // 既存のクライアントがいない場合
        if(clients.length === 0) {
            /*
            playerSpawnPoints = [];
            data.playerSpawnPoints.forEach(function(data) {
                var playerSpawnPoint = {
                    playerSpawnPosition: data.playerSpawnPosition,
                    playerSpawnRotation: data.playerSpawnRotation
                };
                playerSpawnPoints.push(playerSpawnPoint);
            });
            */

            /*
            playerSpawnPositions = [];
            data.playerSpawnPositions.forEach(function(data) {
                var playerSpawnPosition = {
                    playerSpawnPosition: data.playerSpawnPosition
                }
                playerSpawnPositions.push(playerSpawnPosition);
            });

            playerSpawnRotations = [];
            data.playerSpawnRotations.forEach(function(data) {
                var playerSpawnRotation = {
                    playerSpawnRotation: data.playerSpawnRotation
                }
                playerSpawnRotations.push(playerSpawnRotation);
            });
            */

            var playerSpawnPosition = {
                playerSpawnPosition: data.playerSpawnPosition
            }

            var playerSpawnRotation = {
                playerSpawnRotation: data.playerSpawnRotation
            }

        }

        // クライアントの列に現在接続したプレイヤーを加える
        clients.push(currentPlayer);
        // 現在入っているプレイヤーを出力
        console.log(currentPlayer.name + ' emit: play: '+JSON.stringify(currentPlayer));
        socket.emit('play', currentPlayer);
        // 既にいるプレイヤー達に自分が入ったことを伝える
        socket.broadcast.emit('other player connected', currentPlayer);
    });

    // 敵キャラの生成
    socket.on('enemy', function(enemyData){
        enemySpawnPoints = [];
        enemyData.enemySpawnPoints.forEach(function(_enemySpawnPoint) {
            var enemySpawnPoint = {
                name: guid(),
                position: _enemySpawnPoint.position,
                rotation: _enemySpawnPoint.rotation,
                health: 100
            };
            enemySpawnPoints.push(enemySpawnPoint);
        });
        console.log(currentPlayer.name + ' enemy genarated');
    });

    // 脇差の生成
    socket.on('wakizashi', function(wakizashiData){
        wakizashiSpawnPoints = [];
        wakizashiData.wakizashiSpawnPoints.forEach(function(_wakizashiSpawnPoint) {
            var wakizashiSpawnPoint = {
                name: guid(),
                position: _wakizashiSpawnPoint.position,
                rotation: _wakizashiSpawnPoint.rotation,
                health: 100
            };
            wakizashiSpawnPoints.push(wakizashiSpawnPoint);
        });
        console.log(currentPlayer.name + ' wakizashi genarated');
    });

    // ヘッドの移動
    socket.on('head move', function(data) {
        console.log(currentPlayer.name+' recv: head move: '+JSON.stringify(data));
        currentPlayer.headPosition = data.headPosition;
        socket.broadcast.emit('head move', currentPlayer);
    });

    // ヘッドの回転
    socket.on('head turn', function(data) {
        console.log(currentPlayer.name+' recv: head turn: '+JSON.stringify(data));
        currentPlayer.headRotation = data.headRotation;
        socket.broadcast.emit('head turn', currentPlayer);
    });


    // プレイヤーの移動
    socket.on('player move', function(data) {
        console.log(currentPlayer.name+' recv: move: '+JSON.stringify(data));
        currentPlayer.position = data.position;
        socket.broadcast.emit('player move', currentPlayer);
    });

    // プレイヤーの回転
    socket.on('player turn', function(data) {
        console.log(currentPlayer.name+' recv: turn: '+JSON.stringify(data));
        currentPlayer.rotation = data.rotation;
        socket.broadcast.emit('player turn', currentPlayer);
    });

    // 右手の移動
    socket.on('right hand move', function(data) {
        console.log(currentPlayer.name+' recv: right hand move: '+JSON.stringify(data));
        currentPlayer.rightHandPosition = data.rightHandPosition;
        socket.broadcast.emit('right hand move', currentPlayer);
    });

    // 右手の回転
    socket.on('right hand turn', function(data) {
        console.log(currentPlayer.name+' recv: right hand turn: '+JSON.stringify(data));
        currentPlayer.rightHandRotation = data.rightHandRotation;
        socket.broadcast.emit('right hand turn', currentPlayer);
    });

    // 左手の移動
    socket.on('left hand move', function(data) {
        console.log(currentPlayer.name+' recv: left hand move: '+JSON.stringify(data));
        currentPlayer.leftHandPosition = data.leftHandPosition;
        socket.broadcast.emit('left hand move', currentPlayer);
    });

    // 左手の回転
    socket.on('left hand turn', function(data) {
        console.log(currentPlayer.name+' recv: left hand turn: '+JSON.stringify(data));
        currentPlayer.leftHandRotation = data.leftHandRotation;
        socket.broadcast.emit('left hand turn', currentPlayer);
    });

    // 敵キャラの移動
    socket.on('enemy move', function(data) {
        console.log(currentPlayer.name+' recv: enemy move: '+JSON.stringify(data));
        currentPlayer.position = data.position;
        socket.broadcast.emit('enemy move', currentPlayer);
    });

    // 敵キャラの回転
    socket.on('enemy turn', function(data) {
        console.log(currentPlayer.name+' recv: enemy turn: '+JSON.stringify(data));
        currentPlayer.rotation = data.rotation;
        socket.broadcast.emit('enemy turn', currentPlayer);
    });

    // 脇差の移動
    socket.on('wakizashi move', function(data) {
        console.log(currentPlayer.name+' recv: wakizashi move: '+JSON.stringify(data));
        currentPlayer.position = data.position;
        socket.broadcast.emit('wakizashi move', currentPlayer);
    });

    // 脇差の回転
    socket.on('wakizashi turn', function(data) {
        console.log(currentPlayer.name+' recv: wakizashi turn: '+JSON.stringify(data));
        currentPlayer.rotation = data.rotation;
        socket.broadcast.emit('wakizashi turn', currentPlayer);
    });


    // プレイヤーの接続解除
    socket.on('disconnect', function() {
        console.log(currentPlayer.name+' recv: disconnect '+currentPlayer.name);
        socket.broadcast.emit('other player disconnected', currentPlayer);
        console.log(currentPlayer.name+' bcst: other player disconnected '+JSON.stringify(currentPlayer));
        // クライアント一覧から、離脱者のみを消去する
        for(var i=0; i<clients.length; i++) {
            if(clients[i].name === currentPlayer.name) {
                clients.splice(i,1);
            }
        }
    });

    // キャラクターの体力
    socket.on('health', function(data) {
        console.log(currentPlayer.name+' bcst: health: '+JSON.stringify(data));
        if(data.from === currentPlayer.name) {
            var indexDamaged = 0;
            if(!data.isEnemy) {
                clients = clients.map(function(client, index) {
                    if(client.name === data.name) {
                        indexDamaged = index;
                        client.health -= data.healthChange;
                    }
                    return client;
                });
            } else {
                enemies = enemies.map(function(enemy, index) {
                    if(enemy.name === data.name) {
                        indexDamaged = index;
                        enemy.health -= data.healthChange;
                    }
                    return enemy;
                });
            }

            var responce = {
                name: (!data.isEnemy) ? clients[indexDamaged].name : enemies[indexDamaged].name,
                health: (!data.isEnemy) ? clients[indexDamaged].health : enemies[indexDamaged].health
            };
            console.log(currentPlayer.name+' bcst: health: '+JSON.stringify(responce));
            socket.emit('health', response);
            socket.broadcast.emit('health', response);
        }

    });
});

console.log('server is running...'+guid());

function guid() {
    function s4() {
        // 乱数でランダムな数字を取得して英数字の文字列を生成する
        return Math.floor((1+Math.random()) * 0x10000).toString(16).substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +s4() +s4() + s4();
}
