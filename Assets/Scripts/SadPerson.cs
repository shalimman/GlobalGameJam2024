using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SadPerson : MonoBehaviour
{

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("Happy", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
