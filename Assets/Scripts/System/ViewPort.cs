using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPort : Singleton<ViewPort>
{
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;
    private float _middleX;
    //Ϊ���÷ɻ��ƶ�ʱ���嶼�����Ӵ��� ��Ҫ���������߾� �����ƶ�λ��
    private float _paddingX = 0.8f;
    private float _paddingY = 0.2f;

    public float Max_X => _maxX;


    private void Start()
    {
        //��ȡ�ӿڵ���ߵ� �Ҳ��Ϸ���(1,1) ����͵� ����·���(0,0)
        //ͨ������ת������������
        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f));
        Vector2 middle = Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f));
        //ͨ����������� ��� ��ҿ����ƶ���������������ֵ��Сֵ
        _minX = bottomLeft.x;
        _minY = bottomLeft.y;
        _maxX = topRight.x;
        _maxY = topRight.y;
        _middleX = middle.x;
    }

    public Vector3 PlayerMoveablePosition(Vector3 playerPosition)
    {
        Vector3 position = Vector3.zero;
        //�Ƚ���������Ƿ������ֵ��Сֵ���� �ǵĻ��ͻ��ǷŻر���
        //���������ֵ����С����Сֵʱ ��ֱ�Ӹ�ֵ���ֵ������Сֵ
        //���ӱ߾�ʹ�ɻ����屣�����Ӵ���
        position.x = Mathf.Clamp(playerPosition.x, _minX+_paddingX, _maxX-_paddingX);
        position.y = Mathf.Clamp(playerPosition.y, _minY+_paddingY, _maxY-_paddingY);

        return position;
    }

    /// <summary>
    /// <para>����������ɵĵ��˳���λ��</para>
    /// <para>��λ��λ���ӿ�֮����Ҳ�</para>
    /// </summary>
    /// <param name="paddingX">ģ�͵�X�ᰲȫ�߾�</param>
    /// <param name="paddingY">ģ�͵�Y�ᰲȫ�߾�</param>
    /// <returns></returns>
    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = _maxX + paddingX;
        position.y = Random.Range(_minY + paddingY, _maxY - paddingY);
        return position;
    }
    /// <summary>
    /// �����Ұ�� ���˿��ƶ���λ�÷�Χ
    /// </summary>
    /// <param name="paddingX">ģ�͵�X�ᰲȫ�߾�</param>
    /// <param name="paddingY">ģ�͵�Y�ᰲȫ�߾�</param>
    /// <returns></returns>
    public Vector3 RandomRightHalfPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(_middleX, _maxX - paddingX);
        position.y = Random.Range(_minY + paddingY, _maxY - paddingY);
        return position;
    }
    //��Ҫ�����к����һ�����ƶ���Χʱ����д
    //public Vector3 RandomEnemyMovePosition();
}
