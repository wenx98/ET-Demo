// namespace ET.Client;
//
// [Event(SceneType.Demo)]
// public class TestEventStruct_Debug : AEvent<Scene, TestEventStruct>
// {
//     protected override async ETTask Run(Scene scene, TestEventStruct a)
//     {
//         await scene.GetComponent<TimerComponent>().WaitAsync(2000);
//         
//         Log.Debug(a.TestValue.ToString());
//         await ETTask.CompletedTask;
//     }
// }