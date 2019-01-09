using System;

namespace FluidDbClient.Sql
{
    [Flags]
    public enum DataBindingOptions
    {
        /// <summary>
        /// Strict
        /// </summary>
        Default = 0,

        CoerceTypes = 0x1,
        AllowMissingProperties = 0x2
    }
}
