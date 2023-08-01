using System.Linq;
using Dapper;
using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Infra.Interfaces;
using SME.SGP.Dto;
using System.Collections;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoConsulta : RepositorioBase<Evento>, IRepositorioEventoConsulta
    {
        public RepositorioEventoConsulta(ISgpContextConsultas conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<EventoEADto>> ObterEventosEscolaAquiPorDreUeTurmaMes(string dre_id, string ue_id, string turma_id, int modalidadeCalendario, DateTime mesAno)
        {
            var dataInicio = new DateTime(mesAno.Year, mesAno.Month, 1);
            var dataFim = dataInicio.AddMonths(1).AddMilliseconds(-1);
            var queryDre = string.IsNullOrWhiteSpace(dre_id) ? "" : "and (dre_id isnull or dre_id = @dre_id)";
            var queryUe = string.IsNullOrWhiteSpace(ue_id) ? "" : "and (ue_id isnull or ue_id = @ue_id)";
            var queryTurmna = string.IsNullOrWhiteSpace(turma_id) ? "" : "and (turma_id isnull or turma_id = @turma_id)";

			var sql = $@"with vw_eventos as (select 
							coalesce (cc.descricao_sgp, cc.descricao) as componente_curricular,							
							null::varchar as evento_id, aa.nome_avaliacao nome, aa.descricao_avaliacao descricao, aa.data_avaliacao data_inicio, aa.data_avaliacao data_fim,
							aa.dre_id, aa.ue_id, 
							0::int tipo_evento,
							aa.turma_id,
							t.ano_letivo,
							t.modalidade_codigo modalidade_turma,
							null::int modalidade_calendario,
							null::int tipo_calendario_id
						from atividade_avaliativa aa 
						inner join turma t on t.turma_id = aa.turma_id
						inner join atividade_avaliativa_disciplina aad on aad.atividade_avaliativa_id = aa.id
						inner join componente_curricular cc on cc.id::varchar = aad.disciplina_id 
						where
							(aa.migrado isnull or aa.migrado = false) 
							and not aa.excluido 
						union 
						select
							null as componente_curricular,
							e.id::varchar as evento_id, e.nome, e.descricao, e.data_inicio, e.data_fim,
							e.dre_id, e.ue_id,
							e.tipo_evento_id tipo_evento,
							null::varchar turma_id,
							tc.ano_letivo,
							null::int modalidade_turma,
							tc.modalidade modalidade_calendario,
							tc.id tipo_calendario_id
						from evento e
						inner join evento_tipo et on et.id = e.tipo_evento_id
						inner join tipo_calendario tc on tc.id = e.tipo_calendario_id 
						left join wf_aprovacao_nivel wan on wan.wf_aprovacao_id = e.wf_aprovacao_id 
						left join wf_aprovacao_nivel_notificacao wann on wann.wf_aprovacao_nivel_id = wan.id 
						left join notificacao n2 on n2.id = wann.notificacao_id 
						where
							(e.status = 1) and
							(e.migrado isnull or e.migrado = false) and
							(et.evento_escolaaqui)
							and not e.excluido)
						select distinct * from vw_eventos 
						 where (data_inicio between @dataInicio and @dataFim or data_fim between @dataInicio and @dataFim)
							and (modalidade_calendario is null or modalidade_calendario = @modalidadeCalendario)
							{queryDre}
							{queryUe}
							{queryTurmna}";

            var parametros = new { dataInicio, dataFim, dre_id, ue_id, turma_id, modalidadeCalendario };
            return await database.Conexao.QueryAsync<EventoEADto>(sql, parametros);

        }
    }
}