﻿namespace Framework.Contracts.Services.ThreadManaging
{
    public interface IManagedThread
    {
        string Name { get; }

        void Run(IManagedThreadHandle handle);
    }
}
