/******************************************************************************
 * Filename    = ConfirmState.cs
 *
 * Author      = unniisme
 *
 * Product     = OnlineShoppingCart
 * 
 * Project     = CartStateMachine
 *
 * Description = Represents the state of confirming an order in an
 *               online shopping application. Defines the behavior and 
 *               transitions of the cart when confirming an order.
 *****************************************************************************/

namespace CartStateMachine
{
    /// <summary>
    /// Represents the state of confirming an order in an online shopping application.
    /// </summary>
    internal class ConfirmState : ICartState
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

        private readonly int _totalPrice;

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
        /// <returns>An empty dictionary since no prices are available in the confirm state.</returns>
        public Dictionary<string , int> GetStoreRates() => new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmState"/> class.
        /// </summary>
        /// <param name="context">The context, typically representing the previous state.</param>
        public ConfirmState( ICartState context )
        {
            errorStatus = false;
            _cartItems = context.GetCartItems();
            _storeItems = context.GetStoreItems();
            _totalPrice = 0;

            Dictionary<string , int> storeRates = context.GetStoreRates();

            foreach (KeyValuePair<string , int> entry in _cartItems)
            {
                _totalPrice += storeRates[entry.Key] * entry.Value;
            }
        }

        private void SetError( string error )
        {
            errorStatus = true;
            errorMessage = error;
        }

        private void UnsetError()
        {
            errorStatus = false;
            errorMessage = null;
        }

        /// <summary>
        /// Adds an item to the shopping cart with the specified quantity.
        /// </summary>
        /// <param name="itemName">The name of the item to add.</param>
        /// <param name="quantity">The quantity of the item to add.</param>
        public void AddItem( string itemName , int quantity )
        {
            SetError( "Currently confirming order" );
        }

        /// <summary>
        /// Confirms the order, transitioning the cart to a confirmed state.
        /// </summary>
        public void Confirm()
        {
            SetError( "Currently already confirming order" );
        }

        /// <summary>
        /// Performs a payment transaction for the cart.
        /// </summary>
        /// <param name="amount">The payment amount.</param>
        public void PerformPayment( int amount )
        {
            if (amount < _totalPrice)
            {
                SetError( "Insufficient amount for purchase" );
            }
            else
            {
                foreach (KeyValuePair<string , int> entry in _cartItems)
                {
                    _storeItems[entry.Key] -= entry.Value;
                }
                _cartItems.Clear();
                UnsetError();
            }
        }

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        /// <param name="itemName">The name of the item to remove.</param>
        public void RemoveItem( string itemName )
        {
            SetError( "Currently confirming order" );
        }
    }
}
