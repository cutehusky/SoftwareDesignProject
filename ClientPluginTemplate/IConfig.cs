namespace ClientPluginTemplate
{
    public interface IConfig
    {
        public string ID { get; }
        public Type EntryPoint { get; }
        public string APIEndPoint { set; }
    }
}
