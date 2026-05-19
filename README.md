# Review Backend

ASP.NET Core Web API for an asset review system. The API uses Identity bearer auth and API key auth for protected endpoints.

## About the API

- The API uses two auth mechanisms:
  - Identity bearer token for login and API key management.
  - API key (`Authorization: ApiKey <api-key-token>`) for protected resource access.
- Projects use membership-based access:
  - Project members can read and update project data.
  - Only the project creator (`CreatedByUserId`) can delete a project.
  - A delete attempt by a non-creator returns `403 Forbidden`.
  - TODO: Add RBAC for project membership.

## Prerequisites

- .NET 10 SDK
- A HTTP client for testing APIs (e.g. Postman, Insomnia, curl, etc.)

## Run Locally

1. Clone the repository:
   - `git clone <repo-url>`
   - `cd <repo-name>`
2. Restore dependencies:
   - `dotnet restore`
3. Start the API:
   - `dotnet run`
4. Open Swagger UI:
   - `https://localhost:7186`
   - (HTTP alternative: `http://localhost:5186`)

## Development Defaults

- The app runs with `ASPNETCORE_ENVIRONMENT=Development` via `launchSettings.json`.
- Database is currently in-memory, so data is reset when the app restarts.
- A seeded user is available in development:
  - Email: `john.doe@example.com`
  - Password: `password`

## Authentication Flow (Quick Start)

Use `http://localhost:5186` as `baseUrl` in the examples below.

1. Register a user:

```http
POST /register
Content-Type: application/json

{
  "email": "email@example.com",
  "password": "password"
}
```

2. Login and get bearer token (`accessToken` in response):

```http
POST /login
Content-Type: application/json

{
  "email": "email@example.com",
  "password": "password"
}
```

3. Create an API key (requires bearer token):

```http
POST /ApiKeys
Authorization: Bearer <access-token>
Content-Type: application/json

{
  "name": "my-first-key"
}
```

4. Use protected CRUD endpoints with API key:

```http
GET /Projects
Authorization: ApiKey <api-key-token>
```

```http
POST /Projects
Authorization: ApiKey <api-key-token>
Content-Type: application/json

{
  "name": "My Project"
}
```

```http
PUT /Projects/{projectId}
Authorization: ApiKey <api-key-token>
Content-Type: application/json

{
  "name": "My Updated Project"
}
```

```http
DELETE /Projects/{projectId}
Authorization: ApiKey <api-key-token>
```

## Run With Postman

Use the provided Postman files to run the same end-to-end flow as `Review.Api.http`.

1. Start the API locally (`dotnet run`).
2. In Postman, import:
   - `Review.Api.postman_collection.json`
   - `Review.Api.postman_environment.json`
3. Select the environment `Review API Local`.
4. Update environment values for your setup:
   - `baseUrl` (default: `http://localhost:5186`)
   - `Email` and `Password`
   - `OtherEmail` and `OtherPassword`
5. Run the collection with Collection Runner from top to bottom.

Notes:

- Requests are ordered and chained by collection variables (tokens and IDs are captured automatically in test scripts).
- If your local API runs on another port, only `baseUrl` needs to be changed.

---

## Assignment

[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/fwqWTTOB)

### Inlämningsuppgift

Detta projekt är startpunkten för din inlämingsuppgift. Ni behöver skapa ett eget api som är skyddat med hjälp av api-nycklar. Ni skall definiera resurser (data) som skall kunna skickas från ert api efter att en användare gör ett korrekt anrop och inkluderar sin api-nyckel.

#### Användarregistrering

- [x] En användare ska kunna registrera sig i systemet.
- [x] Användaren ska kunna logga in.
- [x] Du väljer inloggningsstrategi:
  - [x] Individuella konton (med e-post och lösenord)

#### API-nyckel

- [x] Efter registrering och inloggning ska användaren kunna begära en API-nyckel.
- [x] API-nyckeln ska sparas i databasen och kopplas till användaren.

#### Skyddade API-slutpunkter med CRUD-funktionalitet

- [x] Du ska skapa minst en resurs (t.ex. recept, sportresultat, speldata, personliga anteckningar) som användaren kan hantera via CRUD:
  - [x] Create – Lägga till data.
  - [x] Read – Hämta data.
  - [x] Update – Ändra befintlig data.
  - [x] Delete – Ta bort data.
- [x] Alla CRUD-operationer ska kräva giltig API-nyckel.
- [x] API-nyckeln ska skickas med i anropet och valideras innan data returneras eller ändras.

#### Databas

- [x] Du väljer:
  - [x] Entity Framework + SQL

#### Betygsättning

Denna uppgift bedöms med IG (icke godkänd), G (godkänd) och VG (Väl Godkänt).

##### För godkänt (G) krävs:

- [x] Användare kan registrera sig och logga in.
- [x] Användare kan begära och få en API-nyckel.
- [x] CRUD-funktionalitet finns för vald resurs och är skyddad med API-nyckel.
- [x] API-nyckeln valideras korrekt vid varje anrop.
- [x] Databasen fungerar enligt vald lösning (SQL med EF).
- [x] Ni använder kontroller som endpoints och hanterar logiken i dessa.
- [x] Korrekta svarskoder skickas från ditt API.

##### För Väl godkänt (VG) krävs:

- [x] Samtliga punkter från G
- [x] Ni har valt en komplex struktur av data att returnera och använder er av DTO:er för att begränsa informationen.
- [x] Ni använder designmönster med tjänster och repositories
- [x] Ni använder korrekt validering och har skapat minst en egen validering (custom validation).
- [x] En fungerande Swagger

#### Inlämning

- [ ] En länk till ett GitHub-repo på itslearning
- [ ] Bifoga en README.md som beskriver:
  - [x] Hur projektet startas.
  - [x] Hur man registrerar en användare och får en API-nyckel.
  - [x] Exempel på anrop till de skyddade CRUD-slutpunkterna.
