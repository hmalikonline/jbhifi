'use server'

import { fetchWeather } from "@/lib/data-access"
import { WeatherRequest } from "@/lib/dtos"
import { Weather } from "@/lib/entities"


interface OperationState {
    success: boolean,
    errors: string[]
}

export type GetWeatherFormState = OperationState & {    
    weather: Weather|null
}

export const getWeather = async (prevState: any, formData: FormData) : Promise<GetWeatherFormState> => {
    //read and validate form data
    const weatherRequest:WeatherRequest = {
        city: formData.get("city") as string,
        country: formData.get("country") as string
    }
    
    let errors: GetWeatherFormState['errors'] = [];
    let weather:Weather|null = null;

    //fetch weather from data access layer
    try{
        weather = await fetchWeather(weatherRequest);
    }
    catch (ex) {
        errors.push(ex instanceof Error ? ex.message : String(ex));        
    }

    return {
        errors:errors,
        success: true,
        weather: weather
    };
}