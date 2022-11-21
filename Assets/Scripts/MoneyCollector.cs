using System.Collections.Generic;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;

public class MoneyCollector : MonoBehaviour
{
    [SerializeField] private Transform moneysPivot;
    [SerializeField] private float moneyThickness = .2f, spendInterval = .1f;
    [SerializeField] private List<Money> moneys;


    private void Update()
    {
        if (moneys.Count > 0) SetPositions();
    }

    private void SetPositions()
    {
        moneys[0].transform.position =
            Vector3.Lerp(moneys[0].transform.position, moneysPivot.position, Time.deltaTime * 50);
        moneys[0].transform.rotation =
            Quaternion.Lerp(moneys[0].transform.rotation, moneysPivot.rotation, Time.deltaTime * 50);
        for (var i = 1; i < moneys.Count; i++)
        {
            var m = moneys[i].transform;
            var targetPos = moneys[i - 1].transform.position + Vector3.up * moneyThickness;
            var targetRot = moneys[i - 1].transform.rotation;
            m.position = Vector3.Lerp(m.position, targetPos, Time.deltaTime * 50);
            m.rotation = Quaternion.Lerp(m.rotation, targetRot, Time.deltaTime * 50);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Money m) && !m.IsCollected)
        {
            m.IsCollected = true;
            var jumpPos = moneys.Count > 0
                ? moneys[^1].transform.position + Vector3.up * moneys.Count * moneyThickness
                : moneysPivot.position + Vector3.up * moneys.Count * moneyThickness;
            other.transform.DOJump(jumpPos, moneys.Count * moneyThickness, 1, .25f).OnComplete(
                () => { moneys.Add(m); });
        }
    }

    private float spendTime;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out FillingArea fa) && moneys.Count > 0)
        {
            if (spendTime > spendInterval)
            {
                var spendedMoney = moneys[^1];
                moneys.Remove(moneys[^1]);
                fa.Fill(spendedMoney.Value);
                var newSeq = DOTween.Sequence();
                newSeq.Append(spendedMoney.transform.DOJump(fa.transform.position, 1, 1, .5f));
                newSeq.Join(spendedMoney.transform.DOScale(Vector3.zero, .5f)).SetEase(Ease.InCubic);
                newSeq.Play().OnComplete(
                    () => { LeanPool.Despawn(spendedMoney); });
                spendTime = 0;
            }
            else
            {
                spendTime += Time.deltaTime;
            }
        }
    }
}