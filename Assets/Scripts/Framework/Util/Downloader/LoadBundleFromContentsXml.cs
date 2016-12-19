
//////////////////////////////////////////////////////////////////////////
// MODIFIED AND RECREATED BY PAUL SHIN.
//////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using System.Net;
using System.Xml;

namespace FrameWork.Util.Downloader
{

    public class LoadBundleFromContentsXml : FrameWork.Behaviour.CommonBehaviour
    {
        //Lists of all the scripts that is downloading from asset bundle
        //에셋 번들로부터 다운로드 해야하는 모든 스크립트들
        private List<LoadAssetFromBundle> assetsToLoad = new List<LoadAssetFromBundle>();

        //다운 받을 번들들의 파일 사이즈
        private Dictionary<string, int> dicBundleSize = new Dictionary<string, int>();
        //다운 받을 번들들의 버전
        private Dictionary<string, int> dicBundleVersion = new Dictionary<string, int>();

        //Since I might want to asset from the same bundle, I'll queue the downloads
        //i.e not download simultaneously so that i don't download the same bundle twice.
        //같은 번들로부터 제공된 에셋을 원하기 때문에 큐에 다운로드할 것을 넣어둔다
        //동시에 다운로드 하지 않기 때문에 같은 번들을 중복해서 다운받지 않는다
        private bool isDownloaded = true;

        //The url to the AssetBundles folder 

        private string filePrefix = "file://";
        private int totalSize = 0;

        private string baseURL = "";
        public bool local = false;
        public float updateInterval = 0.1f;
        public const string UPDATE_DOWNLOADER = "UPDATE_DOWNLOADER";


        public string baseUrlQa = "";
        public string baseUrlDev = "";
        public string baseUrlReal = "";

        public enum URL_TYPE
        {
            dev,
            qa,
            real
        }

        public URL_TYPE typeUrl;

        IEnumerator Start()
        {
            if (local)
            {
                baseURL = filePrefix + Application.dataPath + "/../AssetBundles/";
            }
            else if (baseURL == "")
            {
                switch (typeUrl)
                {
                    case URL_TYPE.dev:
                        baseURL = baseUrlDev;
                        break;
                    case URL_TYPE.qa:
                        baseURL = baseUrlQa;
                        break;
                    case URL_TYPE.real:
                        baseURL = baseUrlReal;
                        break;
                }


                //baseURL = "http://bubblemon.hs.llnwd.net/v1/bubblemon/qa1/AssetBundles/";
            }


            string strVersionControlXml = "";

            // Set Version Control List
            using (WWW www = new WWW(baseURL + "bundleControlFile.xml"))
            {
                yield return www;

                if (www.error != null)
                {
                    throw new System.Exception("XML - WWW download:" + www.error);
                }
                strVersionControlXml = www.text;
                Debug.Log(strVersionControlXml);

                www.Dispose();
            }

            if (strVersionControlXml != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strVersionControlXml);


                XmlNodeList bundles = xmlDoc.GetElementsByTagName("bundle");


                foreach (XmlNode bundle in bundles)
                {
                    int nVersionNumber = System.Convert.ToInt32(bundle.Attributes["VersionNumber"].InnerText);
                    string strBundleName = bundle.Attributes["BundleName"].InnerText;

                    if (strBundleName != null)
                    {
                        if (dicBundleVersion.ContainsKey(strBundleName))
                        {
                            dicBundleVersion[strBundleName] = nVersionNumber;
                        }
                        else
                        {
                            dicBundleVersion.Add(strBundleName, nVersionNumber);
                        }

                    }
                }
            }


            string strContentsXml = "";


            // Set Contents Information
            using (WWW www = new WWW(baseURL + "bundleContents.xml"))
            {
                yield return www;

                if (www.error != null)
                {
                    throw new System.Exception("XML - WWW download:" + www.error);
                }
                strContentsXml = www.text;
                Debug.Log(strContentsXml);

                www.Dispose();
            }

            if (strContentsXml != "")
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(strContentsXml);


                XmlNodeList bundles = xmlDoc.GetElementsByTagName("bundle");

                int nTotalSize = 0;


                foreach (XmlNode bundle in bundles)
                {
                    int nFileSize = System.Convert.ToInt32(bundle.Attributes["FileSize"].InnerText);
                    int nNumOfAssets = System.Convert.ToInt32(bundle.Attributes["NumberOfAssets"].InnerText);
                    string strBundleName = bundle.Attributes["BundleName"].InnerText;

                    int nVersion = 0;
                    if (dicBundleVersion.ContainsKey(strBundleName))
                    {
                        nVersion = dicBundleVersion[strBundleName];
                    }



                    if (strBundleName != null && nVersion != 0)
                    {
                        nTotalSize += nFileSize;



                        foreach (XmlNode asset in bundle.ChildNodes)
                        {
                            if (asset.Name == "asset")
                            {
                                string strAssetName = asset.Attributes["AssetName"].InnerText;

                                bool bInstantiateWhenReady = strAssetName.GetExtentionFromPath() == "prefab";


                                LoadAssetFromBundle oLoadAsset = this.gameObject.AddComponent<LoadAssetFromBundle>();
                                oLoadAsset.baseURL = baseURL;
                                oLoadAsset.QueueBundleDownload(strAssetName.GetFilenameFromPath(), strBundleName, nVersion, bInstantiateWhenReady);
                                assetsToLoad.Add(oLoadAsset);
                            }
                        }


                        if (dicBundleSize.ContainsKey(strBundleName))
                        {
                            dicBundleSize[strBundleName] = nFileSize;
                        }
                        else
                        {
                            dicBundleSize.Add(strBundleName, nFileSize);
                        }
                    }
                }


                foreach (int nSize in dicBundleSize.Values)
                {
                    totalSize += nSize;
                }


                StartCoroutine(LoadContents());
            }
        }



        IEnumerator LoadContents()
        {
            while (assetsToLoad.Count > 0)
            {
                Dictionary<string, int> dicAssetForDownload = new Dictionary<string, int>();

                string strCurrentDnBundleName = "";

                int nDnFileSize = 0;
                int nDnFileProgress = 0;

                for (int i = (assetsToLoad.Count - 1); i >= 0; i--)
                {
                    LoadAssetFromBundle asset = assetsToLoad[i];
                    if (asset.IsDownloadDone)
                    {
                        //다운로드가 완료 되면 번들에 있는 에셋을 생성한다
                        asset.InstantiateAsset();
                        //다운로드 목록에 있는 에셋을 제거한다
                        assetsToLoad.RemoveAt(i);
                        //다운로드가 완료되었으므로 다운로드에 사용한 다운로드 스크립트를 제거한다
                        Destroy(asset);
                        //하나의 에셋이 다운로드 완료 되었으므로 다음 것을 다운 받을 준비가 되었다
                        isDownloaded = true;
                    }
                    else
                    {
                        int nFileSize = dicBundleSize[asset.AssetBundleName];
                        nFileSize = (int)(nFileSize * (1.0f - asset.DownloadProgress));

                        if (dicAssetForDownload.ContainsKey(asset.AssetBundleName))
                        {
                            dicAssetForDownload[asset.AssetBundleName] = nFileSize;
                        }
                        else
                        {
                            dicAssetForDownload.Add(asset.AssetBundleName, nFileSize);
                        }


                        if (asset.HasDownloadStarted)
                        {
                            strCurrentDnBundleName = asset.AssetBundleName;
                            nDnFileSize = nFileSize;
                            nDnFileProgress = (int)(asset.DownloadProgress * 100.0f);
                        }

                    }
                }

                if (isDownloaded) //에셋이 하나 다운로드되었다
                {
                    //다음 에셋을 다운로드한다
                    foreach (LoadAssetFromBundle asset in assetsToLoad)
                    {
                        if (!asset.HasDownloadStarted)
                        {
                            //다운로드를 시작한다
                            asset.DownloadAsset();

                            //다시 다운로드 플래그를 False로 바꾼다
                            isDownloaded = false;

                            //루프를 벗어난다
                            break;
                        }
                    }
                }

                int nTotalRemainSize = 0;
                foreach (int nFilesize in dicAssetForDownload.Values)
                {
                    nTotalRemainSize += nFilesize;
                }

                //Broadcast to listener that progress changed
                DispatchEvent(UPDATE_DOWNLOADER, new FrameWork.Event.BasicEventArgs(totalSize, nTotalRemainSize, strCurrentDnBundleName, nDnFileProgress));
                Debug.Log(nTotalRemainSize);
                yield return new WaitForSeconds(updateInterval);
            }

            //Destroy(this.gameObject);
        }


        // Modified by Paul Shin.
        // If we check file exist in the web, We must use different method.
        static public bool WebFileExists(string uri)
        {
            long fileLength = -1;
            WebRequest request = HttpWebRequest.Create(uri);
            request.Method = "HEAD";
            WebResponse resp = null;
            try
            {
                resp = request.GetResponse();
            }
            catch
            {
                resp = null;
            }
            if (resp != null)
            {
                long.TryParse(resp.Headers.Get("Content-Length"), out fileLength);
            }
            return fileLength > 0;
        }

        // Modified by Paul Shin.
        // If we check file exist in the web, We must use different method.
        // In this case, below method is not used.
        public static bool UrlExists(string file)
        {
            bool exists = false;
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(file);
            request.Method = "HEAD";
            request.Timeout = 5000; // milliseconds
            request.AllowAutoRedirect = false;


            try
            {
                response = (HttpWebResponse)request.GetResponse();
                exists = response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                exists = false;
            }
            finally
            {
                // close your response.
                if (response != null)
                    response.Close();
            }
            return exists;
        }
    }
}