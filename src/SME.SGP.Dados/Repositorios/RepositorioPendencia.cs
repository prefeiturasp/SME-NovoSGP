using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendencia : RepositorioBase<Pendencia>, IRepositorioPendencia
    {
        public RepositorioPendencia(ISgpContext database) : base(database)
        {
        }

        public void AtualizarPendencias(long fechamentoId, SituacaoPendencia situacaoPendencia, TipoPendencia tipoPendencia)
        {
            var query = @"update pendencia p
                           set situacao = @situacaoPendencia
                         from pendencia_fechamento f
                        where f.pendencia_id = p.id
	                        and p.tipo = @tipoPendencia
                            and f.fechamento_turma_disciplina_id = @fechamentoId                            
                            and not p.excluido ";

            database.Conexao.Execute(query, new { fechamentoId, situacaoPendencia, tipoPendencia });
        }

        public void ExcluirPendenciasFechamento(long fechamentoId, TipoPendencia tipoPendencia)
        {
            var query = @"update pendencia p
                             set excluido = true
                         from pendencia_fechamento f
                        where not p.excluido
                          and p.situacao = 1
                          and p.id = f.pendencia_id
                          and p.tipo = @tipoPendencia
	                      and f.fechamento_turma_disciplina_id = @fechamentoId";

            database.Conexao.Execute(query, new { fechamentoId, tipoPendencia });
        }

    }
}