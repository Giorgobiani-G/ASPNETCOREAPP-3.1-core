using System;

namespace ASPNETCOREAPP.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base("Entity not Found!")
        {   
        }
    }
}
