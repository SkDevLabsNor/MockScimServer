namespace MockScimServer.Domain;

public class ScimUser
{
    public string[] Schemas { get; set; } = { "urn:ietf:params:scim:schemas:core:2.0:User" };
    public string Id { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public ScimName Name { get; set; } = new();
    public List<ScimEmail> Emails { get; set; } = new();
}

public class ScimName
{
    public string GivenName { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
}

public class ScimEmail
{
    public string Value { get; set; } = string.Empty;
    public string Type { get; set; } = "work";
    public bool Primary { get; set; } = false;
}
