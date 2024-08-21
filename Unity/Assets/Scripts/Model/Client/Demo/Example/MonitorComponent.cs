namespace ET.Client
{
    [ComponentOf(typeof(Computer))]
    public class MonitorComponent : Entity, IAwake<int>, IDestroy
    {
        public int Brightness;
    }   
}