using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FillingArea : MonoBehaviour
{
    [SerializeField] private float price;
    [SerializeField] private Image fillBar;
    [SerializeField] private TextMeshProUGUI remaining;
    [SerializeField] private GameObject resultObject;
    [SerializeField] private Collider collider;
    [SerializeField] private Transform workerPoses;
    private float progress;
    private bool isFilling;
    private int respawnCount;

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        progress = 0;
        remaining.text = "$" + price;
        collider.enabled = true;
        fillBar.fillAmount = 0;
    }

    private void Respawn()
    {
        respawnCount++;
        if (respawnCount > workerPoses.childCount - 1)
            //SOME CONDITIONS
            return;

        transform.position = workerPoses.GetChild(respawnCount).position;
        price = Mathf.RoundToInt(price * 1.5f);
        Init();
    }

    public void Fill(float v)
    {
        progress += v;
        remaining.text = "$" + (price - progress);
        collider.enabled = progress < price;
        if (!isFilling) StartCoroutine(Filling());
    }

    private IEnumerator Filling()
    {
        isFilling = true;
        while (fillBar.fillAmount < progress / price)
        {
            Debug.Log(progress / price);
            fillBar.fillAmount = Mathf.MoveTowards(fillBar.fillAmount, progress / price, .002f);
            yield return null;
        }

        if (fillBar.fillAmount > .99f) SpawnResult();

        isFilling = false;
    }

    private void SpawnResult()
    {
        Instantiate(resultObject, transform.position, Quaternion.identity);
        resultObject.transform.parent = workerPoses.GetChild(respawnCount);
        Respawn();
    }
}