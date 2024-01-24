namespace Boiler.Mobile.Framework.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class HintAttribute : Attribute
{
    public static readonly HintAttribute Default = new HintAttribute();
    private string description;

    public HintAttribute() : this(string.Empty)
    {
    }

    public HintAttribute(string description)
    {
        this.description = description;
    }

    public virtual string Description
    {
        get
        {
            return DescriptionValue;
        }
    }

    protected string DescriptionValue
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == this)
        {
            return true;
        }

        HintAttribute other = obj as HintAttribute;

        return (other != null) && other.Description == Description;
    }

    public override int GetHashCode()
    {
        return Description.GetHashCode();
    }

#if !SILVERLIGHT
    /// <internalonly/>
    /// <devdoc>
    /// </devdoc>
    public override bool IsDefaultAttribute()
    {
        return (this.Equals(Default));
    }
#endif
}