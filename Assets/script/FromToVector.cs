using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromToVector : MonoBehaviour
{
    [SerializeField]
    GameObject _start;
    [SerializeField]
    GameObject _end;
    [SerializeField]
    GameObject _newStart;
    [SerializeField]
    GameObject _newEnd;
    [SerializeField]
    GameObject _toRotate;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = _start.transform.position - _end.transform.position;
        Vector3 target = _newStart.transform.position - _newEnd.transform.position;
        Debug.DrawLine(Vector3.zero, origin);
        if (Input.GetKeyDown(KeyCode.Space))
        {


            Quaternion rotation = RotateFromTo(origin, target);
            _toRotate.transform.rotation = rotation;
        }
    }

    /// <summary>
    /// Get the quaternion for rotation between two vectors. (this only take one axis into account)
    /// </summary>
    /// <param name="origin">Original orientation vector</param>
    /// <param name="target">Target orientation vector</param>
    public Quaternion RotateFromTo(Vector3 origin, Vector3 target)
    {
        origin.Normalize();
        target.Normalize();

        float dot = Vector3.Dot(origin, target);
        float s = Mathf.Sqrt((1 + dot) * 2);
        float invs = 1 / s;

        Vector3 c = Vector3.Cross(origin, target);

        Quaternion rotation = new Quaternion();

        rotation.x = c.x * invs;
        rotation.y = c.y * invs;
        rotation.z = c.z * invs;
        rotation.w = s * 0.5f;

        rotation.Normalize();

        return rotation;

        //source: https://stackoverflow.com/questions/21828801/how-to-find-correct-rotation-from-one-vector-to-another
    }

}
