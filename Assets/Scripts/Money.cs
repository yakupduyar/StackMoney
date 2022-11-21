using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private float value;
    public float Value => value;
    public bool IsCollected;

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        IsCollected = false;
    }
}