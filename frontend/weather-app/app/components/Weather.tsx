'use client'

import { useActionState, useState } from "react";
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
        <div>
            Hi, welcome to the Weather App! Here you can check current weather at any of your favourite locations.
        </div>
        <form action={formAction} className="form-container">
            <div className="form-row">
                <label htmlFor="city">City</label>
                <input 
                    id="city" 
                    name="city" 
                    type="text" 
                    className="input-text"
                    defaultValue={formState.data?.city}
                    placeholder="e.g. Melbourne"
                    required
                />
            </div>
            <div className="form-row">
                <label htmlFor="city">Country</label>                
                <select 
                    id="country" 
                    name="country" 
                    className="select-dropdown"
                    required
                    defaultValue={formState.data?.country} 
                    key={formState.data?.country}                    
                >
                    <option value="au">Australia</option>
                    <option value="uk">United Kingdom</option>                    
                </select>
            </div>
            <button 
                disabled={isPending}
                className="btn-primary form-row"
                >
                {isPending ? 'Checking...': 'Check Weather'}
            </button>                   
        <div style={{fontSize: '0.8rem'}}>* <span style={{fontWeight: 'bold'}}>Please note:</span> You can check the weather up to 5 times per hour. Use your quota wisely!</div>
            {
                !isPending && formState.weather !== null && 
                <div className="message-primary form-row">
                    The weather in {formState.data?.city}, {formState.data?.country} is &quot;{formState.weather.description}&quot;
                </div>
            }
            {
                !isPending && formState.weather == null && formState.errors.length > 0 && 
                <div className="message-error form-row">
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