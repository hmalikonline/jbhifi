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

    //validate form entries

    //remove any spaces on either side, if present
    weatherRequest.city = weatherRequest.city.trimStart().trimEnd();

    if(weatherRequest.city === "")
        result.errors.push("Please provide a valid city.")

    const regex = /^[a-zA-Z ]+$/;

    if (!regex.test(weatherRequest.city)) {
        result.errors.push("Please provide a valid city. City must contain only letters and spaces.")
    }
    

    //stop further processing if there are validation errors
    if (result.errors.length > 0)
        return result;

    //remove any trailing 

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