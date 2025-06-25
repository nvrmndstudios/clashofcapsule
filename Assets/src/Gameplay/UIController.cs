using System.Collections;
using System.Collections.Generic;
using MPUIKIT;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UIController : MonoBehaviour
{
   [SerializeField] private Canvas canvas;
   [SerializeField] private GameObject killPrefab;


   private List<GameObject> _generatedClones = new List<GameObject>();
   public void OnEnemyDie(Transform enemyPosition)
   {
      GameObject instance = Instantiate(killPrefab);
      _generatedClones.Add(instance);
      instance.GetComponent<FloatingTextEffect>().Initialize(enemyPosition.position, canvas);
   }

    [Header("Screens")]
    public GameObject splashScreen;
    public GameObject menuScreen;
    public GameObject gameplayScreen;
    public GameObject resultScreen;

    [Header("Menu Screen UI")]
    public TMP_Text highKillCount;
    public TMP_Text soundTxt;
    // public List<Image> soundSprites;

    [Header("Gameplay UI")]
    public TMP_Text killCountText;
    
    public TMP_Text wavesText;

    
    [Header("Result Screen UI")]
    public TMP_Text finalScoreText;

    // ==============================
    // Screen Visibility Handlers
    // ==============================

    public void ShowOnlySplash()
    {
        splashScreen.SetActive(true);
        menuScreen.SetActive(false);
        gameplayScreen.SetActive(false);
        resultScreen.SetActive(false);
    }

    public void ShowOnlyMenu()
    {
        splashScreen.SetActive(false);
        menuScreen.SetActive(true);
        gameplayScreen.SetActive(false);
        resultScreen.SetActive(false);
    }

    public void ShowOnlyGameplay()
    {
        splashScreen.SetActive(false);
        menuScreen.SetActive(false);
        gameplayScreen.SetActive(true);
        resultScreen.SetActive(false);
    }

    public void ShowOnlyResult()
    {
        if (_generatedClones != null && _generatedClones.Count > 0)
        {
            foreach (var t in _generatedClones)
            {
                if (t != null)
                {
                    Destroy(t);
                }
            }
        }

        _generatedClones = new List<GameObject>();

        splashScreen.SetActive(false);
        menuScreen.SetActive(false);
        gameplayScreen.SetActive(false);
        resultScreen.SetActive(true);
    }

    // ==============================
    // Menu UI Updates
    // ==============================

    public void UpdateHighKillCount(int highKillCount)
    {
        this.highKillCount.text = $"{highKillCount}";
    }

    // ==============================
    // Gameplay UI Updates
    // ==============================

    public void UpdateKillCount(int score)
    {
        killCountText.text = $"KILLED {score}";
    }

    public void UpdateWave(int wave)
    {
        wavesText.text = $"WAVE {wave}";
    }
    // ==============================
    // Result Screen Actions
    // ==============================

    public void SetFinalScore(int score)
    {
        finalScoreText.text = $"{score}";
    }

    public void OnClickRestart()
    {
        SoundManager.Instance.PlayClick();
        GameManager.Instance.ChangeState(GameManager.GameState.Gameplay);
    }

    public void OnClickHome()
    {
        SoundManager.Instance.PlayClick();
        GameManager.Instance.ChangeState(GameManager.GameState.Menu);
    }

    public void ToggleSound()
    {
       bool isSoundOn = SoundManager.Instance.ToggleSound();
       soundTxt.text = !isSoundOn ? "SOUND OFF" : "SOUND ON";
    } 
}
