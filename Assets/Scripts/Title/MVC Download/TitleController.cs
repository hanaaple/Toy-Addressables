using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utils;

namespace Title.MVC_Download
{
    // MVC (Model - View - Controller 모델)
    // 예외사항 구현 X - 일부 다운로드 실패, 인터넷 접속 에러 등
    public class TitleController : MonoBehaviour
    {   
        // views, 기능이 아닌 UI 화면 상태에 따른 View 구분
        public TitleView titleView;
        public ClearCachePopUpView clearCachePopUpView;
        public DownloadPopUpView downloadPopUpView;
        public DownloadView downloadView;
    
        // addressables - label
        public string[] keys;
    
        // models
        private DownloadSizeModel m_DownloadSizeModel;
        private DownloadModel m_DownloadModel;
    
        private void Start()
        {
            m_DownloadSizeModel = new DownloadSizeModel(0);
            m_DownloadModel = new DownloadModel();

            // 옵저버 패턴 - Subscribe View Button Events
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
            clearCachePopUpView.Subscribe(() => { ClearCacheAsync(clearCachePopUpView.PopDown); },clearCachePopUpView.PopDown);
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
            Debug.Log("DownLoad 시작");
            
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
            downloadView.WaitAndClose(5f);

            Debug.Log("DownLoad 완료");
            
            LoadScene();
        }

        private void LoadScene()
        {
            Debug.Log("Load Scene!");
            // SceneManager.LoadScene()
        }

        private async void ClearCacheAsync(UnityAction onComplete)
        {
            Debug.Log("캐시 클리어 시도");
            
            var asyncOperationHandles = keys.Select(AddressablesFixes.ClearDependencyCacheForKey);
            foreach (var asyncOperationHandle in asyncOperationHandles)
            {
                await asyncOperationHandle.Task;
            }
            
            Debug.Log("캐시 클리어 완료");
            onComplete?.Invoke();
        }
    }
}
