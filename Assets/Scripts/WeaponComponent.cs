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
    public float fireStartDelay;
    public float fireRate;
    public float fireDistance;
    public bool repeating; // hold down to keep shooting, TEMP
    public LayerMask weaponHitLayers;
}


public class WeaponComponent : MonoBehaviour
{
    public Transform gripLocation;
    public WeaponStats weaponStats;
    protected WeaponHolder weaponHolder;

    public bool isFiring;
    public bool isRealoding;

    protected Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    protected virtual void FireWeapon()
    {
        weaponStats.bulletsInClip--;
        Debug.Log("FIRING WEAPON Bullets in clip : " + weaponStats.bulletsInClip);
    }
}
