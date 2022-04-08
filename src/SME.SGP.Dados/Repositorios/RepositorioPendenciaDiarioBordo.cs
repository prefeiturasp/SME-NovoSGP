using Npgsql;
using NpgsqlTypes;
using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioPendenciaDiarioBordo : RepositorioBase<PendenciaDiarioBordo>, IRepositorioPendenciaDiarioBordo
    {
        public RepositorioPendenciaDiarioBordo(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task Excluir(long pendenciaId)
        {
            await database.Conexao.ExecuteScalarAsync("delete from pendencia_diario_bordo where id = @pendenciaId", new { pendenciaId }, commandTimeout: 60);
        }

        public async Task<IEnumerable<AulaComComponenteDto>> ListarPendenciasDiario(string turmaId, long[] componentesCurricularesId)
        {
            var listaRetorno = new List<Aula>();
            var sqlQuery = new StringBuilder();

            sqlQuery.AppendLine("select distinct a.id as Id,");
            sqlQuery.AppendLine("                a.turma_id as TurmaId,");
            sqlQuery.AppendLine("                db.componente_curricular_id as ComponenteId");
            sqlQuery.AppendLine("  	from aula a");
            sqlQuery.AppendLine("  		inner join turma t");
            sqlQuery.AppendLine("  			on a.turma_id = t.turma_id and t.modalidade_codigo = @modalidadeCodigo");
            sqlQuery.AppendLine("  		inner join ue");
            sqlQuery.AppendLine("  			on t.ue_id = ue.id");

            sqlQuery.AppendLine($"  	left join diario_bordo db");
            sqlQuery.AppendLine($"  		on db.aula_id = a.id and db.componente_curricular_id = ANY(@componentesCurricularesId)");
            sqlQuery.AppendLine($"  	left join pendencia_diario_bordo pdb");
            sqlQuery.AppendLine($"  		on pdb.aula_id = a.id and pdb.componente_curricular_id = ANY(@componentesCurricularesId)");
            sqlQuery.AppendLine("  		left join pendencia p");
            sqlQuery.AppendLine("  			on p.id = pdb.pendencia_id");
            sqlQuery.AppendLine("  			and not p.excluido");
            sqlQuery.AppendLine("  			and p.tipo = @tipoPendencia");

            sqlQuery.AppendLine("  where not a.excluido");
            sqlQuery.AppendLine("	and a.data_aula < @hoje");
            sqlQuery.AppendLine("   and t.turma_id = @turmaId");

            sqlQuery.AppendLine("	and pdb.id is null");
            sqlQuery.AppendLine("   and p.id is null");

            try
            {
                return await database.Conexao.QueryAsync<AulaComComponenteDto>(sqlQuery.ToString(), new
                {
                    hoje = DateTime.Today.Date,
                    turmaId,
                    componentesCurricularesId,
                    modalidadeCodigo = Modalidade.EducacaoInfantil,
                    tipoPendencia = TipoPendencia.DiarioBordo
                }, commandTimeout: 200);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
