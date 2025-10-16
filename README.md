# turbo-enigma
Repo for DT Review. The name of the repo is the automatic name.

If you want to run this locally you can using the docker image:

create a directory and add the following two file: 

```
TestReview/
├── compose.yml
└── .env
```

You shoud then have this in your compose file:
```dockerfile

services:
  weathernow.api:
    image: stefanwilliamworrall/weathernow-api:latest
    container_name: weathernow.api
    ports:
      - "8080:8080"
    environment:
      - OpenWeatherMap__ApiKey
    networks:
      - weathernow-network
    restart: unless-stopped

  weathernow.client:
    image: stefanwilliamworrall/weathernow-client:latest
    container_name: weathernow.client
    ports:
      - "80:80"
    depends_on:
      - weathernow.api
    networks:
      - weathernow-network
    restart: unless-stopped

networks:
  weathernow-network:
    driver: bridge
```
You will also need to set up the environment variables: 
```
OpenWeatherMap__ApiKey=< Your API Key for Open Weather Map Services />
```
Afterwards you can then do the following command
```
#You might need to login for this, even though it is a public repo
docker login
docker compose --env-file .env up -d 
```

This should then start both the Client React App and the API localy.

http://localhost/ - React Client App URL 
http://localhost:8080/ - Root API URL
http://localhost:8080/swagger/index.html - Swagger for the Weather API