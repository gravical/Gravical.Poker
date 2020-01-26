using System;

namespace Gravical.Poker.Core
{
    public interface ITable
    {
        Guid Id { get; }

        TableStatus Status { get; }

        void Initialize();
        void DealTheFlop();
        void DealTheTurn();
        void DealTheRiver();

        TableFinal ToFinal();
    }
}
