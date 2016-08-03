namespace SpaceShipFartrothu.Interfaces
{
    public interface IItem : IGameObject
    {
        int ItemHealth { get; }
        int ItemDamage { get; }
        int ItemArmor { get; }
        int ItemBulletSpeed { get; }
        int ItemShipSpeed { get; }
    }
}
