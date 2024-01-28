using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Laugh
{
    Hee,
    Ha
};

public class SadPerson : MonoBehaviour
{
    public int state;
    private Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Happy", false);
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReceiveInput(Laugh heeha)
    {
        switch (state)
        {
            case 0:
                if (heeha == Laugh.Hee)
                {
                    anim.SetBool("Alert", true);
                    state++;
                }
                else
                {
                    state = 0;
                    anim.SetBool("Alert", false);
                }
                break;
            case 1:
                if (heeha == Laugh.Hee)
                {
                    state++;
                }
                else
                {
                    state = 0;
                    anim.SetBool("Alert", false);
                }
                break;
            case 2:
                if (heeha == Laugh.Ha)
                {
                    state++;
                }
                else
                {
                    state = 0;
                    anim.SetBool("Alert", false);
                }
                break;
            case 3:
                if (heeha == Laugh.Ha)
                {
                    state++;
                    anim.SetBool("Happy", true);
                }
                else
                {
                    state = 0;
                    anim.SetBool("Alert", false);
                }
                break;

            default:
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            anim.SetBool("Alert", true);
    }
}
