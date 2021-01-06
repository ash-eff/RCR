using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float lerpSpeed, shakeDuration, shakeMagnitude;
    [SerializeField] private bool followPlayer;
    [SerializeField] private Camera mainCam;
    [SerializeField] private PlayerManager player;
    [SerializeField] private Vector3 targetPos;

    private void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
        //playerCursorPos = player.GetCursorPos;
        if (followPlayer)
        {
           transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        }
    }

    private void FixedUpdate()
    {
        if (followPlayer)
        {
            //if (gameController.IsGameOver)
            //{
            //    return;
            //}
            FollowPlayerTarget(player.transform.position);
        }
    }

    public void ResetCamera()
    {
        transform.position = player.transform.position;
    }

    void FollowPlayerTarget(Vector3 _target)
    {
        //targetPos = new Vector3(_target.x, _target.y, -10f);
        targetPos = new Vector3(_target.x, _target.y,   -10f);
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
    }

    //void FloatCameraTowardCursor()
    //{
    //    Vector3 targetPos = new Vector3(playerCursorPos.x, playerCursorPos.y, -10f);
    //    transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.fixedDeltaTime);
    //}

    public void CameraShake()
    {
        Debug.Log("Shake");
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
