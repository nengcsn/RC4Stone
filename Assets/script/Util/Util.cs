using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public static class Util 
{
    public static bool IsPointInCollider(Collider collider, Vector3 point)
    {
        var bounds = collider.bounds;

        if (!bounds.Contains(point)) //is point contains in the box
        {
            return false;
        }
        else
        {
            RaycastHit raycasthit;
            var upraycast = new Ray(point, Vector3.up);
            var downraycast = new Ray(point, Vector3.down);

            if (collider.Raycast(upraycast, out raycasthit, float.MaxValue) || collider.Raycast(downraycast, out raycasthit, float.MaxValue))
            {
                return false;
            }
            else
            {
                float updirection, downdirection;
                bounds.IntersectRay(upraycast, out updirection);
                bounds.IntersectRay(downraycast, out downdirection);

                Vector3 upoint = upraycast.GetPoint(-updirection * 1.5f);
                Vector3 dpoint = downraycast.GetPoint(-downdirection * 1.5f);

                if (collider.Raycast(new Ray(upoint, Vector3.down), out raycasthit, float.MaxValue)
                    || collider.Raycast(new Ray(dpoint, Vector3.up), out raycasthit, float.MaxValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }

    /// <summary>
    /// Get the quaternion for rotation between two vectors. (this only take one axis into account)
    /// </summary>
    /// <param name="origin">Original orientation vector</param>
    /// <param name="target">Target orientation vector</param>
    public static Quaternion RotateFromTo(Vector3 origin, Vector3 target)
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

    public static float[] AsArray(this Vector3 vector)
    {
        return new float[] { vector.x, vector.y, vector.z };
    }

    public static Vector3 AsVector(this float[] vector)
    {
        return new Vector3( vector[0], vector[1], vector[2]);
    }

    public static bool CheckBounds(VoxelGrid grid, Vector3Int index)
    {
        return !(index.x < 0 || index.x >= grid.GridSize.x ||
            index.y < 0 || index.y >= grid.GridSize.y ||
            index.z < 0 || index.z >= grid.GridSize.z);
    }

}
