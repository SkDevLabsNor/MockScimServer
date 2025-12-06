Mock SCIM 2.0 Server

A minimal SCIM target for testing user provisioning from Azure Entra ID, Okta, and Genesys Cloud.

Overview

This project implements a lightweight SCIM 2.0 API for testing and validating identity-provisioning flows.
It supports the core SCIM User lifecycle (Create, Read, Update, Delete) and exposes the standard metadata endpoints.

Useful for integration developers, IAM engineers, and teams working with SCIM-compatible SaaS platforms.

Features

SCIM 2.0 Endpoints

/scim/v2/ServiceProviderConfig

/scim/v2/Schemas

/scim/v2/ResourceTypes

/scim/v2/Users

User Operations

List (startIndex, count, simple filter: userName eq "value")

Get by ID

Create

PATCH (replace: userName, active, name, emails, externalId)

Delete

File-backed persistence via users.json

Clean architecture: API → Application → Repository

SCIM-style error responses (400, 404, 409)

Runs with dotnet run or Docker

Quick Start
1. Run locally
dotnet run


The server listens on http://localhost:5000 (or as configured).

2. Create a user
curl -X POST http://localhost:5000/scim/v2/Users \
  -H "Content-Type: application/json" \
  -d '{
        "userName": "alice",
        "active": true,
        "name": { "givenName": "Alice", "familyName": "Smith" },
        "emails": [{ "value": "alice@example.com", "primary": true }]
      }'

3. List users
curl http://localhost:5000/scim/v2/Users

4. Patch a user
curl -X PATCH http://localhost:5000/scim/v2/Users/{id} \
  -H "Content-Type: application/json" \
  -d '{
        "Operations": [
          {
            "op": "replace",
            "path": "active",
            "value": false
          }
        ]
      }'

5. Delete a user
curl -X DELETE http://localhost:5000/scim/v2/Users/{id}

Architecture (High-Level)
HTTP Request
   → UserEndpoints (API)
   → UserService (Application Logic)
   → IUserRepository (Abstraction)
   → FileUserRepository (JSON Persistence)
   → users.json


Utilities:

FilterParser — minimal SCIM filter support

PatchApplier — applies limited SCIM replace operations

ScimErrors — standardized SCIM error output

DTOs + JSON settings for consistent serialization

Configuration

appsettings.json:

{
  "UsersFile": "users.json",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}


Override with environment variables if needed.

Docker

Build and run:

docker build -t mock-scim-server .
docker run -p 5000:80 mock-scim-server

Use Cases

Test Azure Entra ID / Okta SCIM provisioning without external dependencies

Validate user lifecycle logic before integrating with real SaaS systems

Provide a controlled SCIM endpoint for QA and automated tests

Demonstrate SCIM knowledge in consulting and freelance work

Status

Version: 0.1
Functional mock implementation suitable for demos, development, and provisioning tests.
