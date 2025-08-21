using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public float runspeed = 40;
    public Animator animator;

    [Space]
    public Sprite fallSprite;
    public Sprite standSprite;
    public Sprite crouchSprite;

    [Space]
    public KeyCode Crouch = KeyCode.S;
    public KeyCode Jump = KeyCode.W;
    public KeyCode AltCrouch = KeyCode.DownArrow;
    public KeyCode AltJump = KeyCode.UpArrow;

    float horizontalMove = 0f;
    float jumptime = 0f;
    bool jump = false;
    bool crouch = false;
    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runspeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        
        if(Input.GetKeyDown(Jump) || Input.GetKeyDown(AltJump))
        {
            jump = true;
            animator.SetBool("Jumping", true);
            animator.enabled = false;

            this.GetComponent<SpriteRenderer>().sprite = fallSprite;
        }
        
        /*
        Vector3 vel;

        if(controller.m_Grounded == true && Input.GetKeyDown(Jump))
        {
            jump = true;
            vel = Vector3.up * controller.m_JumpForce;
        }

        if(Input.GetKey(Jump) && jump == true)
        {
            if(jumptime > 0)
            {
                vel = Vector3.up * controller.m_JumpForce;
                jumptime -= Time.deltaTime;
            }
            else 
            {
                jump = false;
            }
        }

        if(Input.GetKeyUp(Jump))
        {
            jump = false;
        }
        */


        if(Input.GetKeyDown(Crouch) || Input.GetKeyDown(AltCrouch))
        {
            crouch = true;
        }
        else if(Input.GetKeyUp(Crouch) || Input.GetKeyUp(AltCrouch))
        {
            crouch = false;
        }
    }

    public void OnLanding()
    {
        animator.enabled = true;
        animator.SetBool("Jumping", false);
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    //JUMP 710 - 550
}
