using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour //�ڽĿ��� �̷� ��ɵ��� ������ ����
{
    [SerializeField] protected int Hp; //int �ڷ��� Hp�� �ڽĵ鵵 ������ �ְ� ��

    //public ������, private �ڽŸ�, protected �ڽĿ���

    protected virtual void Start()
    {
        Debug.Log("���� �θ��� Start�Լ��Դϴ�");
    }

    public virtual void Show()
    {
        Debug.Log("���� �θ��Դϴ�");
    }

    private void setResoution()
    {
        float targetRatio = 9f / 16f;
        float ratio = (float)Screen.width / (float)Screen.height;
        float scaleHeight = ratio / targetRatio;
        float fixedWidth = (float)Screen.width / scaleHeight;

        Screen.SetResolution((int)fixedWidth, Screen.height, true);
    }
}
