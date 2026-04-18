# Review Backend

ASP.NET Core Web API for an asset review system. The API uses Identity bearer auth and API key auth for protected endpoints.

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

1. Login (or register) using Identity endpoints:
   - `POST /login`
   - `POST /register`
2. Create an API key with a bearer token:
   - `POST /ApiKeys`
   - `DELETE /ApiKeys/{keyId}`
3. Access protected resources:
   - `GET /Projects`
   - `POST /Projects`
   - `GET /Projects/{projectId}`
   - `PUT /Projects/{projectId}`
   - `DELETE /Projects/{projectId}`
4. To use API key auth, send:
   - `Authorization: ApiKey <api-key-token>`

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

- [ ] Efter registrering och inloggning ska användaren kunna begära en API-nyckel.
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

- [ ] Du väljer:
  - [ ] Entity Framework + SQL

#### Betygsättning

Denna uppgift bedöms med IG (icke godkänd), G (godkänd) och VG (Väl Godkänt).

##### För godkänt (G) krävs:

- [x] Användare kan registrera sig och logga in.
- [x] Användare kan begära och få en API-nyckel.
- [x] CRUD-funktionalitet finns för vald resurs och är skyddad med API-nyckel.
- [x] API-nyckeln valideras korrekt vid varje anrop.
- [ ] Databasen fungerar enligt vald lösning (SQL med EF).
- [x] Ni använder kontroller som endpoints och hanterar logiken i dessa.
- [ ] Korrekta svarskoder skickas från ditt API.

##### För Väl godkänt (VG) krävs:

- [ ] Samtliga punkter från G
- [ ] Ni har valt en komplex struktur av data att returnera och använder er av DTO:er för att begränsa informationen.
- [ ] Ni använder designmönster med tjänster och repositories
- [ ] Ni använder korrekt validering och har skapat minst en egen validering (custom validation).
- [ ] En fungerande Swagger

#### Inlämning

- [ ] En länk till ett GitHub-repo på itslearning
- [ ] Bifoga en README.md som beskriver:
  - [ ] Hur projektet startas.
  - [ ] Hur man registrerar en användare och får en API-nyckel.
  - [ ] Exempel på anrop till de skyddade CRUD-slutpunkterna.
