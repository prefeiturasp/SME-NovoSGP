using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioDinamicoNAAPA : IRepositorioRelatorioDinamicoNAAPA
    {
        private readonly ISgpContext contexto;

        public RepositorioRelatorioDinamicoNAAPA(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public async Task<RelatorioDinamicoNAAPADto> ObterRelatorioDinamicoNAAPA(FiltroRelatorioDinamicoNAAPADto filtro, Paginacao paginacao)
        {
            var queryTabelaResposta = await ObterTabelaResposta();
            var queryFiltro = ObterFiltro(filtro);
            var sqlPaginada = ObterQueryPaginada(queryFiltro, paginacao, queryTabelaResposta);
            var sqlTotalDeRegistros = ObterQueryTotalDeRegistros(queryFiltro, queryTabelaResposta, filtro);
            var sql = string.Concat(sqlPaginada, ";", sqlTotalDeRegistros);
            var situacao = new List<int>() { (int)SituacaoNAAPA.AguardandoAtendimento, (int)SituacaoNAAPA.EmAtendimento };
            var retornoPaginado = new PaginacaoResultadoDto<EncaminhamentoNAAPARelatorioDinamico>();
            IEnumerable<TotalRegistroPorModalidadeRelatorioDinamicoNAAPA> retornoTotalDeRegitros = null;

            var parametros = new
            {
                filtro.DreId,
                filtro.UeId,
                filtro.Historico,
                filtro.AnoLetivo,
                Anos = filtro.Anos.ToArray(),
                situacao = situacao.ToArray(),
                Modalidades = filtro.Modalidades?.Select(modalidade => (int)modalidade).ToArray()
            };

            using (var encaminhamentosNAAPA = await contexto.Conexao.QueryMultipleAsync(sql, parametros))
            {
                retornoPaginado.Items = encaminhamentosNAAPA.Read<EncaminhamentoNAAPARelatorioDinamico>();
                retornoTotalDeRegitros = encaminhamentosNAAPA.Read<TotalRegistroPorModalidadeRelatorioDinamicoNAAPA>();
                retornoPaginado.TotalRegistros = retornoTotalDeRegitros.Sum(registro => (int)registro.Total);
            }

            var encaminhamentosNAAPAIds = retornoPaginado.Items.Select(s => s.Id).Distinct().ToArray();

            retornoPaginado.Items = retornoPaginado.Items.OrderBy(rr => rr.Dre)
                                                            .ThenBy(rr => rr.UnidadeEscolar)
                                                            .ThenBy(rr => rr.Estudante);

            retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return new RelatorioDinamicoNAAPADto()
            {
                TotalRegistro = retornoPaginado.TotalRegistros,
                EncaminhamentosNAAPAPaginado = retornoPaginado,
                TotalRegistroPorModalidadesAno = retornoTotalDeRegitros.Count() > 1 ? retornoTotalDeRegitros : null,
                EncaminhamentosNAAPAIds = encaminhamentosNAAPAIds,
            };
        }

        private Task<IEnumerable<string>> ObterNomesComponentes()
        {
            var sql = new StringBuilder();
            sql.AppendLine(@"SELECT DISTINCT nome_componente
                             FROM questionario q
                             JOIN questao on q.id = questao.questionario_id
                            WHERE q.tipo = @tipo");

            return contexto.Conexao.QueryAsync<string>(sql.ToString(), new { tipo = (int)TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA });
        }

        private string ObterQueryPaginada(string filtro, Paginacao paginacao, string queryTabelaResposta)
        {
            var sql = new StringBuilder();
            var camposRetorno = @"distinct np.id, dre.abreviacao as Dre, ue.nome as NomeEscola, ue.tipo_escola as TipoEscola, 
                                  concat(np.aluno_nome, ' (', np.aluno_codigo, ')') as Estudante,
                                  t.modalidade_codigo as Modalidade, t.ano";

            sql.AppendLine(ObterQuery(filtro, queryTabelaResposta, camposRetorno));

            sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");

            return sql.ToString();
        }

        private string ObterQueryTotalDeRegistros(string queryFiltro, string queryTabelaResposta, FiltroRelatorioDinamicoNAAPADto filtro)
        {
            var sql = new StringBuilder();
            var tupla = ObterCamposEGrupoParaQueryTotalDeRegistro(filtro);

            sql.AppendLine(ObterQuery(queryFiltro, queryTabelaResposta, tupla.campos));
            sql.AppendLine(tupla.groupBy);

            return sql.ToString();
        }

        private (string campos, string groupBy) ObterCamposEGrupoParaQueryTotalDeRegistro(FiltroRelatorioDinamicoNAAPADto filtro)
        {
            if (filtro.Modalidades.Count() != 1)
                return ("count(distinct np.id) Total, t.modalidade_codigo as Modalidade", " GROUP BY t.modalidade_codigo ");

            return ("count(distinct np.id) Total, t.ano, t.modalidade_codigo as Modalidade", " GROUP BY t.ano, t.modalidade_codigo");
        }

        private string ObterQuery(string filtro, string queryTabelaResposta, string camposRetorno)
        {
            var sql = new StringBuilder();

            sql.AppendLine(queryTabelaResposta);

            sql.AppendLine(@$"SELECT {camposRetorno}
                            FROM encaminhamento_naapa np
                                    JOIN turma t ON t.id = np.turma_id
                                    JOIN ue ON t.ue_id = ue.id
                                    JOIN dre ON dre.id = ue.dre_id
                                    LEFT JOIN tab_resposta resposta ON resposta.id = np.id");

            sql.AppendLine(filtro);

            return sql.ToString();
        }

        private string ObterColunasTabelaResposta(List<string> nomesComponentes)
        {
            var sql = new StringBuilder();

            foreach(var nomeComponente in nomesComponentes)
            {
                sql.AppendLine($",{nomeComponente} text[]");
            }

            return sql.ToString();
        }

        private async Task<string> ObterTabelaResposta()
        {
            var nomesComponentes = await ObterNomesComponentes();
            var colunas = ObterColunasTabelaResposta(nomesComponentes.ToList());
            var sql = new StringBuilder();

            sql.AppendLine(@$"WITH tab_resposta as (
                                          SELECT * from CROSSTAB (
                                                    'SELECT ens.encaminhamento_naapa_id, 
                                                            q.nome_componente,
                                                            CASE WHEN 
                                                                    q.tipo = {(int)TipoQuestao.Radio} OR q.tipo = {(int)TipoQuestao.Combo} OR 
                                                                    q.tipo = {(int)TipoQuestao.Checkbox} OR q.tipo = {(int)TipoQuestao.ComboMultiplaEscolha} 
                                                                 THEN array_agg(opr.ordem::text) ELSE array_agg(enr.texto) END resposta
                                                    FROM encaminhamento_naapa_secao ens
                                                    JOIN encaminhamento_naapa_questao enq ON ens.id = enq.encaminhamento_naapa_secao_id
                                                    JOIN questao q ON enq.questao_id = q.id
                                                    JOIN encaminhamento_naapa_resposta enr ON enr.questao_encaminhamento_id = enq.id
                                                    JOIN secao_encaminhamento_naapa secao ON secao.id = ens.secao_encaminhamento_id
                                                    JOIN questionario questio ON questio.id = secao.questionario_id
                                                    LEFT JOIN opcao_resposta opr ON opr.id = enr.resposta_id
                                                    WHERE questio.tipo = {(int)TipoQuestionario.EncaminhamentoNAAPA}
                                                      and not ens.excluido 
                                                      and not enq.excluido 
                                                      and not enr.excluido 
                                                    group by ens.encaminhamento_naapa_id, q.nome_componente, q.tipo
                                                    order by ens.encaminhamento_naapa_id',

                                                    'SELECT DISTINCT nome_componente 
                                                     FROM questionario q
                                                     JOIN questao on q.id = questao.questionario_id
                                                     WHERE q.tipo = {(int)TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA}') AS tab_pivot
                                                    (id int8 {colunas}))");

            return sql.ToString();
        }

        private string ObterFiltro(FiltroRelatorioDinamicoNAAPADto filtro)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" WHERE not np.excluido ");
            sql.AppendLine("   AND np.situacao = ANY(@situacao)");

            var funcoes = new List<Func<FiltroRelatorioDinamicoNAAPADto, string>> 
            { 
                ObterFiltroDre, 
                ObterFiltroUe,
                ObterFiltroAnoLetivo,
                ObterFiltroModalidades,
                ObterFiltroAnos,
                ObterFiltroAvancado
            };

            foreach (var funcao in funcoes)
            {
                sql.AppendLine(funcao(filtro));
            }

            return sql.ToString();
        }

        private string ObterFiltroDre(FiltroRelatorioDinamicoNAAPADto filtro)
            => filtro.DreId.HasValue ? $" AND dre.id = @DreId" : string.Empty;

        private string ObterFiltroUe(FiltroRelatorioDinamicoNAAPADto filtro)
            => filtro.UeId.HasValue ? " AND ue.id = @UeId" : string.Empty;

        private string ObterFiltroAnoLetivo(FiltroRelatorioDinamicoNAAPADto filtro)
            => " AND t.historica = @Historico AND t.ano_letivo = @AnoLetivo";

        private string ObterFiltroModalidades(FiltroRelatorioDinamicoNAAPADto filtro)
            => filtro.Modalidades.PossuiRegistros() ? " AND t.modalidade_codigo = any(@Modalidades)" : string.Empty;

        private string ObterFiltroAnos(FiltroRelatorioDinamicoNAAPADto filtro)
            => filtro.Anos.NaoEhNulo() && filtro.Anos.Any() ? " AND t.ano = ANY(@Anos)" : string.Empty;

        private string ObterFiltroAvancado(FiltroRelatorioDinamicoNAAPADto filtro)
        {
            if (filtro.FiltroAvancado.NaoEhNulo() && filtro.FiltroAvancado.Any())
            {
                var sql = new StringBuilder();
                var grupoComponente = filtro.FiltroAvancado.GroupBy(filtro => filtro.NomeComponente);

                foreach (var componente in grupoComponente)
                {
                    sql.AppendLine(ObterCondicaoFiltroAvancado(componente));
                }      
                
                return sql.ToString();
            }

            return string.Empty;
        }

        private string ObterCondicaoFiltroAvancado(IGrouping<string, FiltroComponenteRelatorioDinamicoNAAPA> grupoComponente)
        {
            if (grupoComponente.Any(c => c.TipoQuestao == TipoQuestao.Periodo))
                return ObterPeriodoData(grupoComponente.Key, grupoComponente.FirstOrDefault().Resposta);

            return ObterGrupos(grupoComponente.Key, grupoComponente.ToList());
        }

        private string ObterGrupos(string campo, List<FiltroComponenteRelatorioDinamicoNAAPA> filtros)
        {
            var valores = filtros.Select(filtro => filtro.OrdemResposta);

            return $" AND ARRAY['{string.Join("','", valores.ToArray())}'] && {campo}";
        }

        private string ObterPeriodoData(string campo, string valor)
        {
            const int DATA_INICIO = 0;
            const int DATA_FIM = 1;
            var respostaRetorno = valor.Replace("\\", "").Replace("\"", "").Replace("[", "").Replace("]", "");
            string[] periodos = respostaRetorno.ToString().Split(',');

            if (periodos.Length > 0) 
                return $" AND TO_DATE(array_to_string({campo}, ''),'yyyy-MM-dd') BETWEEN '{DateTime.Parse(periodos[DATA_INICIO]).Date.ToString("yyyy-MM-dd")}' AND '{DateTime.Parse(periodos[DATA_FIM]).Date.ToString("yyyy-MM-dd")}'";
            
            return string.Empty;
        }
    }
}
