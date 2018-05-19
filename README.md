# Unity Chemical Engine

SJTU Game Programming final



## 基本的运行逻辑

* VR模拟器操作，用手柄碰到可交互物体时周围有黄线提示
* 左键可以拿起来，再按左键放下
* 把铜块放进烧杯之后，会触发UCE_Chemical.cs中的OnTriggerEnter脚本，调用UCE_Engine输入反应物质，获取反应效果，然后调用UCE_Animation显示动画（现在是铜块消失，烧杯里水减半）
* 之后化学反应引擎部分修改UCE_Engine.cs，动画部分修改UCE_Animation就差不多了



* 另外一方面滴管正在做，截至目前拿起来之后按右键触发Dropper.cs中脚本，切换有水/无水状态，之后改来从试管里吸取液体

