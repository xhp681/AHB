﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Globalization;
using System.Numerics;

namespace AHB.Web
{
    public class InvariantNumberModelBinderProvider: IModelBinderProvider
    {
        private static readonly HashSet<Type> _integerTypes = new()
        {
            typeof(int),
            typeof(long),
            typeof(short),
            typeof(sbyte),
            typeof(byte),
            typeof(ulong),
            typeof(ushort),
            typeof(uint),
            typeof(BigInteger)
        };

        private static readonly HashSet<Type> _floatingPointTypes = new()
        {
            typeof(double),
            typeof(decimal),
            typeof(float)
        };

        /// <summary>
        /// Creates a model binder
        /// </summary>
        /// <param name="context">Context object</param>
        /// <returns>Instance of model binder for floating-point types</returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var modelType = context.Metadata.UnderlyingOrModelType;

            if (modelType is null)
                return null;

            if (_floatingPointTypes.Contains(modelType))
                return new InvariantNumberModelBinder(NumberStyles.Float, new FloatingPointTypeModelBinderProvider().GetBinder(context));

            if (_integerTypes.Contains(modelType))
                return new InvariantNumberModelBinder(NumberStyles.Integer, new SimpleTypeModelBinderProvider().GetBinder(context));

            return null;
        }
    }
}
