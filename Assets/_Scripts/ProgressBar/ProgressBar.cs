using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
   [Tooltip("Image component used to display the progress.")]
   [SerializeField] private Image _progressImage;
   
   [Tooltip("Default speed for the progress animation.")]
   [SerializeField] private float _defaultSpeed = 1f;
   
   [Tooltip("Event triggered with the current progress value.")]
   [SerializeField] private UnityEvent<float> _onProgress;
  
   [Tooltip("Event triggered when the progress completes.")]
   [SerializeField] private UnityEvent _onCompleted;
   
   [Tooltip("Gradient used to color the progress image based on its fill amount.")]
   [SerializeField] private Gradient _colorGradient;
   
   private Coroutine _animationCoroutine; // Reference to the current animation coroutine
   
   private void Start() // Start is called before the first frame update
   {
      // Ensure the progress image is of type "Filled"
      if (_progressImage.type != Image.Type.Filled)
      {
         Debug.LogError($"{name} ProgressImage is not of type \"Filled\"; Disabling;");
         this.enabled = false; // Disable the script if the condition is not met
      }
     
   }
   private IEnumerator AnimateProgress(float progress, float speed) // Coroutine to animate the progress bar
   {
      float time = 0; // Time elapsed since the animation started
      float initialProgress = _progressImage.fillAmount; // Initial fill amount of the progress image

      while (time < 1)
      {
         // Lerp the fill amount based on the elapsed time
         _progressImage.fillAmount = Mathf.Lerp(initialProgress, progress, time);
         time += Time.deltaTime * speed; // Increment the elapsed time based on the speed
         
         // Update the color of the progress image based on the current fill amount
         _progressImage.color = _colorGradient.Evaluate(1 - _progressImage.fillAmount);
         
         // Invoke the onProgress event with the current fill amount
         _onProgress?.Invoke(_progressImage.fillAmount);
         yield return null; // Wait for the next frame
      }
      // Set the final progress and color
      _progressImage.fillAmount = progress;
      _progressImage.color = _colorGradient.Evaluate(1 - _progressImage.fillAmount);
      
      // Invoke the onProgress and onCompleted events
      _onProgress?.Invoke(progress);
      _onCompleted?.Invoke();
   }
   public void SetProgress(float progress) // Set the progress with the default speed
   {
      SetProgress(progress, _defaultSpeed);
   }
   public void SetProgress(float progress, float speed) // Set the progress with a specified speed
   {
      // Validate the progress value
      if(progress is <= 0 or > 1)
      {
         Debug.LogWarning($"Expected value is between 0 and 1;");
         progress = Mathf.Clamp01(progress); // Clamp the progress value between 0 and 1
      }

      // If the new progress is different from the current fill amount
      if (progress != _progressImage.fillAmount)
      {
         // Stop the existing animation coroutine if it's running
         if (_animationCoroutine != null)
         {
            StopCoroutine(_animationCoroutine);
         }
         
         // Start a new animation coroutine
         _animationCoroutine = StartCoroutine(AnimateProgress(progress, speed));
      }
   }
   public void ResetProgress() // Reset the progress to the initial state
   {
      _progressImage.fillAmount = 1; // Reset fill amount to 1
      _progressImage.color = _colorGradient.Evaluate(1 - _progressImage.fillAmount); // Update color based on the fill amount 
   }
}
