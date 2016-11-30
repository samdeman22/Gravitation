using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float moveCoefficient = 1;
    public float VerticalSpeed = 1;
    Rigidbody rb;

	void Awake()
	{
        //Cursor.lockState = CursorLockMode.Locked;
        //Screen.fullScreen = true;
        Cursor.visible = false;
	}

	// Use this for initialization
	void Start ()
	{
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
	}
	
	// Update is called once per frame
	void Update () 
	{
        float vertical = 0;

        if (Input.GetKey(KeyCode.Space))
            vertical += 1f;
        if (Input.GetKey(KeyCode.LeftShift))
            vertical -= 1f;

        Vector3 v = new Vector3(0, 0, 0);
		v = new Vector3(Input.GetAxis("Horizontal")*moveCoefficient, 0, Input.GetAxis("Vertical")*moveCoefficient);
		rb.velocity = Camera.main.transform.rotation * v;
        rb.velocity += new Vector3(0, vertical * VerticalSpeed, 0);
        //rb.AddRelativeForce(new Vector3(Input.GetAxis("Horizontal") * moveCoefficient, 0, Input.GetAxis("Vertical") * moveCoefficient), ForceMode.VelocityChange);
        //rb.AddForce(new Vector3(0, vertical * VerticalSpeed, 0), ForceMode.VelocityChange);
        Debug.Log("vertical " + Input.GetAxis("Vertical") + " horizontal " + Input.GetAxis("Horizontal") + " | " + v + " vs " + rb.velocity);
    }
}
