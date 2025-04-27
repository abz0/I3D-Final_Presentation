using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryboardDirector : MonoBehaviour
{
    [Header("Characters")]
    public GameObject kaiCharacter;
    public GameObject liamCharacter;
    public GameObject noahCharacter;
    
    [Header("Camera")]
    public Camera mainCamera;
    
    [Header("Camera Positions")]
    public GameObject Panel1CameraPos;
    
    public GameObject Panel2CameraPos;
    
    public GameObject Panel3CameraPos;
    
    public GameObject Panel4CameraPos;
    
    [Header("Scene Objects")]
    public GameObject bridgeObject;
    public GameObject signObject;
    public Transform bridgeStart;
    
    [Header("Camera Follow")]
    public Vector3 followOffset = new Vector3(3, 3, -7);
    
    [Range(0.1f, 10f)]
    public float followSmoothness = 2.0f;
    
    private CharacterController kaiController;
    private CharacterController liamController;
    private CharacterController noahController;
    
    void Start()
    {
        if (kaiCharacter) kaiController = kaiCharacter.GetComponent<CharacterController>();
        if (liamCharacter) liamController = liamCharacter.GetComponent<CharacterController>();
        if (noahCharacter) noahController = noahCharacter.GetComponent<CharacterController>();
        
        bool allCamerasPresent = Panel1CameraPos != null && 
                                Panel2CameraPos != null &&
                                Panel3CameraPos != null &&
                                Panel4CameraPos != null;
        
        if (!allCamerasPresent) {
            Debug.LogWarning("Some camera positions are missing. Please assign all camera positions in the inspector.");
        }
        
        LogCharacterPositions();
        
        StartCoroutine(PlayStoryboard());
    }
    
    void LogCharacterPositions()
    {
        Debug.Log("Using character positions as placed in the scene");
        
        if (kaiCharacter) Debug.Log($"Kai is at position: {kaiCharacter.transform.position}");
        if (liamCharacter) Debug.Log($"Liam is at position: {liamCharacter.transform.position}");
        if (noahCharacter) Debug.Log($"Noah is at position: {noahCharacter.transform.position}");
    }
    
    IEnumerator PlayStoryboard()
    {
        Debug.Log("Starting storyboard playback...");
        
        yield return new WaitForSeconds(0.5f);
        
        yield return StartCoroutine(Panel1_BridgeOverview());
        
        yield return StartCoroutine(Panel2_CharactersApproach());
        
        yield return StartCoroutine(Panel3_SignCloseup());
        
        yield return StartCoroutine(Panel4_CharactersReadingSign());
        
        Debug.Log("Completed first four scenes");
    }
    
    IEnumerator Panel1_BridgeOverview()
    {
        Debug.Log("Starting Panel 1: Bridge overview");
        
        if (Panel1CameraPos == null) {
            Debug.LogError("Panel 1 camera position is not assigned!");
            yield break;
        }
        
        yield return StartCoroutine(MoveCameraToPosition(
            Panel1CameraPos.transform.position,
            Panel1CameraPos.transform.rotation,
            3.0f
        ));
        
        yield return new WaitForSeconds(2.0f);
        
        Debug.Log("Completed Panel 1");
    }
    
    IEnumerator Panel2_CharactersApproach()
    {
        Debug.Log("Starting Panel 2: Characters approach with dynamic follow");
        
        if (Panel2CameraPos == null) {
            Debug.LogError("Panel 2 camera position is not assigned!");
            yield break;
        }
        
        yield return StartCoroutine(MoveCameraToPosition(
            Panel2CameraPos.transform.position,
            Panel2CameraPos.transform.rotation,
            2.0f
        ));
        
        yield return new WaitForSeconds(1.0f);
        
        Vector3 bridgeDirection = Vector3.forward;
        if (bridgeStart != null && bridgeObject != null) {
            bridgeDirection = (bridgeObject.transform.position - bridgeStart.position).normalized;
            bridgeDirection.y = 0;
        }
        
        Vector3 bridgeStartPos = bridgeStart != null ? 
            bridgeStart.position : 
            GetCharactersCenter() + bridgeDirection * 10f;
        
        Vector3 noahTargetPos = bridgeStartPos + Vector3.Cross(bridgeDirection, Vector3.up) * 1.5f;
        Vector3 liamTargetPos = bridgeStartPos;
        Vector3 kaiTargetPos = bridgeStartPos - Vector3.Cross(bridgeDirection, Vector3.up) * 1.5f;
        
        if (kaiCharacter) kaiTargetPos.y = kaiCharacter.transform.position.y;
        if (liamCharacter) liamTargetPos.y = liamCharacter.transform.position.y;
        if (noahCharacter) noahTargetPos.y = noahCharacter.transform.position.y;
        
        if (kaiController) {
            StartCoroutine(kaiController.MoveToPosition(kaiTargetPos, kaiController.walkSpeed));
        }
        
        if (liamController) {
            StartCoroutine(liamController.MoveToPosition(liamTargetPos, liamController.walkSpeed));
        }
        
        if (noahController) {
            StartCoroutine(noahController.MoveToPosition(noahTargetPos, noahController.walkSpeed));
        }
        
        float followDuration = 5.0f;
        float elapsedTime = 0;
        
        while (elapsedTime < followDuration)
        {
            Vector3 charactersCenter = GetCharactersCenter();
            
            Vector3 targetFollowPosition = charactersCenter + followOffset;
            
            Quaternion targetFollowRotation = Quaternion.LookRotation(charactersCenter - targetFollowPosition);
            
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position, 
                targetFollowPosition, 
                Time.deltaTime * followSmoothness
            );
            
            mainCamera.transform.rotation = Quaternion.Slerp(
                mainCamera.transform.rotation, 
                targetFollowRotation, 
                Time.deltaTime * followSmoothness
            );
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(1.0f);
        
        Debug.Log("Completed Panel 2");
    }
    
    IEnumerator Panel3_SignCloseup()
    {
        Debug.Log("Starting Panel 3: Sign closeup");
        
        if (Panel3CameraPos == null) {
            Debug.LogError("Panel 3 camera position is not assigned!");
            yield break;
        }
        
        yield return StartCoroutine(MoveCameraToPosition(
            Panel3CameraPos.transform.position,
            Panel3CameraPos.transform.rotation,
            2.0f
        ));
        
        yield return new WaitForSeconds(3.0f);
        
        Debug.Log("Completed Panel 3");
    }
    
    IEnumerator Panel4_CharactersReadingSign()
    {
        Debug.Log("Starting Panel 4: Characters reading sign");
        
        if (Panel4CameraPos == null) {
            Debug.LogError("Panel 4 camera position is not assigned!");
            yield break;
        }
        
        Vector3 signPosition = signObject != null ? 
            signObject.transform.position : 
            (bridgeStart != null ? bridgeStart.position : Vector3.zero);
        
        float arcRadius = 2.0f;
        
        Vector3 kaiPosition = signPosition + Quaternion.Euler(0, -30, 0) * Vector3.left * arcRadius;
        Vector3 liamPosition = signPosition + Vector3.left * arcRadius;
        Vector3 noahPosition = signPosition + Quaternion.Euler(0, 30, 0) * Vector3.left * arcRadius;
        
        if (kaiCharacter) kaiPosition.y = kaiCharacter.transform.position.y;
        if (liamCharacter) liamPosition.y = liamCharacter.transform.position.y;
        if (noahCharacter) noahPosition.y = noahCharacter.transform.position.y;
        
        List<Coroutine> movementCoroutines = new List<Coroutine>();
        
        if (kaiController) {
            movementCoroutines.Add(StartCoroutine(kaiController.MoveToPosition(kaiPosition, kaiController.walkSpeed)));
        }
        
        if (liamController) {
            movementCoroutines.Add(StartCoroutine(liamController.MoveToPosition(liamPosition, liamController.walkSpeed)));
        }
        
        if (noahController) {
            movementCoroutines.Add(StartCoroutine(noahController.MoveToPosition(noahPosition, noahController.walkSpeed)));
        }
        
        yield return new WaitForSeconds(2.0f);
        
        if (kaiCharacter) {
            Vector3 directionToSign = (signPosition - kaiCharacter.transform.position).normalized;
            directionToSign.y = 0;
            
            kaiCharacter.transform.rotation = Quaternion.LookRotation(directionToSign);
        }
        
        if (liamCharacter) {
            Vector3 directionToSign = (signPosition - liamCharacter.transform.position).normalized;
            directionToSign.y = 0;
            liamCharacter.transform.rotation = Quaternion.LookRotation(directionToSign);
        }
        
        if (noahCharacter) {
            Vector3 directionToSign = (signPosition - noahCharacter.transform.position).normalized;
            directionToSign.y = 0;
            noahCharacter.transform.rotation = Quaternion.LookRotation(directionToSign);
        }
        
        yield return new WaitForSeconds(2.0f);
        
        yield return StartCoroutine(MoveCameraToPosition(
            Panel4CameraPos.transform.position,
            Panel4CameraPos.transform.rotation,
            1.5f
        ));
        
        yield return new WaitForSeconds(3.0f);
        
        Debug.Log("Completed Panel 4");
    }
    
    private IEnumerator MoveCameraToPosition(Vector3 targetPosition, Quaternion targetRotation, float duration)
    {
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;
        float elapsedTime = 0;
        
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float smoothT = Mathf.SmoothStep(0, 1, t);
            
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, smoothT);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, smoothT);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
    }
    
    private Vector3 GetCharactersCenter()
    {
        Vector3 charactersCenter = Vector3.zero;
        int characterCount = 0;
        
        if (kaiCharacter) {
            charactersCenter += kaiCharacter.transform.position;
            characterCount++;
        }
        
        if (liamCharacter) {
            charactersCenter += liamCharacter.transform.position;
            characterCount++;
        }
        
        if (noahCharacter) {
            charactersCenter += noahCharacter.transform.position;
            characterCount++;
        }
        
        if (characterCount > 0) {
            charactersCenter /= characterCount;
        }
        
        return charactersCenter;
    }
}