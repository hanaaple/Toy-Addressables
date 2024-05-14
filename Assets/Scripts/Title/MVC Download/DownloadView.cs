using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Title.MVC_Download
{
    public class DownloadView : MonoBehaviour
    {
        [SerializeField] private TMP_Text downloadGuideText;
        [SerializeField] private TMP_Text percentageText;
        [SerializeField] private Image slide;

        [SerializeField] private GameObject downloadPanel;

        [SerializeField] private int maxCount = 3;
        [SerializeField] private float tick = 0.4f;
        
        private int m_Count;
        private float m_Time;

        public void UpdateView(DownloadModel downloadModel)
        {
            percentageText.text = $"{(int)(downloadModel.PercentComplete * 100)} %";
            slide.fillAmount = downloadModel.PercentComplete;
        }

        public void Open()
        {
            downloadPanel.SetActive(true);
            StartCoroutine(Downloading());
        }

        private void Close()
        {
            downloadPanel.SetActive(false);
        }
        
        public void WaitAndClose()
        {
            StopAllCoroutines();
            downloadGuideText.text = "다운로드 완료!";

            Invoke(nameof(Close), 5);
        }

        private IEnumerator Downloading()
        {
            m_Time = 0;
            m_Count = 0;
            downloadGuideText.text = $"다운로드 중{string.Concat(Enumerable.Repeat(".", m_Count + 1))}";
            
            while (true)
            {
                while (m_Time < tick)
                {
                    m_Time += Time.deltaTime;
                    yield return null;
                }

                m_Time = 0;
                m_Count = (m_Count + 1) % maxCount;
                downloadGuideText.text = $"다운로드 중{string.Concat(Enumerable.Repeat(".", m_Count + 1))}";
            }
        }
    }
}