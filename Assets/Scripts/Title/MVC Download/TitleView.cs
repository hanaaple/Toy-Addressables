using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Title.MVC_Download
{
    public class TitleView: MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button clearCacheButton;
        
        public void Subscribe(UnityAction onStart, UnityAction onClearCache)
        {
            startButton.onClick.AddListener(onStart);
            clearCacheButton.onClick.AddListener(onClearCache);
        }
    }
}