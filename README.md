# Unity Chemical Engine

This is SJTU Game Programming final based on Unity & VRTK.

### 6.16更新 - API

- 查气密性：
  - ironStandComplex.tube.faker用于空气热胀冷缩的虚假反应物，在加热时产生气体
  - ironStandComplex.pipe&plug group是导管组，其中`PipeTrigger.cs`脚本中
    - `bool isGood`表示这个导管是否漏气
    - `bool doPresentation`勾选后会在连接集气瓶时直接触发集气瓶的收集函数
  - 集气瓶可以移动，当瓶口与导管接触时，导管才会触发集气瓶的集气函数`StartCollecting()`，所以此时应该将集气瓶移开。
- 装药品：
  - 勺子去药瓶中右键可拾取药品，再在试管中右键放置药品，此时会tube.powder.`SetActive(true)`，此模型用于挂载高锰酸钾化学反应脚本
- 预热：
  - 酒精灯只能用左手拿。火焰与试管中的三个碰撞体需各自连续接触1s以上才能成功。通过调用`HeatingCheckpoint.CheckHeating()`获取预热是否成功。
- 试管炸裂：
  - ironStandComplex.tubeBreak - `TubeExplode.cs`
    - `Explode()`触发爆炸
    - `bool doPresentation`勾选后会在场景运行1s后试管爆炸
- 集气：
  - 当导管与集气瓶口接触时，设置导管`PipeTrigger.airComing = true`，则集气瓶中液面开始下降。

### 6.14 更新

* 因为桌子放不下了，所以器材在场景中分为equipments-1(之前的器材), equipments-2(高锰酸钾器材)

* 加入温度计及脚本Thermometer

* 加入压强反应器材及脚本PressureContainer，压强通过`UCE_Global.env_pressure`获取

* 加入高锰酸钾反应器材，高锰酸钾器材支持以下操作：

  * 移动：酒精灯、试管、集气瓶、棉花
  * 安装：在试管中装棉花、导管，在集气瓶上盖盖玻片
  * Note: 碰撞体有点多，移动物体的时候可能会把其他东西挤飞

* **酒精灯、试管因为碰撞体冲突的原因，只能用左手拾取**

* 改变材质颜色Tip：可以不用多个透明模型进行叠加，可以在脚本中改变MeshRenderer属性

  ```c#
  transform.GetComponent<MeshRenderer>().material.color = new Color();
  ```

* 集气瓶

  * 导管伸入时，触发Bottle.cs中的StartCollecting函数，开始收集气体，逐渐缩小液体模型
  * 集气瓶收集完成后，可以正放在桌上紫色区域（只有编辑模式下可以看到）

* 药品

  * 勺子可以在药瓶中右键拾取药，并在试管中右键放置
