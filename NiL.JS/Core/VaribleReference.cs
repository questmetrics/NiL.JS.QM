﻿
namespace NiL.JS.Core
{
    public abstract class VaribleReference : Statement
    {
        public abstract string Name { get; }
        public abstract VaribleDescriptor Descriptor { get; internal set; }

        protected override Statement[] getChildsImpl()
        {
            return null;
        }
    }
}
