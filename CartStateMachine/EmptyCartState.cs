namespace CartStateMachine
{
    public class EmptyCartState : ICartState
    {
        public bool errorStatus { get; private set; }

        public string? errorMessage { get; private set; }

        private readonly Dictionary<string, int> _cartItems;
        private readonly Dictionary<string, int> _storeItems;

        public Dictionary<string , int> GetCartItems() => _cartItems;
        public Dictionary<string , int> GetStoreItems() => _storeItems;
        public Dictionary<string , int> GetStoreRates() => new();

        public EmptyCartState(ICartState context)
        {
            errorStatus = false;
            _storeItems = context.GetStoreItems();
            _cartItems = context.GetCartItems();
        }

        private void SetError(string error) 
        {
            errorStatus = true;
            errorMessage = error;
        }
        private void UnsetError()
        {
            errorStatus = false;
            errorMessage = null;
        }

        public void AddItem(string itemName, int quantity)
        {
            if (_storeItems.ContainsKey(itemName)) 
            {
                if (_storeItems[itemName] < quantity) 
                {
                    SetError($"{quantity} of {itemName} not available");
                }
                else
                {
                    UnsetError();
                    _cartItems[itemName] = quantity;
                }
            }
            else
            {
                SetError($"{itemName} not available");
            }
        }

        public void Confirm()
        {
            SetError("Nothing in cart to confirm");
        }

        public void PerformPayment(int amount)
        {
            SetError("No confirmation");
        }

        public void RemoveItem(string itemName)
        {
            SetError("Nothing in cart to remove");
        }
    }
}
