Mock SCIM 2.0 Server

A minimal SCIM implementation for testing user provisioning flows from Azure Entra ID, Okta, and Genesys Cloud.

Overview

This project provides a lightweight SCIM 2.0 API with support for basic user lifecycle operations:

Create

Read

Update (PATCH)

Delete

It exposes standard SCIM metadata endpoints and stores data in a local JSON file to simplify testing and development.
The server is intended for learning, prototyping, and validating SCIM integrations without requiring access to a real SCIM-capable platform.

Requirements

.NET 8 SDK or later

Any REST client (curl, Postman, Thunder Client, etc.)

SCIM Endpoints
Metadata

/scim/v2/ServiceProviderConfig

/scim/v2/Schemas

/scim/v2/ResourceTypes

Users

/scim/v2/Users

/scim/v2/Users/{id}

Supported features:

Filter: userName eq "value"

PATCH (replace):

userName

active

name.givenName

name.familyName

emails

externalId

User data is stored in users.json at the project root unless configured otherwise.

Running the Server
dotnet run


The server starts at:

http://localhost:5000


You can verify it is running by visiting:

http://localhost:5000/scim/v2/ServiceProviderConfig

Configuration

The server loads configuration from appsettings.json.

Default:

{
  "UsersFile": "users.json"
}


To change the location of the user storage file, update the UsersFile value or override it through environment variables.

Project Structure
API Endpoints
  → UserEndpoints
  → MetadataEndpoints

Application Layer
  → UserService
  → FilterCriteria / UserListResult
  → PatchApplier

Domain Model
  → ScimUser / ScimName / ScimEmail

Infrastructure
  → IUserRepository
  → FileUserRepository (JSON storage)

Utils
  → FilterParser
  → ScimErrors
  → ScimJson
  → JsonSettings


High-level flow:

HTTP Request
  → Endpoint
    → UserService
      → IUserRepository
        → FileUserRepository
          → users.json

Development Notes

No authentication is implemented.

Only a small subset of SCIM filtering and PATCH operations is supported.

This is not intended for production use.

The goal is to provide a simple, inspectable reference implementation and a mock target for SCIM provisioning tests.

Use Cases

Testing SCIM provisioning from Azure Entra ID or Okta

Experimenting with SCIM user lifecycle flows

Local development without external dependencies

Demonstrating SCIM understanding for integration or consulting work
