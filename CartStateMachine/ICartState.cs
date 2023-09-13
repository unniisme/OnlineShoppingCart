
namespace CartStateMachine
{
    public interface ICartState
    {
        bool errorStatus { get; }
        string? errorMessage { get; }

        void AddItem(string itemName, int quantity);
        void RemoveItem(string itemName);
        void Confirm();
        void PerformPayment(int amount);
    }
}
