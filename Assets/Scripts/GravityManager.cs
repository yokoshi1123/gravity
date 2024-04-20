using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float jumpForce = 20.0f;
    public float gravityDefault = 5.0f;

    private Vector2 startMPosition = Vector2.zero;
    private Vector2 endMPosition = Vector2.zero;
    private float CAMERAZPOSITION = -20f;
    private float GFIELDHEIGHT = 16f;
    private GameObject destroyGF;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject gravityField = (GameObject)Resources.Load("GravityField");

        if (Input.GetMouseButtonDown(0))
        { 
            startMPosition = Input.mousePosition;
            startMPosition = Camera.main.ScreenToWorldPoint(new Vector3(startMPosition.x, startMPosition.y, CAMERAZPOSITION)); 
            // Debug.Log("Start:(" + startMPosition.x + ", " + startMPosition.y + ")");

        }

        if (Input.GetMouseButtonUp(0))
        {
            endMPosition = Input.mousePosition;
            endMPosition = Camera.main.ScreenToWorldPoint(new Vector3(endMPosition.x, endMPosition.y, CAMERAZPOSITION));
            Debug.Log("End:(" + endMPosition.x + ", " + endMPosition.y + ")");

            destroyGF = GameObject.FindWithTag("GravityField");
            if (destroyGF != null) 
            {
                Destroy(destroyGF);
            }
            GameObject gField = (GameObject)Instantiate(gravityField, (startMPosition + endMPosition) / 2, Quaternion.identity);

            // Debug.Log("Instantiated");
            gField.transform.localScale = new Vector2(Mathf.Abs(startMPosition.x) + Mathf.Abs(endMPosition.x), GFIELDHEIGHT);

        }
    }
}
