using Boiler.Mobile.Models.Thermometer;

namespace Boiler.Mobile.Components.Pages.Thermometer.List;

public class Actions
{
    public class GetThermometers
    {
        public string? SearchValue { get; set; }
        public int? Skip {  get; set; }
        public int? Take { get; set; }
        public GetThermometers(string? searchValue = "", int? skip = 0, int? take = 20) { 
            SearchValue = searchValue;
            Skip = skip;
            Take = take;
        }
    }

    public class Error
    {
        public string? ErrorMessage { get; set; }
        public Error(string? errorMessage) {
            ErrorMessage = errorMessage;
        }
    }

    public class SetThermometers
    {
        public List<ThermometerModel> Thermometers { get; set; }
        public SetThermometers(List<ThermometerModel> thermometers)
        {
            Thermometers = thermometers;
        }
    }

    public class ThermometerDetail
    {
        public string ThermometerId { get; set; }
        public ThermometerDetail(string thermometerId)
        {
            ThermometerId = thermometerId;
        }
    }

    public class ClearState { }
}
