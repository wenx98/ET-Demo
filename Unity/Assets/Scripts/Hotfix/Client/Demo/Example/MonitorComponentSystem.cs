namespace ET.Client
{
    [EntitySystemOf(typeof(MonitorComponent))]
    [FriendOfAttribute(typeof(ET.Client.MonitorComponent))]
    public static partial class MonitorComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.MonitorComponent self, int args2)
        {
            Log.Debug("MonitorComponent Awake");

            self.Brightness = args2;
        }
        [EntitySystem]
        private static void Destroy(this ET.Client.MonitorComponent self)
        {
            Log.Debug("MonitorComponent Destroy");
        }

        public static void ChangeBrightness(this MonitorComponent self, int value)
        {
            self.Brightness = value;
        }
    }   
}