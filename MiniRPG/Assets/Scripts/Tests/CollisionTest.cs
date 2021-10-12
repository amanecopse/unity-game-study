using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionTest : MonoBehaviour
{

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"Collision object name: {other.gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger object name: {other.gameObject.name}");
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            RaycastHit raycastHit;
            LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");
            // int mask = (1<<8) | (1<<9)
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

            if (Physics.Raycast(ray, out raycastHit, 100.0f, mask))
            {
                Debug.Log($"Raycast hit object name: {raycastHit.collider.gameObject.tag}");
            }

            // Vector3 mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            // Vector3 dir = (mousePoint - Camera.main.transform.position).normalized;
            // RaycastHit raycastHit;
            // Debug.DrawRay(Camera.main.transform.position, dir * 100.0f, Color.red, 1.0f);

            // if (Physics.Raycast(Camera.main.transform.position, dir, out raycastHit, 100.0f))
            // {
            //     Debug.Log($"Raycast hit object name: {raycastHit.collider.gameObject.name}");
            // }
        }

    }
}
