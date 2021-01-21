using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class Weapon : MonoBehaviour
{
    public bool isOwned = false;
    public new string name;
    public string description;

    public Sprite artwork;

    public int damage;
    public float rateOfFire;
    public float reloadTime;
    public float lastShot = 0;
    public float totalAmmo;
    public float currentAmmo;

    public SpriteRenderer gunSprite;
    public Vector2 gunPosition;
    public Transform muzzlePosition;
    public GameObject projectilePrefab;
    public GameObject shellPrefab;
    public GameObject impactPrefab;

    public virtual void FireWeapon(float rot)
    {
        if (currentAmmo > 0)
        {
            var offset = Random.Range(-4, 4);
            rot += offset;
            Instantiate(shellPrefab, transform.position, Quaternion.identity);
            GameObject obj = Instantiate(projectilePrefab, muzzlePosition.position, Quaternion.Euler(0, 0, rot));
            currentAmmo--;
            lastShot = Time.time;
        }
    }

    //private void Reload()
    //{
    //    reloadBar.gameObject.SetActive(true);
    //    ammoAmount = 0;
    //    ammoImage.sprite = ammoSprites[ammoAmount];
    //    
    //    StartCoroutine(IeReload());
    //    StartCoroutine(TimerBar());
//
    //    IEnumerator TimerBar()
    //    {
    //        var timer = 0f;
    //        reloadBar.fillAmount = timer;
    //        reloadBarHolder.SetActive(true);
    //        
    //        while (timer < reloadTime)
    //        {
    //            timer += Time.deltaTime / reloadTime;
    //            reloadBar.fillAmount = timer;
    //            yield return null;
    //        }
    //        
    //        reloadBarHolder.SetActive(false);
    //    }
    //    
    //    IEnumerator IeReload()
    //    {
    //        var reloadCount = 0;
    //        var reloadInterval = reloadTime / maxAmmo;
    //        //reloadCount++;
    //        //ammoImage.sprite = ammoSprites[reloadCount];
    //        
    //        while (reloadCount < maxAmmo)
    //        {
    //            reloadCount++;
    //            ammoImage.sprite = ammoSprites[reloadCount];
    //            yield return new WaitForSeconds(reloadInterval);
    //        }
    //        
    //        ammoAmount = maxAmmo;
    //        isReloading = false;
    //        canReload = true;
    //    }
    //}
    
    //public void HideWeapon(bool isHidden)
    //{
    //    gunSpr.enabled = !isHidden;
    //    fistSprite.enabled = !isHidden;
    //}
}
