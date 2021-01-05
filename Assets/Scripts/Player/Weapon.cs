using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using Ash.MyUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Player Weapon", menuName = "New Player Weapon")]
public class Weapon : ScriptableObject
{
    public new string name;
    public string description;

    public Sprite artwork;

    public int damage;
    public float rateOfFire;
    public float shakeIntesity;
    public float shakeTime;
    public float totalAmmo;
    public float currentAmmo;
    public GameObject projectilePrefab;
    public GameObject shellPrefab;
    public GameObject impactPrefab;

    public bool singleShot, burstShot, blastShot, beamShot;

    public void FireWeapon(Vector3 dir, Vector3 startPosition)
    {
        var rot = MyUtils.GetAngleFromVectorFloat(dir);
        if (singleShot)
        {
          if (currentAmmo > 0)
          {
              var offset = Random.Range(-4, 4);
              rot += offset;
              Instantiate(shellPrefab, startPosition, quaternion.identity);
              GameObject obj = Instantiate(projectilePrefab, startPosition, Quaternion.Euler(0, 0, rot));
              obj.GetComponent<Projectile>().delayTime = 0;
              obj.GetComponent<Projectile>().shotFromWeapon = this;
              currentAmmo--;
          }
        }
        else if(blastShot)
        {
            if (currentAmmo > 0)
            {
                rot -= 4 * 3.5f;
                Instantiate(shellPrefab, startPosition, quaternion.identity);
                for (int i = 0; i < 7; i++)
                {
                    GameObject obj = Instantiate(projectilePrefab, startPosition, Quaternion.Euler(0, 0, rot));
                    obj.GetComponent<Projectile>().destroyAfter = .5f;
                    obj.GetComponent<Projectile>().delayTime = 0;
                    obj.GetComponent<Projectile>().shotFromWeapon = this;
                    rot += 4;
                }
                currentAmmo--;
            }
        }
        else if (burstShot)
        {
            for (int i = 0; i < 3; i++)
            {
                if (currentAmmo > 0)
                {
                    var offset = Random.Range(-4, 4);
                    rot += offset;
                    Instantiate(shellPrefab, startPosition, quaternion.identity);
                    GameObject obj = Instantiate(projectilePrefab, startPosition, Quaternion.Euler(0, 0, rot));
                    obj.GetComponent<Projectile>().delayTime = .05f * i;
                    obj.GetComponent<Projectile>().shotFromWeapon = this;
                    currentAmmo--;
                }
            }
        }
    }
}
