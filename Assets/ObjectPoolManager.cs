using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject objectPrefab; // プールするオブジェクトのプレハブ
    public int initialPoolSize = 10; // 最初から生成しておくオブジェクトの数

    private Queue<GameObject> objectPool = new Queue<GameObject>();

    void Awake() // Startより早く呼ばれるAwakeで初期化
    {
        // 初期オブジェクトを生成してプールに格納
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false); // 最初は非アクティブにしておく
            obj.transform.SetParent(this.transform); // プールマネージャーの子にする（Hierarchy整理のため）
            objectPool.Enqueue(obj); // キューに追加
        }
    }

    /// <summary>
    /// プールからオブジェクトを取得する
    /// </summary>
    public GameObject GetPooledObject()
    {
        if (objectPool.Count == 0)
        {
            // プールが空の場合、新しいオブジェクトを生成して追加（必要に応じてプールサイズを拡張）
            Debug.LogWarning("Object pool exhausted! Expanding pool size.");
            GameObject newObj = Instantiate(objectPrefab);
            newObj.SetActive(false);
            newObj.transform.SetParent(this.transform);
            objectPool.Enqueue(newObj);
        }

        GameObject objToReuse = objectPool.Dequeue();
        objToReuse.SetActive(true); // アクティブにする
        return objToReuse;
    }

    /// <summary>
    /// オブジェクトをプールに戻す
    /// </summary>
    public void ReturnPooledObject(GameObject obj)
    {
        obj.SetActive(false); // 非アクティブにする
        obj.transform.position = Vector3.zero; // 位置をリセット（任意）
        objectPool.Enqueue(obj); // キューに戻す
    }
}