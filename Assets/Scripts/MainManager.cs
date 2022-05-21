using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText, BestScore, PlayerName;

    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points, m_Score, bestScore;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        LoadPoint();
        PlayerName.text = $"Player: {DataManager.instance.playerName}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            SavePoint();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void AddPoint(int point)
    {
        m_Points += point;
        
        ScoreText.text = $"Score: {m_Points}";

        if (m_Points > bestScore)
        {
            bestScore = m_Points;           
        }

    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);   
    }

    [System.Serializable]
    class Point
    {
        public int savePoint;
    }

    public void SavePoint()
    {
        
        Point data = new Point();
        data.savePoint = bestScore;
        string jsonData = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", jsonData);
    }

    public void LoadPoint()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Point data = JsonUtility.FromJson<Point>(json);

            m_Score = data.savePoint;
        }

        BestScore.text = $"Best Score: {m_Score}";
        bestScore = m_Score;
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene(0);
    }
}
