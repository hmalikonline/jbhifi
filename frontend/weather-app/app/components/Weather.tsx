'use client'

import { useActionState } from "react";
import { getWeather, GetWeatherFormState } from "../handlers/weather-handler";

const Weather = () => {

    const inititalFormState:GetWeatherFormState = {
        weather: null,
        errors: [],
        data: null
    };

    const [formState, formAction, isPending] = useActionState(getWeather, inititalFormState)

    return(
        <>
        <div>Hi, welcome to the Weather App! You can check current weather at any location.</div>
        <form action={formAction}>
            <div>
                <label htmlFor="city">City</label>
                <input id="city" name="city" type="text" defaultValue={formState.data?.city}/>
            </div>
            <div>
                <label htmlFor="city">Country</label>
                <select id="country" name="country" >
                    <option value="au">Australia</option>
                    <option value="uk">United Kingdom</option>                    
                </select>
            </div>
            <button disabled={isPending}>{isPending ? 'Checking...': 'Check Weather'}</button>
            <div>Please note that you can check weather at your favourite locations 5 times in an hour.</div>
            {
                !isPending && formState.weather !== null && 
                <div>
                    The weather in {formState.data?.city},{formState.data?.country} is {formState.weather.description}
                </div>
            }
            {
                !isPending && formState.weather == null && formState.errors.length > 0 && 
                <div>
                    {
                    formState.errors.map((e, index) => {
                        return <div key={index}>{e}</div>
                    })
                    }
                </div>
            }
        </form>
        </>
    )
}

export default Weather;