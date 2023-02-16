using System;
namespace BugTrackingSystem.Exceptions
{
    public class DomainInvariantException : Exception
    {
        public DomainInvariantException(string message) : base(message) { }
    }
}
