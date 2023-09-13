

namespace CartStateMachine
{
    internal class Context : ICartState
    {
        public bool errorStatus { get; private set; }
        public string? errorMessage { get; private set; }

        public Context()
        {
            errorStatus = false;
        }

        public void AddItem( string itemName , int quantity )
        {
            throw new NotImplementedException();
        }

        public void Confirm()
        {
            throw new NotImplementedException();
        }

        public void PerformPayment( int amount )
        {
            throw new NotImplementedException();
        }

        public void RemoveItem( string itemName )
        {
            throw new NotImplementedException();
        }
    }
}
