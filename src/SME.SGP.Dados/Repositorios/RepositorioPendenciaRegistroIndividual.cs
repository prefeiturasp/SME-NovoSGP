using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaRegistroIndividual : RepositorioBase<PendenciaRegistroIndividual>, IRepositorioPendenciaRegistroIndividual
    {
        public RepositorioPendenciaRegistroIndividual(ISgpContext database)
            : base(database)
        {
        }

        public async Task<PendenciaRegistroIndividual> ObterPendenciaRegistroIndividualPorTurmaESituacao(long turmaId, SituacaoPendencia situacaoPendencia)
        {
            const string sql = @"SELECT 
	                                pri.id, 
	                                pri.alterado_por, 
	                                pri.alterado_rf, 
	                                pri.alterado_em, 
	                                pri.criado_por, 
	                                pri.criado_rf, 
	                                pri.criado_em, 
	                                pri.pendencia_id, 
	                                pri.turma_id,
	                                pria.id,
	                                pria.codigo_aluno,
	                                pria.pendencia_registro_individual_id,
									pria.situacao,
									p.id, 
									p.titulo, 
									p.descricao, 
									p.situacao, 
									p.tipo, 
									p.excluido, 
									p.migrado, 
									p.criado_em, 
									p.criado_por, 
									p.alterado_em, 
									p.alterado_por, 
									p.criado_rf, 
									p.alterado_rf, 
									p.instrucao
                                FROM 
	                                public.pendencia_registro_individual pri 
                                inner join
	                                public.pendencia p 
	                                on pri.pendencia_id = p.id
                                left join
	                                public.pendencia_registro_individual_aluno pria 
	                                on pri.id = pria.pendencia_registro_individual_id
                                where
	                                pri.turma_id = @turmaId
	                                and p.situacao = @situacao
	                                and not p.excluido";

            PendenciaRegistroIndividual resultado = null;
            await database.Conexao.QueryAsync<PendenciaRegistroIndividual, PendenciaRegistroIndividualAluno, Pendencia, PendenciaRegistroIndividual>(sql,
                (pendenciaRegistroIndividual, pendenciaRegistroIndividualAluno, pendencia) =>
                {
                    if(resultado is null)
                    {
                        resultado = pendenciaRegistroIndividual;
						resultado.Pendencia = pendencia;
                    }

                    resultado.Alunos = resultado.Alunos ?? new List<PendenciaRegistroIndividualAluno>();
                    resultado.Alunos.Add(pendenciaRegistroIndividualAluno);
                    return resultado;
                },
                new { turmaId, situacao = (short)situacaoPendencia });

            return resultado;
        }
    }
}