using System.Collections.Generic;
using System.Linq;

namespace DND.Middleware.System
{
    public class ObjectModelStateDictionary : Dictionary<string, List<string>>
    {
        public bool IsValid => Count == 0 || Values.All(x => x.Count == 0);
    }
}