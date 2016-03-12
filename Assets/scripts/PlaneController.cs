using UnityEngine;
using System.Collections;

public class PlaneController : MonoBehaviour {

    void OnPreRender()
    {
        GL.wireframe = true;
    }

    void OnPostRender()
    {
        GL.wireframe = false;
    }
}
