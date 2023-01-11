using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //desprolijo, sacar de este script -- TBD

public class PlayerStats : CharacterStats
{
    /// TBD
    //Virtual functions with enemy / player variables

    public int life = 100;

    void Update()
    {
        if (life <= 0)
        {
            Death();
        }
    }
}
