using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private GameInput _gameInput;
    private bool _isWalking;
    private GameObject _playerVisual;
    private GameObject _bodyGameObject;
    private GameObject _headGameObject;
    private void Start()
    {
        _bodyGameObject = GameObject.Find("PlayerVisual/Body");
        _headGameObject = GameObject.Find("PlayerVisual/Head");
    }
    private void Update()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDistance = Time.deltaTime * _moveSpeed;
        float playerRadius = _bodyGameObject.transform.localScale.y / 2;
        float playerHeight = _bodyGameObject.GetComponent<Renderer>().bounds.size.y
                             + _headGameObject.GetComponent<Renderer>().bounds.size.y
                             - playerRadius;
        
        bool canMove = Physics.CapsuleCast(
            transform.position, 
            transform.position + Vector3.up * playerHeight,
            playerRadius, 
            moveDirection, 
            moveDistance) is false;

        if (canMove is false)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = Physics.CapsuleCast(
                transform.position, 
                transform.position + Vector3.up * playerHeight,
                playerRadius, 
                moveDirectionX, 
                moveDistance) is false;
        
            if (canMove) moveDirection = moveDirectionX;
            else
            {
                Vector3 moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirectionZ,
                    moveDistance) is false;
                
                if (canMove) {moveDirection = moveDirectionZ;}
            }
        }
        
        if (canMove) transform.position += moveDirection * moveDistance;

        _isWalking = moveDirection != Vector3.zero;
        
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
    
    public bool IsWalking()
    {
        return _isWalking;
    }
}
