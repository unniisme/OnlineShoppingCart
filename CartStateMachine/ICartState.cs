
namespace CartStateMachine
{
    public interface ICartState
    {
        bool errorStatus { get; }
        string? errorMessage { get; }

        Dictionary<string , int> GetCartItems();
        Dictionary<string , int> GetStoreItems();
        Dictionary<string , int> GetStoreRates();

        void AddItem(string itemName, int quantity);
        void RemoveItem(string itemName);
        void Confirm();
        void PerformPayment(int amount);
    }
}
