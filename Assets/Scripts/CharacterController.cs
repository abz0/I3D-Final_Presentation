using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 1.0f;
    public float runSpeed = 3.0f;
    
    [Header("Animation")]
    public string walkParamName = "isWalking";
    
    public string idleParamName = "isIdle";
    
    private Animator animator;
    private bool isMoving = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (animator) {
            animator.SetBool(walkParamName, false);
            animator.SetBool(idleParamName, true);
            Debug.Log($"{gameObject.name} initial animation state set to idle");
            
            AnimatorControllerParameter[] parameters = animator.parameters;
            foreach (var param in parameters) {
                Debug.Log($"Animation parameter found: {param.name} (Type: {param.type})");
            }
        } else {
            Debug.LogWarning("No Animator component found on " + gameObject.name);
        }
    }
    
    void Update()
    {
        UpdateAnimationState();
    }
    
    private void UpdateAnimationState()
    {
        if (animator) {
            bool actuallyMoving = GetComponent<Rigidbody>() ? 
                GetComponent<Rigidbody>().velocity.magnitude > 0.1f : 
                isMoving;
                
            animator.SetBool(walkParamName, actuallyMoving);
            animator.SetBool(idleParamName, !actuallyMoving);
        }
    }
    
    public IEnumerator MoveToPosition(Vector3 targetPosition, float speed = -1)
    {
        if (speed < 0) speed = walkSpeed;
        
        Vector3 direction = (targetPosition - transform.position).normalized;
        
        transform.rotation = Quaternion.LookRotation(direction);
        
        isMoving = true;
        if (animator)
        {
            animator.SetBool(walkParamName, true);
            animator.SetBool(idleParamName, false);
            Debug.Log($"{gameObject.name} setting walk animation state");
        }
        
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPosition, 
                speed * Time.deltaTime
            );
            yield return null;
        }
        
        transform.position = targetPosition;
        
        isMoving = false;
        if (animator)
        {
            animator.SetBool(walkParamName, false);
            animator.SetBool(idleParamName, true);
            Debug.Log($"{gameObject.name} setting idle animation state");
        }
    }
    
    public IEnumerator ActNervous(float duration)
    {
        Debug.Log("Simulating nervous behavior for " + gameObject.name);
        
        Vector3 originalPosition = transform.position;
        float elapsed = 0;
        
        while (elapsed < duration)
        {
            transform.position = originalPosition + new Vector3(
                Random.Range(-0.05f, 0.05f),
                0,
                Random.Range(-0.05f, 0.05f)
            );
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = originalPosition;
        
        yield return new WaitForSeconds(0.5f);
    }
    
    public IEnumerator LookAt(Vector3 target, float duration)
    {
        Vector3 direction = (target - transform.position).normalized;
        
        direction.y = 0;
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion startRotation = transform.rotation;
        
        float elapsed = 0;
        float rotationDuration = 0.5f;
        
        while (elapsed < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                elapsed / rotationDuration
            );
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.rotation = targetRotation;
        
        yield return new WaitForSeconds(duration);
    }
}