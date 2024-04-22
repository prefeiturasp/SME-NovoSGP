using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoMapeamentoEstudante : RepositorioBase<SecaoMapeamentoEstudante>, IRepositorioSecaoMapeamentoEstudante
    {

        public RepositorioSecaoMapeamentoEstudante(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        { }

        public async Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesQuestionarioDto(long? mapeamentoEstudanteId = null)
        {
            var query = @"SELECT sme.id
                                , sme.nome
                                , sme.questionario_id as questionarioId
                                , mes.concluido
                                , sme.etapa
                                , sme.ordem
                                , sme.nome_componente as nomeComponente
                         FROM secao_mapeamento_estudante sme 
                         inner join questionario q on q.id = sme.questionario_id 
                         left join mapeamento_estudante_secao mes on mes.mapeamento_estudante_id = @mapeamentoEstudanteId 
                                                                 and mes.secao_mapeamento_estudante_id = sme.id
                                                                 and not mes.excluido   
                         WHERE not sme.excluido 
                               AND q.tipo = @tipoQuestionario
                         ORDER BY sme.etapa, sme.ordem ";

            return await database.Conexao.QueryAsync<SecaoQuestionarioDto>(query, new { tipoQuestionario = (int)TipoQuestionario.MapeamentoEstudante, mapeamentoEstudanteId = mapeamentoEstudanteId ?? 0 });
        }

        public async Task<IEnumerable<SecaoMapeamentoEstudante>> ObterSecoesMapeamentoEstudante(long? mapeamentoEstudanteId = null)
        {
            var query = new StringBuilder(@"SELECT sme.*, mes.*, q.*
                                            FROM secao_mapeamento_estudante sme 
                                                join questionario q on q.id = sme.questionario_id 
                                                left join mapeamento_estudante_secao mes on mes.mapeamento_estudante_id = @mapeamentoEstudanteId
                                                                                        and mes.secao_mapeamento_estudante_id = sme.id 
                                                                                        and not mes.excluido  
                                            WHERE not sme.excluido 
                                            ORDER BY sme.etapa, sme.ordem; ");

            return await database.Conexao
                .QueryAsync<SecaoMapeamentoEstudante, MapeamentoEstudanteSecao, Questionario, SecaoMapeamentoEstudante>(
                    query.ToString(), (secaoMapeamentoEstudante, mapeamentoEstudanteSecao, questionario) =>
                    {
                        secaoMapeamentoEstudante.MapeamentoEstudanteSecao = mapeamentoEstudanteSecao;
                        secaoMapeamentoEstudante.Questionario = questionario;
                        return secaoMapeamentoEstudante;
                    }, new { mapeamentoEstudanteId = mapeamentoEstudanteId ?? 0 }, splitOn: "id");
        }
    }
}
