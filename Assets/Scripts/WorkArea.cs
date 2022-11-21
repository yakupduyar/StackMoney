using DG.Tweening;
using Lean.Pool;
using UnityEngine;

public class WorkArea : MonoBehaviour
{
    [SerializeField] private Transform moneyPrefab;
    [SerializeField] private float moneyRadius = 1;

    private void OnEnable()
    {
        InvokeRepeating(nameof(CreateMoney), 3, 3);
    }

    private void CreateMoney()
    {
        var newMoney = LeanPool.Spawn(moneyPrefab, transform.position, Quaternion.identity);
        newMoney.DOJump(RandomPointAround(), 1, 1, 1);
    }

    private Vector3 RandomPointAround()
    {
        Vector3 point = Random.insideUnitCircle.normalized;
        return transform.position + (Vector3.right * point.x + Vector3.forward * point.y) * moneyRadius;
    }
}