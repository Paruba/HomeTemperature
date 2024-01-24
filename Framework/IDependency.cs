namespace Boiler.Mobile.Framework;

public interface IDependency
{
}

public interface ISingletonDependency : IDependency
{
}

public interface ITransientDependency : IDependency
{
}