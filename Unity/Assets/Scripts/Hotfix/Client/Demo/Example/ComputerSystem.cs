namespace ET.Client
{
    [EntitySystemOf(typeof(Computer))]
    public static partial class ComputerSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Client.Computer self)
        {
            Log.Debug("Computer Awake");
        }
        [EntitySystem]
        private static void Update(this ET.Client.Computer self)
        {
            Log.Debug("Computer Update");
        }
        [EntitySystem]
        private static void Destroy(this ET.Client.Computer self)
        {
            Log.Debug("Computer Destroy");
        }

        public static void Open(this Computer self)
        {
            Log.Debug("Computer Open!");
        }
    }   
}