using System.Data;
using Dapper;
using SimpleEcommerce.Domain.ValueObjects;

namespace SimpleEcommerce.Infrastructure.Persistence.TypeHandlers;

public sealed class EmailTypeHandler : SqlMapper.TypeHandler<Email>
{
    public override void SetValue(IDbDataParameter parameter, Email? value) => parameter.Value = value?.Address;
    public override Email Parse(object value) => new((string)value);
}