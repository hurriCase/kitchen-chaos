using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }
    
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask _counterLayerMask;
    [SerializeField] private Transform _kitchenObjectHoldPoint;
    
    private KitchenObject _kitchenObject;
    private GameObject _playerVisual;
    private GameObject _bodyGameObject;
    private GameObject _headGameObject;
    private BaseCounter _selectedCounter;
    private Vector3 _lastInteractDirection;
    private Vector3 _lastMoveDirection;
    private bool _isWalking;
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
        _gameInput.OnInteractAlternateAction += GameInputOnInteractAlternateAction;
        
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
            _selectedCounter.Interact(this);
        }
    }
    private void GameInputOnInteractAlternateAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlternate();
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
        var inputVector = _gameInput.GetMovementVectorNormalized();
        var moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        const float interactDistance = 2f;

        if (moveDirection != Vector3.zero)
        {
            _lastInteractDirection = moveDirection;
        }

        if (Physics.Raycast(
                transform.position,
                _lastInteractDirection, 
                out var raycastHit, 
                interactDistance, 
                _counterLayerMask
            ))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if (baseCounter != _selectedCounter) SetSelectedCounter(baseCounter);
            }
            else SetSelectedCounter(null);
        } 
        else SetSelectedCounter(null);
    }
    
    private void HandleMovement()
    {
        var inputVector = _gameInput.GetMovementVectorNormalized();
        var moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        var moveDistance = Time.deltaTime * _moveSpeed;
        var playerRadius = _bodyGameObject.transform.localScale.y / 2;
        var playerHeight = _bodyGameObject.GetComponent<Renderer>().bounds.size.y
                           + _headGameObject.GetComponent<Renderer>().bounds.size.y
                           - playerRadius;
        
        var canMove = Physics.CapsuleCast(
            transform.position, 
            transform.position + Vector3.up * playerHeight,
            playerRadius, 
            moveDirection, 
            moveDistance) is false;

        if (canMove is false)
        {
            var moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            
            canMove = moveDirectionX != Vector3.zero && Physics.CapsuleCast(
                transform.position, 
                transform.position + Vector3.up * playerHeight,
                playerRadius, 
                moveDirectionX, 
                moveDistance) is false;
        
            if (canMove) moveDirection = moveDirectionX;
            else
            {
                var moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                
                canMove = moveDirectionZ != Vector3.zero && Physics.CapsuleCast(
                    transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirectionZ,
                    moveDistance) is false;
                
                if (canMove) moveDirection = moveDirectionZ;
            }
        }

        if (canMove)
            transform.position += moveDirection * moveDistance;
        
        _isWalking = moveDirection != Vector3.zero;
        
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * _rotateSpeed);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            SelectedCounter = _selectedCounter
        });
    }
    
    public Transform GetKitchenObjectFollowTransform()
    {
        return _kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }
    
    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }
 
    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
