using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDashBoardBuscaAtiva : IRepositorioDashBoardBuscaAtiva
    {
        private readonly ISgpContextConsultas database;
        private const string NOME_COMPONENTE_QUESTAO_JUSTIFICATIVA_MOTIVO_FALTA = "JUSTIFICATIVA_MOTIVO_FALTA";

        public RepositorioDashBoardBuscaAtiva(ISgpContextConsultas database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<DadosGraficoMotivoAusenciaBuscaAtivaDto>> ObterDadosGraficoMotivoAusencia(int anoLetivo, Modalidade modalidade, long? ueId, long? dreId, int? semestre)
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"select count(ba_resposta.id) as quantidade, 
                                    or2.nome as MotivoAusencia
                             from registro_acao_busca_Ativa ba
                             inner join registro_acao_busca_ativa_secao ba_secao on ba_secao.registro_acao_busca_ativa_id = ba.id 
                             inner join registro_acao_busca_ativa_questao ba_questao on ba_questao.registro_acao_busca_ativa_secao_id = ba_secao.id 
                             inner join registro_acao_busca_ativa_resposta ba_resposta on ba_resposta.questao_registro_acao_id = ba_questao.id 
                             inner join questao q on q.id = ba_questao.questao_id 
                             inner join opcao_resposta or2 on or2.id = ba_resposta.resposta_id 
                             inner join turma t on t.id = ba.turma_id 
                             inner join ue u on u.id = t.ue_id 
                               where q.nome_componente  = @nomeComponenteQuestao
                                     and not ba.excluido 
                                     and not ba_secao.excluido 
                                     and not ba_questao.excluido 
                                     and not ba_resposta.excluido
                                     and t.ano_letivo = @anoLetivo
                                     and t.modalidade_codigo = @modalidade");
            if (ueId.NaoEhNulo())
                sql.AppendLine(@"    and t.ue_id= @ueId ");
            if (dreId.NaoEhNulo())
                sql.AppendLine(@"    and u.dre_id = @dreId ");
            if (semestre.NaoEhNulo())
                sql.AppendLine(@"    and t.semestre = @semestre ");
            sql.AppendLine(@"group by or2.nome;");
            return await database.Conexao
                                 .QueryAsync<DadosGraficoMotivoAusenciaBuscaAtivaDto>(sql.ToString(), 
                                                  new { anoLetivo, modalidade = (int)modalidade,
                                                        nomeComponenteQuestao = NOME_COMPONENTE_QUESTAO_JUSTIFICATIVA_MOTIVO_FALTA,
                                                        ueId, dreId, semestre }, commandTimeout: 60);
        }
    }
}
