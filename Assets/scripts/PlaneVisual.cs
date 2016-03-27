using UnityEngine;

public class PlaneVisual : MonoBehaviour {

    public Transform target;

    void Update()
    {
        if (target != null)
            transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
    }
}
