using CartStateMachine;

namespace UnitTesting
{
    [TestClass]
    public class CartStateMachineTests
    {
        [TestMethod]
        public void AddItemTest()
        {
            Dictionary<string , int> storeItems = new()
            {
                { "Smart TV", 2 },
                { "Wireless Headphone", 5 },
                { "Men's T-Shirt", 7 },
                { "Women's T-Shirt", 6 },
                { "Viggies", 17 }
            };

            Dictionary<string , int> storeRates = new()
            {
                { "Smart TV", 20000 },
                { "Wireless Headphone", 2300 },
                { "Men's T-Shirt", 400 },
                { "Women's T-Shirt", 700 },
                { "Viggies", 30 }
            };

            ICartState cartContext = new Context(storeItems, storeRates);

            cartContext.Confirm();
            Assert.IsTrue( cartContext.errorStatus , "Is able to confirm while cart is empty" );
            cartContext.PerformPayment( 100 );
            Assert.IsTrue( cartContext.errorStatus , "Is able to pay while cart is empty" );
            cartContext.RemoveItem( "Smart TV" );
            Assert.IsTrue( cartContext.errorStatus , "Able to remove item while cart is empty" );


            cartContext.AddItem( "PC" , 2 );
            Assert.IsTrue( cartContext.errorStatus , "Is able to add unknown item to cart, while in emptyCartState" );

            cartContext.AddItem( "Smart TV" , 2 );
            cartContext.AddItem( "Veggies" , 5 );
            cartContext.AddItem( "Wireless Headphone" , 3 );
            Assert.Equals( cartContext.GetCartItems().Count , 3 );

            cartContext.AddItem( "Smart TV" , 2 );
            Assert.IsTrue( cartContext.errorStatus , "Able to overflow item in cart" );

            cartContext.RemoveItem( "Smart TV" );
            Assert.IsFalse( cartContext.GetCartItems().ContainsKey( "Smart TV" ) , "'Smart TV' not removed from cart" );

            cartContext.Confirm();
            Assert.IsFalse( cartContext.errorStatus , cartContext.errorMessage );

            cartContext.PerformPayment( 100 );
            Assert.IsTrue( cartContext.errorStatus , "Able to confirm payment using amount 100" );

            cartContext.PerformPayment( 3 * 2300 + 30 * 5 );
            Assert.IsFalse( cartContext.errorStatus, cartContext.errorMessage );

            Assert.Equals( storeItems["Wireless Headphone"] , 2 );
            Assert.Equals( storeItems["Veggie"] , 12 );
            Assert.Equals( storeItems["Smart TV"] , 2 );

            cartContext.Confirm();
            Assert.IsTrue( cartContext.errorStatus , "Is able to confirm while cart is empty" );
        }
    }
}
