using System;

namespace Rotativa.AspNetCore.Options
{
    class OptionFlag : Attribute
    {
        public string Name { get; }

        public OptionFlag(string name)
        {
            Name = name;
        }
    }
}