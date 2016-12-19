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
        //���� ����κ��� �ٿ�ε� �ؾ��ϴ� ��� ��ũ��Ʈ��
        private List<LoadAssetFromBundle> assetsToLoad = new List<LoadAssetFromBundle>();

        //Since I might want to asset from the same bundle, I'll queue the downloads
        //i.e not download simultaneously so that i don't download the same bundle twice.
        //���� ����κ��� ������ ������ ���ϱ� ������ ť�� �ٿ�ε��� ���� �־�д�
        //���ÿ� �ٿ�ε� ���� �ʱ� ������ ���� ������ �ߺ��ؼ� �ٿ���� �ʴ´�
        private bool isDownloaded = true;

        //The url to the AssetBundles folder 
        private string baseURL;
        private string filePrefix = "file://";


        private bool local = false;

        void Start()
        {
            //be sure to use the same location you specified as Export Location in the Bundle Creator
            // or if this is in the web player, direct the path to your online folder where the bundles are uploaded
            //BundleCreator���� �����ϴ� ��θ� Ȯ���ض�
            //���� �����带 �ϴ°��̸� �ʰ� ���ε��� �¶��� ��θ� �����ض�
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
            //������ �ε��Ѵ�
            if (bFileExist)
            {
                //Load the two logo files stored in the bundle
                //������ ���ԵǾ��ִ� �ΰ��� �ΰ� �ε��Ѵ�.
                LoadAssetFromBundle cryWolfLogo = this.gameObject.AddComponent<LoadAssetFromBundle>();
                cryWolfLogo.QueueBundleDownload("pre_cryWolfLogo", "logobundle_02.unity3d", 1);
                cryWolfLogo.baseURL = baseURL;

                LoadAssetFromBundle cryWolfLogoURL = this.gameObject.AddComponent<LoadAssetFromBundle>();
                cryWolfLogoURL.QueueBundleDownload("pre_cryWolfLogo_url", "logobundle_02.unity3d", 1);
                cryWolfLogoURL.baseURL = baseURL;

                //Add them to the download list
                //�ε��� �͵��� �ٿ�ε� ����Ʈ�� �ִ´�
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
                        //�ٿ�ε尡 �Ϸ� �Ǹ� ���鿡 �ִ� ������ �����Ѵ�
                        asset.InstantiateAsset();
                        //Remove the asset from the loading list
                        //�ٿ�ε� ��Ͽ� �ִ� ������ �����Ѵ�
                        assetsToLoad.RemoveAt(i);
                        //Destroy the LoadAssetFromBundle Script
                        //�ٿ�ε尡 �Ϸ�Ǿ����Ƿ� �ٿ�ε忡 ����� �ٿ�ε� ��ũ��Ʈ�� �����Ѵ�
                        Destroy(asset);
                        //This means an asset is downloaded, which means you can start on the next one
                        //�ϳ��� ������ �ٿ�ε� �Ϸ� �Ǿ����Ƿ� ���� ���� �ٿ� ���� �غ� �Ǿ���
                        isDownloaded = true;
                    }
                }

                if (isDownloaded) //The download is complete //������ �ϳ� �ٿ�ε�Ǿ���
                {
                    //Start the next download
                    //���� ������ �ٿ�ε��Ѵ�
                    foreach (LoadAssetFromBundle asset in assetsToLoad)
                    {
                        if (!asset.HasDownloadStarted)
                        {
                            //Start the download
                            //�ٿ�ε带 �����Ѵ�
                            asset.DownloadAsset();

                            //set the isDownloaded to false again
                            //�ٽ� �ٿ�ε� �÷��׸� False�� �ٲ۴�
                            isDownloaded = false;

                            //break the loop
                            //������ �����
                            break;
                        }
                    }
                }
            }
            else //If there is nothing left to load, then destroy this game object //���̻� �ٿ� ���� ���� ���ٸ�, �� ������Ʈ�� �����Ѵ�
            {
                Destroy(this.gameObject);
            }
        }
    }

}