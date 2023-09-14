/******************************************************************************
 * Filename    = ICartState.cs
 *
 * Author      = unniisme
 *
 * Product     = OnlineShoppingCart
 * 
 * Project     = CartStateMachine
 *
 * Description = Represents the state of a shopping cart in an
 *               online shopping application. Defines methods and properties
 *               to manage the cart's behavior and transitions between states.
 *****************************************************************************/

namespace CartStateMachine
{
    /// <summary>
    /// Represents the state interface for a shopping cart in an online shopping application.
    /// </summary>
    public interface ICartState
    {
        /// <summary>
        /// Gets the error status of the cart.
        /// </summary>
        bool errorStatus { get; }

        /// <summary>
        /// Gets the error message associated with the cart.
        /// </summary>
        string? errorMessage { get; }

        /// <summary>
        /// Retrieves the items currently in the cart.
        /// </summary>
        /// <returns>A dictionary containing cart item names and their respective quantities.</returns>
        Dictionary<string , int> GetCartItems();

        /// <summary>
        /// Retrieves the available items in the store.
        /// </summary>
        /// <returns>A dictionary containing store item names and their quantities.</returns>
        Dictionary<string , int> GetStoreItems();

        /// <summary>
        /// Retrieves the prices of items in the store.
        /// </summary>
        /// <returns>A dictionary containing store item names and their prices.</returns>
        Dictionary<string , int> GetStoreRates();

        /// <summary>
        /// Adds an item to the shopping cart with the specified quantity.
        /// </summary>
        /// <param name="itemName">The name of the item to add.</param>
        /// <param name="quantity">The quantity of the item to add.</param>
        void AddItem(string itemName, int quantity);

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        /// <param name="itemName">The name of the item to remove.</param>
        void RemoveItem(string itemName);

        /// <summary>
        /// Confirms the order, transitioning the cart to a confirmed state.
        /// </summary>
        void Confirm();

        /// <summary>
        /// Performs a payment transaction for the cart.
        /// </summary>
        /// <param name="amount">The payment amount.</param>
        void PerformPayment(int amount);
    }
}
