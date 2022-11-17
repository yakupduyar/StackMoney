using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoneyCollector : MonoBehaviour
{
    [SerializeField] private Transform moneysPivot;
    [SerializeField] private float moneyThickness=.2f;
    [SerializeField] private List<Transform> moneys;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Money") && !moneys.Contains(other.transform))
        {
            other.tag = "Untagged";
            Vector3 jumpPos = (moneys.Count > 0) ? moneys[^1].position + Vector3.up * moneys.Count * moneyThickness
                    : moneysPivot.position + Vector3.up * moneys.Count * moneyThickness;
            other.transform.DOJump(jumpPos, moneys.Count*moneyThickness,1,.25f).OnComplete(
                () =>
                {
                    moneys.Add(other.transform);
                });
        }
    }

    private void Update()
    {
        if(moneys.Count>0) SetPositions();
    }

    private void SetPositions()
    {
        moneys[0].position = Vector3.Lerp(moneys[0].position, moneysPivot.position, Time.deltaTime*50);
        moneys[0].rotation = Quaternion.Lerp(moneys[0].rotation, moneysPivot.rotation, Time.deltaTime*50);
        for (int i = 1; i < moneys.Count; i++)
        {
            Transform m = moneys[i];
            Vector3 targetPos = moneys[i-1].position + Vector3.up * moneyThickness;
            Quaternion targetRot = moneys[i - 1].rotation;
            m.position = Vector3.Lerp(m.position, targetPos, Time.deltaTime*50);
            m.rotation = Quaternion.Lerp(m.rotation,targetRot,Time.deltaTime*50);
        }
    }
}
