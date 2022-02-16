namespace Assets.Scripts.Interfaces
{
    public interface IMonitorable
    {
        public void DeSelect();

        public string GetData();

        public string GetName();

        public void Select();
    }
}