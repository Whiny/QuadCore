using UnityEngine;
using System.Collections;

public class RotationMove : MonoBehaviour {

   public float agularSpeed;
   public Transform groundPosition;
   public GameObject groundPrefab;
   private Vector3 direction;

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
