using UnityEngine;

public class ShootControl : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    public int maxAmmo = 20;
    private int currentAmmo;
    public float reloadTime = 3f;
    private bool isReloading = false;
    public float fireRate = 0.25f;
    private float nextFireTime = 0f;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        currentAmmo--;
        nextFireTime = Time.time + fireRate;
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Recarregando...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Munição recarregada!");
    }
}

