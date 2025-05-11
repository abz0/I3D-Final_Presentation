using UnityEngine;

public class EnableOnSignal : MonoBehaviour
{
    public GameObject objectToToggle;

    public void EnableObject()
    {
        if (objectToToggle != null)
            objectToToggle.SetActive(true);
    }

    public void DisableObject()
    {
        if (objectToToggle != null)
            objectToToggle.SetActive(false);
    }
}
