using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class time : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private TextMeshProUGUI TextTime;
    [SerializeField] private TextMeshProUGUI GoalMesseage;

    private float elapsedTime;

    private int f_Goal;

    void Start()
    {
        elapsedTime = 0.0f;
        f_Goal = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (f_Goal == 0)
        {
            elapsedTime += Time.deltaTime;
        }
        TextTime.text = string.Format("{0:f2} sec", elapsedTime);

        if (f_Goal == 1)
        {
            GoalMesseage.text = "Goal!!";
        }
    }

    //衝突を判定する処理を追加する
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Goal")
        {
            f_Goal = 1;
        }
    }
}
