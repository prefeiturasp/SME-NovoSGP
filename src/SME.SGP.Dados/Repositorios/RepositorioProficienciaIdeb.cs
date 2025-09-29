using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioProficienciaIdeb : RepositorioBase<ProficienciaIdeb>, IRepositorioProficienciaIdeb
    {
        public RepositorioProficienciaIdeb(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<bool> ExcluirPorAnoEscolaSerie(int anoLetivo, string codigoEOLEscola, long serieAno)
        {
            var query = "delete from proficiencia_ideb where ano_letivo = @anoLetivo and codigo_eol_escola = @CodigoEOLEscola and serie_ano = @SerieAno";

            await database.Conexao.ExecuteAsync(query, new { anoLetivo, codigoEOLEscola, serieAno });

            return true;
        }
    }
}
