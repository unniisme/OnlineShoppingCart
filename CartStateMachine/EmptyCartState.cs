/******************************************************************************
 * Filename    = EmptyCartState.cs
 *
 * Author      = unniisme
 *
 * Product     = OnlineShoppingCart
 * 
 * Project     = CartStateMachine
 *
 * Description = Represents the state of an empty shopping cart in an
 *               online shopping application. Defines the behavior and 
 *               transitions of the cart when it is empty.
 *****************************************************************************/


namespace CartStateMachine
{
    /// <summary>
    /// Represents the state of an empty shopping cart in an online shopping application.
    /// </summary>
    public class EmptyCartState : ICartState
    {
        /// <summary>
        /// Gets the error status of the cart.
        /// </summary>
        public bool errorStatus { get; private set; }

        /// <summary>
        /// Gets the error message associated with the cart.
        /// </summary>
        public string? errorMessage { get; private set; }

        private readonly Dictionary<string, int> _cartItems;
        private readonly Dictionary<string, int> _storeItems;

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
        /// <returns>An empty dictionary since no prices are available in an empty cart state.</returns>
        public Dictionary<string , int> GetStoreRates() => new();

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyCartState"/> class.
        /// </summary>
        /// <param name="context">The context, typically representing the previous state.</param>
        public EmptyCartState( ICartState context )
        {
            errorStatus = false;
            _storeItems = context.GetStoreItems();
            _cartItems = context.GetCartItems();
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
            if (_storeItems.ContainsKey( itemName ))
            {
                if (_storeItems[itemName] < quantity)
                {
                    SetError( $"{quantity} of {itemName} not available" );
                }
                else
                {
                    UnsetError();
                    _cartItems[itemName] = quantity;
                }
            }
            else
            {
                SetError( $"{itemName} not available" );
            }
        }

        /// <summary>
        /// Confirms the order, transitioning the cart to a confirmed state.
        /// </summary>
        public void Confirm()
        {
            SetError( "Nothing in cart to confirm" );
        }

        /// <summary>
        /// Performs a payment transaction for the cart.
        /// </summary>
        /// <param name="amount">The payment amount.</param>
        public void PerformPayment( int amount )
        {
            SetError( "No confirmation" );
        }

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        /// <param name="itemName">The name of the item to remove.</param>
        public void RemoveItem( string itemName )
        {
            SetError( "Nothing in cart to remove" );
        }
    }
}
