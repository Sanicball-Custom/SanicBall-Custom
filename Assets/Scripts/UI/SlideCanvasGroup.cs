using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sanicball.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SlideCanvasGroup : MonoBehaviour
    {
        public bool isOpen = false;
        public Vector2 closedPosition;
        public float time = 1f;
        public UnityEvent onOpen;
        public UnityEvent onClose;
        private CanvasGroup cg;
        private float pos = 0f;
        private Vector2 startPosition;
        private RectTransform rectTransform;

        public void Open()
        {
            isOpen = true;
            gameObject.SetActive(true);
            cg.alpha = 1f;
            cg.interactable = true;
            onOpen.Invoke();
        }

        public void Close(bool closeSelf = false)
        {
            StartCoroutine(CloseAllChildren(closeSelf));
        }

        public IEnumerator CloseAllChildren(bool closeSelf) {
            SlideCanvasGroup childGroup = GetComponentsInChildren<SlideCanvasGroup>().Where(el => el != this && el.isOpen).FirstOrDefault();
            if (childGroup != null) 
                yield return StartCoroutine(childGroup.CloseAllChildren(closeSelf));
            if (closeSelf || childGroup == null) {
                isOpen = false;
                yield return new WaitUntil(() => pos == 0);
                cg.interactable = false;
                onClose.Invoke();
            }
            yield return null;
        }

        public void Toggle() {
            if (isOpen) Close();
            else Open();
        }

        // Use this for initialization
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            cg = GetComponent<CanvasGroup>();
            startPosition = rectTransform.anchoredPosition;
            cg.interactable = isOpen;
            cg.alpha = isOpen ? 1f : 0f;
            if (isOpen)
                pos = 1f;
            else
                gameObject.SetActive(false);
            UpdatePosition();
        }

        private void Update()
        {
            if (isOpen && pos < 1f)
            {
                pos = Mathf.Min(1, pos + Time.deltaTime / time);
            }
            if (!isOpen && pos > 0f)
            {
                pos = Mathf.Max(0, pos - Time.deltaTime / time);
                if (pos <= 0f)
                {
                    cg.alpha = 0f;
                    gameObject.SetActive(false);
                }
            }

            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var smoothedPos = Mathf.SmoothStep(0f, 1f, pos);

            (transform as RectTransform).anchoredPosition = Vector2.Lerp(startPosition + closedPosition, startPosition, smoothedPos);
        }
    }
}
