# Mock SCIM 2.0 Server
*A minimal SCIM target for testing user provisioning from Azure Entra ID, Okta, and Genesys Cloud.*

---

## Overview
This project implements a lightweight SCIM 2.0 API for testing and validating identity-provisioning flows.  
It supports the core SCIM User lifecycle (**Create**, **Read**, **Update**, **Delete**) and exposes the standard metadata endpoints.

---

## Features

### **SCIM 2.0 Endpoints**
- `/scim/v2/ServiceProviderConfig`
- `/scim/v2/Schemas`
- `/scim/v2/ResourceTypes`
- `/scim/v2/Users`

### **User Operations**
- List users (`startIndex`, `count`, and `userName eq "value"`)
- Get user by ID  
- Create user  
- PATCH user (replace operations)  
- Delete user  

### **Implementation**
- File-based persistence (`users.json`)
- Clean architecture (API → Application → Repository)
- SCIM-style error responses  
- Runs with `dotnet run` or Docker  

---

## Quick Start

### **Run locally**
```bash
dotnet run
