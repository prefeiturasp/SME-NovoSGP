using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendencia : RepositorioBase<Pendencia>, IRepositorioPendencia
    {
        public RepositorioPendencia(ISgpContext database) : base(database)
        {
        }

        public async Task<AuditoriaPersistenciaDto> AtualizarPendencia(long pendenciaId, SituacaoPendencia situacaoPendencia)
        {
            var pendencia = ObterPorId(pendenciaId);
            if (pendencia == null)
                throw new NegocioException("Pendência de fechamento não localizada com o identificador consultado");

            pendencia.Situacao = situacaoPendencia;
            await SalvarAsync(pendencia);
            return (AuditoriaPersistenciaDto)pendencia;
        }

        public void AtualizarPendencias(long fechamentoId, SituacaoPendencia situacaoPendencia, TipoPendencia tipoPendencia)
        {
            var query = @"update
	                        pendencia
                        set
	                        situacao = @situacaoPendencia
                        where
	                        tipo = @tipoPendencia
                            and not excluido
	                        and exists (
	                        select
		                        1
	                        from
		                        pendencia_fechamento
	                        where
		                        pendencia.id = pendencia_fechamento.pendencia_id
		                        and pendencia_fechamento.fechamento_turma_disciplina_id = @fechamentoId)";

            database.Conexao.Execute(query, new { fechamentoId, situacaoPendencia, tipoPendencia });
        }
    }
}