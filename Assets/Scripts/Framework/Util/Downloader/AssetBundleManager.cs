//
//  AssetBundleManager.cs
//
// This class keeps track of all the downloaded asset bundles. 
// It contains functions to add, destroy and unload a bundle
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

    /// <summary>
    /// Asset bundle container class that contains an asset bundle and a list of all the objects that's using it
    /// �� �����̳� Ŭ������ �ϳ��� ���¹���� �ϳ��� ����� ������Ʈ ����� ����ִ� 
    /// </summary>
    public class AssetBundleContainer
    {
        private AssetBundle thisAssetBundle;
        private string bundleName; // used for more readable debug messages // ����뿡 ����� ��
        private List<GameObject> objectList = new List<GameObject>();

        private bool autoUnload = true;


        public bool AutoUnload
        {
            get
            {
                return autoUnload;
            }
            set
            {
                autoUnload = value;
            }
        }

        /// <summary>
        /// Gets or sets the this asset bundle.
        /// </summary>
        public AssetBundle ThisAssetBundle
        {
            get
            {
                return thisAssetBundle;
            }
            set
            {
                thisAssetBundle = value;
            }
        }

        /// <summary>
        /// A list with all the object references that uses this assetbundle
        /// ���¹��鿡 ���Ե� ������Ʈ ���
        /// </summary>
        /// <value>
        /// The object list.
        /// </value>
        public List<GameObject> ObjectList
        {
            get
            {
                return objectList;
            }
        }

        /// <summary>
        /// Determines whether the list with references to this assetbundle is empty.
        /// ���¹��鿡 �ִ� ����� ������� Ȯ��
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is list empty; otherwise, <c>false</c>.
        /// </returns>
        public bool IsListEmpty()
        {
            if (objectList.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the name of the bundle.
        /// </summary>
        /// <value>
        /// The name of the bundle.
        /// </value>
        public string BundleName
        {
            get
            {
                return bundleName;
            }
            set
            {
                bundleName = value;
            }
        }

        /// <summary>
        /// Clear all objects that are null and not used by the AssetBundle anymore
        /// ��� �����ϰ� ���̻� ������� �ʴ´�
        /// </summary>
        /// <returns></returns>
        public void ClearEmptyObjects()
        {
            for (int i = (objectList.Count - 1); i >= 0; i--)
            {
                //loop through the list until a null object is found and delete it.
                if (objectList[i] == null)
                {
                    objectList.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Unloads the assetBundle
        /// ���¹����� ��ε��Ѵ�
        /// </summary>
        public void Unload()
        {

            if (null != thisAssetBundle)
            {
                Debug.Log("Objects that holds a reference to " + bundleName + ": " + objectList.Count); //This should always show zero
                Debug.Log("Unloading AssetBundle(true):" + bundleName);
                thisAssetBundle.Unload(true);

            }
        }
    }

    public class AssetBundleManager : MonoBehaviour
    {
        #region Singleton
        private static AssetBundleManager instance = null;
        public static AssetBundleManager Instance
        {
            get
            {
                if (instance == null) // if the static instance is null, then create an instance of the manager
                {
                    Debug.Log("Creating an AssetBundle manager instance");
                    GameObject go = new GameObject();
                    instance = go.AddComponent<AssetBundleManager>();
                    go.name = "AssetBundleManager";

                    DontDestroyOnLoad(go);
                }

                return instance;
            }
        }
        #endregion

        private Dictionary<string, AssetBundleContainer> assetBundles = new Dictionary<string, AssetBundleContainer>();

        void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }


            //Check for unused AssetBundles every 5 seconds
            //������� �ʴ� ���� ������ �ִ��� 5�ʸ��� üũ�Ѵ�
            InvokeRepeating("CheckForUnusedBundles", 5, 5);
        }

        //Remove and Unload not used asset bundles every 5 seconds(Invoked in Start())
        //������� �ʴ� ���¹����� �����Ѵ�
        void CheckForUnusedBundles()
        {
            if (assetBundles.Count > 0)
            {
                List<string> keysToRemove = new List<string>();
                foreach (KeyValuePair<string, AssetBundleContainer> pair in assetBundles)
                {
                    if (false == pair.Value.AutoUnload)
                        continue;
                                        
                    pair.Value.ClearEmptyObjects();
                    if (pair.Value.IsListEmpty())
                    {
                        //Unload the AssetBundle
                        //���¹����� ��ε��Ѵ�
                        pair.Value.Unload();
                        //Add the key to a list for removal
                        //������ ������ Ű�� ������Ű��Ͽ� �߰��Ѵ�
                        keysToRemove.Add(pair.Key);
                    }
                }

                //Delete all the objects in the dicationary with the specified key
                //������ Ű ��Ͽ��ִ� ���¹����� �����Ѵ�
                foreach (string key in keysToRemove)
                {
                    assetBundles.Remove(key);
                }
            }
        }

        public bool ContainsKey(string bundleName)
        {
            return assetBundles.ContainsKey(bundleName);
        }

        /// <summary>
        /// Adds the bundle for removal management, if no gameobjects are using the assetbundle it will be
        /// removed automatically(if you use this method for all objects created from asset bundles)
        /// ���� ������ ���� ������ �߰��ϴ� ��쿡 � ���ӿ�����Ʈ�� ���¹����� ������� �ʴ´�. �װ��� �ڵ����� ���ŵȴ�
        /// (���� �� �޼ҵ带 ���¹���κ��� ������ ��� ������Ʈ���� ����� ���)
        /// </summary>
        public void AddBundle(string bundleName, AssetBundle assetBundle, GameObject instantiatedObject)
        {
            //Check if the assetbundle already has a container in the dictionary
            //������ �̹� ���¹����� ���ԵǾ��ִ��� üũ�Ѵ�
            if (!assetBundles.ContainsKey(bundleName))
            {
                //Create a new container and store the referenced game object
                //���ο� �������� �߰��Ѵ�
                AssetBundleContainer bundleContainer = new AssetBundleContainer();
                bundleContainer.ThisAssetBundle = assetBundle;
                bundleContainer.ObjectList.Add(instantiatedObject);
                bundleContainer.BundleName = bundleName;
                assetBundles.Add(bundleName, bundleContainer);
            }
            else
            {
                //if the key exists, get the container and add the referenced object to its list.
                //�̹� Ű ���� �ִ� ���, �����̳ʿ� ������Ʈ�� �ּҸ� �����Ѵ�
                AssetBundleContainer bundleContainer = null;
                assetBundles.TryGetValue(bundleName, out bundleContainer);

                if (bundleContainer != null)
                {
                    bundleContainer.ObjectList.Add(instantiatedObject);
                }
                else
                {
                    Debug.LogError("AssetBundleManager.cs: Couldn't get the container for assetbundle: " + bundleName + ". " +
                        "Removal Management for object:" + instantiatedObject.name + " will not work");
                }
            }
        }


        /// <summary>
        /// Adds the bundle for non prefabs
        /// </summary>
        public void AddBundleForNonIntantiatable(string bundleName, AssetBundle assetBundle)
        {
            //Check if the assetbundle already has a container in the dictionary
            //������ �̹� ���¹����� ���ԵǾ��ִ��� üũ�Ѵ�
            if (!assetBundles.ContainsKey(bundleName))
            {
                //Create a new container and store the referenced game object
                //���ο� �������� �߰��Ѵ�
                AssetBundleContainer bundleContainer = new AssetBundleContainer();
                bundleContainer.ThisAssetBundle = assetBundle;
                bundleContainer.BundleName = bundleName;

                // Make Container is not unload auto, becase it is possible to contain null object list
                bundleContainer.AutoUnload = false;


                assetBundles.Add(bundleName, bundleContainer);
            }
            else
            {
                //if the key exists, get the container and add the referenced object to its list.
                //�̹� Ű ���� �ִ� ���, �����̳ʿ� ������Ʈ�� �ּҸ� �����Ѵ�
                AssetBundleContainer bundleContainer = null;
                assetBundles.TryGetValue(bundleName, out bundleContainer);


                if (assetBundles != null)
                {
                    // Make Container is not unload auto, becase it is possible to contain null object list
                    bundleContainer.AutoUnload = false;
                }

            }
        }

        /// <summary>
        /// Gets the asset bundle for the specified key.
        /// </summary>
        /// <returns>
        /// The asset bundle.
        /// </returns>
        /// <param name='bundleName'>
        /// Bundle name key.
        /// </param>
        public AssetBundleContainer GetAssetBundle(string bundleName)
        {
            AssetBundleContainer thisBundle = null;
            assetBundles.TryGetValue(bundleName, out thisBundle);

            return thisBundle;
        }

        /// <summary>
        /// Destroys and unloads an asset bundle and all its referenced objects with
        /// the specified key.
        /// ������ Ű���� �ش��ϴ� ������ ��ε��ϰ� �����Ѵ�
        /// </summary>
        /// <param name='bundleName'>
        /// Bundle name.
        /// </param>
        public void DestroyAssetBundle(string bundleName)
        {
            AssetBundleContainer thisBundle = null;
            assetBundles.TryGetValue(bundleName, out thisBundle);
            if (thisBundle != null)
            {
                //Destroy all the game objects that are referencing to this bundle
                foreach (GameObject obj in thisBundle.ObjectList)
                {
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }
                thisBundle.ObjectList.Clear();
                thisBundle.Unload();
                assetBundles.Remove(bundleName);
            }
        }


        public void DestroyObjectInAssetBundle(string bundleName, GameObject objRemove)
        {
            AssetBundleContainer thisBundle = null;
            assetBundles.TryGetValue(bundleName, out thisBundle);
            if (thisBundle != null)
            {
                //Destroy all the game objects that are referencing to this bundle
                foreach (GameObject obj in thisBundle.ObjectList)
                {
                    if (obj == objRemove)
                    {
                        thisBundle.ObjectList.Remove(obj);
                        Destroy(obj);
                    }
                }

                if (thisBundle.IsListEmpty() && thisBundle.AutoUnload)
                {
                    thisBundle.Unload();
                    assetBundles.Remove(bundleName);
                }


            }
        }

        /// <summary>
        /// Destroy and unload all asset bundles at once and all of their referenced objects.
        /// </summary>
        public void DestroyAllBundles()
        {
            foreach (KeyValuePair<string, AssetBundleContainer> bundle in assetBundles)
            {
                foreach (GameObject obj in bundle.Value.ObjectList)
                {
                    //Destroy all the game objects that are referencing to this bundle
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }
                bundle.Value.ObjectList.Clear();
                bundle.Value.Unload();
            }
            assetBundles.Clear();
        }

    }
}