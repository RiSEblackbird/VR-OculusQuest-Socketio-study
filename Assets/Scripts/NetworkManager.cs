using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public Canvas canvas;
    // public SocketIOComponent socket;
    public InputField playerNameInput;
    public GameObject player;

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
        
    }

    
    public void JoinGame()
    {
        StartCoroutine(ConnectToServer());
    }

    #region Commands


    IEnumerator ConnectToServer()
    {
        yield return new WaitForSeconds(0.5f);
    }

    #endregion

    #region Listening

    #endregion

    #region JSONMessageClasses

    [Serializable]
    public class PlayerJSON
    {
        public string name;
        public List<PointJSON> playersSpawnPoints;
        
        public PlayerJSON(string _name, List<SpawnPoint> _playerSpawnPoint)
        {
            playersSpawnPoints = new List<PointJSON>();
            name = _name;
            foreach (SpawnPoint playerSpawnPoint in _playerSpawnPoint)
            {
                PointJSON pointJSON = new PointJSON(playerSpawnPoint);
                playersSpawnPoints.Add(pointJSON);
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
    public class UserJSON
    {
        public string name;
        public float[] postition;
        public float[] rotation;
        // public int health; 体力など定義あれば。

        public static UserJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<UserJSON>(data);
        }
    }

    [Serializable]
    public class EnemiesJSON
    {
        public List<UserJSON> enemies;

        public static EnemiesJSON CreateFromJSON(string data)
        {
            return JsonUtility.FromJson<EnemiesJSON>(data);
        }
    }

    #endregion
}
