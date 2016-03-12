using UnityEngine;
using System.Collections.Generic;

public class Obj : MonoBehaviour {

    public Vector3 v = Vector3.zero, a = Vector3.zero;
    public float mass = 1f;
    public float scale = 0.5f;
    public static List<Obj> effectingMasses = new List<Obj>();

    private TrailRenderer tr;
    private Rigidbody rb;
    private MeshRenderer mr;
    private Controller controller;

    public void AddEffectingMass(Obj o)
    {
        effectingMasses.Add(o);
    }

    public void SetEffectingMasses(List<Obj> os)
    {
        effectingMasses = os;
    }

    void Awake()
    {
        //initialise the components we need
        controller = (Controller)FindObjectOfType(typeof(Controller));
        rb = (Rigidbody)gameObject.AddComponent(typeof(Rigidbody));
        tr = (TrailRenderer)gameObject.AddComponent(typeof(TrailRenderer));
        mr = (MeshRenderer)gameObject.GetComponent(typeof(MeshRenderer));

        mass = controller.mass;
        scale = controller.massScale;
        effectingMasses.Add(this);

        //none of this, thank you very much
        rb.useGravity = false;

        rb.angularDrag = 0;
        rb.drag = controller.dampening;
        rb.constraints = RigidbodyConstraints.None;
        rb.mass = Mathf.Abs(mass);
        transform.localScale = Vector3.one * scale;
        v = new Vector3(Random.Range(-controller.randomseed, controller.randomseed), Random.Range(-controller.randomseed, controller.randomseed), Random.Range(-controller.randomseed, controller.randomseed));
        rb.AddForce(v, ForceMode.VelocityChange);
        v = Vector3.zero;

        //generate a random colour
        Color c = new Color(Random.value, Random.value, Random.value, 1f);
        mr.material = Resources.Load("star") as Material;
        mr.material.color = c;
        mr.material.SetColor("_EmissionColor", c);

        //set the trail to the inverse colour
        tr.material = Resources.Load("trail") as Material;
        tr.material.SetColor("_TintColor", new Color(1f - c.r, 1f - c.g, 1f - c.b, 0.1f));
        tr.time = controller.trailTime;
        tr.startWidth = 0.05f;
        tr.endWidth = 0.005f;
    }

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update () {
        rb.AddForce(GetGravity());
        rb.drag = controller.dampening;
        ReactToInput();
	}

    void ReactToInput()
    {
        //togglable abilities
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //lights
            if (Input.GetKey(KeyCode.L))
            {

            }

            //trails
            if (Input.GetKeyDown(KeyCode.T))
                tr.enabled = !tr.enabled;
        }

        if (Input.GetKeyDown(KeyCode.T))
            tr.Clear();
    }

    Vector3 GetGravity()
    {
        Vector3 A = Vector3.zero;
        foreach (Obj other in effectingMasses)
        {
            if (other != this)
            {
                Vector3 direction = other.transform.position - transform.position;
                float mag = direction.magnitude;
                direction.Normalize();
                direction /= 2;
                //divide by 2, because we are applying the same force in both directions,
                //in order to get the correct relative acceleration between any two objects
                switch (controller.gt)
                {
                    case Controller.GravityType.Linear:
                        A += direction * controller.G * other.mass * mass * mag;
                        break;
                    case Controller.GravityType.Reciprocal:
                        A += direction * controller.G * other.mass * mass / mag;
                        break;
                    case Controller.GravityType.Sqare:
                        A += direction * controller.G * other.mass * mass * Mathf.Pow(mag, 2);
                        break;
                    case Controller.GravityType.InverseSquare:
                        A += direction * controller.G * other.mass * mass / Mathf.Pow(mag, 2);
                        break;
                    case Controller.GravityType.Cube:
                        A += direction * controller.G * other.mass * mass * Mathf.Pow(mag, 3);
                        break;
                    case Controller.GravityType.InverseCube:
                        A += direction * controller.G * other.mass * mass / Mathf.Pow(mag, 3);
                        break;
                    case Controller.GravityType.ConstantProportionate:
                        A += direction * controller.G * other.mass * mass;
                        break;
                    case Controller.GravityType.Repulsive:
                        A += -direction * controller.G * other.mass * mass / Mathf.Pow(mag, 2);
                        break;
                    case Controller.GravityType.RepulsiveLinear:
                        A += -direction * controller.G * other.mass * mass * mag;
                        break;
                    case Controller.GravityType.Sinusoidal:
                        A += direction * controller.G * Mathf.Sin(Time.deltaTime) * other.mass * mass / Mathf.Pow(mag, 2);
                        break;
                    case Controller.GravityType.WobblyInverseSquare:
                        A += direction * Random.Range(0, 2 * controller.G) * other.mass * mass / Mathf.Pow(mag, 2);
                        break;
                    case Controller.GravityType.AttractiveRandom:
                        A += direction * Random.Range(0, controller.G) * other.mass * mass;
                        break;
                    case Controller.GravityType.Random:
                        A += direction * Random.Range(-controller.G, controller.G) * other.mass * mass * mag;
                        break;
                    case Controller.GravityType.SuicideRandom:
                        A += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * Random.Range(-controller.G * 100, 100 * controller.G);
                        break;
                    case Controller.GravityType.Spring:
                        A += direction * controller.G * other.mass * mass * (mag - controller.O);
                        break;
                    case Controller.GravityType.SpringModuloRadius:
                        A += direction * controller.G * other.mass * mass * (mag - controller.O * Mathf.Abs(Mathf.Sin(controller.G * Time.deltaTime)));
                        break;
                }
            }
        }
        return A;
    }

	public void SetAcceleration (Vector3 a)
	{
		this.a = a;
	}

	public void AddAcceleration (Vector3 a)
	{
		this.a += a;
	}

	public void SetVelocity(Vector3 v)
	{
		this.v = v;
	}

	public void AddVelocity (Vector3 v)
	{
		this.v += v;
	}

	public void SetPosition (Vector3 p)
	{
		transform.position = p;
	}
}
