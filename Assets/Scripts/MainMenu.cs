using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private RectTransform scoreRectTransform;
    private void Start()
    {
        scoreRectTransform.anchoredPosition = new Vector2(scoreRectTransform.anchoredPosition.x, 15) ;
        GetComponentInChildren<TMPro.TextMeshProUGUI>()
        .gameObject.LeanScale(to: new Vector3(x: 1.2f, y: 1.2f), time: 0.3f).setLoopPingPong();
    }
    public void Play()
        {
        GetComponent<CanvasGroup>().LeanAlpha(to: 0, time: 0.2f).setOnComplete(OnComplete);
        
        }
    private void OnComplete() 
    {
        scoreRectTransform.LeanMoveY(to: -63f, time: 0.75f).setEaseOutBounce();

        gameManager.Enabled();
        Destroy(gameObject);

    }
}