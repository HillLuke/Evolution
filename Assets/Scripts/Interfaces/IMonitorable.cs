namespace Assets.Scripts.Interfaces
{
    public interface IMonitorable
    {
        public string GetName();
        public string GetData();

        public void Select();

        public void DeSelect();
    }
}
