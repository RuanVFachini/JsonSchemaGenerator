namespace JsonSchema.Tests.Commons;

public class User {
    public Guid? Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }
    public IEnumerable<Address> Addresses { get; set; }
    public Address OtherAddress { get; set; }
}

public class Address
{
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
}