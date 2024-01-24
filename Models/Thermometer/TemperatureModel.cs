using Boiler.Mobile.Framework.Time;

namespace Boiler.Mobile.Models.Thermometer;

public class TemperatureModel
{
    public long Id { get; set; }
    public string? Value { get; set; }
    public DateTime? Time { get; set; } = TimeExtension.PragueDateNowAsUtc().AddHours(-1);
    public string? ThermometerId { get; set; }
    public virtual ThermometerModel? Thermometer { get; set; }
}