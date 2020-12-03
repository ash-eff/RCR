using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private Transform virtualCam;
    [SerializeField] private float relativeMoveX;
    [SerializeField] private float relativeMoveY;

    private void Update()
    {
        var position = virtualCam.position;
        transform.position =
            new Vector3(position.x * relativeMoveX, position.y * relativeMoveY, 0);
    }
}
