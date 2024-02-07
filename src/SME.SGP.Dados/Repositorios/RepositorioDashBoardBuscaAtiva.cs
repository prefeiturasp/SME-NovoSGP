using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDashBoardBuscaAtiva : IRepositorioDashBoardBuscaAtiva
    {
        private readonly ISgpContextConsultas database;
        private const string NOME_COMPONENTE_QUESTAO_JUSTIFICATIVA_MOTIVO_FALTA = "JUSTIFICATIVA_MOTIVO_FALTA";
        private const string NOME_COMPONENTE_QUESTAO_PROCEDIMENTO_TRABALHO_CONTATOU_RESP = "PROCEDIMENTO_REALIZADO";
        private const string NOME_COMPONENTE_QUESTAO_PROCEDIMENTO_TRABALHO_NAO_CONTATOU_RESP = "PROCEDIMENTO_REALIZADO_NAO_CONTATOU_RESP";

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

        public async Task<IEnumerable<DadosGraficoProcedimentoTrabalhoDreBuscaAtivaDto>> ObterDadosGraficoProcedimentoTrabalho(EnumProcedimentoTrabalhoBuscaAtiva tipoProcedimentoRealizado, int anoLetivo, Modalidade modalidade, long? ueId, long? dreId, int? semestre)
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"select count(ba_resposta.id) as quantidade, 
                                    case when q.nome_componente = @nomeComponenteQuestao_ContatoRealizado then 'Com sucesso' 
                                         else 'Sem sucesso' end as RealizouProcedimentoTrabalho,
                                    d.abreviacao as Dre
                             from registro_acao_busca_Ativa ba
                             inner join registro_acao_busca_ativa_secao ba_secao on ba_secao.registro_acao_busca_ativa_id = ba.id 
                             inner join registro_acao_busca_ativa_questao ba_questao on ba_questao.registro_acao_busca_ativa_secao_id = ba_secao.id 
                             inner join registro_acao_busca_ativa_resposta ba_resposta on ba_resposta.questao_registro_acao_id = ba_questao.id 
                             inner join questao q on q.id = ba_questao.questao_id 
                             inner join opcao_resposta or2 on or2.id = ba_resposta.resposta_id 
                             inner join turma t on t.id = ba.turma_id 
                             inner join ue u on u.id = t.ue_id 
                             inner join dre d on d.id = u.dre_id
                               where q.nome_componente in (@nomeComponenteQuestao_ContatoRealizado, @nomeComponenteQuestao_ContatoNaoRealizado)
                                     and not ba.excluido 
                                     and not ba_secao.excluido 
                                     and not ba_questao.excluido 
                                     and not ba_resposta.excluido
                                     and or2.ordem = @TipoProcedimentoRealizado
                                     and t.ano_letivo = @anoLetivo
                                     and t.modalidade_codigo = @modalidade");
            if (ueId.NaoEhNulo())
                sql.AppendLine(@"    and t.ue_id= @ueId ");
            if (dreId.NaoEhNulo())
                sql.AppendLine(@"    and u.dre_id = @dreId ");
            if (semestre.NaoEhNulo())
                sql.AppendLine(@"    and t.semestre = @semestre ");
            sql.AppendLine(@"group by q.nome_componente, d.nome;");
            return await database.Conexao
                                 .QueryAsync<DadosGraficoProcedimentoTrabalhoDreBuscaAtivaDto>(sql.ToString(),
                                                  new
                                                  {
                                                      tipoProcedimentoRealizado,
                                                      anoLetivo,
                                                      modalidade = (int)modalidade,
                                                      nomeComponenteQuestao_ContatoRealizado = NOME_COMPONENTE_QUESTAO_PROCEDIMENTO_TRABALHO_CONTATOU_RESP,
                                                      nomeComponenteQuestao_ContatoNaoRealizado = NOME_COMPONENTE_QUESTAO_PROCEDIMENTO_TRABALHO_NAO_CONTATOU_RESP,
                                                      ueId,
                                                      dreId,
                                                      semestre
                                                  }, commandTimeout: 60);
        }
    }
}
