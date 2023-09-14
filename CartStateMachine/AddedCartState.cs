

namespace CartStateMachine
{
    internal class AddedCartState : ICartState
    {
        public bool errorStatus { get; private set; } = false;
        public string? errorMessage { get; private set; }

        private readonly Dictionary<string, int> _cartItems;
        private readonly Dictionary<string, int> _storeItems;

        public Dictionary<string , int> GetCartItems() => _cartItems;
        public Dictionary<string , int> GetStoreItems() => _storeItems;
        public Dictionary<string , int> GetStoreRates() => new();


        public AddedCartState(ICartState context)
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
                int sumQuantity;
                if (_cartItems.ContainsKey(itemName))
                {
                    sumQuantity = quantity + _cartItems[itemName];
                }
                else
                {
                    sumQuantity = quantity;
                }

                if (_storeItems[itemName] < sumQuantity)
                {
                    SetError($"{sumQuantity} of {itemName} not available");
                }
                else
                {
                    _cartItems[itemName] = sumQuantity;
                    UnsetError();
                }
            }
            else
            {
                SetError($"{itemName} not available");
            }
        }

        public void Confirm()
        {
            UnsetError();
        }

        public void PerformPayment( int amount )
        {
            SetError("No confirmation");
        }

        public void RemoveItem( string itemName )
        {
            if (_cartItems.ContainsKey(itemName))
            {
                _cartItems.Remove(itemName);
                UnsetError();
            }
            else
            {
                SetError($"Item {itemName} not in cart.");
            }

        }
    }
}
