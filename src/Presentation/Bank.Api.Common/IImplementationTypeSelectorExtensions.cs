using Scrutor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Api.Common
{
    public static class IImplementationTypeSelectorExtensions
    {
        private static HashSet<Type> _decorators;

        static IImplementationTypeSelectorExtensions()
        {
            _decorators = new HashSet<Type>(new[]
            {
                typeof(RetryDecorator<>)
            });
        }

        public static IImplementationTypeSelector RegisterHandlers(this IImplementationTypeSelector selector, Type type)
        {
            return selector.AddClasses(c =>
                    c.AssignableTo(type)
                        .Where(t => !_decorators.Contains(t))
                )
                .UsingRegistrationStrategy(RegistrationStrategy.Append)
                .AsImplementedInterfaces()
                .WithScopedLifetime();
        }
    }
}
