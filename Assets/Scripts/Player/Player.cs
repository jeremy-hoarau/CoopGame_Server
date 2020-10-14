using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id, otherPlayerId;
    public string username;
    public Rigidbody rb;
    public new CapsuleCollider collider;

    public float groundDetectionDistance = 0.2f,
        moveVelocity = 500,
        airMoveVelocity = 120,
        moveDeceleration = 250,
        maxSpeed = 8,
        jumpSpeed = 8,
        jumpCD = 0.2f;

    public bool isTryingToGrapple,
        isTryingToInteract,
        isTryingToGrab,
        canMove = true,
        canJump = true,
        debug_jump,
        debug_grapple,
        debug_interact,
        debug_grab,
        waiting = true;
    
    [HideInInspector]public Vector3 cameraPosition;
    
    private bool[] movementInputs;
    private bool[] inputs;

    private Vector3 lastVelocity,
        lastDirection;

    private bool jumpInCD = false;
    private bool isGrounded;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        otherPlayerId = (_id == 1 ? 2 : 1);
        gameObject.tag = (id == 1 ? "Player1" : "Player2");
        username = _username;
        waiting = true;
        
        movementInputs = new bool[4];
        inputs = new bool[4];
        /*
        0 - jump 
        1 - grapple 
        2 - interaction 
        3 - grab
        */
    }

    public void FixedUpdate()
    {
        if(waiting)
            return;
        isGrounded = IsGrounded();
        Vector2 _inputDirection = Vector2.zero;
        if (movementInputs[0])
            _inputDirection.x = 1;
        else if (movementInputs[1])
            _inputDirection.x = -1;
        if (movementInputs[2])
            _inputDirection.y = 1;
        else if (movementInputs[3])
            _inputDirection.y = -1;

        Move(_inputDirection);
        MakeOtherPlayerJump();
        MakeOtherPlayerGrapple();
        Interact();
        Grab();

        SendPositionRotation();
    }

    private void Move(Vector2 _inputDirection)
    {
        // TRANSFORM.TRANSLATE MOVEMENTS
        //
        // if (isGrounded)
        // {
        //     transform.Translate(new Vector3(_inputDirection.x, 0, _inputDirection.y) * (moveSpeed * Time.deltaTime));
        //     lastDirection = transform.TransformDirection(new Vector3(_inputDirection.x, 0, _inputDirection.y));
        // }
        // else
        // {
        //     Vector3 airMovements = transform.TransformDirection(new Vector3(_inputDirection.x, 0, _inputDirection.y));
        //     transform.Translate((lastDirection * (moveSpeed-airMoveSpeed) + airMovements * airMoveSpeed) * Time.deltaTime, Space.World);
        // }
        
        Vector3 vel = rb.velocity;
        Vector3 _localDir = new Vector3(_inputDirection.x, 0, _inputDirection.y).normalized;
        Vector3 _localVel = transform.InverseTransformVector(new Vector3(vel.x, 0, vel.z)).normalized;

        float magnitude = Mathf.Sqrt(vel.x * vel.x + vel.z * vel.z);


        //limit the speed
        if (canMove)
        {
            if (magnitude > maxSpeed)
            {
                if (_localVel.x > 0.5f && _localDir.x > 0)
                    _localDir.x = 0;
                else if (_localVel.x < -0.5f && _localDir.x < 0)
                    _localDir.x = 0;
                if (_localVel.z > 0.5f && _localDir.z > 0)
                    _localDir.z = 0;
                else if (_localVel.z < -0.5f && _localDir.z < 0)
                    _localDir.z = 0;
            }
        }
        
        if (isGrounded)
        {
            if (canMove)
            {
                rb.AddRelativeForce(_localDir * (moveVelocity * Time.fixedDeltaTime));
            }

            if (_inputDirection.x == 0 && _inputDirection.y == 0 && magnitude > 0)
            {
                if(magnitude < 0.02f)
                    rb.velocity = new Vector3(0, vel.y, 0);
                else
                {
                    //slow down the player to stop him when not trying to move
                    float _moveDeceleration = moveDeceleration * Time.fixedDeltaTime;
                    
                    rb.velocity = vel - 1/_moveDeceleration * vel;
                }
            }
        }
        else
        {
            rb.AddRelativeForce(_localDir * (airMoveVelocity * Time.fixedDeltaTime));
        }
    }

    private void MakeOtherPlayerJump()
    {
        if (debug_jump && inputs[0])
        {
            Server.clients[id].player.Jump();
            return;
        }
        if (inputs[0] && Server.clients.ContainsKey(otherPlayerId) && Server.clients[otherPlayerId].player != null)
        {
            Server.clients[otherPlayerId].player.Jump();
        }
    }

    private void MakeOtherPlayerGrapple()
    {
        if (debug_grapple)
        {
            Server.clients[id].player.isTryingToGrapple = inputs[1];
            return;
        }
        if (Server.clients.ContainsKey(otherPlayerId) && Server.clients[otherPlayerId].player != null)
        {
            Server.clients[otherPlayerId].player.isTryingToGrapple = inputs[1];
        }
    }
    
    private void Interact()
    {
        if (debug_interact && Server.clients.ContainsKey(otherPlayerId) && Server.clients[otherPlayerId].player != null)
        {
            Server.clients[otherPlayerId].player.isTryingToInteract = inputs[2];
            return;
        }
        
        Server.clients[id].player.isTryingToInteract = inputs[2];
        
            // if (debug)
        // {
        //     Server.clients[id].player.isTryingToInteract = inputs[2];
        //     return;
        // }
        // if (Server.clients[otherPlayerId].tcp.socket != null)
        // {
        //     Server.clients[otherPlayerId].player.isTryingToInteract = inputs[2];
        // }
    }
    
    private void Grab()
    {
        if (debug_grab && Server.clients.ContainsKey(otherPlayerId) && Server.clients[otherPlayerId].player != null)
        {
            Server.clients[otherPlayerId].player.isTryingToGrab = inputs[3];
            return;
        }
        
        Server.clients[id].player.isTryingToGrab = inputs[3];
        
        // if (debug)
        // {
        //     Server.clients[id].player.isTryingToGrab = inputs[3];
        //     return;
        // }
        // if (Server.clients[otherPlayerId].tcp.socket != null)
        // {
        //     Server.clients[otherPlayerId].player.isTryingToGrab = inputs[3];
        // }
    }

    private void Jump()
    {
        if (!isGrounded || !canJump || jumpInCD) return;
        
        Vector3 velocity = rb.velocity;
        velocity.y = 0;
        rb.velocity = velocity;
        
        rb.AddForce(Vector3.up*jumpSpeed, ForceMode.Impulse);
        StartCoroutine(JumpCD());
    }

    private void SendPositionRotation()
    {
        ServerSend.PlayerPositionCameraPosition(this);
        ServerSend.PlayerRotation(this);
    }

    private bool IsGrounded()
    {
        float radius = collider.radius;
        Vector3 origin = transform.position;
        origin.y += 1f;
        
        return Physics.BoxCast(origin, new Vector3(radius, 0.1f, radius), Vector3.down, Quaternion.identity, 1+groundDetectionDistance);
    }

    IEnumerator JumpCD()
    {
        jumpInCD = true;
        yield return new WaitForSeconds(jumpCD);
        jumpInCD = false;
    }

    public void SetMovementInput(bool[] _movementInputs, Quaternion _rotation, Vector3 _cameraPosition)
    {
        movementInputs = _movementInputs;
        transform.rotation = _rotation;
        cameraPosition = _cameraPosition;
        
        //send the player inputs the the other player to sync the animations
        ServerSend.OtherPlayerInputs(id, _movementInputs);
    }

    public void SetInput(bool[] _inputs)
    {
        inputs = _inputs;
    }
}