namespace SpaceShipFartrothu.Interfaces
{
    using GameObjects;

    public interface ICollidable
    {
        void ReactOnColission(IGameObject target);
    }
}
