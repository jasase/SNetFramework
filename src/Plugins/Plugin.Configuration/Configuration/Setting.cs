using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationPlugin.Configuration
{
    public class Setting : AreaElement
    {
        public string Value { get; set; }

        public Setting()
        {
            Value = string.Empty;
        }

        public override void Accept(IAreaElementVisitor visitor)
        {
            visitor.Handle(this);
        }
    }
}
