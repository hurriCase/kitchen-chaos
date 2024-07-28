using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter SelectedCounter;
    }
    
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _counterLayerMask;
    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    private GameObject _playerVisual;
    private GameObject _bodyGameObject;
    private GameObject _headGameObject;
    private ClearCounter _selectedCounter;
    private bool _isPaused;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        } 
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInteractAction += GameInputOnInteractAction;
        
        _bodyGameObject = GameObject.Find("PlayerVisual/Body");
        _headGameObject = GameObject.Find("PlayerVisual/Head");
    }

    private void OnEnable()
    {
        Application.focusChanged += ApplicationOnfocusChanged;
    }

    private void OnDisable()
    {
        Application.focusChanged -= ApplicationOnfocusChanged;
    }

    private void ApplicationOnfocusChanged(bool hasFocus)
    {
        _isPaused = !hasFocus;

        if (_isPaused) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact();
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    
    public bool IsWalking()
    {
        return _isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        const float interactDistance = 2f;

        if (moveDirection != Vector3.zero)
        {
            _lastInteractDirection = moveDirection;
        }

        if (Physics.Raycast(
                transform.position,
                _lastInteractDirection, 
                out RaycastHit raycastHit, 
                interactDistance, 
                _counterLayerMask
            ))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                if (clearCounter != _selectedCounter) SetSelectedCounter(clearCounter);
            }
            else SetSelectedCounter(null);
        } 
        else SetSelectedCounter(null);
    }
    private void HandleMovement()
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
}

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            SelectedCounter = _selectedCounter
        });
    }
}
