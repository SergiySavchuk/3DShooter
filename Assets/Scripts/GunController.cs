using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GunController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private float bulletSpeed = 20f, shootCooldown = 0.5f;

    private AudioSource audioSource;
    private float shootTimer = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;
    }

    public void Shoot()
    {
        // якщо не пройшов час між пострілами нічого не роблю
        if (shootTimer < shootCooldown) return;

        // запускаю звук, якщо він ввімкнений
        if (PlayerPrefsManager.GetSound())
            audioSource.Play();

        // створюю кулю і даю їй прискорення
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(firePoint.forward * bulletSpeed, ForceMode.Impulse);

        shootTimer = 0f;

        // якщо куля нікуди не влучить, то через 5 секунд вона знищеться
        Destroy(bullet, 5f);
    }
}
