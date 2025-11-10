using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
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

        public async Task<bool> ExcluirProficienciaAsync(int anoLetivo, string codigoUe, SerieAnoIndiceDesenvolvimentoEnum serieAno, ComponenteCurricularEnum componenteCurricular)
        {
            string componenteCurricularStr = ((short)componenteCurricular).ToString();
            var query = "delete from proficiencia_ideb where ano_letivo = @anoLetivo and codigo_eol_escola = @CodigoUe and serie_ano = @SerieAno and componente_curricular = @componenteCurricular";

            await database.Conexao.ExecuteAsync(query, new { anoLetivo, codigoUe, serieAno, componenteCurricular = componenteCurricularStr });

            return true;
        }
    }
}
