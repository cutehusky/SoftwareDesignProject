using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CONVERT_TEMPERATURE
{
    class Service
    {
        private double ConvertCelsiusToKelvin(double celsius)
        {
            return celsius + 273.15;
        }

        private double ConvertFahrenheitToKelvin(double fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9 + 273.15;
        }

        private double ConvertRankineToKelvin(double rankine)
        {
            return rankine * 5 / 9;
        }

        private double ConvertDelisleToKelvin(double delisle)
        {
            return 373.15 - delisle * 2 / 3;
        }

        private double ConvertNewtonToKelvin(double newton)
        {
            return newton * 100 / 33 + 273.15;
        }

        private double ConvertKelvinToCelsius(double kelvin)
        {
            return kelvin - 273.15;
        }

        private double ConvertKelvinToFahrenheit(double kelvin)
        {
            return (kelvin - 273.15) * 9 / 5 + 32;
        }

        private double ConvertKelvinToRankine(double kelvin)
        {
            return kelvin * 9 / 5;
        }

        private double ConvertKelvinToDelisle(double kelvin)
        {
            return (373.15 - kelvin) * 3 / 2;
        }

        private double ConvertKelvinToNewton(double kelvin)
        {
            return (kelvin - 273.15) * 33 / 100;
        }

        public Response ConvertTemperature(double value, string type)
        {
            Response response = new Response();
            switch (type)
            {
                case "k":
                    response.valueK = value.ToString();
                    response.valueC = ConvertKelvinToCelsius(value).ToString();
                    response.valueF = ConvertKelvinToFahrenheit(value).ToString();
                    response.valueR = ConvertKelvinToRankine(value).ToString();
                    response.valueD = ConvertKelvinToDelisle(value).ToString();
                    response.valueN = ConvertKelvinToNewton(value).ToString();
                    break;
                case "c":
                    response.valueK = ConvertCelsiusToKelvin(value).ToString();
                    response.valueC = value.ToString();
                    response.valueF = ConvertKelvinToFahrenheit(ConvertCelsiusToKelvin(value)).ToString();
                    response.valueR = ConvertKelvinToRankine(ConvertCelsiusToKelvin(value)).ToString();
                    response.valueD = ConvertKelvinToDelisle(ConvertCelsiusToKelvin(value)).ToString();
                    response.valueN = ConvertKelvinToNewton(ConvertCelsiusToKelvin(value)).ToString();
                    break;
                case "f":
                    response.valueK = ConvertFahrenheitToKelvin(value).ToString();
                    response.valueC = ConvertKelvinToCelsius(ConvertFahrenheitToKelvin(value)).ToString();
                    response.valueF = value.ToString();
                    response.valueR = ConvertKelvinToRankine(ConvertFahrenheitToKelvin(value)).ToString();
                    response.valueD = ConvertKelvinToDelisle(ConvertFahrenheitToKelvin(value)).ToString();
                    response.valueN = ConvertKelvinToNewton(ConvertFahrenheitToKelvin(value)).ToString();
                    break;
                case "r":
                    response.valueK = ConvertRankineToKelvin(value).ToString();
                    response.valueC = ConvertKelvinToCelsius(ConvertRankineToKelvin(value)).ToString();
                    response.valueF = ConvertKelvinToFahrenheit(ConvertRankineToKelvin(value)).ToString();
                    response.valueR = value.ToString();
                    response.valueD = ConvertKelvinToDelisle(ConvertRankineToKelvin(value)).ToString();
                    response.valueN = ConvertKelvinToNewton(ConvertRankineToKelvin(value)).ToString();
                    break;
                case "d":
                    response.valueK = ConvertDelisleToKelvin(value).ToString();
                    response.valueC = ConvertKelvinToCelsius(ConvertDelisleToKelvin(value)).ToString();
                    response.valueF = ConvertKelvinToFahrenheit(ConvertDelisleToKelvin(value)).ToString();
                    response.valueR = ConvertKelvinToRankine(ConvertDelisleToKelvin(value)).ToString();
                    response.valueD = value.ToString();
                    response.valueN = ConvertKelvinToNewton(ConvertDelisleToKelvin(value)).ToString();
                    break;
                case "n":
                    response.valueK = ConvertNewtonToKelvin(value).ToString();
                    response.valueC = ConvertKelvinToCelsius(ConvertNewtonToKelvin(value)).ToString();
                    response.valueF = ConvertKelvinToFahrenheit(ConvertNewtonToKelvin(value)).ToString();
                    response.valueR = ConvertKelvinToRankine(ConvertNewtonToKelvin(value)).ToString();
                    response.valueD = ConvertKelvinToDelisle(ConvertNewtonToKelvin(value)).ToString();
                    response.valueN = value.ToString();
                    break;
            }
            return response;
        }
    }
}
