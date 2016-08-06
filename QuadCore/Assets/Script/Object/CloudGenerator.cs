using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudGenerator : MonoBehaviour
{
    public GameObject[] cloudPrefab; // 구름 프리팹 배열
    public static Queue<GameObject> cloudList = new Queue<GameObject>(); // 구름 리스트
    public int maxCount;
    public static int cloudCount = 0;

    public float minSize,maxSize; // 구름 최대, 최소 크기
    public float minHeight, maxHeight; // 구름 생성 최대, 최소 높이
    public float minSpeed, maxSpeed; // 구름 시작 속도 최대, 최소 속도
    public float minLifeTime, maxLifeTime; // 구름 생명 길이 최대, 최소
    public float minGenerateTime, maxGenerateTime; // 생성 주기

    public bool direction;

    enum DIRECTION { RIGHT = 1, LEFT = -1 }; // 방향
    private DIRECTION dir;

    private GameObject spawn;
    private Cloud_Script spawnCloud;
    private float t;

    void Start()
    {
        if (cloudList != null)
            cloudList.Clear();
        Generate();
    }

    void Update()
    {
        if (!direction)
            dir = DIRECTION.RIGHT;
        else if (direction)
            dir = DIRECTION.LEFT;
    }

    void FixedUpdate()
    {
        t += Time.deltaTime;

        if (t > Random.Range(0, maxGenerateTime) + minGenerateTime)
        {
            if (cloudCount >= maxCount + 1)
                Generate(cloudList.Dequeue());
            else
                Generate();
            t = 0;
        }
    }

    void Generate()
    {
        spawn = Instantiate(cloudPrefab[Random.Range(0, cloudPrefab.Length)],new Vector3(transform.position.x,transform.position.y + Random.Range(0,maxHeight) + minHeight, transform.position.z),transform.rotation) as GameObject; // 생성 위치
        spawn.GetComponent<Transform>().localScale = new Vector3(1, 1, 1) * (Random.Range(0, maxSize) + minSize); // 크기 설정

        spawnCloud = spawn.GetComponent<Cloud_Script>();
        spawnCloud.LifeTime = Random.Range(0, maxLifeTime) + minLifeTime; // 생명 시간 설정

        spawnCloud.Speed = (Random.Range(0, maxSpeed) + minSpeed) * (int)dir; // 속도 설정

        cloudList.Enqueue(spawn);
        cloudCount++;
    }

    void Generate(GameObject cloud)
    {
        Transform cloudTransform = cloud.GetComponent<Transform>();
        cloudTransform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(0, maxHeight) + minHeight, transform.position.z);
        cloudTransform.localScale = new Vector3(1, 1, 1) * (Random.Range(0, maxSize) + minSize); // 크기 설정

        spawnCloud = cloud.GetComponent<Cloud_Script>();
        spawnCloud.LifeTime = Random.Range(0, maxLifeTime) + minLifeTime; // 생명 시간 설정

        spawnCloud.Speed = (Random.Range(0, maxSpeed) + minSpeed) * (int)dir; // 속도 설정

        cloud.SetActive(true);
        cloudList.Enqueue(cloud);
    }
}
