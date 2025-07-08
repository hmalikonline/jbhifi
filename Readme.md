# Overview

# Testing

Running integration tests in Powershell

Setup controlled rate limiting settings for integration tests, by executing the below commands in powershell.

$Env:ApplicationConfiguration__RateLimit__MaxRequests = "1"
$Env:ApplicationConfiguration__RateLimit__TimeWindowInSeconds = "4"

Then run the tests, by executing the below command in the same shell you ran the above commands.
dotnet test

# Backend

# Frontend
