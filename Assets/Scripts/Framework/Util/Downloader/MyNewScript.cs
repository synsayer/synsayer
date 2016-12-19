// MyNewScript.cs
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

namespace FrameWork.Util.Downloader
{

    public class MyNewScript : MonoBehaviour
    {
        //Lists of all the scripts that is downloading from asset bundle(Plain copy from LoadBundlesScene.cs)
        //�ٿ�ε��� ���¹��鿡 ���Ե� ��ũ��Ʈ�� ���
        private List<LoadAssetFromBundle> assetsToLoad = new List<LoadAssetFromBundle>();

        //Keep track if the current downloadable bundle is downloaded(Plain copy from LoadBundlesScene.cs)
        //������ �ϳ��� �ٿ�ް� �ִ��� üũ�ϱ� ���� �÷���
        private bool isDownloaded = true;

        private string baseURL;//(Plain copy from LoadBundlesScene.cs)
        private string filePrefix = "file://"; // I use this prefix to show that i'm currently loading the bundle from my
        //local computer, if i were to load it from the web - this wouldn't be necessary.
        //��ǻ�ͷκ��� �ε��Ҷ�. ���� ������ �ε��Ѵٸ� �� ������ ���� ����� �ʿ�� ����

        void Start()
        {
            //This command ONLY works if you run in the editor, because i'm just getting whatever value I got stored in
            // the AssetBundleCreator as export folder.
            //�� Ŀ���� �����Ϳ����� �۵��Ѵ�, �ֳ��ϸ� � ���̵� AssetBundleCreator���ִ� ���������� ������ ���̱� �����̴�

            //If I would want to load this from let's say an iPhone, 
            //�̸��׸� ���������� �̰��� �۵��ϰ��� �Ѵٸ�

            // I would have to store them in a specific folder on the phone(Which wouldn't make any sense because I could just use Resources.Load instead.
            // �ݵ�� Ư�� ������ �����ؾ� �Ѵ�(�ٵ� �̰� ������ �ʿ䵵 ���µ� �ϴ�, �ֳ��ϸ� �׳� Resource.Load �޼ҵ带 ����ϸ� �Ǳ� �����̴�)

            // So the option on testing on the iOS device itself would either be from the editor like this, 
            // ���� iOS��⿡�� �׽�Ʈ�� �� �ִ� ������ �����Ϳ� ���� �����ϰų�
            // OR store them online on a server and set the baseURL to its http address.
            // �װ��� �¶��� ������ �����ؼ� �� ���ּҸ� ����ϴ� ���̴�
            baseURL = filePrefix + Application.dataPath + PlayerPrefs.GetString("cws_exportFolder");

            //So.. on to the loading of the bundles.
            // ������ �ε�����
            //When loading them this way, I put each "loading command" in a list
            //�Ʒ��� ���� ������� �ε��Ҷ�, �� �ε� Ŀ��带 ��Ͽ� �־�� ���̴�
            // That list is being checked and maintained in the Update() method.
            //�� ����� Update�Լ����� ������ ���̴�
            //Whenever a download is complete, it instantiates it as a gameobject
            //�ٿ�ε尡 ������ ������ �Ѵ�
            // and proceeds to the next bundle to download.
            // �׸��� �״��� �۾��� �����Ѵ�

            //To do that, I create a new LoadAssetFromBundle component
            // �ռ��̾߱��� ���� �ϱ����� LoadAssetFromBundle ��ũ��Ʈ�� �߰��Ѵ�
            LoadAssetFromBundle myAssetToLoad = this.gameObject.AddComponent<LoadAssetFromBundle>();

            //I set the asset name, the name of the bundle, and the current version of the bundle
            //������ �̸��� �����̸��� ������ ����Ѵ�
            myAssetToLoad.QueueBundleDownload("MY_ASSET_NAME", "MY_BUNDLE_NAME.unity3d", 1);

            //Then, I set the URL from where it should download the bundle
            //with the value I set in the beginning of Start()
            //������ �ٿ� ���� �ּҷ� �����Ѵ�
            myAssetToLoad.baseURL = baseURL;

            //To start the download, I add them to the "things to download list"
            // and it will start the download in the Update() method.
            //�ٿ�ε带 ������ ���� �ٿ���� ����� �ۼ��Ѵ�
            //�׵� update�Լ����� �ٿ�ε带 �����Ѵ�
            assetsToLoad.Add(myAssetToLoad);

            //To load more than on asset this way, just create another LoadAssetFromBundle component
            // and make sure to add it to the assetsToLoad list.
            //�߰��� �ٿ��� �ް��� �ϸ� LoadAssetFromBundle ��ũ��Ʈ�� �߰��ϰ�
            //��Ͽ� ������ ��!

        }

        void Update() // It takes care of the loading and instantiation of each one. // �ε��ϰ� �����Ҷ� ������ ��
        {
            //... IF you are loading AND instantiating prefabs, you can just copy this function from the LoadBundlesScene.cs
            // HOWEVER, If it's a plain asset(Texture, Audio, TextAsset... etc),
            // you need to load, you'll need to modify it a little, so that it doesn't instantiate the asset.
            // �ε��ϰ� ������ ���� LaodBundlesScene���� �� �Լ��� ������ �ᵵ �ȴ�
            // ���� �ٿ��� ������ �ٿ�ް��� �ϴ� ����
            // �ε��ؾ� �Ѵ�, ���ݼ����ؼ� ��� �ȴ�. �ֳĸ� �װ͵��� �����Ҽ� �����״ϱ� ��
        }
    }

}