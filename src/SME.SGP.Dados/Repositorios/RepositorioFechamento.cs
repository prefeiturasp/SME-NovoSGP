using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Linq;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamento : RepositorioBase<Fechamento>, IRepositorioFechamento
    {
        public RepositorioFechamento(ISgpContext conexao) : base(conexao)
        {
        }

        public Fechamento ObterPorTipoCalendarioDreEUE(long tipoCalendarioId, string dreId, string ueId)
        {
            var query = new StringBuilder("select f.id as fechamento, f.*, fb.id as fechamento_bimestre,");
            query.AppendLine("fb.*, p.id as periodo_escolar, p.*, t.id as tipo_calendario, t.*");
            query.AppendLine("from");
            query.AppendLine("fechamento f");
            query.AppendLine("inner join fechamento_bimestre fb on");
            query.AppendLine("f.fechamento_bimestre_id = fb.id");
            query.AppendLine("inner join periodo_escolar p on");
            query.AppendLine("fb.periodo_escolar_id = p.id");
            query.AppendLine("inner join tipo_calendario t on");
            query.AppendLine("p.tipo_calendario_id = t.id");
            query.AppendLine("where 1=1");
            query.AppendLine("and p.tipo_calendario_id = @tipoCalendarioId");

            if (!string.IsNullOrWhiteSpace(dreId))
                query.AppendLine("and f.dre_id = @dreId");
            else query.AppendLine("and f.dre_id is null");

            if (!string.IsNullOrWhiteSpace(ueId))
                query.AppendLine("and f.ue_id = @ueId");
            else query.AppendLine("and f.ue_id is null");

            return database.Conexao.Query<Fechamento, FechamentoBimestre, PeriodoEscolar, TipoCalendario, Fechamento>(query.ToString(), (fechamento, fechamentoBimestre, periodoEscolar, tipoCalendario) =>
               {
                   periodoEscolar.AdicionarTipoCalendario(tipoCalendario);
                   fechamento.AdicionarFechamentoBimestre(periodoEscolar, fechamentoBimestre);
                   return fechamento;
               }, new
               {
                   tipoCalendarioId,
                   dreId,
                   ueId
               },
                splitOn: "fechamento,fechamento_bimestre,periodo_escolar, tipo_calendario").FirstOrDefault();
        }
    }
}