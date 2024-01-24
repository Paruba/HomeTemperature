using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Boiler.Mobile.Framework.Attributes;

public static class AttributeHelper
{
    public static string GetPropertyName<T>(Expression<Func<T>> expression)
    {
        MemberExpression propertyExpression = (MemberExpression)expression.Body;
        var displayAttribute = propertyExpression.Member.GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name;
    }

    public static string GetPropertyName<T, K>(Expression<Func<T, K>> expression)
    {
        MemberExpression propertyExpression;

        if (expression.Body is UnaryExpression unaryExpression)
        {
            propertyExpression = (MemberExpression)unaryExpression.Operand;
        }
        else
        {
            propertyExpression = (MemberExpression)expression.Body;
        }

        var displayAttribute = propertyExpression.Member.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute?.Name;
    }
    public static string GetPropertyHintName<T>(Expression<Func<T>> expression)
    {
        MemberExpression propertyExpression = (MemberExpression)expression.Body;
        var displayAttribute = propertyExpression.Member.GetCustomAttribute<HintAttribute>();

        return displayAttribute?.Description;
    }

    public static string GetPropertyHintName<T, K>(Expression<Func<T, K>> expression)
    {
        MemberExpression propertyExpression;

        if (expression.Body is UnaryExpression unaryExpression)
        {
            propertyExpression = (MemberExpression)unaryExpression.Operand;
        }
        else
        {
            propertyExpression = (MemberExpression)expression.Body;
        }

        var displayAttribute = propertyExpression.Member.GetCustomAttribute<HintAttribute>();
        return displayAttribute?.Description;
    }
    public static string GetEnumName(this Enum enumValue)
    {
        return enumValue.GetType().GetMember(enumValue.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>()
            .GetDescription() ?? "neznamý";
    }
}