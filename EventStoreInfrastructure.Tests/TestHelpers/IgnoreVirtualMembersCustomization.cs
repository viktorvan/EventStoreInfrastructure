using System;
using System.Reflection;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace EventStoreInfrastructure.Tests.TestHelpers
{
    /// <summary>
    /// Customisation to ignore the virtual members in the class - helps ignoring the navigational 
    /// properties and makes it quicker to generate objects when you don't care about these
    /// </summary>
    public class IgnoreVirtualMembers : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var pi = request as PropertyInfo;
            if (pi == null)
            {
                return new NoSpecimen();
            }

            if (pi.GetGetMethod().IsVirtual)
            {
                return null;
            }
            return new NoSpecimen();
        }
    }

    public class IgnoreVirtualMembersCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new IgnoreVirtualMembers());
        }
    }
}
