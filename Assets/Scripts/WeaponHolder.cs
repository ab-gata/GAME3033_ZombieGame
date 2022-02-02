using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [Header("WeaponToSpawn"), SerializeField]
    GameObject weaponToSpawn;

    public Sprite crossHairImage;

    [SerializeField]
    GameObject weaponSocketLocation;
    [SerializeField]
    Transform gripIKSocketLocation;

    public Sprite crosshairImage;

    WeaponComponent equippedWeapon;
    public PlayerController playerController;
    Animator playerAnimator;

    bool firingPressed = false;

    public readonly int isFiringHash = Animator.StringToHash("IsFiring");
    public readonly int isReloadingHash = Animator.StringToHash("IsReloading");

    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnedWeapon = Instantiate(weaponToSpawn,
                                               weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation,
                                               weaponSocketLocation.transform);

        playerController = GetComponent<PlayerController>();
        playerAnimator = GetComponent<Animator>();

        equippedWeapon = spawnedWeapon.GetComponent<WeaponComponent>();
        equippedWeapon.Initialize(this);
        gripIKSocketLocation = equippedWeapon.gripLocation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!playerController.isReloading)
        {
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, gripIKSocketLocation.transform.position);
        }
    }

    public void OnReload(InputValue value)
    {
        playerController.isReloading = value.isPressed;
        playerAnimator.SetBool(isReloadingHash, playerController.isReloading);
    }

    public void StartReloading()
    {

    }

    public void OnFire(InputValue value)
    {
        firingPressed = value.isPressed;
        if (firingPressed)
        {
            StartFiring();
        }
        else
        {
            StopFiring();
        }
    }

    void StartFiring()
    {
        if (equippedWeapon.weaponStats.bulletsInClip <= 0) return;

        playerAnimator.SetBool(isFiringHash, true);
        playerController.isFiring = true;
        equippedWeapon.StartFiringWeapon();
    }

    void StopFiring()
    {
        playerAnimator.SetBool(isFiringHash, false);
        playerController.isFiring = false;
        equippedWeapon.StopFiringWeapon();
    }
}
