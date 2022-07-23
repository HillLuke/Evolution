namespace Assets.Scripts.Interfaces
{
    public interface IEntity
    {
        public float Health { get; }

        public float Damage(float damage);
    }
}