'use server'

import { fetchWeather } from "@/lib/data-access"
import { WeatherRequest } from "@/lib/dtos"
import { Weather } from "@/lib/entities"


export type GetWeatherFormState = {    
    weather: Weather|null,
    errors: string[],
    data: WeatherRequest|null
}

export const getWeather = async (prevState: any, formData: FormData) : Promise<GetWeatherFormState> => {
    //read and validate form data
    const weatherRequest:WeatherRequest = {
        city: formData.get("city") as string,
        country: formData.get("country") as string
    }
    
    console.log(weatherRequest);

    let result:GetWeatherFormState = {
        weather: null,
        errors: [],
        data: weatherRequest
    };

    //TODO: validate form entries

    //stop further processing if there are validation errors
    if (result.errors.length > 0)
        return result;

    //fetch weather from data access layer
    try{
        result.weather = await fetchWeather(weatherRequest);
    }
    catch (ex) {
        console.log(ex)
        result.errors.push(ex instanceof Error ? ex.message : String(ex));        
    }

    return result;
     
}