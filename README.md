Mock SCIM 2.0 Server

A minimal SCIM target for testing user provisioning from Azure Entra ID, Okta, and Genesys Cloud.

Overview

This project implements a lightweight SCIM 2.0 API for testing and validating identity-provisioning flows.
It supports the core SCIM User lifecycle (Create, Read, Update, Delete) and exposes the standard metadata endpoints.

This is useful for integration developers, IAM engineers, and teams working with SCIM-compatible SaaS platforms.

Features
SCIM 2.0 Endpoints

/scim/v2/ServiceProviderConfig

/scim/v2/Schemas

/scim/v2/ResourceTypes

/scim/v2/Users

User Operations

List (supports startIndex, count, userName eq "value")

Get by ID

Create

PATCH (replace operations)

Delete

Implementation Notes

File-based persistence (users.json)

Clean architecture (API → Application → Repository)

SCIM-style error responses (400/404/409)

Works with dotnet run or Docker

Quick Start
Run locally
dotnet run


Server runs on:
http://localhost:5000

Create a user
curl -X POST http://localhost:5000/scim/v2/Users \
  -H "Content-Type: application/json" \
  -d '{
        "userName": "alice",
        "active": true,
        "name": { "givenName": "Alice", "familyName": "Smith" },
        "emails": [{ "value": "alice@example.com", "primary": true }]
      }'

List users
curl http://localhost:5000/scim/v2/Users

Patch a user
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

Delete a user
curl -X DELETE http://localhost:5000/scim/v2/Users/{id}

Architecture (High-Level)
HTTP
  → API Endpoints
  → UserService (application logic)
  → IUserRepository (abstraction)
  → FileUserRepository (JSON persistence)
  → users.json


Supporting components:

FilterParser — minimal SCIM filter parsing

PatchApplier — applies SCIM patch ops

ScimErrors — consistent SCIM error responses

DTOs & JSON settings

Configuration

appsettings.json example:

{
  "UsersFile": "users.json",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}

Docker

Build and run:

docker build -t mock-scim-server .
docker run -p 5000:80 mock-scim-server

Use Cases

Test Azure Entra ID or Okta SCIM provisioning

Validate SCIM user lifecycle flows offline

Build automated provisioning tests

Demonstrate SCIM expertise in consulting engagements

Status

v0.1 — Minimal functional mock implementation
Suitable for demos, development, and identity provisioning tests.
