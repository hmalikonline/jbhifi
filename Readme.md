# The Weather App

# Overview

Please allow me to call this app "The Weather App"!

The app has been developed to be configurable and secure.

- The app has a clean mobile-first UI that has a touch of JB Hifi branding 😊, with black and yellow colours.
- For best user experience I've made entry of country via a dropdown selection, rather than free text. Additionaly it maps country name with country code behind the scenes that is required for OpenWeather API calls. For this assessment, I've pre-configured the app with 2 countries, defaulting to Australia.
- It has a basic API key scheme of 7 character length keys.
- The API has been pre-configured with 5 API keys. See configuration file. 
- The app has been pre-configured to rate limit 5 weather reports an hour. See configuration file. 
- The app has a basic mechanism to spread the usage among multiple OpenWeather API keys by randomly selecting one when making a call. 

# Security

- OpenWeather API keys haven't been stored in source control for security reasons.
- The frontend of the app calls backend API with required api key on server side, rather than client side, thus keeping the api key hidden from browsers.

# Testing

Integration tests have been written to verify api key security, rate limiting, input validation and weather response from OpenWeather API.

Since there isn't any business logic other than fetching OpenWeather API, I haven't written any unit tests that will mock OpenWeather API calls. However, the code has been written with Dependency injection to ensure it's unit testable.

Please follow the below steps to run the integration tests in powershell. You can perform equivalent steps if you are using another shell such as bash.

## Running integration tests in Powershell

- Open a powershell window.
- Change working directory to \backend\tests\integration-tests  
- Edit "run-tests.ps1" powershell file in your favourite text editor
- Replace placeholder dummy values of KEY1,KEY2 with real OpenWeather API keys. NOTE: There shouldn't be space between the comma separated keys. You can also supply a single key.
- Please don't change other environment variable values in the file as they've been setup for controlled rate limiting tests.
- Save changes.
- Run the powershell "run-tests.ps1" file.
- After about 10-15 seconds, you should see a message that confirms all tests have passed.


# Running application

## Running Backend API
- Open a new powershell window. 
- NOTE: Don't use the powershell window that was used to run integration tests as it will give undesired results since environment variables in integration tests were setup for controlled rate limiting.
- Change working directory to \backend\api  
- Edit "run-api.ps1" powershell file in your favourite text editor
- Similar to step for integration tests, replace placeholder dummy values of KEY1,KEY2 with real OpenWeather API keys. NOTE: There shouldn't be space between the comma separated keys. You can also supply a single key.
- Save changes.
- Run the powershell "run-api.ps1" file.

## Running Frontend React App

Once API is up and running, note down the URL of API in the powershell window.

- Open a new powershell window. 
- Change working directory to the React App that is located in \frontend\weather-app  
- Edit "run-reactapp.ps1" powershell file in your favourite text editor.
- Replace API_URL with the url of the API running on your machine - the one you noted before.
- NOTE: An API key has been pre-configured in the file. Please don't change it.
- Save changes.
- Run the powershell "run-reactapp.ps1" file.
- Click on the link in the window, to launch the Weather App.

### Testing rate limiting by key

With Weather App up and running, once you've hit the rate limit of 5 weather reports an hour. You can change the API key that the app is using, which should give you another 5 attempts. Follow the below steps to test this scenario.

- Find the window running React app.
- Press Ctrl+C to terminate the app server.
- Edit "run-reactapp.ps1" powershell file in your favourite text editor.
- Replace swap API key values with commented one.
- Run the powershell "run-reactapp.ps1" file.
- Click on the link in the window, to launch the Weather App.
- Now, you should be able to again check the weather.  


