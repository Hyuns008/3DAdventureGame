using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    [Header("���� �̵��� ��")]
    [SerializeField] private string senenName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SceneManager.LoadSceneAsync(senenName);
        }
    }
}
