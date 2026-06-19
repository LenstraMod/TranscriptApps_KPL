namespace Backend
{
    /// <summary>
    /// Design by Contract helper.
    /// Requires  = precondition  — must be true BEFORE the method body runs.
    /// Ensures   = postcondition — must be true AFTER the method body runs.
    /// Invariant = internal state that must always hold inside a class.
    ///
    /// All violations throw ContractException so callers can distinguish
    /// contract failures from normal business-logic exceptions.
    /// </summary>
    public static class Contract
    {
        /// <summary>Precondition: the caller is responsible for satisfying this.</summary>
        public static void Requires(bool condition, string message)
        {
            if (!condition)
                throw new ContractException($"[Precondition violated] {message}");
        }

        /// <summary>Postcondition: the method is responsible for satisfying this.</summary>
        public static void Ensures(bool condition, string message)
        {
            if (!condition)
                throw new ContractException($"[Postcondition violated] {message}");
        }

        /// <summary>Invariant: internal state consistency check inside a class.</summary>
        public static void Invariant(bool condition, string message)
        {
            if (!condition)
                throw new ContractException($"[Invariant violated] {message}");
        }
    }

    /// <summary>Thrown when a Design-by-Contract condition is not met.</summary>
    public class ContractException : Exception
    {
        public ContractException(string message) : base(message) { }
    }
}
