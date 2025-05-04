using UnityEngine;
using UnityEngine.Pool;
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
    [SerializeField] private int lineIndex;

    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private Transform releasedContainer;

    private void Awake()
    {
      if (Instance == null)
      {
        Instance = this;
        brickPool = new ObjectPool<Brick>(() =>
          {
            // Create
            var obj = Instantiate(brickPrefab, releasedContainer);
            return obj.GetComponent<Brick>();
          },
          brick =>
          {
            // Get
            brick.gameObject.SetActive(true);
            brick.transform.SetParent(transform);
          },
          brick =>
          {
            // Release
            brick.gameObject.SetActive(false);
            brick.transform.SetParent(releasedContainer);
          },
          brick =>
          {
            // destroy
            Destroy(brick.gameObject);
          });

        {
          var temp = camera.ScreenToWorldPoint(Vector3.zero);
          leftPos = temp.x;
          rightPos = camera.ScreenToWorldPoint(Vector3.right).x;
          destroyHeight = temp.y;
        }
      }
      else
      {
        Destroy(this);
      }
    }

    private void Start()
    {
      SpawnLine();
    }

    private bool left = true;
    private float leftPos, rightPos;

    public void SpawnLine()
    {
      // get start position
      left = !left;
      var startPos = left ? leftPos : rightPos;
      // spawn line
      var count = Random.Range(3, 6);
      var obj = Instantiate(linePrefab, transform);
      currentLine = obj.GetComponent<BrickLine>();

      currentLine.Generate(count);
      currentLine.transform.position = new Vector3(startPos, lineIndex - 3, 0);
      currentLine.index = lineIndex++;
    }

    public void PlaceLine()
    {
      if (currentLine != null)
      {
        currentLine.Deconstruct();
        destroyHeight = camera.ScreenToWorldPoint(Vector3.zero).y;
      }
    }
  }
}