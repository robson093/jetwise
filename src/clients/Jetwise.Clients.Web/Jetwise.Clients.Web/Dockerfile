# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Jetwise.Clients.Web.csproj", "."]
RUN dotnet restore "Jetwise.Clients.Web.csproj"
COPY . .
RUN dotnet publish "Jetwise.Clients.Web.csproj" -c Release -o /app/publish

# Runtime stage (nginx)
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/conf.d/default.conf
# Optional: custom nginx.conf if you need SPA routing (Blazor requires it for deep links!)
# COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]