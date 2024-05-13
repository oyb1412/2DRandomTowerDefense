using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public Animator[] Character;
    public GameObject[] SelectOn;
    public bool GunEffect;

    public void OnAnimation(string spell)
    {
        for (int i = 0; i < Character.Length; i++)
        {
            if (spell == "IsWalking")
            {
                Character[i].SetBool(spell, true);
                Character[i].SetTrigger("Move");
            }
            if (spell == "IsZombi")
            {
                Character[i].SetBool(spell, true);
                Character[i].SetTrigger("Move");
                SelectOn[0].SetActive(true);
                SelectOn[1].SetActive(false);

            }
            if (spell == "IsHuman")
            {
                Character[i].SetBool("IsZombi", false);
                Character[i].SetTrigger("Move");
                SelectOn[0].SetActive(false);
                SelectOn[1].SetActive(true);

            }
            if (spell == "Idle")
            {
                Character[i].SetBool("IsWalking", false);
                Character[i].SetTrigger(spell);
            }
            if (spell == "Fire")
            {
                Character[i].SetTrigger("Fire");
            }
            if (spell == "RapidFire")
            {
                if (GunEffect == true)
                {
                    Character[i].SetBool("RapidFire", GunEffect);
                  //  SelectOn[2].SetActive(true);
                    GunEffect = false;
                }
                else if (GunEffect == false)
                {
                    Character[i].SetBool("RapidFire", GunEffect);
                  //  SelectOn[2].SetActive(false);
                    GunEffect = true;
                }
                Character[i].SetTrigger("FireStart");
            }

            Character[i].SetTrigger(spell);
        }
    }

}
