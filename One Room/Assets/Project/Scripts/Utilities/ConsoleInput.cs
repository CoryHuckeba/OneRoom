using UnityEngine;
using UnityEngine.UI;

public class ConsoleInput : MonoBehaviour
{
    public void KeepActive()
    {
        GetComponent<InputField>().ActivateInputField();
    }

    void Start()
    {
        KeepActive();
    }
}