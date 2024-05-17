using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Title.MVC_Download
{
    /// <summary>
    /// Clear Cache Yes/No 화면 View
    /// </summary>
    public class ClearCachePopUpView : MonoBehaviour
    {
        [SerializeField] private Button clearCacheButton;
        [SerializeField] private Button cancelButton;
        
        [SerializeField] private GameObject popupPanel;

        public void Subscribe(UnityAction onStartClear, UnityAction onCancel)
        {
            clearCacheButton.onClick.AddListener(onStartClear);
            cancelButton.onClick.AddListener(onCancel);
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