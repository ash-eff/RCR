using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ash.MyUtils
{
    public class MyUtils
    {
        public static Vector3 SnapToGrid(Vector3 position, float byAmount)
        {
            float x = Mathf.Round(position.x / byAmount) * byAmount;
            float y = Mathf.Round(position.y / byAmount) * byAmount;
            float z = 0; 
            return new Vector3(x, y, z);
        }

        public static Vector3 SnapToGrid(Vector3 position, float xAmount, float yAmount, float zAmount)
        {
            float x = xAmount == 0f ? 0 : Mathf.Floor(position.x / xAmount) * xAmount;
            float y = yAmount == 0f ? 0 : Mathf.Floor(position.y / yAmount) * yAmount;
            float z = zAmount == 0f ? 0 : Mathf.Floor(position.z / zAmount) * zAmount;
            return new Vector3(x, y, z);
        }

        public static float DistanceBetweenObjects(Vector3 to, Vector3 from)
        {
            float distance = (to - from).magnitude;
            return distance;
        }

        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

       //public static TextMeshProUGUI CreateWorldTextMeshPro(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TMPro.TextAlignmentOptions textAlignment, TMP_FontAsset font)
       //{
       //    GameObject gameObject = new GameObject("World Text", typeof(TextMeshProUGUI));
       //    gameObject.layer = 5;
       //    Transform transform = gameObject.transform;
       //    transform.SetParent(parent, false);
       //    transform.localPosition = localPosition;
       //    TextMeshProUGUI textMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
       //    RectTransform rectTrans = textMeshPro.GetComponent<RectTransform>();
       //    rectTrans.position = parent.transform.position;
       //    textMeshPro.enableWordWrapping = false;
       //    textMeshPro.raycastTarget = false;
       //    textMeshPro.font = font;
       //    textMeshPro.fontSize = fontSize;
       //    textMeshPro.alignment = textAlignment;
       //    textMeshPro.text = text;
       //    rectTrans.sizeDelta = new Vector2(text.Length / fontSize, 3f);
       //    textMeshPro.color = color;
       //    return textMeshPro;
       //}

        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }

        public static Vector3Int GetWorldIntPosition(Vector3 position)
        {
            Vector3Int worldIntPosition = new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
            return worldIntPosition;
        }

        public static float GetLegalAngle(float angle)
        {
            if (angle < 0)
                return 360 - Mathf.Abs(angle);
            if (angle > 360)
                return angle - 360;

            return angle;
        }

        public static bool IsInRadiusOfTarget(Vector3 startLocation, Vector3 endLocation, float radius)
        {
            float dist = Vector3.Distance(startLocation, endLocation);
            if (dist < radius)
            {
                //Debug.DrawLine(startLocation, endLocation, Color.green, .25f);
                return true;
            }

            //Debug.DrawLine(startLocation, endLocation, Color.red, .25f);
            return false;
        }
        
        public static Vector3 GetRandomDir() {
            return new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)).normalized;
        }
        
        public static Vector2 Direction2D(Vector2 fromPos, Vector2 toPos)
        {
            Vector2 direction = toPos - fromPos;
            return direction;
        }
        
        public static Vector3 Vec2DTo3D(Vector2 vecToChange)
        {
            Vector3 newVec3 = new Vector3(vecToChange.x, vecToChange.y, 0f);
            return newVec3;
        }
        

        public static Vector3 GetVectorFromAngle(float angle) {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI/180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float GetAngleFromVectorFloat(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        public static int GetAngleFromVector(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static int GetAngleFromVector180(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static Vector3 ApplyRotationToVector(Vector3 vec, Vector3 vecRotation) {
            return ApplyRotationToVector(vec, GetAngleFromVectorFloat(vecRotation));
        }

        public static Vector3 ApplyRotationToVector(Vector3 vec, float angle) {
            return Quaternion.Euler(0,0,angle) * vec;
        }
    }
}

