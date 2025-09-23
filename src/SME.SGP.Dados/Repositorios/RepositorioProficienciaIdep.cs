using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioProficienciaIdep : RepositorioBase<ProficienciaIdep>, IRepositorioProficienciaIdep
    {
        public RepositorioProficienciaIdep(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<bool> ExcluirPorAnoEscolaSerie(int anoLetivo, string CodigoEOLEscola, long SerieAno)
        {
            var query = "delete from proficiencia_idep where ano_letivo = @anoLetivo and codigo_eol_escola = @CodigoEOLEscola and serie_ano = @SerieAno";

            await database.Conexao.ExecuteAsync(query, new { anoLetivo, CodigoEOLEscola, SerieAno });

            return true;
        }
    }
}
