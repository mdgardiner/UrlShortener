# URL Shortener

A url shortener application build with an dot net core API backend that uses EF Core to integrate with a Postgres database. A front-end application is provided written in React to use the backend api, allow urls to be shortened and to allow redirection to a shortened url.

All components are containerised and the system can be run locally on a machine using the `docker-compose.yml' included.

## Directory Structure

```
UrlShortener/
├── UrlShortener.API/
│   ├── [Source Folders/Files]
│   ├── URLShortener.API.csproj
│   └── Dockerfile
├── UrlShortener.API.Tests/
│   ├── [Source Folders/Files]
│   └── UrlShortener.API.Tests
├── UrlShortener.Frontend/
│   └── url-shortener-app/
│       ├── [Source Folders/Files]
│       └── Dockerfile
├── docker-compose.yml
├── README.md
└── UrlShortener.sln
```

- Open the `UrlShortner.sln` in a suitable IDE to view the backend code
- The API has a unit test project that test the services classes provided in the solution
- The frontend source code can be found in the `UrlShortener.Frontend/url-shortener-app` folder

## Running the solution

On a host machine with docker desktop installed, `cd` into the folder where you have cloned the code - you should be in the folder containing the `docker-compose.yml` - and run in a command line/terminal:

```
> docker compose build
> docker compose up
```

In a browser, navigate to `http://localhost:3000`.

On the landing page enter a url to be shortened into the text input and then press the "Shorten URL" button. The generated short url will be displayed on the page as a link.

If you click the url or paste it into the url bar and go to it the browser should be redirected to the original url.

## Notes and future improvements

### Notes

- EF Core database migrations are automatically run at startup when the project is in `Development` mode
- The Postgres database is exposed locally on port 6000 so it can be viewed using a suitable client
- The Backend API is exposed locally on port 8080 - swagger is available in `Development` mode at [http://localhost:8080/swagger](http://localhost:8080/swagger)
- The React Frontend is exposed locally on port 3000 [http://localhost:3000](http://localhost:3000)

### Future improvements

- The database needs tuning with indexes to improve performance
- The retrieval of the shortened urls could be improved by introducing a caching layer with Redis or similar to improve performance
- Needs work to use HTTPS rather than HTTP
