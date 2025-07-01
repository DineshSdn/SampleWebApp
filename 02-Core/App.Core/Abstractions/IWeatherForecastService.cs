using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Common.Models;
using App.Core.Models;

namespace App.Core.Abstractions
{
    public interface IWeatherForecastService
    {
        Task<AppResponseDto<List<WeatherForecastDto>>> GetWeatherForecasts(CancellationToken cancellationToken = default);
    }
}
