using StreamHub.Util;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace StreamHub.Scenes.PersonalWorld.Stack
{
  public class StackManager : MonoBehaviour
  {
    public static StackManager Instance { get; private set; }

    public ObjectPool<Brick> brickPool;
    public float destroyHeight;
    [SerializeField] private new Camera camera;

    [Header("Line option")] public BrickLine currentLine;
    public int lineIndex = -1;

    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private Transform releasedContainer;

    [Header("Score")] [SerializeField] [GetSet("Score")]
    private int score;

    [SerializeField] private int fixedBrickCount;
    private int bestFixedBrickCount;
    [SerializeField] private TMP_Text scoreText;

    public int FixedBrickCount
    {
      get => fixedBrickCount;
      set
      {
        fixedBrickCount = value;
        bestFixedBrickCount = Mathf.Max(fixedBrickCount, bestFixedBrickCount);
        if (fixedBrickCount <= 0)
          GameOver();
      }
    }

    public int Score
    {
      get => score;
      set
      {
        score = value;
        scoreText.text = $"Score : {score}";
      }
    }
    
    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
        brickPool = new ObjectPool<Brick>(() =>
          {
            // Create
            var obj = Instantiate(brickPrefab, transform);
            var brick = obj.GetComponent<Brick>();
            brick.gameObject.SetActive(true);

            return brick;
          },
          brick =>
          {
            // Get
            brick.gameObject.SetActive(true);
            brick.transform.SetParent(transform);
            brick.positionFixed = false;
          },
          brick =>
          {
            // Release
            if (brick.index != -1 && brick.positionFixed) FixedBrickCount--;
            brick.gameObject.SetActive(false);
            brick.transform.SetParent(releasedContainer);
          },
          brick =>
          {
            // destroy
            Destroy(brick.gameObject);
          });

        #region SetupPosition
        {
          var temp = camera.ViewportToWorldPoint(Vector3.zero);
          leftPos = temp.x;
          Debug.Log(leftPos);
          rightPos = camera.ViewportToWorldPoint(Vector3.right).x;
          Debug.Log(rightPos);
          destroyHeight = temp.y;
        }

        #endregion
      }
      else
      {
        Destroy(this);
      }
    }

    private void Start()
    {
      Time.timeScale = 0;
      SpawnLine();
    }

    private void FixedUpdate()
    {
      // Camera Tracking
      camera.transform.position = Vector3.Lerp(camera.transform.position,
        new Vector3(releasedContainer.position.x, releasedContainer.position.y, camera.transform.position.z),
        Time.deltaTime * 5);

      if (Input.GetKeyDown(KeyCode.Escape) && !pausePanel.activeSelf)
      {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
      }
    }

    public bool left = true;
    public float leftPos, rightPos;

    public void SpawnLine()
    {
      // get start position
      var startPos = left ? leftPos : rightPos;
      // spawn line
      lineIndex++;

      var count = Random.Range(3, 12);
      var obj = Instantiate(linePrefab, transform);
      currentLine = obj.GetComponent<BrickLine>();

      currentLine.index = lineIndex;
      currentLine.Generate(count);
      currentLine.transform.position = new Vector3(startPos, lineIndex - 3, 0);
    }

    [Header("GameOver")] [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverText;

    public void GameOver()
    {
      Time.timeScale = 0;

      gameOverPanel.SetActive(true);
      gameOverText.text = $"Game Over\n점수 : {score}\n블럭 최대 활성화: {bestFixedBrickCount}";
    }

    public void Back()
    {
      Time.timeScale = 1;
      SceneManager.LoadSceneAsync("PersonalWorldScene");
    }

    public void PlaceLine()
    {
      if (currentLine == null || Time.timeScale == 0) return;

      if (lineIndex > 3)
        releasedContainer.transform.position = new Vector3(0, lineIndex - 2.5f, 0);

      currentLine.Deconstruct();
      destroyHeight = camera.ScreenToWorldPoint(Vector3.zero).y;
      left = !left;
      SpawnLine();
    }

    [Header("Description")]
    [SerializeField] private GameObject descriptionPanel;
    public void CloseDescriptionPanel()
    {
      descriptionPanel.SetActive(false);
      Time.timeScale = 1;
    }
    
    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;
    public void ClosePausePanel()
    {
      Time.timeScale = 1;
      pausePanel.SetActive(false);
    }

    public void FinishGame()
    {
      Time.timeScale = 0;
      gameOverPanel.SetActive(true);
      gameOverText.text = $"Finished!\n점수 : {score}\n블럭 최대 활성화: {bestFixedBrickCount}";
    }
  }
}