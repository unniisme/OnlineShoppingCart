/******************************************************************************
 * Filename    = AddedCartState.cs
 *
 * Author      = unniisme
 *
 * Product     = OnlineShoppingCart
 * 
 * Project     = CartStateMachine
 *
 * Description = Represents the state of a shopping cart with items added
 *               in an online shopping application. Defines the behavior and 
 *               transitions of the cart when items have been added to it.
 *****************************************************************************/

namespace CartStateMachine
{
    /// <summary>
    /// Represents the state of a shopping cart with items added in an online shopping application.
    /// </summary>
    internal class AddedCartState : ICartState
    {
        /// <summary>
        /// Gets the error status of the cart.
        /// </summary>
        public bool errorStatus { get; private set; } = false;

        /// <summary>
        /// Gets the error message associated with the cart.
        /// </summary>
        public string? errorMessage { get; private set; }

        private readonly Dictionary<string , int> _cartItems;
        private readonly Dictionary<string , int> _storeItems;

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
        /// <returns>An empty dictionary since no prices are available in the added cart state.</returns>
        public Dictionary<string , int> GetStoreRates() => new();

        /// <summary>
        /// Initializes a new instance of the <see cref="AddedCartState"/> class.
        /// </summary>
        /// <param name="context">The context, typically representing the previous state.</param>
        public AddedCartState( ICartState context )
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
                int sumQuantity;
                if (_cartItems.ContainsKey( itemName ))
                {
                    sumQuantity = quantity + _cartItems[itemName];
                }
                else
                {
                    sumQuantity = quantity;
                }

                if (_storeItems[itemName] < sumQuantity)
                {
                    SetError( $"{sumQuantity} of {itemName} not available" );
                }
                else
                {
                    _cartItems[itemName] = sumQuantity;
                    UnsetError();
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
            UnsetError();
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
            if (_cartItems.ContainsKey( itemName ))
            {
                _cartItems.Remove( itemName );
                UnsetError();
            }
            else
            {
                SetError( $"Item {itemName} not in cart." );
            }
        }
    }
}
