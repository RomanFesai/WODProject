using UnityEngine;
using System.Collections;
using TMPro;

public class RayCastShotgun : MonoBehaviour
{
    public float Damage = 1f;
    public float Range = 100f;
    public float FireRate = 15f;
    public bool gunready1 = false;
    public float impactForce = 30f;
    public float nextTimeToFire = 0f;
    public Animator ShotgunPump;
    public Animator ShotgunReload;
    public ParticleSystem muzzleFlash;
    public TextMeshProUGUI ammoInfo;

    public int maxAmmo = 5;
    public static int magazineSize = 0;
    private int currentAmmo = -1;
    public float reloadTime = 1f;
    public bool isReloading = false;
    private bool NoAmmo = false;

    public Camera fpsCam;


    [Header("Reference Points:")]
    public Transform recoilPosition;
    public Transform rotationPoint;
    [Space(10)]

    [Header("Speed Settings:")]
    public float positionalRecoilSpeed = 8f;
    public float rotationalRecoilSpeed = 8f;
    [Space(10)]

    public float positionalReturnSpeed = 18f;
    public float rotationalReturnSpeed = 38f;
    [Space(10)]

    [Header("Amount Settings:")]
    public Vector3 RecoilRotation = new Vector3(10, 5, 7);
    public Vector3 RecoilKickBack = new Vector3(0.015f, 0f, -0.2f);
    [Space(10)]
    public Vector3 RecoilRotationAim = new Vector3(10, 4, 6);
    public Vector3 RecoilKickBackAim = new Vector3(0.015f, 0f, -0.2f);
    [Space(10)]

    Vector3 rotationalRecoil;
    Vector3 positionalRecoil;
    Vector3 Rot;
    [Header("State:")]
    public static bool aiming = false;

    private void Start()
    {
        ShotgunPump = GameObject.Find("shotgun_pump").GetComponent<Animator>();
        currentAmmo = maxAmmo;
        magazineSize = 0;
    }
    private void FixedUpdate()
    {
        rotationalRecoil = Vector3.Lerp(rotationalRecoil, Vector3.zero, rotationalReturnSpeed * Time.deltaTime);
        positionalRecoil = Vector3.Lerp(positionalRecoil, Vector3.zero, positionalReturnSpeed * Time.deltaTime);

        recoilPosition.localPosition = Vector3.Slerp(recoilPosition.localPosition, positionalRecoil, positionalRecoilSpeed * Time.fixedDeltaTime);
        Rot = Vector3.Slerp(Rot, rotationalRecoil, rotationalRecoilSpeed * Time.fixedDeltaTime);
        rotationPoint.localRotation = Quaternion.Euler(Rot);
    }
    void Update()
    {
        ammoInfo.text = currentAmmo + " / " + magazineSize;

        if(currentAmmo == 0 && magazineSize == 0)
        {
            NoAmmo = true;
            ShotgunReload.SetBool("Aiming", false);
            return;
        }
        else
        {
            NoAmmo = false;
        }

        if (isReloading)
        {
            return; 
        }

        if(Input.GetKeyDown(KeyCode.R) && !isReloading && gunready1 && currentAmmo!=maxAmmo && magazineSize!=0 || currentAmmo == 0 && !isReloading)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButtonDown(1) && gunready1 && !isReloading && PlayerMovement.isSprinting == false)
        {
            ShotgunReload.SetBool("Aiming", true);
            aiming = true;
        }

        if (Input.GetMouseButtonUp(1) && gunready1)
        {
            ShotgunReload.SetBool("Aiming", false);
            aiming = false;
        }

        if (Input.GetMouseButtonDown(0) && gunready1 && Time.time >=nextTimeToFire && !NoAmmo && PlayerMovement.isSprinting == false)
        {
            nextTimeToFire = Time.time + 1f / FireRate;
            Shoot();
            FindObjectOfType<AudioManager>().Play("Shotgun");
            //Recoil.SetTrigger("Shoot");
            Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward);
            ShotgunPump.SetBool("Pump", true);
            muzzleFlash.Play();
        }
        else
        {
            ShotgunPump.SetBool("Pump", false);
        }

        if (gunready1)
        { 
            ShotgunReload.SetBool("isWalking", PlayerMovement.isWalking);
            ShotgunReload.SetBool("isRunning", PlayerMovement.isSprinting);
        }

    }

    IEnumerator Reload()
    {
        isReloading = true;
        ShotgunReload.SetBool("Aiming", false);
        ShotgunReload.SetBool("Reloading", true);
        reloadTime = maxAmmo - currentAmmo;
        if (magazineSize > 0)
        {
            while (currentAmmo != maxAmmo)
            {
                if (magazineSize <= 0)
                {
                    break;
                }
                yield return new WaitForSeconds(1f);
                FindObjectOfType<AudioManager>().Play("ShotgunReload");
                currentAmmo += 1;
                magazineSize -= 1;
            }
        }
        //yield return new WaitForSeconds(reloadTime - 0.25f);
        ShotgunReload.SetBool("Reloading", false);
        //yield return new WaitForSeconds(0.25f);
       /* if (magazineSize > 0)
        {
            while (currentAmmo != maxAmmo)
            {
                if (magazineSize <= 0)
                {
                    break;
                }
                yield return null;
                currentAmmo += 1;
                magazineSize -= 1;
            }
        }*/
        isReloading = false;
    }

    void Shoot()
    {
        currentAmmo--;

        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, Range))
        {
            EnemyRagdoll enemy = hit.transform.GetComponent<EnemyRagdoll>();
            
            if (enemy != null)
            {
                HitActive();
                Invoke("HitDisabled", 0.2f);
                enemy.TakeDamage(Damage);
               
            }
            if (hit.rigidbody != null && enemy == null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            
        }

        rotationalRecoil += new Vector3(-RecoilRotation.x, Random.Range(-RecoilRotation.y, RecoilRotation.y), Random.Range(-RecoilRotation.z, RecoilRotation.z));
        positionalRecoil += new Vector3(Random.Range(-RecoilKickBack.x,RecoilKickBack.x),Random.Range(-RecoilKickBack.y,RecoilKickBack.y),RecoilKickBack.z);
    }

    public void HitActive()
    {
        //hitmarker.SetActive(true);
    }

    public void HitDisabled()
    {
        //hitmarker.SetActive(false);
    }
}