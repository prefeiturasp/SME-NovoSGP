using Dapper;
using SME.SGP.Dominio;
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
            var sqlPaginada = ObterQueryPaginada(filtro, paginacao, queryTabelaResposta);
            var retornoPaginado = new PaginacaoResultadoDto<EncaminhamentoNAAPARelatorioDinamico>();
            var parametros = new
            {
                filtro.DreId,
                filtro.UeId,
                filtro.Historico,
                filtro.AnoLetivo,
                Anos = filtro.Anos.ToArray(),
                filtro.Modalidade
            };

            using (var encaminhamentosNAAPA = await contexto.Conexao.QueryMultipleAsync(sqlPaginada, parametros))
            {
                retornoPaginado.Items = encaminhamentosNAAPA.Read<EncaminhamentoNAAPARelatorioDinamico>();
                //retornoPaginado.TotalRegistros = encaminhamentosNAAPA.ReadFirst<int>();
            }

            //retornoPaginado.TotalPaginas = (int)Math.Ceiling((double)retornoPaginado.TotalRegistros / paginacao.QuantidadeRegistros);

            return new RelatorioDinamicoNAAPADto()
            {
                TotalRegistro = retornoPaginado.TotalRegistros,
                EncaminhamentosNAAPAPaginado = retornoPaginado
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

        private string ObterQueryPaginada(FiltroRelatorioDinamicoNAAPADto filtro, Paginacao paginacao, string queryTabelaResposta)
        {
            var sql = new StringBuilder();

            sql.AppendLine(queryTabelaResposta);

            sql.AppendLine(@$"SELECT np.id, dre.abreviacao as Dre, ue.nome as UnidadeEscolar, 
                                     concat(np.aluno_nome, ' (', np.aluno_codigo, ')') as Estudante,
                                     t.modalidade_codigo as ModalidadeCodigo, t.ano
                            FROM encaminhamento_naapa np
                                    JOIN turma t ON t.id = np.turma_id
                                    JOIN ue ON t.ue_id = ue.id
                                    JOIN dre ON dre.id = ue.dre_id
                                    LEFT JOIN tab_resposta resposta ON resposta.id = np.id");

            sql.AppendLine(ObterFiltro(filtro));

            sql.AppendLine($" OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY ");

            return sql.ToString();
        }

        private string ObterColunasTabelaResposta(List<string> nomesComponentes)
        {
            var sql = new StringBuilder();

            foreach(var nomeComponente in nomesComponentes)
            {
                sql.AppendLine($",{nomeComponente} text");
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
                                                            q.tipo,
                                                            q.nome_componente,
                                                            CASE WHEN 
                                                                    q.tipo = {(int)TipoQuestao.Radio} OR q.tipo = {(int)TipoQuestao.Combo} OR 
                                                                    q.tipo = {(int)TipoQuestao.Checkbox} OR q.tipo = {(int)TipoQuestao.ComboMultiplaEscolha} 
                                                                 THEN opr.ordem::text ELSE enr.texto END resposta
                                                    FROM encaminhamento_naapa_secao ens
                                                    JOIN encaminhamento_naapa_questao enq ON ens.id = enq.encaminhamento_naapa_secao_id
                                                    JOIN questao q ON enq.questao_id = q.id
                                                    JOIN encaminhamento_naapa_resposta enr ON enr.questao_encaminhamento_id = enq.id
                                                    JOIN secao_encaminhamento_naapa secao ON secao.id = ens.secao_encaminhamento_id
                                                    JOIN questionario questio ON questio.id = secao.questionario_id
                                                    LEFT JOIN opcao_resposta opr ON opr.id = enr.resposta_id
                                                    WHERE questio.tipo = {(int)TipoQuestionario.EncaminhamentoNAAPA}',

                                                    'SELECT DISTINCT nome_componente 
                                                     FROM questionario q
                                                     JOIN questao on q.id = questao.questionario_id
                                                     WHERE q.tipo = {(int)TipoQuestionario.RelatorioDinamicoEncaminhamentoNAAPA}') AS tab_pivot
                                                    (id int8, tipo int4 {colunas}))");

            return sql.ToString();
        }

        private string ObterFiltro(FiltroRelatorioDinamicoNAAPADto filtro)
        {
            var sql = new StringBuilder();

            sql.AppendLine(" WHERE 1=1 ");

            var funcoes = new List<Func<FiltroRelatorioDinamicoNAAPADto, string>> 
            { 
                ObterFiltroDre, 
                ObterFiltroUe,
                ObterFiltroAnoLetivo,
                ObterFiltroModalidade,
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

        private string ObterFiltroModalidade(FiltroRelatorioDinamicoNAAPADto filtro)
            => filtro.Modalidade.HasValue ? " AND t.modalidade_codigo = @Modalidade" : string.Empty;

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

            if (grupoComponente.Count() > 1)
                return ObterGrupos(grupoComponente.Key, grupoComponente.ToList());
            
             return $" AND {grupoComponente.Key} = '{grupoComponente.FirstOrDefault().Resposta}'";
        }

        private string ObterGrupos(string campo, List<FiltroComponenteRelatorioDinamicoNAAPA> filtros)
        {
            var valores = filtros.Select(filtro => filtro.Resposta);

            return $" AND {campo} IN('{string.Join("','", valores.ToArray())}')";
        }

        private string ObterPeriodoData(string campo, string valor)
        {
            const int DATA_INICIO = 0;
            const int DATA_FIM = 1;
            var respostaRetorno = valor.Replace("\\", "").Replace("\"", "").Replace("[", "").Replace("]", "");
            string[] periodos = respostaRetorno.ToString().Split(',');

            if (periodos.Length > 0) 
                return $" AND TO_DATE({campo},'yyyy-MM-dd') BETWEEN '{DateTime.Parse(periodos[DATA_INICIO]).Date.ToString("yyyy-MM-dd")}' AND '{DateTime.Parse(periodos[DATA_FIM]).Date.ToString("yyyy-MM-dd")}'";
            
            return string.Empty;
        }
    }
}
