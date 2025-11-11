using System.Data;
using Dapper;
using SimpleEcommerce.Domain.ValueObjects;

namespace SimpleEcommerce.Infrastructure.Persistence.TypeHandlers;

public sealed class PasswordTypeHandler : SqlMapper.TypeHandler<Password>
{
    public override void SetValue(IDbDataParameter parameter, Password? value) => parameter.Value = value?.Hash;
    public override Password Parse(object value) => Password.FromHash((string)value);
}