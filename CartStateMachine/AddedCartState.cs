

namespace CartStateMachine
{
    internal class AddedCartState : ICartState
    {
        public bool errorStatus { get; private set; } = false;
        public string? errorMessage { get; private set; }

        private readonly Dictionary<string, int> _cartItems;
        private readonly Dictionary<string, int> _storeItems;

        public AddedCartState(Dictionary<string, int> cartItems, Dictionary<string, int> storeItems)
        {
            errorStatus = false;
            _storeItems = storeItems;
            _cartItems = cartItems;
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
                int sumQuantity = 0;
                if (_cartItems.ContainsKey(itemName))
                {
                    sumQuantity = quantity + _cartItems[itemName];
                }
                else
                {
                    _cartItems[itemName] = quantity;
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
            throw new NotImplementedException();
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
