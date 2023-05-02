namespace CodeBase.Logic
{
    public interface IGetDamage
    {
        bool IsDead();

        void GetDamage(int damage);
    }
}