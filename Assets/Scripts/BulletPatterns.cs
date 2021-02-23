using System;
using System.Collections;
using System.Collections.Generic;
using Ash.MyUtils;
using UnityEngine;

public class BulletPatterns : MonoBehaviour
{
    public GameObject bulletPrefab;
    public bool isShooting = false;
    [SerializeField] private float timeBetweenSpreadShots;
    [SerializeField] private int numberOfSpreadBullets;
    [SerializeField] private int numberOfSpreadCycles;

    [SerializeField] private float timeBetweenSpiralShots;
    [SerializeField] private int numberOfSpiralBullets;
    [SerializeField] private int numberOfSpiralCycles;
    
    [SerializeField] private float timeBetweenPatterns;

    public void ThreeSixtySpread()
    {
        StartCoroutine(IeThreeSixtySpread());
        
        IEnumerator IeThreeSixtySpread()
        {
            isShooting = true;
            var angleOffset = Mathf.RoundToInt(360 / numberOfSpreadBullets);
            var angle = 0;
            var angleDiff = Mathf.RoundToInt(angleOffset / 2);

            for (int c = 0; c < numberOfSpreadCycles; c++)
            {
                angle = c * angleDiff;
                for (int i = 0; i < numberOfSpreadBullets; i++)
                {
                    var rotation = Quaternion.Euler(0, angle, 0);
                    //var direction = MyUtils.GetVectorFromAngle(angle);
                    Instantiate(bulletPrefab, transform.position, rotation);
                    angle += angleOffset;
                }

                yield return new WaitForSeconds(timeBetweenSpreadShots);
            }

            isShooting = false;
            yield return new WaitForSeconds(timeBetweenPatterns);
            ThreeSixtySpiral();
        }
    }

    public void ThreeSixtySpiral()
    {
        StartCoroutine(IeThreeSixtySpiral());
        
        IEnumerator IeThreeSixtySpiral()
        {
            isShooting = true;
            var angleOffset = Mathf.RoundToInt(360 / numberOfSpiralBullets);
            var angle = 0;

            for (int c = 0; c < numberOfSpiralCycles; c++)
            {
                for (int i = 0; i < numberOfSpiralBullets; i++)
                {
                    var rotation = Quaternion.Euler(0, angle, 0);
                    Instantiate(bulletPrefab, transform.position, rotation);
                    if(c % 2 == 0)
                        angle += angleOffset;
                    else
                        angle -= angleOffset;

                    yield return new WaitForSeconds(timeBetweenSpiralShots);
                }

                angle = 0;
                yield return null;
            }

            isShooting = false;
            yield return new WaitForSeconds(timeBetweenPatterns);
            ThreeSixtySpread();
        }
    }

    public void StopShooting()
    {
        StopAllCoroutines();
    }
}
