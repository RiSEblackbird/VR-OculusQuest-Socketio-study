var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);

server.listen(3000);

var enemies = [];
var playerSpawnPoints = [];
var clients = [];

app.get('/', function(req, res) {
    res.send('ホーム');
});

// 通信について
io.on('connection', function(socket){

    var currentPlayer = {};
    currentPlayer.name = 'unknown';

    // ①プレイヤーの接続
    socket.on('player connect', function(){
        console.log(currentPlayer.name+' recv: player connect');
        // すべてのクライアントについて名前と位置と回転を定義
        for(var i=0; i<clients.length; i++) {
            var playerConnected = {
                name:clients[i].name,
                
                position:clients[i].position,
                rotation:clients[i].rotation,

                health:clients[i].health
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

            socket.emit('other player connected', playerConnected);
            socket.emit('other player head', connectedHead);
            socket.emit('other player right hand', connectedRightHand);
            socket.emit('other player left hand', connectedLeftHand);
            // 接続された」プレイヤーの情報を表示
            console.log(currentPlayer.name+' emit: other player connected: '+JSON.stringify(playerConnected));
            console.log(currentPlayer.name+' emit: other player head: '+JSON.stringify(connectedHead));
            console.log(currentPlayer.name+' emit: other player right hand: '+JSON.stringify(connectedRightHand));
            console.log(currentPlayer.name+' emit: other player leftt hand: '+JSON.stringify(connectedLeftHand));
        }
    });

    // ②ゲームプレイ中の通信
    socket.on('play', function(data){
        console.log(currentPlayer.name+' recv: play: '+JSON.stringify(data));

        // 既存のクライアントがいない場合
        if(clients.length === 0) {
            playerSpawnPoints = [];
            data.playerSpawnPoints.forEach(function(_playerSpawnPoint) {
                var playerSpawnPoint = {
                    position: _playerSpawnPoint.position,
                    rotation: _playerSpawnPoint.rotation
                };
                playerSpawnPoints.push(playerSpawnPoint);
            });
        }

        var enemiesResponse = {
            enemies: enemies
        };

        console.log(currentPlayer.name+' emit: enemies: '+JSON.stringify(enemiesResponse));
        socket.emit('enemies', enemiesResponse);
        var randamSpawnPoint = playerSpawnPoints[Math.floor(Math.random() * playerSpawnPoints.length)];
        currentPlayer = {
            name: data.name,
            position: randamSpawnPoint.position,
            rotation: randamSpawnPoint.rotation
        };

        // クライアントの列に現在接続したプレイヤーを加える
        clients.push(currentPlayer);
        // 現在入っているプレイヤーを出力
        console.log(currentPlayer.name + ' emit: play: '+JSON.stringify(currentPlayer));
        socket.emit('play', currentPlayer);
        // 既にいるプレイヤー達に自分が入ったことを伝える
        socket.broadcast.emit('other player connected', currentPlayer);
    });

    // ③ヘッドの移動
    socket.on('head move', function(data) {
        console.log(currentPlayer.name+' recv: head move: '+JSON.stringify(data));
        currentPlayer.headPosition = data.headPosition;
        socket.broadcast.emit('head move', currentPlayer);
    });

    // ④ヘッドの回転
    socket.on('head turn', function(data) {
        console.log(currentPlayer.name+' recv: head turn: '+JSON.stringify(data));
        currentPlayer.headRotation = data.headRotation;
        socket.broadcast.emit('heade turn', currentPlayer);
    });


    // ③プレイヤーの移動
    socket.on('player move', function(data) {
        console.log(currentPlayer.name+' recv: move: '+JSON.stringify(data));
        currentPlayer.position = data.position;
        socket.broadcast.emit('player move', currentPlayer);
    });

    // ④プレイヤーの回転
    socket.on('player turn', function(data) {
        console.log(currentPlayer.name+' recv: turn: '+JSON.stringify(data));
        currentPlayer.rotation = data.rotation;
        socket.broadcast.emit('player turn', currentPlayer);
    });

    // ③右手の移動
    socket.on('right hand move', function(data) {
        console.log(currentPlayer.name+' recv: right hand move: '+JSON.stringify(data));
        currentPlayer.rightHandPosition = data.rightHandPosition;
        socket.broadcast.emit('right hand move', currentPlayer);
    });

    // ④右手の回転
    socket.on('right hand turn', function(data) {
        console.log(currentPlayer.name+' recv: right hand turn: '+JSON.stringify(data));
        currentPlayer.rightHandRotation = data.rightHandRotation;
        socket.broadcast.emit('right hand turn', currentPlayer);
    });

    // ③左手の移動
    socket.on('left hand move', function(data) {
        console.log(currentPlayer.name+' recv: left hand move: '+JSON.stringify(data));
        currentPlayer.leftHandPosition = data.leftHandPosition;
        socket.broadcast.emit('left hand move', currentPlayer);
    });

    // ④左手の回転
    socket.on('left hand turn', function(data) {
        console.log(currentPlayer.name+' recv: left hand turn: '+JSON.stringify(data));
        currentPlayer.leftHandRotation = data.leftHandRotation;
        socket.broadcast.emit('left hand turn', currentPlayer);
    });


    // ⑤プレイヤーの接続解除
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

    // ⑥キャラクターの体力
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
