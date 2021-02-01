using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShotgun : Bullet
{
    public int blastShotAmount;
    public int blastAngle;
    
    public override void FireBullet(float rotation, Vector2 gunPosition)
    {
        Debug.Log("Shotgun");
        for (int i = 0; i < blastShotAmount; i++)
        {
            var offset = Random.Range(-4, 4);
            rotation += offset;
            var angleOffset = blastAngle / (blastShotAmount - 1f);
            var startingAngle = rotation - blastAngle / 2f;
            var newAngle = startingAngle + i * angleOffset;
            FireProjectile(newAngle);
        }
        EjectShell(gunPosition);
    }
}
