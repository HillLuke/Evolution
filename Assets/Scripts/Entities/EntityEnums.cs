namespace Assets.Scripts.Entities
{
    public enum Action
    {
        NONE,
        Eat,
        Drink,
        Mate
    }

    public enum LookFor
    {
        NONE,
        Food,
        Water,
        Mate
    }

    public enum State
    {
        NONE,
        Wander,
        GoTo,
        Do
    }
}