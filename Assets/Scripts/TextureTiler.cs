using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTiler : MonoBehaviour
{
    private MeshRenderer meshRendererHolder;
    private Vector2 _scaler;
    [SerializeField] Transform _wallsHolder;
    [SerializeField] Transform _groundHolder;
    [SerializeField] Transform _ceilingHolder;
    [SerializeField] Transform _wallsWithDoorsHolder;

    void Start()
    {
        foreach(Transform t in _wallsHolder)
        {
            ScaleTexture(t);
        }

        foreach(Transform t in _groundHolder)
            ScaleTexture(t);

        foreach(Transform t in _ceilingHolder)
            ScaleTexture(t);

        foreach(Transform t in _wallsWithDoorsHolder)
        {
            for(int i = 1; i < 4; i++)
            {
                ScaleTexture(t.GetChild(i));
            }
        }
    }

    private void ScaleTexture(Transform wallTransform)
    {
        meshRendererHolder = wallTransform.GetComponent<MeshRenderer>();
        if(meshRendererHolder == null) return;

        if(wallTransform.lossyScale.x < 1)
            _scaler.x = wallTransform.lossyScale.z;
        else
            _scaler.x = wallTransform.lossyScale.x;

        _scaler.x /= 8;

        if(wallTransform.lossyScale.y < 1)
        {
            _scaler.y = 1f * (wallTransform.lossyScale.y / 4);
        }
        else
            _scaler.y = 2;

        meshRendererHolder.material.mainTextureScale = _scaler;
    }
}
