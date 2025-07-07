import { WeatherRequest } from "./dtos";
import { Weather } from "./entities";

 
export const fetchWeather = async (weatherRequest: WeatherRequest): Promise<Weather> => {
        
    const request: RequestInit = {            
            cache: 'no-store',
            method: 'GET',
            headers: {
                "x-api-key": `${process.env.API_KEY}`
              }    
        };

    const apiEndPointUrl = `${process.env.API_BASE_URL}/weatherforecast?city=${weatherRequest.city}&country=${weatherRequest.country}`

    // Add a fake delay to make waiting noticeable.
    // await new Promise(resolve => {
    //   setTimeout(resolve, 2000);
    // });
    
    // const status:number = 429;
    // // switch(status)
    // //         {
    // //             case 429:
    // //                 throw new Error("Sorry, you'll have to wait before you can check weather again.");
    // //             case 400:
    // //                 throw new Error("Please ensure you've entered a known city in the country you've selected.");
    // //             case 404:
    // //                 throw new Error("Looks like weather isn't available for your selected city/country. You can check weather for another location.");
    // //             default:
    // //                 throw new Error("Apologies, we aren't able to check the weather. Please try again.");
    // //         }    

    // return {description:"Sunny and happy"};
    let response;
    let status:number = 0;
    try{
            response = await fetch(apiEndPointUrl, request);

            console.log(response); //log response for investigation

            if(response.ok)
            {
                var weather: Weather = await response.json();
                return weather;            
            }else{
                status = response.status;
            }
        }
        catch (ex)
        {
            console.log(ex); //log exception  for alert/investigation
            throw new Error("Apologies, we aren't able to check the weather. Please try again later.");      
        }        
        
        //if we receive any status other than http OK status code, we check and communicate an appropriate message for the user
        switch(status)
        {
            case 429:
                throw new Error("Sorry, you'll have to wait before you can check weather again.");
            case 400:
                throw new Error("Please ensure you've entered a valid city.");
            case 404:
                throw new Error("Looks like weather isn't available for your selected city/country. Please ensure you've entered a known city in the country you've selected.");
            default:
                throw new Error("Apologies, we aren't able to check the weather. Please try again later.");
        }
        
}