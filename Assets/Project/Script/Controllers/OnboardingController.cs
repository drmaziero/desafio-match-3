using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Views.Onboarding;

namespace Controllers
{
    public class OnboardingController : MonoBehaviour
    {
        [field: SerializeField] private OnboardingPageView[] allPages;
        [field: SerializeField] private OnboardingButton continueButton;
        [field: SerializeField] private OnboardingButton closeButton;

        private int _pageIndex = 0;

        private void OnEnable()
        {
            _pageIndex = 0;
            
            continueButton.ButtonClicked += NextPage;
            closeButton.ButtonClicked += ClosePage;
            
            continueButton.Init();
            closeButton.Init();

            StartCoroutine(ShowPageCoroutine());
        }
        
        private void OnDisable()
        {
            continueButton.ButtonClicked -= NextPage;
            closeButton.ButtonClicked -= ClosePage;
            
            continueButton.UnregisterClick();
            closeButton.UnregisterClick();
        }
        
        private void NextPage()
        {
            StartCoroutine(NextPageCoroutine());
        }
        
        private void ClosePage()
        {
            StartCoroutine(ClosePageCoroutine());
            gameObject.SetActive(false);
        }

        private IEnumerator ShowPageCoroutine()
        {
            allPages[_pageIndex].gameObject.SetActive(true);
            yield return allPages[_pageIndex].Show().WaitForCompletion();
        }

        private IEnumerator NextPageCoroutine()
        {
            yield return ClosePageCoroutine();
            _pageIndex++;
            yield return ShowPageCoroutine();
        }

        private IEnumerator ClosePageCoroutine()
        {
            yield return allPages[_pageIndex].Close().WaitForCompletion();
            allPages[_pageIndex].gameObject.SetActive(false);
        }
    }
}