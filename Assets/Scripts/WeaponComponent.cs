using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{ 
    NONE, 
    PISTOL, 
    MACHINEGUN
}

public enum WeaponFiringPattern
{
    SEMIAUTO, 
    FULLAUTO, 
    THREESHOTBURST, 
    FIVESHOTBURST,
    PUMPACTION
}

[System.Serializable]
public struct WeaponStats {
    public WeaponType weaponType;
    public WeaponFiringPattern firingPattern;
    public string weaponName;
    public float damage;
    public int bulletsInClip;
    public int clipSize;
    public int totalBullets;
    public float fireStartDelay;
    public float fireRate;
    public float fireDistance;
    public bool repeating;
    public LayerMask weaponHitLayers;
}


public class WeaponComponent : MonoBehaviour
{
    public Transform gripLocation;
    public WeaponStats weaponStats;
    protected WeaponHolder weaponHolder;

    [SerializeField]
    protected ParticleSystem firingEffect;

    public bool isFiring;
    public bool isRealoding;

    protected Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void Initialize(WeaponHolder _weaponHolder)
    {
        weaponHolder = _weaponHolder;
    }

    public virtual void StartFiringWeapon()
    {
        isFiring = true;
        if (weaponStats.repeating)
        {
            // fireeee
            InvokeRepeating(nameof(FireWeapon), weaponStats.fireStartDelay, weaponStats.fireRate);
        }
        else
        {
            FireWeapon();
        }
    }

    public virtual void StopFiringWeapon()
    {
        isFiring = false;
        CancelInvoke(nameof(FireWeapon));

        if (firingEffect && firingEffect.isPlaying)
        {
            firingEffect.Stop();
        }
    }

    protected virtual void FireWeapon()
    {
        weaponStats.bulletsInClip--;
        Debug.Log("FIRING WEAPON Bullets in clip : " + weaponStats.bulletsInClip);
    }

    public virtual void StartReloading()
    {
        isRealoding = true;
        ReloadWeapon();
    }

    public virtual void StopReloading()
    {
        isRealoding = false;
    }

    protected virtual void ReloadWeapon()
    {
        // stop firing effect if there is one playing
        if (firingEffect && firingEffect.isPlaying)
        {
            firingEffect.Stop();
        }

        int bulletsToReload = weaponStats.clipSize - weaponStats.totalBullets;

        if (bulletsToReload < 0)
        {
            weaponStats.bulletsInClip = weaponStats.clipSize;
            weaponStats.totalBullets -= weaponStats.clipSize;
        }
        else
        {
            weaponStats.bulletsInClip = weaponStats.totalBullets;
            weaponStats.totalBullets = 0;
        }
    }
}
