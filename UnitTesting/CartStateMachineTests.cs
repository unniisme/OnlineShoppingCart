using CartStateMachine;

namespace UnitTesting
{
    [TestClass]
    public class CartStateMachineTests
    {
        Dictionary<string , int> _storeItems = new();
        Dictionary<string , int> _storeRates = new();

        [TestInitialize]
        public void Initialize() 
        {
            _storeItems = new()
            {
                { "Smart TV", 2 },
                { "Wireless Headphone", 5 },
                { "Men's T-Shirt", 7 },
                { "Women's T-Shirt", 6 },
                { "Veggies", 17 }
            };

            _storeRates = new()
            {
                { "Smart TV", 20000 },
                { "Wireless Headphone", 2300 },
                { "Men's T-Shirt", 400 },
                { "Women's T-Shirt", 700 },
                { "Veggies", 30 }
            };
        }

        [TestMethod]
        public void AddItemTest()
        {
            // Initialize cart context and store data as in the original test
            ICartState cartContext = new Context( _storeItems , _storeRates );

            // Attempt to confirm an empty cart
            cartContext.Confirm();
            Assert.IsTrue( cartContext.errorStatus , "Can confirm an empty cart" );

            // Attempt to pay with an empty cart
            cartContext.PerformPayment( 100 );
            Assert.IsTrue( cartContext.errorStatus , "Can pay with an empty cart" );

            // Attempt to remove an item from an empty cart
            cartContext.RemoveItem( "Smart TV" );
            Assert.IsTrue( cartContext.errorStatus , "Can remove an item from an empty cart" );

            // Add items to the cart
            cartContext.AddItem( "Smart TV" , 2 );
            cartContext.AddItem( "Veggies" , 5 );
            cartContext.AddItem( "Wireless Headphone" , 3 );

            // Try to add an item that is not in the store
            cartContext.AddItem( "Non-existent Item" , 2 );
            Assert.IsTrue( cartContext.errorStatus , "Able to add a non-existent item to the cart" );

            // Ensure that there are 3 items in the cart
            Assert.AreEqual( 3 , cartContext.GetCartItems().Count );

            // Attempt to add an unknown item to the cart
            cartContext.AddItem( "PC" , 2 );
            Assert.IsTrue( cartContext.errorStatus , "Can add an unknown item to the cart" );

            // Attempt to overflow an item in the cart
            cartContext.AddItem( "Smart TV" , 2 );
            Assert.IsTrue( cartContext.errorStatus , "Can overflow an item in the cart" );
        }


        [TestMethod]
        public void RemoveItemTest()
        {
            // Initialize cart context and store data as in AddItemTest
            ICartState cartContext = new Context( _storeItems , _storeRates );

            // Try to remove an item that is not in the store
            cartContext.RemoveItem( "Non-existent Item" );
            Assert.IsTrue( cartContext.errorStatus , "Able to remove a non-existent item from the cart" );

            // Add an item to the cart
            cartContext.AddItem( "Smart TV" , 2 );

            // Remove the added item
            cartContext.RemoveItem( "Smart TV" );
            Assert.IsFalse( cartContext.GetCartItems().ContainsKey( "Smart TV" ) , "'Smart TV' not removed from cart" );
        }

        [TestMethod]
        public void ConfirmTest()
        {
            // Initialize cart context and store data as in AddItemTest
            ICartState cartContext = new Context( _storeItems , _storeRates );

            // Confirm an empty cart
            cartContext.Confirm();
            Assert.IsTrue( cartContext.errorStatus , "Is able to confirm an empty cart" );

            // Add items to the cart
            cartContext.AddItem( "Smart TV" , 2 );
            cartContext.AddItem( "Veggies" , 5 );
            cartContext.AddItem( "Wireless Headphone" , 3 );

            // Confirm the cart with items
            cartContext.Confirm();
            Assert.IsFalse( cartContext.errorStatus , cartContext.errorMessage );
        }

        [TestMethod]
        public void PerformPaymentTest()
        {
            // Initialize cart context and store data as in AddItemTest
            ICartState cartContext = new Context(_storeItems, _storeRates);

            // Try to perform payment with an empty cart
            cartContext.PerformPayment( 100 );
            Assert.IsTrue( cartContext.errorStatus , "Able to perform payment with an empty cart" );

            // Add items to the cart
            cartContext.AddItem( "Smart TV" , 2 );
            cartContext.AddItem( "Veggies" , 5 );
            cartContext.AddItem( "Wireless Headphone" , 3 );

            // Try to perform payment before confirming
            cartContext.PerformPayment( 100 );
            Assert.IsTrue( cartContext.errorStatus , "Able to perform payment before confirming" );

            // Confirm the cart with items
            cartContext.Confirm();
            Assert.IsFalse( cartContext.errorStatus , cartContext.errorMessage );

            // Try to perform payment with insufficient funds
            cartContext.PerformPayment( 100 );
            Assert.IsTrue( cartContext.errorStatus , "Able to perform payment with insufficient funds" );

            // Perform payment with sufficient funds
            cartContext.PerformPayment( 3 * 2300 + 30 * 5 + 2*20000 );
            Assert.IsFalse( cartContext.errorStatus , cartContext.errorMessage );

            // Check if store items have been updated correctly
            Assert.AreEqual( _storeItems["Wireless Headphone"] , 2 );
            Assert.AreEqual( _storeItems["Veggies"] , 12 );
            Assert.AreEqual( _storeItems["Smart TV"] , 0 );
        }

    }
}
