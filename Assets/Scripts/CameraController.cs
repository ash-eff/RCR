using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float lerpSpeed, shakeDuration, shakeMagnitude;
    [SerializeField] private Camera mainCam;

    [SerializeField] private PlayerManager player;
    //private Vector3 targetPos;
    //[SerializeField] private List<Transform> targets;

    private void FixedUpdate()
    {
        //Vector3 centerPoint = GetCenterPoint();
        FollowTarget(player.transform.position);
    }

    //private Vector3 GetCenterPoint()
    //{
    //    var bounds = new Bounds(targets[0].position, Vector3.zero);
    //    bounds.Encapsulate(targets[1].position);
//
    //    return bounds.center;
    //}

    void FollowTarget(Vector3 _target)
    {
        //targetPos = new Vector3(_target.x, _target.y,   -10f);
        //var originPos = targets[0].position;
        //transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
        //transform.position = _target;
        //Vector2 clampedPos = transform.position;
        //clampedPos.x = Mathf.Clamp(transform.position.x, originPos.x - 2, originPos.x + 2);
        //clampedPos.y = Mathf.Clamp(transform.position.y, originPos.y - 4, originPos.y + 4);
        //transform.position = clampedPos;
        
        transform.position = Vector3.Lerp(transform.position, _target, lerpSpeed * Time.fixedDeltaTime);
    }
    
    public void CameraShake()
    {
        StartCoroutine(Shake(shakeDuration, shakeMagnitude));
        
        IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 originalPos = mainCam.transform.localPosition;

            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1, 1) * magnitude;
                float z = Random.Range(-1, 1) * magnitude;

                mainCam.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y, originalPos.z + z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            mainCam.transform.localPosition = originalPos;
        }
    }
}
