using UnityEngine;
using System.Collections;

public class RotationMove : MonoBehaviour {

   public float agularSpeed; // 각속도
   public Transform groundPosition; // 땅 위치
   public GameObject groundPrefab; // 땅 프리팹

   private Vector3 direction; public Vector3 Direction { get { return direction; } set { direction = value; }} // 방향

   void Start()
   {
       direction = new Vector3(0, 0, 1);
       groundPrefab = Instantiate(groundPrefab) as GameObject;
   }

   void FixedUpdate()
   {
        transform.Rotate(direction * agularSpeed * Time.deltaTime);
        groundPrefab.transform.position = groundPosition.position;
   }
}
