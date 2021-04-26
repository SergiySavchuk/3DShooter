using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float turnSpeed = 10f;

    public GunController gunController;

    private Plane playerPlane;

    // Start is called before the first frame update
    void Start()
    {
        // потрібна для розрахування відстані на промені
        playerPlane = new Plane(Vector3.up, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        Shoot();
    }

    private void Look()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // перевіряю, де промінь зіткнувся з поверхнею
        if (playerPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 point = cameraRay.GetPoint(rayLength);

            // повертаю гравця до цією точки
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(point - transform.position), turnSpeed * Time.deltaTime);
        }
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"))
        {
            // викликаю метод зброї
            gunController.Shoot();
        }
    }

}
