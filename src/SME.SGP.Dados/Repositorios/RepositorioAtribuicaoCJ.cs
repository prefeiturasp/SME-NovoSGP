using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtribuicaoCJ : RepositorioBase<AtribuicaoCJ>, IRepositorioAtribuicaoCJ
    {
        public RepositorioAtribuicaoCJ(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<AtribuicaoCJ>> ObterPorComponenteTurmaModalidadeUe(Modalidade? modalidade, string turmaId, string ueId, long componenteCurricularId)
        {
            var query = new StringBuilder();

            query.AppendLine("select *");

            query.AppendLine("from");
            query.AppendLine("atribuicao_cj a");
            query.AppendLine("inner join componente_curricular cc");
            query.AppendLine("on a.componente_curricular_id = cc.id");
            query.AppendLine("where");

            if (modalidade.HasValue)
                query.AppendLine("a.modalidade = @modalidade");

            if (!string.IsNullOrEmpty(ueId))
                query.AppendLine("and a.ue_id = @ueId");

            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("and a.turma_id = @turmaId");

            if (componenteCurricularId > 0)
                query.AppendLine("and a.componente_curricular_id = @componenteCurricularId");

            return (await database.Conexao.QueryAsync<AtribuicaoCJ, ComponenteCurricular, AtribuicaoCJ>(query.ToString(), (atribuicaoCJ, componenteCurricular) =>
            {
                atribuicaoCJ.ComponenteCurricular = componenteCurricular;
                return atribuicaoCJ;
            }, new
            {
                modalidade = (int)modalidade,
                ueId,
                turmaId,
                componenteCurricularId
            }, splitOn: "Id, Id"));
        }
    }
}