using FluentMigrator;

namespace SimpleEcommerce.Infrastructure.Migrations;

[Migration(202511180002, "Create brands, categories, products")]
public sealed class CreateBrandCategoryProduct : Migration
{
    public override void Up()
    {
        Create.Table("brands")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsCustom("text").NotNullable()
            .WithColumn("slug").AsCustom("text").NotNullable()
            .WithColumn("created_at").AsCustom("timestamptz").NotNullable()
            .WithColumn("updated_at").AsCustom("timestamptz").Nullable();

        Create.Index("ux_brands_slug").OnTable("brands").OnColumn("slug").Ascending().WithOptions().Unique();

        Create.Table("categories")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("name").AsCustom("text").NotNullable()
            .WithColumn("slug").AsCustom("text").NotNullable()
            .WithColumn("parent_category_id").AsGuid().Nullable()
            .WithColumn("created_at").AsCustom("timestamptz").NotNullable()
            .WithColumn("updated_at").AsCustom("timestamptz").Nullable();

        Create.Index("ux_categories_slug").OnTable("categories").OnColumn("slug").Ascending().WithOptions().Unique();

        Create.ForeignKey("fk_categories_parent")
            .FromTable("categories").ForeignColumn("parent_category_id")
            .ToTable("categories").PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.SetNull);

        Create.Table("products")
            .WithColumn("id").AsGuid().PrimaryKey()
            .WithColumn("sku").AsCustom("text").NotNullable()
            .WithColumn("name").AsCustom("text").NotNullable()
            .WithColumn("description").AsCustom("text").NotNullable()
            .WithColumn("price_amount").AsDecimal(18, 2).NotNullable()
            .WithColumn("price_currency").AsString(3).NotNullable()
            .WithColumn("stock").AsInt32().NotNullable()
            .WithColumn("slug").AsCustom("text").NotNullable()
            .WithColumn("brand_id").AsGuid().NotNullable()
            .WithColumn("category_id").AsGuid().NotNullable()
            .WithColumn("status").AsInt32().NotNullable().WithDefaultValue(0)
            .WithColumn("created_at").AsCustom("timestamptz").NotNullable()
            .WithColumn("updated_at").AsCustom("timestamptz").Nullable();

        Create.Index("ux_products_sku").OnTable("products").OnColumn("sku").Ascending().WithOptions().Unique();
        Create.Index("ux_products_slug").OnTable("products").OnColumn("slug").Ascending().WithOptions().Unique();
        Create.Index("idx_products_brand").OnTable("products").OnColumn("brand_id");
        Create.Index("idx_products_category").OnTable("products").OnColumn("category_id");
        Create.Index("idx_products_status").OnTable("products").OnColumn("status");

        Create.ForeignKey("fk_products_brand")
            .FromTable("products").ForeignColumn("brand_id")
            .ToTable("brands").PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.None);

        Create.ForeignKey("fk_products_category")
            .FromTable("products").ForeignColumn("category_id")
            .ToTable("categories").PrimaryColumn("id")
            .OnDeleteOrUpdate(System.Data.Rule.None);
    }

    public override void Down()
    {
        Delete.ForeignKey("fk_products_category").OnTable("products");
        Delete.ForeignKey("fk_products_brand").OnTable("products");
        Delete.Table("products");

        Delete.ForeignKey("fk_categories_parent").OnTable("categories");
        Delete.Table("categories");

        Delete.Table("brands");
    }
}
