using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGamesCatalog.Exceptions
{
    public class GameDoesNotExistException : Exception
    {
        public GameDoesNotExistException()
            : base("Game does not exist in the database.")
        { }
    }
}
