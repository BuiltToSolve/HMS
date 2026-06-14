#!/bin/bash

# Script to run all three .NET services simultaneously with colored prefixes

cleanup() {
    # Remove the trap to avoid infinite loops when kill 0 triggers EXIT
    trap - EXIT INT TERM
    echo -e "\n\033[1;31m[System] Stopping all services...\033[0m"
    kill 0
}

# Trap SIGINT (Ctrl+C), SIGTERM, and EXIT
trap cleanup INT TERM EXIT

echo -e "\033[1;34m[System] Starting all services...\033[0m"

# 1. Start AuthAPI (Cyan)
dotnet run --project Application.Services.AuthAPI/Application.Services.AuthAPI.csproj 2>&1 | while read -r line; do
    printf "\033[36m[AuthAPI]\033[0m %s\n" "$line"
done &

# 2. Start HMS (Green)
dotnet run --project Application.Services.HMS/Application.Services.HMS.csproj 2>&1 | while read -r line; do
    printf "\033[32m[HMS]\033[0m %s\n" "$line"
done &

# 3. Start APIGateway (Magenta)
dotnet run --project Application.APIGateway/Application.APIGateway.csproj 2>&1 | while read -r line; do
    printf "\033[35m[APIGateway]\033[0m %s\n" "$line"
done &

echo -e "\033[1;32m[System] All services initiated in background. Press Ctrl+C to stop them.\033[0m"

# Wait for all background processes to exit (which keeps the script running)
wait
