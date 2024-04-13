using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
   [SerializeField] private Image _progressImage;
   [SerializeField] private float _defaultSpeed = 1f;
   [SerializeField] private UnityEvent<float> _onProgress;
   [SerializeField] private UnityEvent _onCompleted;
   [SerializeField] private Gradient _colorGradient;
   
   private Coroutine _animationCoroutine;

   private void Start()
   {
      if (_progressImage.type != Image.Type.Filled)
      {
         Debug.LogError($"{name} ProgressImage is not of type \"Filled\"; Disabling;");
         this.enabled = false;
      }
     
   }

   public void SetProgress(float progress)
   {
      SetProgress(progress, _defaultSpeed);
   }

   public void SetProgress(float progress, float speed)
   {
      if(progress is <= 0 or > 1)
      {
         Debug.LogWarning($"Expected value is between 0 and 1;");
         progress = Mathf.Clamp01(progress);
      }

      if (progress != _progressImage.fillAmount)
      {
         if (_animationCoroutine != null)
         {
            StopCoroutine(_animationCoroutine);
         }

         _animationCoroutine = StartCoroutine(AnimateProgress(progress, speed));
      }
   }

   private IEnumerator AnimateProgress(float progress, float speed)
   {
      float time = 0;
      float initialProgress = _progressImage.fillAmount;

      while (time < 1)
      {
         _progressImage.fillAmount = Mathf.Lerp(initialProgress, progress, time);
         time += Time.deltaTime * speed;
         
         _progressImage.color = _colorGradient.Evaluate(1 - _progressImage.fillAmount);
         
         _onProgress?.Invoke(_progressImage.fillAmount);
         yield return null;
      }
      _progressImage.fillAmount = progress;
      _progressImage.color = _colorGradient.Evaluate(1 - _progressImage.fillAmount);
      
      _onProgress?.Invoke(progress);
      _onCompleted?.Invoke();
   }
   
   public void ResetProgress()
   {
      _progressImage.fillAmount = 1;
      _progressImage.color = _colorGradient.Evaluate(1 - _progressImage.fillAmount);
   }
}
