using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity
{
    public System.Action OnPlayerDeath;

    protected override void Death()
    {
        OnPlayerDeath?.Invoke();
        Destroy(gameObject);
    }
}
