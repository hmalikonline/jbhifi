const Weather = () => {
    return(
        <>
        <div>Hi, welcome to the Weather App! You can check current weather at any location.</div>
        <form>
            <div>
                <label htmlFor="city">City</label>
                <input id="city" name="city" type="text"/>
            </div>
            <div>
                <label htmlFor="city">Country</label>
                <select id="country" name="country" >
                    <option value="au">Australia</option>
                    <option value="uk">United Kingdom</option>                    
                </select>
            </div>
            <button>Check Weather</button>
        </form>
        </>
    )
}

export default Weather;