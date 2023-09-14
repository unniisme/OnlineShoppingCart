

namespace CartStateMachine
{
    internal class ConfirmState : ICartState
    {
        public bool errorStatus {get; private set;}
        public string? errorMessage { get; private set; }

        private readonly Dictionary<string, int> _cartItems;
        private readonly Dictionary<string, int> _storeItems;

        private readonly int _totalPrice;

        public Dictionary<string , int> GetCartItems() => _cartItems;
        public Dictionary<string , int> GetStoreItems() => _storeItems;
        public Dictionary<string , int> GetStoreRates() => new();

        public ConfirmState(ICartState context)
        {
            errorStatus = false;
            _cartItems = context.GetCartItems();
            _storeItems = context.GetStoreItems();
            _paymentAmount = 0;
            _totalPrice = 0;

            Dictionary<string, int> storeRates = context.GetStoreRates();

            foreach (KeyValuePair<string, int> entry in _cartItems)
            {
                _totalPrice += storeRates[entry.Key] * entry.Value;
            }
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

        public void AddItem( string itemName , int quantity )
        {
            SetError("Currently confirming order");
        }

        public void Confirm()
        {
            SetError("Currently already confirming order");
        }

        public void PerformPayment( int amount )
        {
            if (amount < _totalPrice)
            {
                SetError("Insufficient amount for purchase");
            }
            else
            {
                foreach (KeyValuePair<string, int> entry in _cartItems)
                {
                    _storeItems[entry.Key] -= entry.Value;
                }
                _cartItems.Clear();
                UnsetError();
            }
        }

        public void RemoveItem( string itemName )
        {
            SetError("Currently confirming order");
        }
    }
}
