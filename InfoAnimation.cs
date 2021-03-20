
using UnityEngine;
using TMPro;

public class InfoAnimation : MonoBehaviour
{
    [SerializeField] float animationTime = 2f;
    [SerializeField] TextMeshProUGUI text;
    void Start()
    {
        
    }
    float valueToLerp;
    // Update is called once per frame
    void Update()
    {
        if (animationTime > 0)
        {
            valueToLerp = 300- Mathf.Lerp(0, 300, animationTime / 2f);
            animationTime -= Time.deltaTime;
            text.transform.localPosition = new Vector3(0, valueToLerp, 0);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
