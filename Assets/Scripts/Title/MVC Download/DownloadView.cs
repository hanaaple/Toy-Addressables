using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Title.MVC_Download
{
    /// <summary>
    /// Download 로딩 화면 view
    /// </summary>
    public class DownloadView : MonoBehaviour
    {
        [SerializeField] private TMP_Text downloadGuideText;
        [SerializeField] private TMP_Text percentageText;
        [SerializeField] private Image slide;

        [SerializeField] private GameObject downloadPanel;

        [SerializeField] private int maxCount = 3;
        [SerializeField] private float tick = 0.4f;
        
        public void UpdateView(DownloadModel downloadModel)
        {
            percentageText.text = $"{(int)(downloadModel.PercentComplete * 100)} %";
            slide.fillAmount = downloadModel.PercentComplete;
        }

        public void Open()
        {
            downloadPanel.SetActive(true);
            StartCoroutine(UpdateDisplayOnDownload());
        }

        private void Close()
        {
            downloadPanel.SetActive(false);
        }
        
        public void WaitAndClose(float waitTime)
        {
            StopAllCoroutines();
            downloadGuideText.text = "다운로드 완료!";

            Invoke(nameof(Close), waitTime);
        }

        private IEnumerator UpdateDisplayOnDownload()
        {
            float time = 0;
            int count = 0;
            downloadGuideText.text = $"다운로드 중{string.Concat(Enumerable.Repeat(".", 1))}";
            
            while (true)
            {
                while (time < tick)
                {
                    time += Time.deltaTime;
                    yield return null;
                }

                time = 0;
                count = (count + 1) % maxCount;
                downloadGuideText.text = $"다운로드 중{string.Concat(Enumerable.Repeat(".", count + 1))}";
            }
        }
    }
}