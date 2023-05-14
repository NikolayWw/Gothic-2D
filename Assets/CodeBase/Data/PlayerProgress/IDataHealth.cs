using System;

namespace CodeBase.Data.PlayerProgress
{
    public interface IDataHealth
    {
        int MaxHealth { get; }
        int CurrentHealth { get; }
        Action OnChangeCurrentHealth { get; set; }
    }
}