# Unity Chemical Engine

This is SJTU Game Programming final based on Unity & VRTK.

### 6.14 更新

* 因为桌子放不下了，所以器材在场景中分为equipments-1(之前的器材), equipments-2(高锰酸钾器材)

* 加入温度计及脚本Thermometer

* 加入压强反应器材及脚本PressureContainer，压强通过`UCE_Global.env_pressure`获取

* 加入高锰酸钾反应器材，高锰酸钾器材支持以下操作：

  * 移动：酒精灯、试管、集气瓶、棉花
  * 安装：在试管中装棉花、导管，在集气瓶上盖盖玻片
  * Note: 碰撞体有点多，移动物体的时候可能会把其他东西挤飞

* 酒精灯、试管因为碰撞体冲突的原因，**只能用左手拾取**

* 改变材质颜色Tip：可以不用多个透明模型进行叠加，可以在脚本中改变MeshRenderer属性

  ```c#
  transform.GetComponent<MeshRenderer>().material.color = new Color();
  ```

### 运行逻辑

* 电脑上可VR模拟器操作，按[alt]在控制视角和控制手柄模式间切换，控制手柄时按[tab]进行手的切换。
* 用手柄碰到可交互物体时周围有黄线提示（似乎Mac上是黑黑的，之后再解决吧），碰到化学物质会显示名称，鼠标左键可以拿起来，再按左键放下，鼠标右键触发使用。
* 把铜块放进烧杯中溶液之后，会触发UCE_Chemical.cs中的OnTriggerEnter脚本，调用UCE_Engine输入反应物质，获取反应效果，然后调用UCE_Animation显示动画（现在是铜块消失，烧杯里水减半）

* 滴管可以通过[使用]在试管中吸取溶液，并放入烧杯中，实现烧杯溶液的切换（**目前效果并不是融合，而是切换**）。
* 火柴可以通过在火柴盒右侧快速移动点燃，通过[使用]进行熄灭，靠近打开的酒精灯后可以点燃，盖上盖子可以熄灭酒精灯。

### 实现细节

* VRTK的可交互物体通过Window->VRTK->Setup Interactable Object进行配置（自己挨个挂脚本也可以），默认是手柄碰到时变色，如果想要碰到时使用边缘线提示，还要加VRTK_Outline Object Copy Highlighter脚本。

* 酒精灯盖使用了VRTK Snap Drop Zone保证了酒精灯盖只能放在酒精灯上的特定位置。还加入了Spring弹簧，使得触发Snap时能够有放下盖子的动画。
* 报错应该只有一个OpenVR Error，猜测需要安装steam上的steam VR并佩戴VR设备才能解决。
