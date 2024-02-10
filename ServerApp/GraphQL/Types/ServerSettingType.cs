public class ServerSetting
{
    public string? Key { get; set; }
    public string? Value { get; set; }
}

public class ServerSettingType : ObjectType<ServerSetting>
{
    protected override void Configure(IObjectTypeDescriptor<ServerSetting> descriptor)
    {
        descriptor.Field(t => t.Key).Type<NonNullType<StringType>>();
        descriptor.Field(t => t.Value).Type<StringType>();
    }
}