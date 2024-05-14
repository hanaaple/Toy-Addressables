using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utils;

// MVC (Model - View - Controller 모델)
// 예외사항 구현 X - 일부 다운로드 실패, 인터넷 접속 에러 등

// 실행방식
// View의 버튼 OnClick -> Controller (Model 업데이트) -> View Update

namespace Title.MVC_Download
{
    public class TitleController : MonoBehaviour
    {   
        public TitleView titleView;
        public ClearCachePopUpView clearCachePopUpView;
        public DownloadPopUpView downloadPopUpView;
        public DownloadView downloadView;
    
        public string[] keys;
    
        private DownloadSizeModel m_DownloadSizeModel;
        private DownloadModel m_DownloadModel;
    
        private void Start()
        {
            m_DownloadSizeModel = new DownloadSizeModel(0);
            m_DownloadModel = new DownloadModel();

            titleView.Subscribe(() =>
                {
                    UpdateDownLoadSizeAsync(() =>
                    {
                        if (m_DownloadSizeModel.DownloadSize > 0)
                        {
                            downloadPopUpView.UpdateView(m_DownloadSizeModel);
                            downloadPopUpView.PopUp();
                        }
                        else
                        {
                            LoadScene();
                        }
                    });
                },
                clearCachePopUpView.PopUp);
        
            downloadPopUpView.Subscribe(() => { StartCoroutine(DownLoad());}, downloadPopUpView.PopDown);
            clearCachePopUpView.Subscribe(() => { ClearCache(clearCachePopUpView.PopDown); },clearCachePopUpView.PopDown);
        }

        private async void UpdateDownLoadSizeAsync(UnityAction onComplete)
        {
            m_DownloadSizeModel.DownloadSize = 0;
            foreach (var key in keys)
            {
                var handle = Addressables.GetDownloadSizeAsync(key);
                handle.Completed += op =>
                {
                    if(op.Status == AsyncOperationStatus.Succeeded)
                        m_DownloadSizeModel.DownloadSize += op.Result;

                    if (handle.OperationException != null)
                    {
                        Addressables.LogException(handle, handle.OperationException);
                    }
                    Addressables.Release(handle);
                };
                await handle.Task;
            }
            
            onComplete?.Invoke();
        }

        private IEnumerator DownLoad()
        {
            downloadPopUpView.PopDown();
            downloadView.Open();

            var handle = Addressables.DownloadDependenciesAsync(keys as IEnumerable, Addressables.MergeMode.Union, true);
            while (!handle.IsDone)
            {
                m_DownloadModel.PercentComplete = handle.PercentComplete;
                downloadView.UpdateView(m_DownloadModel);
                yield return null;
            }

            m_DownloadModel.PercentComplete = 1;
            downloadView.UpdateView(m_DownloadModel);
            downloadView.WaitAndClose();

            // DownLoad 완료
            Debug.Log("DownLoad 완료");
            
            LoadScene();
        }

        private void LoadScene()
        {
            Debug.Log("Load Scene!");
            // SceneManager.LoadScene()
        }

        private async void ClearCache(UnityAction onComplete)
        {
            Debug.Log($"캐시 클리어 {Time.frameCount}");
            // AssetBundle.UnloadAllAssetBundles(true);
            // Addressables.ClearDependencyCacheAsync(key);
            
            var asyncOperationHandles = keys.Select(AddressablesFixes.ClearDependencyCacheForKey);
            foreach (var asyncOperationHandle in asyncOperationHandles)
            {
                await asyncOperationHandle.Task;
            }
            
            Debug.Log($"캐시 클리어 완료 {Time.frameCount}");
            onComplete?.Invoke();
        }
    }
}
