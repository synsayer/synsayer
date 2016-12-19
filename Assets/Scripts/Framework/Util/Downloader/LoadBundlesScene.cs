//
//  LoadBundlesScene.cs
//
// This is an example script on how you could load assets from a bundle
// created with the Bundle Creator. Be sure to build the asset bundles before
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
using System.Collections.Generic;
using System.IO;

using System.Net;


namespace FrameWork.Util.Downloader
{

    public class LoadBundlesScene : MonoBehaviour
    {
        //Lists of all the scripts that is downloading from asset bundle
        //에셋 번들로부터 다운로드 해야하는 모든 스크립트들
        private List<LoadAssetFromBundle> assetsToLoad = new List<LoadAssetFromBundle>();

        //Since I might want to asset from the same bundle, I'll queue the downloads
        //i.e not download simultaneously so that i don't download the same bundle twice.
        //같은 번들로부터 제공된 에셋을 원하기 때문에 큐에 다운로드할 것을 넣어둔다
        //동시에 다운로드 하지 않기 때문에 같은 번들을 중복해서 다운받지 않는다
        private bool isDownloaded = true;

        //The url to the AssetBundles folder 
        private string baseURL;
        private string filePrefix = "file://";


        private bool local = false;

        void Start()
        {
            //be sure to use the same location you specified as Export Location in the Bundle Creator
            // or if this is in the web player, direct the path to your online folder where the bundles are uploaded
            //BundleCreator에서 추출하는 경로를 확인해라
            //만약 웹빌드를 하는것이면 너가 업로드할 온라인 경로를 기입해라
            //baseURL = Application.dataPath + "/../AssetBundles/";


            if (local)
            {
                baseURL = filePrefix + Application.dataPath + "/../AssetBundles/";
            }
            else
            {
                baseURL = "http://bubblemon.hs.llnwd.net/v1/bubblemon/qa1/AssetBundles/";
            }




            bool bFileExist = false;

            if (local)
            {
                bFileExist = File.Exists(baseURL + "logobundle_01.unity3d");
            }
            else
            {
                bFileExist = WebFileExists(baseURL + "logobundle_02.unity3d");
            }


            //Load the bundles
            //번들을 로드한다
            if (bFileExist)
            {
                //Load the two logo files stored in the bundle
                //번을에 포함되어있는 두개의 로고를 로드한다.
                LoadAssetFromBundle cryWolfLogo = this.gameObject.AddComponent<LoadAssetFromBundle>();
                cryWolfLogo.QueueBundleDownload("pre_cryWolfLogo", "logobundle_02.unity3d", 1);
                cryWolfLogo.baseURL = baseURL;

                LoadAssetFromBundle cryWolfLogoURL = this.gameObject.AddComponent<LoadAssetFromBundle>();
                cryWolfLogoURL.QueueBundleDownload("pre_cryWolfLogo_url", "logobundle_02.unity3d", 1);
                cryWolfLogoURL.baseURL = baseURL;

                //Add them to the download list
                //로드할 것들을 다운로드 리스트에 넣는다
                assetsToLoad.Add(cryWolfLogo);
                assetsToLoad.Add(cryWolfLogoURL);
            }
            else
            {
                //The file does not exist, you need to build the bundle first
                Debug.LogError("Bundles are not built! Open the Bundle Creator in Assets->BundleCreator>Asset Bundle Creator to build your bundles.");
            }
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


        void Update()
        {
            if (assetsToLoad.Count > 0)
            {
                for (int i = (assetsToLoad.Count - 1); i >= 0; i--)
                {
                    LoadAssetFromBundle asset = assetsToLoad[i];
                    if (asset.IsDownloadDone)
                    {
                        //The download is done, instantiate the asset from the bundle
                        //다운로드가 완료 되면 번들에 있는 에셋을 생성한다
                        asset.InstantiateAsset();
                        //Remove the asset from the loading list
                        //다운로드 목록에 있는 에셋을 제거한다
                        assetsToLoad.RemoveAt(i);
                        //Destroy the LoadAssetFromBundle Script
                        //다운로드가 완료되었으므로 다운로드에 사용한 다운로드 스크립트를 제거한다
                        Destroy(asset);
                        //This means an asset is downloaded, which means you can start on the next one
                        //하나의 에셋이 다운로드 완료 되었으므로 다음 것을 다운 받을 준비가 되었다
                        isDownloaded = true;
                    }
                }

                if (isDownloaded) //The download is complete //에셋이 하나 다운로드되었다
                {
                    //Start the next download
                    //다음 에셋을 다운로드한다
                    foreach (LoadAssetFromBundle asset in assetsToLoad)
                    {
                        if (!asset.HasDownloadStarted)
                        {
                            //Start the download
                            //다운로드를 시작한다
                            asset.DownloadAsset();

                            //set the isDownloaded to false again
                            //다시 다운로드 플래그를 False로 바꾼다
                            isDownloaded = false;

                            //break the loop
                            //루프를 벗어난다
                            break;
                        }
                    }
                }
            }
            else //If there is nothing left to load, then destroy this game object //더이상 다운 받을 것이 없다면, 이 오브젝트를 제거한다
            {
                Destroy(this.gameObject);
            }
        }
    }

}