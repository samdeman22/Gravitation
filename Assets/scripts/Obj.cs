
using UnityEngine;
using System.Collections.Generic;

public class Obj : MonoBehaviour {

    public Vector3 v = Vector3.zero, a = Vector3.zero;
    public float scale = 0.5f;
    public static List<Obj> effectingMasses = new List<Obj>();
    public bool isOrigin { private set; get; }
    public Rigidbody rb { get; private set; }
    public ParentPosition target { get; set; }

    //private TrailRenderer tr;
    private MeshRenderer mr;
    private Controller controller;
    private ParticleSystem pt;
    private static bool gravityActive = true;
    private Vector3 lastPos = Vector3.zero;

    public void AddEffectingMass(Obj o)
    {
        effectingMasses.Add(o);
    }

    public void SetEffectingMasses(List<Obj> os)
    {
        effectingMasses = os;
    }

    public void SetAsOrigin()
    {
        Controller.origin = this;
        isOrigin = true;
        var dp = -transform.position;
        var dv = -rb.velocity;
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;
        ((PlaneVisual)FindObjectOfType(typeof(PlaneVisual))).target = transform;
        //((ParentPosition)Camera.main.GetComponent(typeof(ParentPosition))).positionParent = transform;
        foreach (Obj other in effectingMasses)
        {
            if (other != this)
            {
                other.isOrigin = false;
                other.transform.position += dp;
                other.rb.velocity += dv;
                //other.tr.Clear();
            }
        }
    }

    void Awake()
    {
        //initialise the components we need
        controller = (Controller)FindObjectOfType(typeof(Controller));
        rb = (Rigidbody)gameObject.AddComponent(typeof(Rigidbody));
        //tr = (TrailRenderer)gameObject.AddComponent(typeof(TrailRenderer));
        pt = Instantiate(Resources.Load<ParticleSystem>("particle trail")) as ParticleSystem;
        pt.transform.SetParent(transform);
        pt.transform.localPosition = Vector3.zero;
        //pt = FindObjectOfType<ParticleSystem>();
        mr = (MeshRenderer)gameObject.GetComponent(typeof(MeshRenderer));
        target = (ParentPosition)gameObject.AddComponent(typeof(ParentPosition));

        rb.mass = controller.mass;
        scale = controller.massScale;
        effectingMasses.Add(this);

        //none of this, thank you very much
        rb.useGravity = false;
        //ahem...

        rb.angularDrag = 0;
        rb.drag = controller.dampening;
        rb.mass = Mathf.Abs(rb.mass);
        transform.localScale = Vector3.one * scale;
        v = new Vector3(Random.Range(-controller.randomseed, controller.randomseed), Random.Range(-controller.randomseed, controller.randomseed), Random.Range(-controller.randomseed, controller.randomseed));

        //generate a random colour
        Color c = new Color(Random.value, Random.value, Random.value, 1f);
        mr.material = Resources.Load("star") as Material;
        mr.material.color = c;
        mr.material.SetColor("_EmissionColor", c);

        pt.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", new Color(1f - c.r, 1f - c.g, 1f - c.b, 0.1f));

        //set the trail to the inverse colour
        //tr.material = Resources.Load("trail") as Material;
        //tr.material.SetColor("_TintColor", new Color(1f - c.r, 1f - c.g, 1f - c.b, 0.1f));
        //tr.time = controller.trailTime;
        //tr.startWidth = 0.05f;
        //tr.endWidth = 0.005f;
        //tr.enabled = controller.trailActive;
    }

    void Start ()
    {
        rb.AddForce(v, ForceMode.VelocityChange);
        rb.drag = controller.dampening;
        if (controller.planar)
        {
            transform.position = new Vector3(transform.position.x,0, transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
        pt.Play();
    }
	
	void Update () {
        if (gravityActive)
            rb.AddForce(GetGravity());
        //tr.enabled = controller.trailActive;

        ParticleSystem.Particle[] parts = new ParticleSystem.Particle[pt.particleCount];
        pt.GetParticles(parts);
        for (int i = 0; i < parts.Length; i++)
            parts[i].position += (transform.position - lastPos);

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (isOrigin && Input.GetKeyDown(KeyCode.R))
            {
                Controller.origin = null;
                isOrigin = false;
                ((PlaneVisual)FindObjectOfType(typeof(PlaneVisual))).target = null;
            }
        }
        lastPos = transform.position;
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.control)
        {
            if (e.keyCode == KeyCode.R)
            {

            }
            else if (e.keyCode == KeyCode.L)
            {

            }
        }

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.T)
        {
            pt.GetComponent<ParticleSystem>().Clear();
            //tr.Clear();
        }
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
                switch (controller.gt)
                {
                case Controller.GravityType.Linear:
                    A += direction * controller.G * other.rb.mass * rb.mass * mag;
                    break;
                case Controller.GravityType.Reciprocal:
                    A += direction * controller.G * other.rb.mass *rb.mass / mag;
                    break;
                case Controller.GravityType.Sqare:
                    A += direction * controller.G * other.rb.mass *rb.mass * Mathf.Pow(mag, 2);
                    break;
                case Controller.GravityType.InverseSquare:
                    A += direction * controller.G * other.rb.mass *rb.mass / Mathf.Pow(mag, 2);
                    break;
                case Controller.GravityType.Cube:
                    A += direction * controller.G * other.rb.mass *rb.mass * Mathf.Pow(mag, 3);
                    break;
                case Controller.GravityType.InverseCube:
                    A += direction * controller.G * other.rb.mass *rb.mass / Mathf.Pow(mag, 3);
                    break;
                case Controller.GravityType.ConstantProportionate:
                    A += direction * controller.G * other.rb.mass *rb.mass;
                    break;
                case Controller.GravityType.Repulsive:
                    A += -direction * controller.G * other.rb.mass *rb.mass / Mathf.Pow(mag, 2);
                    break;
                case Controller.GravityType.RepulsiveLinear:
                    A += -direction * controller.G * other.rb.mass *rb.mass * mag;
                    break;
                case Controller.GravityType.Sinusoidal:
                    A += direction * controller.G * Mathf.Sin(Time.deltaTime) * other.rb.mass *rb.mass / Mathf.Pow(mag, 2);
                    break;
                case Controller.GravityType.WobblyInverseSquare:
                    A += direction * Random.Range(0, 2 * controller.G) * other.rb.mass *rb.mass / Mathf.Pow(mag, 2);
                    break;
                case Controller.GravityType.AttractiveRandom:
                    A += direction * Random.Range(0, controller.G) * other.rb.mass *rb.mass;
                    break;
                case Controller.GravityType.Random:
                    A += direction * Random.Range(-controller.G, controller.G) * other.rb.mass *rb.mass * mag;
                    break;
                case Controller.GravityType.SuicideRandom:
                    A += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * Random.Range(-controller.G * 100, 100 * controller.G);
                    break;
                case Controller.GravityType.Spring:
                    A += direction * controller.G * other.rb.mass *rb.mass * (mag - controller.O);
                    break;
                case Controller.GravityType.SpringModuloRadius:
                    A += direction * controller.G * other.rb.mass *rb.mass * (mag - controller.O * Mathf.Abs(Mathf.Sin(controller.G * Time.deltaTime)));
                    break;
                }
            }
        }
        return A;
    }
}
