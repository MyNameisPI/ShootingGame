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
    //为了让飞机移动时整体都会在视窗内 需要设置两个边距 限制移动位置
    private float _paddingX = 0.8f;
    private float _paddingY = 0.2f;

    public float Max_X => _maxX;


    private void Start()
    {
        //获取视口的最高点 右侧上方点(1,1) 和最低点 左侧下方点(0,0)
        //通过代码转换成世界坐标
        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f));
        Vector2 middle = Camera.main.ViewportToWorldPoint(new Vector3(.5f, .5f));
        //通过这个两个点 获得 玩家可以移动的世界坐标的最大值最小值
        _minX = bottomLeft.x;
        _minY = bottomLeft.y;
        _maxX = topRight.x;
        _maxY = topRight.y;
        _middleX = middle.x;
    }

    public Vector3 PlayerMoveablePosition(Vector3 playerPosition)
    {
        Vector3 position = Vector3.zero;
        //比较玩家坐标是否在最大值最小值区间 是的话就还是放回本身
        //当超过最大值或者小于最小值时 就直接赋值最大值或者最小值
        //增加边距使飞机整体保持在视窗内
        position.x = Mathf.Clamp(playerPosition.x, _minX+_paddingX, _maxX-_paddingX);
        position.y = Mathf.Clamp(playerPosition.y, _minY+_paddingY, _maxY-_paddingY);

        return position;
    }

    /// <summary>
    /// <para>返回随机生成的敌人出生位置</para>
    /// <para>该位置位于视口之外的右侧</para>
    /// </summary>
    /// <param name="paddingX">模型的X轴安全边距</param>
    /// <param name="paddingY">模型的Y轴安全边距</param>
    /// <returns></returns>
    public Vector3 RandomEnemySpawnPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = _maxX + paddingX;
        position.y = Random.Range(_minY + paddingY, _maxY - paddingY);
        return position;
    }
    /// <summary>
    /// 返回右半边 敌人可移动的位置范围
    /// </summary>
    /// <param name="paddingX">模型的X轴安全边距</param>
    /// <param name="paddingY">模型的Y轴安全边距</param>
    /// <returns></returns>
    public Vector3 RandomRightHalfPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;
        position.x = Random.Range(_middleX, _maxX - paddingX);
        position.y = Random.Range(_minY + paddingY, _maxY - paddingY);
        return position;
    }
    //需要敌人有和玩家一样的移动范围时在填写
    //public Vector3 RandomEnemyMovePosition();
}
