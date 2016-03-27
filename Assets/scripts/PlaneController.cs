using UnityEngine;
using System.Collections;

public class PlaneController : MonoBehaviour {

    public static PlaneController main { private set; get; }

    void Start()
    {
        main = this;
    }

    void OnPreRender()
    {
        GL.wireframe = true;
    }

    void OnPostRender()
    {
        GL.wireframe = false;
    }
}
