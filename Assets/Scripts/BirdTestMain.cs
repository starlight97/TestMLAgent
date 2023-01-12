using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BirdTestMain : MonoBehaviour
{
    private BirdAgentTest birdAgentTest;
    public GameObject redScrrenGo;
    public Ground[] grounds;

    public TMP_Text textTime;
    private float aliveTime;

    private Coroutine showRedScreenRoutine;
    void Start()
    {
        this.birdAgentTest = transform.parent.transform.Find("BirdAgent").GetComponent<BirdAgentTest>();
        //this.birdAgentTest = GameObject.FindObjectOfType<BirdAgentTest>();

        this.birdAgentTest.onDie = () =>
        {
            birdAgentTest.transform.localPosition = new Vector3(-8, 0, 0);

            grounds[0].gameObject.transform.localPosition = new Vector3(0, -5.7f , 0);
            grounds[1].gameObject.transform.localPosition = new Vector3(24f, -5.7f , 0);

            if(showRedScreenRoutine == null)
                showRedScreenRoutine = StartCoroutine(ShowRedScreenRoutine());

            this.aliveTime = 0f;
        };

        this.birdAgentTest.onSetLevel = (level)=>
        {
            foreach (var ground in grounds)
            {
                ground.SetLevel(2);
            }
        };
        this.aliveTime = 0f;
    }
    private void Update()
    {
        aliveTime += Time.deltaTime;
        this.textTime.text = "Time : " + aliveTime.ToString("F1");
    }

    private IEnumerator ShowRedScreenRoutine()
    {
        this.redScrrenGo.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        this.redScrrenGo.SetActive(false);
        showRedScreenRoutine = null;

    }
}
