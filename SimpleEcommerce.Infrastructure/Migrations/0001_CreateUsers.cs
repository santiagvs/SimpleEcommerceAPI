using FluentMigrator;

namespace SimpleEcommerce.Infrastructure.Migrations;

[Migration(202511180001, "Create users table")]
public sealed class CreateUsers : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("email").AsCustom("text").NotNullable()
            .WithColumn("password_hash").AsCustom("text").NotNullable()
            .WithColumn("first_name").AsCustom("text").NotNullable()
            .WithColumn("last_name").AsCustom("text").NotNullable()
            .WithColumn("role").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("created_at").AsCustom("timestamptz").NotNullable()
            .WithColumn("updated_at").AsCustom("timestamptz").Nullable();

        Create.Index("ux_users_email").OnTable("users").OnColumn("email").Ascending().WithOptions().Unique();
    }

    public override void Down()
    {
        Delete.Table("users");
    }
}
