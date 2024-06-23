using System.Collections.Generic;

namespace LogicPuddle.Common
{
    public interface ISerializable
    {
        public Dictionary<string, object> Serialize();
        public void Deserialize(Dictionary<string, object> data);
    }
}