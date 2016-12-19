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
        //다운로드할 에셋번들에 포함된 스크립트의 목록
        private List<LoadAssetFromBundle> assetsToLoad = new List<LoadAssetFromBundle>();

        //Keep track if the current downloadable bundle is downloaded(Plain copy from LoadBundlesScene.cs)
        //번들을 하나씩 다운받고 있는지 체크하기 위한 플래그
        private bool isDownloaded = true;

        private string baseURL;//(Plain copy from LoadBundlesScene.cs)
        private string filePrefix = "file://"; // I use this prefix to show that i'm currently loading the bundle from my
        //local computer, if i were to load it from the web - this wouldn't be necessary.
        //컴퓨터로부터 로드할때. 만약 웹에서 로드한다면 이 변수는 굳이 사용할 필요는 없다

        void Start()
        {
            //This command ONLY works if you run in the editor, because i'm just getting whatever value I got stored in
            // the AssetBundleCreator as export folder.
            //이 커멘드는 에디터에서만 작동한다, 왜냐하면 어떤 값이든 AssetBundleCreator에있는 추출폴더에 저장할 것이기 때문이다

            //If I would want to load this from let's say an iPhone, 
            //이를테면 아이폰에서 이것을 작동하고자 한다면

            // I would have to store them in a specific folder on the phone(Which wouldn't make any sense because I could just use Resources.Load instead.
            // 반드시 특정 폴더에 저장해야 한다(근데 이건 생각할 필요도 없는듯 하다, 왜냐하면 그냥 Resource.Load 메소드를 사용하면 되기 때문이다)

            // So the option on testing on the iOS device itself would either be from the editor like this, 
            // 따라서 iOS기기에서 테스트할 수 있는 조건은 에디터와 같이 세팅하거나
            // OR store them online on a server and set the baseURL to its http address.
            // 그것을 온라인 서버에 저장해서 그 웹주소를 사용하는 것이다
            baseURL = filePrefix + Application.dataPath + PlayerPrefs.GetString("cws_exportFolder");

            //So.. on to the loading of the bundles.
            // 번들을 로드하자
            //When loading them this way, I put each "loading command" in a list
            //아래와 같은 방식으로 로딩할때, 매 로딩 커멘드를 목록에 넣어둘 것이다
            // That list is being checked and maintained in the Update() method.
            //그 목록은 Update함수에서 관리할 것이다
            //Whenever a download is complete, it instantiates it as a gameobject
            //다운로드가 끝나면 생성을 한다
            // and proceeds to the next bundle to download.
            // 그리고 그다음 작업을 진행한다

            //To do that, I create a new LoadAssetFromBundle component
            // 앞서이야기한 것을 하기위해 LoadAssetFromBundle 스크립트를 추가한다
            LoadAssetFromBundle myAssetToLoad = this.gameObject.AddComponent<LoadAssetFromBundle>();

            //I set the asset name, the name of the bundle, and the current version of the bundle
            //에셋의 이름과 번들이름과 버전을 기록한다
            myAssetToLoad.QueueBundleDownload("MY_ASSET_NAME", "MY_BUNDLE_NAME.unity3d", 1);

            //Then, I set the URL from where it should download the bundle
            //with the value I set in the beginning of Start()
            //에셋을 다운 받을 주소록 기입한다
            myAssetToLoad.baseURL = baseURL;

            //To start the download, I add them to the "things to download list"
            // and it will start the download in the Update() method.
            //다운로드를 시작할 때에 다운받을 목록을 작성한다
            //그뒤 update함수에서 다운로드를 시작한다
            assetsToLoad.Add(myAssetToLoad);

            //To load more than on asset this way, just create another LoadAssetFromBundle component
            // and make sure to add it to the assetsToLoad list.
            //추가로 다운을 받고자 하면 LoadAssetFromBundle 스크립트를 추가하고
            //목록에 넣으면 끝!

        }

        void Update() // It takes care of the loading and instantiation of each one. // 로딩하고 생성할때 조심할 것
        {
            //... IF you are loading AND instantiating prefabs, you can just copy this function from the LoadBundlesScene.cs
            // HOWEVER, If it's a plain asset(Texture, Audio, TextAsset... etc),
            // you need to load, you'll need to modify it a little, so that it doesn't instantiate the asset.
            // 로딩하고 생성할 때에 LaodBundlesScene에서 이 함수를 가져다 써도 된다
            // 만약 다운한 에셋을 다운받고하 하는 경우는
            // 로드해야 한다, 조금수정해서 써야 된다. 왜냐면 그것들을 생성할수 없을테니까 ㅋ
        }
    }

}