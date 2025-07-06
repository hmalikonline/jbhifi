'use server'

import { fetchWeather } from "@/lib/data-access"
import { WeatherRequest } from "@/lib/dtos"
import { Weather } from "@/lib/entities"

export const getWeather = async (prevState: any, formData: FormData) => {
    //read and validate form data
    const weatherRequest:WeatherRequest = {
        city: formData.get("city") as string,
        country: formData.get("country") as string
    }
    
    //fetch weather from data access layer
    const weather:Weather|null = await fetchWeather(weatherRequest);
}