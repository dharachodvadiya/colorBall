using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    public GameObject pathStartPoint;
    public GameObject pathEndPoint;

    public MeshRenderer[] colorBallRenderer;

    public Path nextPath;
    public Path prevPath;

    private void OnEnable()
    {
        for (int i = 0; i < colorBallRenderer.Length; i++)
        {
            colorBallRenderer[i].gameObject.SetActive(true);
            var material = colorBallRenderer[i].material;

            Color color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            material.SetColor("_BaseColor", color);
            material.SetColor("_EmissionColor", color * Mathf.LinearToGammaSpace(2f));
        }
    }
}
