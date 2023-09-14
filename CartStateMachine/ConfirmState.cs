

namespace CartStateMachine
{
    internal class ConfirmState : ICartState
    {
        public bool errorStatus {get; private set;}
        public string? errorMessage { get; private set; }

        private readonly Dictionary<string, int> _cartItems;

        private int _paymentAmount;
        private readonly int _totalPrice;

        public ConfirmState(Dictionary<string, int> cartItems, Dictionary<string, int> storeRates)
        {
            errorStatus = false;
            _cartItems = cartItems;
            _paymentAmount = 0;
            _totalPrice = 0;

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
            _paymentAmount += amount;

            if (_paymentAmount < _totalPrice)
            {
                SetError("Insufficient amount for purchase");
            }
            else
            {
                // Empty cart after payment
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
