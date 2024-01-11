using System.Text.Json;
using JsonSchema.Tests.Commons;

namespace JsonSchema.Tests;

public class JsonSchemaGeneratorTests
{
    [Fact]
    public void Should_GenerateFor_User()
    {
        var json = JsonSchemaGenerator.GenerateFor<User>();

        var result = JsonSerializer.Deserialize<User>(json);
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public void Should_GenerateFor_User_WithCustomMapper()
    {
        var json = JsonSchemaGenerator.GenerateFor<User>(new CustomTypeMapper());

        var result = JsonSerializer.Deserialize<object>(json);
        
        Assert.NotNull(result);
    }
}