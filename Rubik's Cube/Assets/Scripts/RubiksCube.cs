using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rotation
{
    U, Ui, D, Di,
    L, Li, R, Ri,
    F, Fi, B, Bi
};

public class RubiksCube : MonoBehaviour
{
    private List<Transform> allCubies = new List<Transform>();

    [HideInInspector]
    public bool turning = false;

    public Dictionary<Rotation, Vector3> rotationsToVectors = new Dictionary<Rotation, Vector3>
    {
        { Rotation.U, Vector3.up},
        { Rotation.Ui, Vector3.up},
        { Rotation.D, Vector3.down},
        { Rotation.Di, Vector3.down},
        { Rotation.L, Vector3.right},
        { Rotation.Li, Vector3.right},
        { Rotation.R, Vector3.left},
        { Rotation.Ri, Vector3.left},
        { Rotation.F, Vector3.forward},
        { Rotation.Fi, Vector3.forward},
        { Rotation.B, Vector3.back},
        { Rotation.Bi, Vector3.back},
    };


    void Awake()
    {
        InitCubies();
    }
    public void InitCubies()
    {
        foreach (Transform cubie in GetComponentInChildren<Transform>())
        {
            if (cubie != transform)
            {
                allCubies.Add(cubie);
            }
        }
    }

    public void Turn(string rotation)
    {
        Turn((Rotation)Enum.Parse(typeof(Rotation), rotation));
    }

    public void Turn(Rotation rotation)
    {
        bool inverted = (int)rotation % 2 == 1;

        Vector3 faceNormal = rotationsToVectors[rotation];

        List<Transform> faceCubies = GetCubiesOfFace(faceNormal);

        Transform faceCenter = faceCubies[0];

        foreach (Transform cubie in faceCubies)
        {
            if (cubie != faceCenter)
            {
                cubie.parent = faceCenter;
                Debug.Log($"Added {cubie} to face");
            }
        }

        if (!turning)
        { StartCoroutine(Rotate(faceCenter, faceNormal, inverted)); }
        else
        {
            StopAllCoroutines();
            StartCoroutine(Rotate(faceCenter, faceNormal, inverted));
        }
    }

    IEnumerator Rotate(Transform faceCenter, Vector3 faceNormal, bool counterClockWise)
    {
        turning = true;

        Quaternion startingRotation = faceCenter.rotation;
        Quaternion targetRotation = Quaternion.Euler(startingRotation.eulerAngles + faceNormal * (counterClockWise ? -90 : 90));

        float t = 0;
        while (t < 1)
        {
            faceCenter.rotation = Quaternion.Lerp(startingRotation, targetRotation, t);
            t += Time.deltaTime;
            yield return null;
        }
        faceCenter.rotation = targetRotation;

        // Reset parentage and perfectly round positions and rotations to ensure large amounts of rotations don't break it
        foreach (Transform cubie in faceCenter.GetComponentsInChildren<Transform>())
        {
            if (cubie != faceCenter) { cubie.parent = transform; }
            cubie.position = new Vector3(Mathf.Round(cubie.position.x * 10), Mathf.Round(cubie.position.y * 10), Mathf.Round(cubie.position.z * 10)) / 10;

            Vector3 cubieEuler = cubie.rotation.eulerAngles;
            cubie.rotation = Quaternion.Euler(Mathf.Round(cubieEuler.x), Mathf.Round(cubieEuler.y), Mathf.Round(cubieEuler.z));
        }

        turning = false;
    }

    List<Transform> GetCubiesOfFace(Vector3 normal)
    {
        if (normal.magnitude != 1)
        {
            Debug.LogError("In GetCubiesOfFace(), do not use normals without a magnitude of 1.");

            return new List<Transform>();
        }

        List<Vector3> faceCubiePositions = new List<Vector3>();

        Vector3 centerCubiePosition = normal;
        faceCubiePositions.Add(centerCubiePosition);

        normal = new Vector3(Mathf.Abs(normal.x), Mathf.Abs(normal.y), Mathf.Abs(normal.z));

        if (normal.x == 1 || normal.y == 1)
        {
            faceCubiePositions.Add(centerCubiePosition + Vector3.forward);
            faceCubiePositions.Add(centerCubiePosition + Vector3.back);
        }

        if (normal.x == 1 || normal.z == 1)
        {
            faceCubiePositions.Add(centerCubiePosition + Vector3.up);
            faceCubiePositions.Add(centerCubiePosition + Vector3.down);
        }

        if (normal.y == 1 || normal.z == 1)
        {
            faceCubiePositions.Add(centerCubiePosition + Vector3.left);
            faceCubiePositions.Add(centerCubiePosition + Vector3.right);
        }

        if (normal.y == 1)
        {
            faceCubiePositions.Add(centerCubiePosition + Vector3.forward + Vector3.left);
            faceCubiePositions.Add(centerCubiePosition + Vector3.forward + Vector3.right);
            faceCubiePositions.Add(centerCubiePosition + Vector3.back + Vector3.left);
            faceCubiePositions.Add(centerCubiePosition + Vector3.back + Vector3.right);
        }

        if (normal.x == 1)
        {
            faceCubiePositions.Add(centerCubiePosition + Vector3.down + Vector3.forward);
            faceCubiePositions.Add(centerCubiePosition + Vector3.down + Vector3.back);
            faceCubiePositions.Add(centerCubiePosition + Vector3.up + Vector3.forward);
            faceCubiePositions.Add(centerCubiePosition + Vector3.up + Vector3.back);
        }

        if (normal.z == 1)
        {
            faceCubiePositions.Add(centerCubiePosition + Vector3.left + Vector3.down);
            faceCubiePositions.Add(centerCubiePosition + Vector3.left + Vector3.up);
            faceCubiePositions.Add(centerCubiePosition + Vector3.right + Vector3.down);
            faceCubiePositions.Add(centerCubiePosition + Vector3.right + Vector3.up);
        }

        for (int i = 0; i < faceCubiePositions.Count; i++)
        {
            faceCubiePositions[i] *= 0.5f;
            Debug.Log(faceCubiePositions[i]);
        }

        List<Transform> faceCubies = new List<Transform>();

        foreach (Vector3 faceCubiePosition in faceCubiePositions)
        {
            foreach (Transform cubie in allCubies)
            {
                if (cubie.localPosition == faceCubiePosition)
                {
                    faceCubies.Add(cubie);
                    break;
                }
            }
        }

        return faceCubies;
    }
}
