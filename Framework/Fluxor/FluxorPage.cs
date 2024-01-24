using Boiler.Mobile.Framework.Attributes;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;

namespace Boiler.Mobile.Framework.Fluxor;

public class FluxorPage : FluxorComponent
{

    [Inject]
    protected NavigationManager UriHelper { get; set; }

    public string FormatUrl(string format, object arg0)
    {
        return string.Format(format, arg0);
    }

    public string HintModel<T>(Expression<Func<T>> expression)
    {
        return AttributeHelper.GetPropertyHintName(expression);
    }

    public string DisplayModel<T>(Expression<Func<T>> expression)
    {
        return AttributeHelper.GetPropertyName(expression);
    }

    public string Display<T>(Expression<Func<T, bool>> expression)
    {
        return AttributeHelper.GetPropertyName(expression);
    }

    public string Display<T>(Expression<Func<T, int>> expression)
    {
        return AttributeHelper.GetPropertyName(expression);
    }

    public string Display<T>(Expression<Func<T, string>> expression)
    {
        return AttributeHelper.GetPropertyName(expression);
    }

    public string Display<T>(Expression<Func<T, IEnumerable<string>>> expression)
    {
        return AttributeHelper.GetPropertyName(expression);
    }

    public string Display<T>(Expression<Func<T, byte>> expression)
    {
        return AttributeHelper.GetPropertyName(expression);
    }

    public string Display<T>(Expression<Func<T, decimal>> expression)
    {
        return AttributeHelper.GetPropertyName(expression);
    }

    public string Display<T>(Expression<Func<T, DateTime>> expression)
    {
        return AttributeHelper.GetPropertyName(expression);
    }

    public string Display<T>(Expression<Func<T, DateTime?>> expression)
    {
        return AttributeHelper.GetPropertyName(expression);
    }

    public string Display<T>(Expression<Func<T, Enum>> expression)
    {
        return AttributeHelper.GetPropertyName(expression);
    }

    public string Display<T, K>(Expression<Func<T, K>> expression)
    {
        return AttributeHelper.GetPropertyName(expression);
    }
}