/******************************************************************************
 * Filename    = Context.cs
 *
 * Author      = unniisme
 *
 * Product     = OnlineShoppingCart
 * 
 * Project     = CartStateMachine
 *
 * Description = Represents the context of a shopping cart in an online
 *               shopping application. Manages the current state and 
 *               transitions of the shopping cart.
 *****************************************************************************/

namespace CartStateMachine
{
    /// <summary>
    /// Represents the context of a shopping cart in an online shopping application.
    /// </summary>
    public class Context : ICartState
    {
        /// <summary>
        /// Gets the error status of the cart.
        /// </summary>
        public bool errorStatus { get; private set; }

        /// <summary>
        /// Gets the error message associated with the cart.
        /// </summary>
        public string? errorMessage { get; private set; }

        private readonly Dictionary<string , int> _cartItems;
        private readonly Dictionary<string , int> _storeItems;
        private readonly Dictionary<string , int> _storeRates;

        private ICartState _currState;

        /// <summary>
        /// Retrieves the items currently in the cart.
        /// </summary>
        /// <returns>A dictionary containing item names and their respective quantities.</returns>
        public Dictionary<string , int> GetCartItems() => _cartItems;

        /// <summary>
        /// Retrieves the available items in the store.
        /// </summary>
        /// <returns>A dictionary containing store item names and their quantities.</returns>
        public Dictionary<string , int> GetStoreItems() => _storeItems;

        /// <summary>
        /// Retrieves the prices of items in the store.
        /// </summary>
        /// <returns>A dictionary containing store item names and their prices.</returns>
        public Dictionary<string , int> GetStoreRates() => _storeRates;

        /// <summary>
        /// Initializes a new instance of the <see cref="Context"/> class.
        /// </summary>
        /// <param name="storeItems">The available items in the store.</param>
        /// <param name="storeRates">The prices of items in the store.</param>
        public Context( Dictionary<string , int> storeItems , Dictionary<string , int> storeRates )
        {
            errorStatus = false;
            _storeItems = storeItems;
            _storeRates = storeRates;
            _cartItems = new Dictionary<string , int>();

            _currState = new EmptyCartState( this );
        }

        private bool UpdateErrorFromState()
        {
            errorStatus = _currState.errorStatus;
            errorMessage = _currState.errorMessage;
            return errorStatus;
        }

        /// <summary>
        /// Adds an item to the shopping cart with the specified quantity.
        /// </summary>
        /// <param name="itemName">The name of the item to add.</param>
        /// <param name="quantity">The quantity of the item to add.</param>
        public void AddItem( string itemName , int quantity )
        {
            _currState.AddItem( itemName , quantity );
            if (!UpdateErrorFromState() && (_currState is EmptyCartState))
            {
                _currState = new AddedCartState( this );
            }
        }

        /// <summary>
        /// Confirms the order, transitioning the cart to a confirmed state.
        /// </summary>
        public void Confirm()
        {
            _currState.Confirm();
            if (!UpdateErrorFromState())
            {
                _currState = new ConfirmState( this );
            }
        }

        /// <summary>
        /// Performs a payment transaction for the cart.
        /// </summary>
        /// <param name="amount">The payment amount.</param>
        public void PerformPayment( int amount )
        {
            _currState.PerformPayment( amount );
            if (!UpdateErrorFromState())
            {
                _currState = new EmptyCartState( this );
            }
        }

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        /// <param name="itemName">The name of the item to remove.</param>
        public void RemoveItem( string itemName )
        {
            _currState.RemoveItem( itemName );

            if (_cartItems.Count < 1 && !UpdateErrorFromState())
            {
                _currState = new EmptyCartState( this );
            }
        }
    }
}
