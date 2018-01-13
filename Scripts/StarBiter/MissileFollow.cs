using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 导弹追踪的运动脚本

// 导弹运动轨迹首先是一个限定旋转角度的曲线运动
// 以前做过，但是以前那个问题一直没有解决，就是导弹可能绕着敌机一直转

// 解决方法：转弯角度角度的百分比值随时间逐渐变大，发射后时间越长，转弯角度越大

public class MissileFollow : MonoBehaviour 
{
    [SerializeField]
    private GameObject targetEnemy;

    [SerializeField]
    private float turnRateAcceleration = 18.0f;

    [SerializeField]
    private float turnRate = 5.0f;

    [SerializeField]
    private float laserSpeed = 20f;

    public void Update()
    {
        Follow();
    }

    private void Follow()
    {
        if (targetEnemy)
        {
            // 取得敌机方向
            Vector3 enemyPosition = targetEnemy.transform.position;
            Vector3 relativePosition = enemyPosition - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(relativePosition);

            // 算出导弹从现在方向到敌机方向
            // 按一定比例旋转后的角度
            float targetRotationAngle = targetRotation.eulerAngles.y;
            float currentRotationAngle = transform.eulerAngles.y;

            // 从当前方向到敌机方向按 turnRate 比例旋转

            // Mathf.LerpAngle 和Lerp的原理一样,当他们环绕360度确保插值正确
            currentRotationAngle = Mathf.LerpAngle
                (
                    currentRotationAngle,
                    targetRotationAngle,
                    turnRate * Time.deltaTime
                );

            Quaternion tiltedRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // 逐渐增加转弯角度的比例
            // 防止导弹进入一直转圈而无法击中敌机的状态
            turnRate += turnRateAcceleration * Time.deltaTime;

            transform.rotation = tiltedRotation;
            transform.Translate(new Vector3(0f, 0f, laserSpeed * Time.deltaTime));
        }
    }
}
