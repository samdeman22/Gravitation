using UnityEngine;
using System.Collections;

public class ParentPosition : MonoBehaviour
{
    public Transform positionParent{ get; set; }

	void Update ()
    {
	    if (positionParent != null)
        {
            Vector3 pos = transform.position;
            transform.position = positionParent.transform.position;
            transform.Translate(pos);
        }
	}
}
