

namespace CartStateMachine
{
    internal class Context : ICartState
    {
        public bool errorStatus { get; private set; }
        public string? errorMessage { get; private set; }

        private readonly Dictionary<string , int> _cartItems;
        private readonly Dictionary<string , int> _storeItems;
        private readonly Dictionary<string , int> _storeRates;

        private ICartState _currState;

        private bool UpdateErrorFromState()
        {
            errorStatus = _currState.errorStatus;
            errorMessage = _currState.errorMessage;
            return errorStatus;
        }

        public Context( Dictionary<string , int> storeItems, Dictionary<string , int> storeRates)
        {
            errorStatus = false;
            _storeItems = storeItems;
            _storeRates = storeRates;
            _cartItems = new Dictionary<string , int>();

            _currState = new EmptyCartState(_cartItems, _storeItems);
        }

        public void AddItem( string itemName , int quantity )
        {
            _currState.AddItem( itemName , quantity );   
            if (!UpdateErrorFromState())
            {
                _currState = new AddedCartState( _cartItems , _storeItems );
            }
        }

        public void Confirm()
        {
            _currState.Confirm();
            if (!UpdateErrorFromState())
            {
                _currState = new ConfirmState( _cartItems , _storeRates );
            }
        }

        public void PerformPayment( int amount )
        {
            _currState.PerformPayment(amount);
            if (!UpdateErrorFromState())
            {
                _currState = new EmptyCartState( _cartItems, _storeItems );
            }
        }

        public void RemoveItem( string itemName )
        {
            _currState.RemoveItem(itemName);

            if (_cartItems.Count < 1)
            {
                _currState = new EmptyCartState( _cartItems , _storeItems );
            }
        }
    }
}
