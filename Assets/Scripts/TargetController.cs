using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameManager gameManager;
    public int id;

    public void SetValues(GameManager _gameManager, int _id)
    {
        // зберігаю дані
        gameManager = _gameManager;
        id = _id;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // якщо влучила куля, то повідомляю в яку ціль
        if (collision.gameObject.CompareTag("Bullet"))
        {
            gameManager.TargetGotHit(id);
        }
    }
}
