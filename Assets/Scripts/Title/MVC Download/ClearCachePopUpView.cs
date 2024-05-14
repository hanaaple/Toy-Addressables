using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Title.MVC_Download
{
    public class ClearCachePopUpView : MonoBehaviour
    {
        [SerializeField] private Button clearCacheButton;
        [SerializeField] private Button cancelButton;
        
        [SerializeField] private GameObject popupPanel;

        public void Subscribe(UnityAction clearCall, UnityAction cancelCall)
        {
            clearCacheButton.onClick.AddListener(clearCall);
            cancelButton.onClick.AddListener(cancelCall);
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