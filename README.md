# Mock SCIM 2.0 Server

A minimal SCIM implementation for testing user provisioning flows from Azure Entra ID, Okta, and Genesys Cloud.

## Overview

This project provides a lightweight SCIM 2.0 API supporting the basic user lifecycle:

- Create
- Read
- Update (PATCH)
- Delete

It exposes standard SCIM metadata endpoints and stores user data in a local `users.json` file.

## SCIM Endpoints

### Metadata
- /scim/v2/ServiceProviderConfig
- /scim/v2/Schemas
- /scim/v2/ResourceTypes

### Users
- /scim/v2/Users
- /scim/v2/Users/{id}

Supported features:
- Filter: userName eq "value"
- PATCH replace operations:
  - userName
  - active
  - name.givenName
  - name.familyName
  - emails
  - externalId

## Running

dotnet run

Default URL:
http://localhost:5000

## Architecture (Short)

Endpoints → UserService → IUserRepository → FileUserRepository → users.json

## Purpose

- Test SCIM provisioning locally
- Validate identity flows
- Develop integrations without external dependencies
