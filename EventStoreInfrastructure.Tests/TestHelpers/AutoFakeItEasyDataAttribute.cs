using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoFakeItEasy;
using Ploeh.AutoFixture.Xunit2;

namespace EventStoreInfrastructure.Tests.TestHelpers
{
    public class AutoFakeItEasyDataAttribute : AutoDataAttribute
    {
        public AutoFakeItEasyDataAttribute() : base(new Fixture().Customize(
                new CompositeCustomization(
                    new MultipleCustomization(),
                    new AutoFakeItEasyCustomization(),
                    new IgnoreVirtualMembersCustomisation())))
        {
        }
    }
}