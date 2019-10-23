var app = require('express')();
var server = require('http').Server(app);
var io = require('socket.io')(server);

server.listen(3000);

var enemies = [];
var playerSpawnerPoints = [];
var clients = [];

app.get('/', function(req, res) {
    res.send('ホーム');
});

// 通信について
io.on('connection', function(socket){

    var currentPlyer = {};
    currentPlayer.neme = 'unknown';

    // ①プレイヤーの接続
    socket.on('player connect', function(){
        console.log(currentPlayer.name + ' recv: player connect');
        // すべてのクライアントについて名前と位置と回転を定義
        for(var i; i<clients.length; i++) {
            var playerConnected = {
                name:clients[i].name,
                position:clients[i].position,
                rotation:clients[i].rotation
            };
            socket.emit('other player connected', playerConnected);
            // 接続された」プレイヤーの情報を表示
            console.log(currentPlayer.name + ' emit: other player connected: ' + JSON.stringify(playerConnected));
        }
    });

    // ②ゲームプレイ中の通信
    socket.on('play', function(data){
        console.log(currentPlayer.name + 'recv: play: ' + JSON.stringify(data));

        // 既存のクライアントがいない場合
        if(clients.length == 0) {
            playerSpawnerPoints = [];
            data.playerSpawnerPoints.forEach(function(_playerSpawnPoint) {
                var playerSpawnerPoint = {
                    position: _playerSpawnPoint.position,
                    rotation: _playerSpawnPoint.rotation
                };
                playerSpawnerPoints.push(playerSpawnerPoint);
            });
        }

        var enemiesResponse = {
            enemies: enemies
        };

        console.log(currentPlayer.name + ' emit: enemies: ' + JSON.stringify(enemiesResponse));
        socket.emit('enemies', enemiesResponse);
        var randamSpawnPoint = playerSpawnerPoints[Math, floor(Math.ramdom() * playerSpawnerPoints.length)];
        currentPlayer = {
            name: data.name,
            position: randamSpawnPoint.position,
            rotation: randamSpawnPoint.rotation
        };
        // クライアントの列に現在接続したプレイヤーを加える
        clients.push(currentPlayer);
        // 現在入っているプレイヤーを出力
        console.log(currentPlyer.name + ' emit: play: ' + JSON.stringify(currentPlyer));
        // 既にいるプレイヤー達に自分が入ったことを伝える
        socket.broadcast.emit('other player connected', currentPlyer);
    });

    // ③プレイヤーの移動
    socket.on('player move', function(data) {
        console.log('recv: move: '+JSON.stringify(data));
        currentPlyer.position = data.position;
        socket.broadcast.emit('player move', currentPlyer);
    });

    // ④プレイヤーの回転
    socket.on('player turn', function(data) {
        console.log('recv: turn: '+JSON.stringify(data));
        currentPlyer.rotation = data.rotation;
        socket.broadcast.emit('player turn', currentPlyer);
    })
});

console.log('server is running...' + guid());

function guid() {
    function s4() {
        // 乱数でランダムな数字を取得して英数字の文字列を生成する
        return Math.floor((1+Math.random()) * 0x10000).toString(16).substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +s4() +s4() + s4();
}
