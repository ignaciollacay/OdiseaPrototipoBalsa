using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    //public Stat playerDamage;
    //public Stat playerArmor;
    /*
    public int maxHealth = 100; //lo usa como int, para que no tenga modifiers; quisiera que sea un stat pero complicado para el damage?
    public int currentHealth { get; private set; } //lo usa como int, idem playerHealth

    void Awake()
    {
        currentHealth = maxHealth;
    }

    ///TBD
    ///virtual? pleyer vs enemy?
    public void AttackDamage (int damage)
    {
        ///TBD
        //damage -= playerArmor.GetValue();
        //damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        //overwrite according to character (ie. player or enemy)
        Debug.Log(transform.name + "died.");
    }*/

    public void Death()
    {
        //overwrite according to character (ie. player or enemy)
        Debug.Log(transform.name + "died.");
        //death anim
        ResetScene();

    }
    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
