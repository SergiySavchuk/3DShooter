using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject particlesPrefab;
    [SerializeField]
    private AudioClip audioClip;

    private void OnCollisionEnter(Collision collision)
    {
        // якщо куля кудись попадаю, то створюю ефекти
        SoundManager.Instance?.PlayClip(audioClip, 0.25f);

        GameObject particles = Instantiate(particlesPrefab, transform.position, Quaternion.identity);
        // об'єкт з ефектами знищеться через 2 секунди
        Destroy(particles, 2f); 
        Destroy(gameObject);
    }
}
