### Final Project — .NET MAUI Notes App + ASP.NET Core Web API (MySQL)

This repository contains a simple Notes application consisting of:

- A .NET MAUI client (Android + Desktop) using MVVM and data binding
- An ASP.NET Core Web API backed by MySQL, runnable via Docker

Locations:
- MAUI app: `Final_Project/Maui_App`
- API: `Final_Project/Api`

---

### Features
- MVVM with data binding throughout
- Two pages with navigation:
  - `NotesPage`: list, search, delete
  - `AddEditNotePage`: create and edit
- Search notes by title/content
- Full CRUD via REST to the API
- API uses EF Core (Pomelo MySQL), CORS (dev), and seeds demo data on first run

---

### Prerequisites
- Docker + Docker Compose
- .NET 8 SDK (for running/building MAUI and API locally)
- For MAUI: IDE with MAUI workload (Visual Studio 2022 or JetBrains Rider)

---

### Quick start
1) From the project root, start the API and MySQL using Docker:

```
docker compose up --build
```

This will:
- Start MySQL (db: `notesdb`, user: `notes`, password: `notespwd`)
- Build and run the API on `http://localhost:8080` with Swagger at `/swagger`

2) Run the MAUI app:
- Desktop (MacCatalyst/Windows): open `Final_Project/Maui_App` in your IDE and run.
- Android Emulator: run the MAUI app on the emulator. The app auto-uses `http://10.0.2.2:8080` for the API on Android and `http://localhost:8080` on desktop.

---

### How to use
- Notes page displays notes sorted by date. Pull-to-refresh by navigating back to the page.
- Use the SearchBar to filter by title or content.
- Use row context actions to Edit or Delete.
- Tap "Add Note" to create a new note; Save to persist via the API.

---

### Project structure (key files)
- API (`Final_Project/Api`)
  - `Program.cs`: EF Core, CORS, Swagger, DB seeding
  - `Controllers/NotesController.cs`: async CRUD + `GET /notes/search?query=`
  - `Models/NotesDbContext.cs`, `Models/Note.cs`
  - `Dockerfile`
- Root
  - `docker-compose.yml`: API + MySQL stack
- MAUI (`Final_Project/Maui_App`)
  - `MauiProgram.cs`: DI and platform‑aware API base URL
  - `Services/INotesApiService.cs`, `Services/NotesApiService.cs`
  - `ViewModels/NotesViewModel.cs`, `ViewModels/AddEditNoteViewModel.cs`
  - `Views/NotesPage.xaml(+.cs)`, `Views/AddEditNotePage.xaml(+.cs)`

---

### Configuration notes
- API connection string (MySQL) can be set via `Api/appsettings.json` or `docker-compose.yml` env var `ConnectionStrings__MySql`.
- In development, CORS is wide open for ease of testing.
- DB schema is created with `EnsureCreated()` for demo simplicity. For production, use EF Core migrations.

---

### API endpoints
- `GET /notes` — list notes (newest first)
- `GET /notes/{id}` — get by id
- `POST /notes` — create
- `PUT /notes/{id}` — update
- `DELETE /notes/{id}` — delete
- `GET /notes/search?query=...` — search in title/content


#### GET all notes
```bash
curl -X GET "http://localhost:8080/notes"
```
#### GET a note by ID
```bash
curl -X GET "http://localhost:8080/notes/1"
```

#### POST create a new note
```bash
curl -X POST "http://localhost:8080/notes" \
-H "Content-Type: application/json" \
-d '{
"title": "My Test Note",
"content": "This is a note created via curl."
}'
```

#### PUT update an existing note
```bash
curl -X PUT "http://localhost:8080/notes/1" \
-H "Content-Type: application/json" \
-d '{
"title": "Updated title",
"content": "Updated content",
"date": "2025-02-01T12:00:00Z"
}'
```

#### DELETE a note
```bash
curl -X DELETE "http://localhost:8080/notes/1"
```

#### SEARCH notes
```bash
curl -X GET "http://localhost:8080/notes/search?query=project"
```

---

### Troubleshooting
- Android device (not emulator) cannot reach `localhost`: point the base URL to your machine IP (same network).
- Port conflicts: change host ports in `docker-compose.yml` or stop conflicting services.
- MySQL permissions or startup delays: if API starts before DB is ready, restart the API container.

---

### TODO
- notes search MAUI not working and/or showing results. API works.
- notes details page. Nothing happens on tap for notes list
- macos catalyst check
- dark theme compat