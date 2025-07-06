import { WeatherRequest } from "./dtos";
import { Weather } from "./entities";

 
export const fetchWeather = async (weatherRequest: WeatherRequest): Promise<Weather|null> => {
        
    const request: RequestInit = {            
            cache: 'no-store',
            method: 'GET',
            headers: {
                "x-api-key": `${process.env.API_KEY}`
              }    
        };

    const apiEndPointUrl = `${process.env.API_BASE_URL}/weatherforecast?city=${weatherRequest.city}&country=${weatherRequest.country}`

    try{
        const response = await fetch(apiEndPointUrl, request);

        if(response.ok)
        {
            var weather: Weather|null = await response.json();
            return weather;            
        }else{
            console.log(response); //log failed response for alert/investigation
            throw new Error("Couldn't fetch weather.");
        }
    }catch (ex){
        console.log(ex); //log exception  for alert/investigation
        throw new Error("Couldn't fetch weather.");        
    }
        
}