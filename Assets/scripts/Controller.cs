using UnityEngine;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

    
    public enum GravityType
    {
        [Tooltip("Force towards other mass is proportional to both the distance and both the masses")]
        Linear,
        [Tooltip("Force towards other mass is proportional to the masses and inversely to the distance")]
        Reciprocal,
        [Tooltip("Force towards other mass is proportional to both the masses and the square of the distance")]
        Sqare,
        [Tooltip("Force towards other mass is proportional to the mass and inversely to the square distance")]
        InverseSquare,
        [Tooltip("Force towards other mass is proportional to both the masses and the cube of the distance")]
        Cube,
        [Tooltip("Force towards other mass is proportional to both the masses and inversely to the cube distance")]
        InverseCube,
        [Tooltip("Force towards other mass is proportional only to the masses")]
        ConstantProportionate,
        Repulsive,
        RepulsiveLinear,
        Sinusoidal,
        WobblyInverseSquare,
        AttractiveRandom,
        Random,
        SuicideRandom,
        Spring,
        SpringModuloRadius
    }

    [Tooltip("Select the placement model")]
    public PrimitiveType placementType = PrimitiveType.Sphere;

    public float dampening = 1f;

    [Tooltip("Select an alternate operation mode for gravity")]
    public GravityType gt = GravityType.InverseSquare;

    Obj[] obs;
    Ray ray;
    public float trailTime = 20;
    public float G = 6.673e-11f;
    public float randomseed = 0.5f;
    public float mass = 9e+08f;
    public float massScale = 0.5f;
    public float O = 1;

    public bool randomDistribution = false;
    public int randomVolume = 100;
    public int randomAmount = 100;

    public bool willClump = false;

    void Start()
    {
        foreach (Obj o in GetComponentsInChildren<Obj>())
        {
            o.gameObject.GetComponent<Rigidbody>().drag = dampening;
        }

        if (randomDistribution)
            for (int i = 0; i < randomAmount; i++)
                PlaceNewMass(new Vector3(Random.Range(randomVolume, -randomVolume), Random.Range(randomVolume, -randomVolume), Random.Range(randomVolume, -randomVolume)));

    }

    void PlaceNewMass(Vector3 location)
    {
        int n = FindObjectsOfType<Obj>().Length + 1;
        GameObject newMass = GameObject.CreatePrimitive(placementType);
        newMass.name = "mass" + n;
        newMass.transform.SetParent(transform);
        newMass.transform.position = location;
        newMass.AddComponent<Obj>();
    }

    Vector3 r = Vector3.zero;
    // Use this for initialization
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit rch;
            Physics.Raycast(ray, out rch);
            Debug.Log(rch.point);
            r = rch.point;
            PlaceNewMass(r);
        }
    }

    void OnMouseDown()
    {
        Debug.Log(Input.mousePosition);
        ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        RaycastHit rch;
        Physics.Raycast(ray, out rch);
        Debug.Log(rch.point);
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 200f, 200f), Enum.GetName(typeof(GravityType), gt));
    }
}
