using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTiler : MonoBehaviour
{
    private MeshRenderer meshRendererHolder;
    private Vector2 _scaler;
    [SerializeField] Transform _wallsHolder;
    [SerializeField] Transform _wallsWithDoorsHolder;

    void Start()
    {
        if(_wallsHolder != null)
            foreach(Transform t in _wallsHolder)
            {
                ScaleTexture(t);
            }

        if(_wallsWithDoorsHolder != null)
            foreach(Transform t in _wallsWithDoorsHolder)
            {
                for(int i = 1; i < t.childCount; i++)
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

        _scaler.x /= 4;
        _scaler.y /= 4;

        meshRendererHolder.material.mainTextureScale = _scaler;
    }
}
