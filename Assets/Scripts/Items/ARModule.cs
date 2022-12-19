//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.Linq;
//
//public class ARModule : MonoBehaviour 
//{
//    public float radius;
//    LineRenderer lineRenderer;
//    float timer;
//    Transform currentRocket;
//    Transform rootTransform;
//    float searchTimer;
//
//	void Start ()
//    {
//        rootTransform = transform.parent;
//        lineRenderer = GetComponent<LineRenderer>();
//	}
//
////    void LateUpdate()
////    {
////        if (currentRocket)
////        {
////            lineRenderer.enabled = true;
////            lineRenderer.SetPosition(0, rootTransform.position);
////            lineRenderer.SetPosition(1, currentRocket.position);
////            if ((timer += Time.deltaTime) > 0.18f)
////            {
////                lineRenderer.enabled = false;   
////                Destroy(currentRocket.gameObject);
////                timer = 0f;
////            }
////        }
////        else
////        {
////            lineRenderer.enabled = false;
////            if ((searchTimer += Time.deltaTime) > 0.3f)
////            {
////                float distance = 0f;
////                float currentDistance = 9999f;
////                foreach (var rocket in GameManager.rockets)
////                { 
////                    if (rocket.target && rocket.target.root == PlayerController.inst.transform.root)
////                    {
////                        distance = Vector2.Distance(transform.position, rocket.transform.position);
////                        if (distance < radius && distance < currentDistance)
////                        {
////                            currentDistance = distance;
////                            currentRocket = rocket.transform;
////                        }
////                    }
////                }
////                searchTimer = 0f;
////            }
////        }
////    }
//}
