using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public Canvas canvas;
    public SocketIOComponent socket;
    public InputField playerNameInput;
    public GameObject player;
    public GameObject startMenuCamera;

    void Awake()
    {
        // gameObjectを破壊せずにシーンを切り替えるのに便利(らしい)。参考動画Pt7.10:30
        // https://youtu.be/0jmUQ0ErAyU?t=601

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        // socket.On("enemies", OnEnemies);
        socket.On("other player connected", OnOtherPlayerConnected);
        socket.On("play", OnPlay);
        socket.On("head move", OnHeadMove);
        socket.On("head turn", OnHeadTurn);
        socket.On("player move", OnPlayerMove);
        socket.On("player turn", OnPlayerTurn);
        socket.On("right hand move", OnRightHandMove);
        socket.On("right hand turn", OnRightHandTurn);
        socket.On("left hand move", OnLeftHandMove);
        socket.On("left hand turn", OnLeftHandTurn);
        socket.On("health", OnHealth);
        socket.On("other player disconnected", OnOtherPlayerDisconnected);
    }

    public void JoinGame()
    {
        StartCoroutine(ConnectToServer());
    }

    #region Commands

    // プレイヤー接続
    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);

        socket.Emit("player connect");

        yield return new WaitForSeconds(1f);

        string playerName = playerNameInput.text;
        List<SpawnPoint> playerSpawnPoints = GetComponent<PlayerSpawner>().playerSpawnPoints;
        GameObject startMenuCamera = GetComponent<GameObject>();
        // List<SpawnPoint> enemySpawnPoints = ...
        PlayerJSON playerJSON = new PlayerJSON(playerName, playerSpawnPoints);
        string data = JsonUtility.ToJson(playerJSON);
        socket.Emit("play", new JSONObject(data));
        canvas.gameObject.SetActive(false);
     }

    public void CommandMove(Vector3 vec3)
    {
        string data = JsonUtility.ToJson(new PositionJSON(vec3));
        socket.Emit("player move", new JSONObject(data));
    }

    public void CommandTurn(Quaternion quat)
    {
        string data = JsonUtility.ToJson(new RotationJSON(quat));
        socket.Emit("player turn", new JSONObject(data));
    }

    public void CommandHeadMove(Vector3 vec3)
    {
        string data = JsonUtility.ToJson(new HeadPositionJSON(vec3));
        socket.Emit("head move", new JSONObject(data));
    }

    public void CommandHeadTurn(Quaternion quat)
    {
        string data = JsonUtility.ToJson(new HeadRotationJSON(quat));
        socket.Emit("head turn", new JSONObject(data));
    }

    public void CommandRightHandMove(Vector3 vec3)
    {
        string data = JsonUtility.ToJson(new RightHandPositionJSON(vec3));
        socket.Emit("right hand move", new JSONObject(data));
    }

    public void CommandRightHandTurn(Quaternion quat)
    {
        string data = JsonUtility.ToJson(new RightHandRotationJSON(quat));
        socket.Emit("right hand turn", new JSONObject(data));
    }

    public void CommandLeftHandMove(Vector3 vec3)
    {
        string data = JsonUtility.ToJson(new LeftHandPositionJSON(vec3));
        socket.Emit("left hand move", new JSONObject(data));
    }

    public void CommandLeftHandTurn(Quaternion quat)
    {
        string data = JsonUtility.ToJson(new LeftHandRotationJSON(quat));
        socket.Emit("left hand turn", new JSONObject(data));
    }

    #endregion

    #region Listening

    void OnOtherPlayerConnected(SocketIOEvent socketIOEvent)
    {
        print("Someone else Joined ");
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        Quaternion rotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
        GameObject o = GameObject.Find(userJSON.name) as GameObject;
        if (o != null)
        {
            return;
        }
        GameObject p = Instantiate(player, position, rotation) as GameObject;
        MultiPlayerController pc = p.GetComponent<MultiPlayerController>();
        Transform t = p.transform.Find("Healthbar Canvas");
        Transform t1 = t.transform.Find("Player Name"); 
        Text playerName = t1.GetComponent<Text>();
        playerName.text = userJSON.name;
        Debug.Log(p.name + " Joined!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! isLocalPlayer is " + pc.isLocalPlayer);
        pc.isLocalPlayer = false;
        p.name = userJSON.name;
        Health h = p.GetComponent<Health>();
        h.currentHealth = userJSON.health;
        h.OnChangeHealth();
        Debug.Log(p.name + " Joined!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! isLocalPlayer is " + pc.isLocalPlayer);
    }

    void OnPlay(SocketIOEvent socketIOEvent)
    {
        print("you joined");
        startMenuCamera.gameObject.SetActive(false);
        string data = socketIOEvent.data.ToString();
        UserJSON currentUserJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(currentUserJSON.position[0], currentUserJSON.position[1], currentUserJSON.position[2]);
        Quaternion rotation = Quaternion.Euler(currentUserJSON.rotation[0], currentUserJSON.rotation[1], currentUserJSON.rotation[2]);
        GameObject p = Instantiate(player, position, rotation) as GameObject;
        MultiPlayerController pc = p.GetComponent<MultiPlayerController>();
        Transform t = p.transform.Find("Healthbar Canvas");
        Transform t1 = t.transform.Find("Player Name"); 
        Text playerName = t1.GetComponent<Text>();
        playerName.text = currentUserJSON.name;
        pc.isLocalPlayer = true;
        p.name = currentUserJSON.name;
        GameObject MainCamera = p.transform.Find("OVRCameraRig").gameObject;
        MainCamera.gameObject.SetActive(true);
    }

    void OnHeadMove(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 headPosition = new Vector3(userJSON.headPosition[0], userJSON.headPosition[1], userJSON.headPosition[2]);
        if (userJSON.name == playerNameInput.text)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            p.transform.position = headPosition;
        }

    }

    void OnHeadTurn(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Quaternion headRotation = Quaternion.Euler(userJSON.headRotation[0], userJSON.headRotation[1], userJSON.headRotation[2]);
        if (userJSON.name == playerNameInput.text)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            p.transform.rotation = headRotation;
        }
    }

    void OnPlayerMove(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 position = new Vector3(userJSON.position[0], userJSON.position[1], userJSON.position[2]);
        if (userJSON.name == playerNameInput.text)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            p.transform.position = position;
        }
            
    }

    void OnPlayerTurn(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Quaternion rotation = Quaternion.Euler(userJSON.rotation[0], userJSON.rotation[1], userJSON.rotation[2]);
        if (userJSON.name == playerNameInput.text)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            p.transform.rotation = rotation;
        }
    }

    void OnRightHandMove(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 rightHandPosition = new Vector3(userJSON.rightHandPosition[0], userJSON.rightHandPosition[1], userJSON.rightHandPosition[2]);
        if (userJSON.name == playerNameInput.text)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            p.transform.position = rightHandPosition;
        }

    }

    void OnRightHandTurn(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Quaternion rightHandRotation = Quaternion.Euler(userJSON.rightHandRotation[0], userJSON.rightHandRotation[1], userJSON.rightHandRotation[2]);
        if (userJSON.name == playerNameInput.text)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            p.transform.rotation = rightHandRotation;
        }
    }

    void OnLeftHandMove(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Vector3 leftHandPosition = new Vector3(userJSON.leftHandPosition[0], userJSON.leftHandPosition[1], userJSON.leftHandPosition[2]);
        if (userJSON.name == playerNameInput.text)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            p.transform.position = leftHandPosition;
        }

    }

    void OnLeftHandTurn(SocketIOEvent socketIOEvent)
    {
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Quaternion leftHandRotation = Quaternion.Euler(userJSON.leftHandRotation[0], userJSON.leftHandRotation[1], userJSON.rightHandRotation[2]);
        if (userJSON.name == playerNameInput.text)
        {
            return;
        }
        GameObject p = GameObject.Find(userJSON.name) as GameObject;
        if (p != null)
        {
            p.transform.rotation = leftHandRotation;
        }
    }

    void OnHealth(SocketIOEvent socketIOEvent)
    {
        print("changing the health");
        string data = socketIOEvent.data.ToString();
        UserHealthJSON userHealthJSON = UserHealthJSON.CreateFromJSON(data);
        GameObject p = GameObject.Find(userHealthJSON.name);
        Health h = p.GetComponent<Health>();
        h.currentHealth = userHealthJSON.health;
        h.OnChangeHealth();
    }

    void OnOtherPlayerDisconnected(SocketIOEvent socketIOEvent)
    {
        print("user disconnected");
        string data = socketIOEvent.data.ToString();
        UserJSON userJSON = UserJSON.CreateFromJSON(data);
        Destroy(GameObject.Find(userJSON.name));
    }
    

    #endregion

    #region JSONMessageClasses

    [Serializable]
    public class PlayerJSON
    {
        public string name;
        public List<PointJSON> playerSpawnPoints;
        
        public PlayerJSON(string _name, List<SpawnPoint> _playerSpawnPoints)
        {
            playerSpawnPoints = new List<PointJSON>();
            name = _name;
            foreach (SpawnPoint playerSpawnPoint in _playerSpawnPoints)
            {
                PointJSON pointJSON = new PointJSON(playerSpawnPoint);
                playerSpawnPoints.Add(pointJSON);
            }
        }
    }

    [Serializable]
    public class PointJSON
    {
        public float[] position;
        public float[] rotation;
        public PointJSON(SpawnPoint spawnPoint)
        {
            position = new float[]
            {
                spawnPoint.transform.position.x,
                spawnPoint.transform.position.y,
                spawnPoint.transform.position.z
            };

            rotation = new float[]
            {
                spawnPoint.transform.eulerAngles.x,
                spawnPoint.transform.eulerAngles.y,
                spawnPoint.transform.eulerAngles.z
            };
        }
    }

    [Serializable]
    public class HeadPositionJSON
    {
        public float[] headPosition;

        public HeadPositionJSON(Vector3 _headPosition)
        {
            headPosition = new float[] { _headPosition.x, _headPosition.y, _headPosition.z };
        }
    }

    [Serializable]
    public class HeadRotationJSON
    {
        public float[] headRotation;

        public HeadRotationJSON(Quaternion _headRotation)
        {
            headRotation = new float[] { _headRotation.eulerAngles.x, _headRotation.eulerAngles.y, _headRotation.eulerAngles.z };
        }
    }

    [Serializable]
    public class PositionJSON
    {
        public float[] position;

        public PositionJSON(Vector3 _position)
        {
            position = new float[] { _position.x, _position.y, _position.z };
         }
    }

    [Serializable]
    public class RotationJSON
    {
        public float[] rotation;

        public RotationJSON(Quaternion _rotation)
        {
            rotation = new float[] { _rotation.eulerAngles.x, _rotation.eulerAngles.y, _rotation.eulerAngles.z };
        }
    }

    [Serializable]
    public class RightHandPositionJSON
    {
        public float[] rightHandPosition;

        public RightHandPositionJSON(Vector3 _rightHandPosition)
        {
            rightHandPosition = new float[] { _rightHandPosition.x, _rightHandPosition.y, _rightHandPosition.z };
        }
    }

    [Serializable]
    public class RightHandRotationJSON
    {
        public float[] rightHandRotation;

        public RightHandRotationJSON(Quaternion _rightHandRotation)
        {
            rightHandRotation = new float[] { _rightHandRotation.eulerAngles.x, _rightHandRotation.eulerAngles.y, _rightHandRotation.eulerAngles.z };
        }
    }

    [Serializable]
    public class LeftHandPositionJSON
    {
        public float[] leftHandPosition;

        public LeftHandPositionJSON(Vector3 _leftHandPosition)
        {
            leftHandPosition = new float[] { _leftHandPosition.x, _leftHandPosition.y, _leftHandPosition.z };
        }
    }

    [Serializable]
    public class LeftHandRotationJSON
    {
        public float[] leftHandRotation;

        public LeftHandRotationJSON(Quaternion _leftHandRotation)
        {
            leftHandRotation = new float[] { _leftHandRotation.eulerAngles.x, _leftHandRotation.eulerAngles.y, _leftHandRotation.eulerAngles.z };
        }
    }

    [Serializable]
    public class UserJSON
    {
        public string name;
        public float[] headPosition;
        public float[] headRotation;
        public float[] position;
        public float[] rotation;
        public float[] rightHandPosition;
        public float[] rightHandRotation;
        public float[] leftHandPosition;
        public float[] leftHandRotation;
        public int health;

        public static UserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserJSON>(data);
        }
    }

    [Serializable]
    public class UserHealthJSON
    {
        public string name;
        public int health;

        public static UserHealthJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserHealthJSON>(data);
        }
    }

    /*
    [Serializable]
    public class EnemiesJSON
    {
        public List<UserJSON> enemies;

        public static EnemiesJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<EnemiesJSON>(data);
        }
    }
    */

    #endregion
}
