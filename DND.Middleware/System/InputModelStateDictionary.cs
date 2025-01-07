using System.Collections.Generic;
using System.Linq;

namespace DND.Middleware.System
{
    public class InputModelStateDictionary : Dictionary<string, List<string>>
    {
        public bool IsValid()
        {
            return Count == 0 || Values.All(x => x.Count == 0);
        }
    }
}