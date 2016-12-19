//
//  LoadAssetFromBundle.cs
//
// This is an example script on how you could load assets from a bundle
// and download it. Be sure to build the asset bundles before
// you use this script.
//
// The MIT License (MIT)
//
// Copyright (c) 2013 Niklas Borglund
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to Deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using System.Collections;

namespace FrameWork.Util.Downloader
{

    public class LoadAssetFromBundle : MonoBehaviour
    {

#if UNITY_EDITOR
        public string baseURL = "file://";
#else
	public string baseURL = "ONLINE_URL_HERE";
#endif

        private Object loadedAsset;
        private bool isDone = false;
        private bool downloadStarted = false;
        private string assetName;
        private string bundleName;
        private int version;
        private AssetBundle thisAssetBundle;
        private AssetBundleManager assetManager;
        private float downloadProgess = 0.0f;

        // Add new var for non prefab object
        // i want to load also non prefab object but it cannot instactiate
        // so i add new var before enquue i check that
        private bool instantiateWhenReady = false;
        private bool loadFromCache = true;


        public float DownloadProgress
        {
            get
            {
                return downloadProgess;
            }
        }

        /// <summary>
        /// Gets the name of the asset.
        /// </summary>
        /// <value>
        /// The name of the asset.
        /// </value>
        public string AssetName
        {
            get
            {
                return assetName;
            }
        }
        /// <summary>
        /// Gets the name of the asset bundle.
        /// </summary>
        /// <value>
        /// The name of the asset bundle.
        /// </value>
        public string AssetBundleName
        {
            get
            {
                return bundleName;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this download has started.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has download started; otherwise, <c>false</c>.
        /// </value>
        public bool HasDownloadStarted
        {
            get
            {
                return downloadStarted;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the download is done
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is download done; otherwise, <c>false</c>.
        /// </value>
        public bool IsDownloadDone
        {
            get
            {
                return isDone;
            }
        }
        /// <summary>
        /// Gets the downloadedloaded asset from the downloaded AssetBundle(uninstantiated)
        /// </summary>
        /// <value>
        /// The loaded asset.
        /// </value>
        public Object GetDownloadedAsset
        {
            get
            {
                return loadedAsset;
            }
        }



        /// <summary>
        /// Get Flag wheather instantiate or not.
        /// </summary>
        /// <value>
        /// yes or no, when asset loaded.
        /// </value>
        public bool InstantiateWhenReady
        {
            get
            {
                return this.instantiateWhenReady;
            }
        }

        /// <summary>
        /// Queues the bundle download. Use DownloadAsset to initiate the download
        /// </summary>
        /// <param name='asset'>
        /// Asset name to be loaded from the bundle
        /// </param>
        /// <param name='bundleName'>
        /// the name of the bundle
        /// </param>
        /// <param name='version'>
        /// Version.
        /// </param>
        public void QueueBundleDownload(string asset, string bundleName, int version, bool instantiateWhenReady = false, bool loadFromCache = true)
        {
            //#if UNITY_EDITOR
            //        //Get the base URL to the folder where the asset bundles are
            //        baseURL += Application.dataPath + PlayerPrefs.GetString("cws_exportFolder");
            //#endif

            downloadStarted = false;
            this.assetName = asset;
            this.bundleName = bundleName;
            this.version = version;
            this.instantiateWhenReady = instantiateWhenReady;
            this.loadFromCache = loadFromCache;
        }

        /// <summary>
        /// Initiates the download of the asset bundle. Only works if properties have been set with QueueBundleDownload first.
        /// </summary>
        public void DownloadAsset()
        {
            assetManager = AssetBundleManager.Instance;
            //Check from in the assetBundleManager if this bundle is already downloaded.
            AssetBundleContainer thisBundle = assetManager.GetAssetBundle(bundleName);

            if (thisBundle == null)
            {
                //if not, download it
                //없으면 다운로드
                StartCoroutine(DownloadAssetBundle(assetName, bundleName, version));
            }
            else
            {
                //if it is, just load the asset directly
                //있으면 바로 로드한다
#if UNITY_5
                // 5버전이후로 Load라는 메소드는 정상 작동하지 않아 수정하였으나 이 소스코드가 정상작동하는지는 확인하지 않았다.
                if (null != thisBundle.ThisAssetBundle)
                {
                    loadedAsset = thisBundle.ThisAssetBundle.LoadAsset(assetName, typeof(GameObject));
                }

#else
                loadedAsset = thisBundle.ThisAssetBundle.Load(assetName, typeof(GameObject));
#endif

                isDone = true;
            }
        }

        private IEnumerator DownloadAssetBundle(string asset, string bundleName, int version)
        {
            loadedAsset = null;

            // Wait for the Caching system to be ready
            // 캐싱이 가능해질 때 까지 대기
            while (!Caching.ready)
            {
                yield return null;
            }



            string url = baseURL + bundleName;


            downloadStarted = true;

            // 다운받는다
            using (WWW www = WWW.LoadFromCacheOrDownload(url, version))
            {
                while (!www.isDone)
                {
                    //Debug.Log(www.progress * 100.0f);
                    downloadProgess = www.progress;
                    yield return null;
                }

                if (www.error != null)
                {
                    throw new System.Exception("AssetBundle - WWW download:" + www.error);
                }
                thisAssetBundle = www.assetBundle;

                //다운받은 에셋을 메모리에 로드한다
                if (loadFromCache)
                {

#if UNITY_5
                    // 5버전이후로 Load라는 메소드는 정상 작동하지 않아 수정하였으나 이 소스코드가 정상작동하는지는 확인하지 않았다.
                    if (null != thisAssetBundle)
                    {
                        loadedAsset = thisAssetBundle.LoadAsset(assetName, typeof(GameObject));
                    }

                    //yield break;
#else
                loadedAsset = thisAssetBundle.Load(assetName, typeof(GameObject));
#endif
                }

                www.Dispose();

                isDone = true;
            }


            downloadStarted = false;
        }

        /// <summary>
        /// Instantiates the asset and returns it. 
        /// </summary>
        /// <returns>
        /// The asset.
        /// </returns>
        public GameObject InstantiateAsset()
        {
            if (isDone)
            {
                if (instantiateWhenReady && null != loadedAsset)
                {
                    GameObject newAsset = Instantiate(loadedAsset) as GameObject;
                    assetManager.AddBundle(bundleName, thisAssetBundle, newAsset);

                    return newAsset;

                }
                else
                {
                    assetManager.AddBundleForNonIntantiatable(bundleName, thisAssetBundle);

                    Debug.LogWarning("Asset can not instantiatable or user don't want to instantiate!");
                    return null;
                }



            }
            else
            {
                Debug.LogError("Asset is not downloaded!");
                return null;
            }
        }
    }
}