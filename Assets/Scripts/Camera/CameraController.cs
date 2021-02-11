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
    private bool followPlayer;

    private void FixedUpdate()
    {
        if(followPlayer)
            FollowTarget(player.transform.position);
    }

    void FollowTarget(Vector3 _target)
    {
        transform.position = Vector3.Lerp(transform.position, _target, lerpSpeed * Time.fixedDeltaTime);
    }

    public void PlayerHasSpawned()
    {
        player = FindObjectOfType<PlayerManager>();
        transform.position = player.transform.position;
        followPlayer = true;
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
                float y = Random.Range(-1, 1) * magnitude;
                float z = Random.Range(-1, 1) * magnitude;

                mainCam.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z + z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            mainCam.transform.localPosition = originalPos;
        }
    }
    
    public void CameraShakeTimed(float lengthOfTime)
    {
        StartCoroutine(Shake(lengthOfTime, shakeMagnitude));
        
        IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 originalPos = mainCam.transform.localPosition;

            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-1, 1) * magnitude;
                float y = Random.Range(-1, 1) * magnitude;

                float z = Random.Range(-1, 1) * magnitude;

                mainCam.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z + z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            mainCam.transform.localPosition = originalPos;
        }
    }
}
