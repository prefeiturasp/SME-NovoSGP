using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComunicadoConsulta : RepositorioBase<Comunicado>, IRepositorioComunicadoConsulta
    {
        public RepositorioComunicadoConsulta(ISgpContextConsultas conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {}

        public async Task<IEnumerable<ComunicadoTurmaAlunoDto>> ObterComunicadosAnoAtual()
        {
            const string sql =
                @"
                select
	                c.id Id,
	                c.ano_letivo AnoLetivo,
	                c.codigo_dre CodigoDre,
	                c.codigo_ue CodigoUe,
	                ct.turma_codigo TurmaCodigo,
	                ca.aluno_codigo AlunoCodigo,
	                c.modalidade modalidade,
	                c.series_resumidas SeriesResumidas,
	                c.tipo_comunicado TipoComunicado,
                    u.tipo_escola TipoEscolaId
                from 
	                comunicado c
                left join comunicado_aluno ca on ca.comunicado_id = c.id 
                left join comunicado_turma ct on ct.comunicado_id = c.id
                left join turma t on t.turma_id = ct.turma_codigo 
                left join ue u on u.id = t.ue_id or u.ue_id = c.codigo_ue 
                where 
	                c.ano_letivo >= extract(year from current_date)
                    and not c.excluido  and c.tipo_comunicado <> @tipocomunicado
                    and not coalesce(ca.excluido, false)
                    and not coalesce(ct.excluido, false)";

            return await database.Conexao.QueryAsync<ComunicadoTurmaAlunoDto>(sql, new { tipocomunicado = TipoComunicado.MENSAGEM_AUTOMATICA });
        }
    }
}