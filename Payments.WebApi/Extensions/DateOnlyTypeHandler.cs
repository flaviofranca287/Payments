using System.Data;
using Dapper;

namespace Payments.WebApi.Extensions;

public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
    {
        // Converte o valor vindo do banco (DateTime) para DateOnly
        return DateOnly.FromDateTime((DateTime)value);
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        // Converte o DateOnly para DateTime ao enviar para o banco
        parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        parameter.DbType = DbType.Date;
    }
}
