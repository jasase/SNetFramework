using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationPlugin.Configuration
{
    public class Algorithm : AreaElement
    {
        public string Name { get; set; }
        public List<Setting> Settings { get; private set; }

        public Algorithm ()
        {
            Name = string.Empty;
            Settings = new List<Setting>();
        }

        public override void Accept(IAreaElementVisitor visitor)
        {
            visitor.Handle(this);
        }
    }
}
