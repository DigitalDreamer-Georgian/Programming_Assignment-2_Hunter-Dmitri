using Unity.VisualScripting;
using UnityEngine;

public class DragonHealth : Enemy
{

    public override void Dead()
    {
        health = 0;
        healthBar.gameObject.SetActive(false);
    }
}
