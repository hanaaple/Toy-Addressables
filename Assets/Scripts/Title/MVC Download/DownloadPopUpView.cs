using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Title.MVC_Download
{
    /// <summary>
    /// Download yes/no 화면 view
    /// </summary>
    public class DownloadPopUpView : MonoBehaviour
    {
        [SerializeField] private Button downloadButton;
        [SerializeField] private Button cancelButton;

        [SerializeField] private TMP_Text downloadInfoText;
        
        [SerializeField] private GameObject popupPanel;

        public void Subscribe(UnityAction onStartDownload, UnityAction onCancel)
        {
            downloadButton.onClick.AddListener(onStartDownload);
            cancelButton.onClick.AddListener(onCancel);
        }

        public void UpdateView(DownloadSizeModel downloadSizeModel)
        {
            var convertedDownloadSize = downloadSizeModel.GetConvertedDownloadSize();
            downloadInfoText.text =
                $"{convertedDownloadSize.Item1:#,##0.##}{convertedDownloadSize.Item2}의 데이터를 다운로드 합니다\n\n저장 공간이 부족한 경우에는 타이틀 화면으로 돌아갑니다\n\nWi-Fi 환경에서 다운로드하는 걸 권장합니다.";
        }

        public void PopUp()
        {
            popupPanel.SetActive(true);
        }
        
        public void PopDown()
        {
            popupPanel.SetActive(false);
        }
    }
}