using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePistol : Weapon
{
    public override void SpecialAbility()
    {
        if (isOwned && isEquipped)
        {
            if (currentAmmo >= minAmmoForSpecial)
            {
                StartCoroutine(IeSpecialAbility());
                StartCooldownTimer();
            
                IEnumerator IeSpecialAbility()
                {
                    isUsingSpecial = true;
                
                    var rot = 0f;
                    cam.CameraShakeTimed(2.5f);
                    for (int i = 0; i < 50; i++)
                    {
                        rot = Random.Range(0, 361);
                        GameObject obj = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, rot));
                        obj.GetComponent<Projectile>().damageAmount = 3;
                        currentAmmo--;

                        yield return new WaitForSeconds(.05f);
                    }

                    isUsingSpecial = false;
                }    
            }
        }
    }
}
