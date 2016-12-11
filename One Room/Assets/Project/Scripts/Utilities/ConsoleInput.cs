using UnityEngine;
using UnityEngine.UI;

public class ConsoleInput : MonoBehaviour
{
    private bool stayActive = true;

    public void KeepActive()
    {
        if (stayActive)
            GetComponent<InputField>().ActivateInputField();
    }

    void Start()
    {
        KeepActive();
    }

    public void StayActive(bool stay)
    {
        this.stayActive = stay;
    }
}