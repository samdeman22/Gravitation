using UnityEngine;
using System.Collections.Generic;

public class Controller : MonoBehaviour {

    
    public enum GravityType
    {
        [Tooltip("no force is applied")]
        None,
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
    public bool planar = false;
    public float G = 6.673e-11f;
    public float randomseed = 0.5f;
    public float mass = 9e+08f;
    public float massScale = 0.5f;
    public float O = 1;

    public static Obj origin = null;

    public bool randomDistribution = false;
    public int randomVolume = 100;
    public int randomAmount = 100;
    public bool trailActive { get; private set; }

    private Vector3 rayPoint = Vector3.zero;

    void Start()
    {
        //Camera.main.gameObject.AddComponent(typeof(ParentPosition));
        trailActive = true;
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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit rch;
            Physics.Raycast(ray, out rch);
            //Debug.Log(rch.point);
            Obj other = null;
            if (Input.GetKey(KeyCode.LeftControl) && (other = rch.collider.GetComponent<Obj>()) != null)
            {
                other.SetAsOrigin();
            }
            else
            {
                rayPoint = rch.point;
                PlaceNewMass(rayPoint);
            }
            ReactToInput();
        }
    }

    void ReactToInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.T))
                trailActive = !trailActive;
        }
    }
}
