using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Runtime.InteropServices;

public enum ErrorCode { GenericError, InventoryFull, DuplicatedTool, NotEmpty, NotEnoughResources }

public class CustomException: Exception
{
    ErrorCode errorCode;
    public ErrorCode ErrorCode
    {
        get { return errorCode; }
    }

    public CustomException(ErrorCode errorCode, string message) : base(message)
    {
        this.errorCode = errorCode;
    }

    public override string ToString()
    {
        return string.Format("Error({0}):{1}", errorCode, base.Message);
    }
}


public class Utility
{
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern int GetShortPathName(
                [MarshalAs(UnmanagedType.LPTStr)]
                   string path,
                [MarshalAs(UnmanagedType.LPTStr)]
                   StringBuilder shortPath,
                int shortPathLength
                );

    public static Vector3 GetRandomPointOnMesh(Mesh mesh)

    {
        //if you're repeatedly doing this on a single mesh, you'll likely want to cache cumulativeSizes and total
        float[] sizes = GetTriSizes(mesh.triangles, mesh.vertices);
        float[] cumulativeSizes = new float[sizes.Length];
        float total = 0;

        for (int i = 0; i < sizes.Length; i++)
        {
            total += sizes[i];
            cumulativeSizes[i] = total;
        }

        //so everything above this point wants to be factored out
        float randomsample = UnityEngine.Random.value * total;


        int triIndex = -1;



        for (int i = 0; i < sizes.Length; i++)

        {
           
            if (randomsample <= cumulativeSizes[i])

            {

                triIndex = i;

                break;

            }

        }



        if (triIndex == -1) Debug.LogError("triIndex should never be -1");
        Vector3 a = mesh.vertices[mesh.triangles[triIndex * 3]];
        Vector3 b = mesh.vertices[mesh.triangles[triIndex * 3 + 1]];
        Vector3 c = mesh.vertices[mesh.triangles[triIndex * 3 + 2]];


        //generate random barycentric coordinates
        float r = UnityEngine.Random.value;
        float s = UnityEngine.Random.value;

        if (r + s >= 1)
        {
            r = 1 - r;
            s = 1 - s;
        }

        //and then turn them back to a Vector3
        Vector3 pointOnMesh = a + r * (b - a) + s * (c - a);
        return pointOnMesh;



    }

    static float[] GetTriSizes(int[] tris, Vector3[] verts)

    {

        int triCount = tris.Length / 3;

        float[] sizes = new float[triCount];

        for (int i = 0; i < triCount; i++)

        {

            sizes[i] = .5f * Vector3.Cross(verts[tris[i * 3 + 1]] - verts[tris[i * 3]], verts[tris[i * 3 + 2]] - verts[tris[i * 3]]).magnitude;

        }

        return sizes;



        /*

         * 

         * more readably:

         * 

for(int ii = 0 ; ii < indices.Length; ii+=3)

{

    Vector3 A = Points[indices[ii]];

    Vector3 B = Points[indices[ii+1]];

    Vector3 C = Points[indices[ii+2]];

    Vector3 V = Vector3.Cross(A-B, A-C);

    Area += V.magnitude * 0.5f;

}

         * 

         * 

         * */

    }


    public static bool CameraIsClipped(Vector3 targetPos, Vector3 cameraPos, out Vector3 clippedPos)
    {
        // Clipping
        Vector3 dir = cameraPos - targetPos;
        RaycastHit hit;
        if (Physics.Raycast(targetPos, dir.normalized, out hit, dir.magnitude, LayerMask.GetMask("ClippingEver", "ClippingInside")))
        {
         
            int layer = hit.transform.gameObject.layer;

            if (layer == LayerMask.NameToLayer("ClippingInside") && hit.collider.bounds.Contains(cameraPos) || layer != LayerMask.NameToLayer("ClippingInside"))
            {
                clippedPos = hit.point;
                return true;
            }
            //transform.position = hit.point;
        }
        clippedPos = Vector3.zero;
        return false;
    }

    public static GameObject ObjectPopIn(GameObject prefab, Transform parent = null)
    {
        //GameObject g = GameObject.Instantiate(prefab, parent);

        //g.transform.localPosition = Vector3.zero;
        //g.transform.localRotation = Quaternion.identity;
        //g.transform.localScale = Vector3.zero;

        //LeanTween.scale(g, Vector3.one, 1f).setEaseOutElastic();

        //return g;
        return ObjectPopIn(prefab, Vector3.zero, Vector3.zero, Vector3.one, parent);
    }

    public static GameObject ObjectPopIn(GameObject prefab, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale, Transform parent = null)
    {
        GameObject g;
        
        if(parent != null)
            g = GameObject.Instantiate(prefab, parent);
        else
            g = GameObject.Instantiate(prefab);

        g.transform.localPosition = localPosition;
        g.transform.localEulerAngles = localEulerAngles;
        g.transform.localScale = Vector3.zero;

        LeanTween.scale(g, localScale, 1f).setEaseOutElastic();

        return g;
    }

    public static void ObjectPopOut(GameObject obj)
    {
        LeanTween.scale(obj, Vector3.zero, 1f).setEaseInElastic();
        GameObject.Destroy(obj, 1);
        //ObjectPopOut(obj, 1);
    }

    //public static void ObjectPopOut(GameObject obj, float time)
    //{
    //    LeanTween.scale(obj, Vector3.zero, time).setEaseInElastic();
    //    GameObject.Destroy(obj, time);
    //}

    /**
     * Get a random point on the default navigation mesh;
     * */
    public static Vector3 GetRandomPointOnNavMesh()
    {
        Mesh groundMesh = GameObject.FindGameObjectWithTag("Ground").GetComponent<MeshCollider>().sharedMesh;
        Vector3 source = Utility.GetRandomPointOnMesh(groundMesh);
        UnityEngine.AI.NavMeshHit hit;
        float radius = 10;
        if (UnityEngine.AI.NavMesh.SamplePosition(source, out hit, radius, UnityEngine.AI.NavMesh.AllAreas))
        {
            source = hit.position;
        }

        return source;
    }


    
    public static void ShowItemDescription(UnityEngine.UI.Text ui, string description)
    {
        ui.text = description;
        ui.color = Color.white;
        ui.transform.parent.gameObject.SetActive(true);
    }

    public static void ShowThanks(UnityEngine.UI.Text ui, string thanks)
    {
        ui.text = thanks;
        ui.color = Color.white;
        ui.transform.parent.gameObject.SetActive(true);
    }

    public static void HideItemDescription(UnityEngine.UI.Text ui)
    {
        ui.transform.parent.gameObject.SetActive(false);
        ui.text = "";
    }

    public static void HideThanks(UnityEngine.UI.Text ui)
    {
        ui.transform.parent.gameObject.SetActive(false);
        ui.text = "";
    }
}





