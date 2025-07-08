echo "Setting environment variables for integration tests..."

$Env:OpenWeatherMap__APIKeys = "KEY1,KEY2"  
$Env:ApplicationConfiguration__RateLimit__MaxRequests = "1"  
$Env:ApplicationConfiguration__RateLimit__TimeWindowInSeconds = "4"  

echo "Integration test environment variables have been set."

echo "Running integration tests..."

dotnet test





