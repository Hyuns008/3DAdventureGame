using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildA : Parent //�θ� ��ӹ��� ����鵵 ���� ��ӹ���
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Test()
    {
        //�̷��� ����ϸ� �����ũ���� ������ ���� ��
    }

    // Start is called before the first frame update
    protected override void  Start() //�������̵�, �����ε� => ���������� ����
    {
        base.Start();
    }

    public override void Show()
    {
        Debug.Log("A��ũ��Ʈ");
    }
}
