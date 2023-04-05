using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusencia : RepositorioBase<CompensacaoAusencia>, IRepositorioCompensacaoAusencia
    {
        public RepositorioCompensacaoAusencia(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<RegistroFaltasNaoCompensadaDto>> ObterAusenciaParaCompensacao(FiltroFaltasNaoCompensadasDto filtro)
        {
            var query =  @" with ausencias as(
							select
								rfa.aula_id as AulaId,
								caaa.data_aula as DataAula,
								'Aula ' || caaa.numero_aula as Descricao,
								caaa.registro_frequencia_aluno_id as RegistroFrequenciaAlunoId,
								true Sugestao
							from compensacao_ausencia_aluno_aula caaa
							join registro_frequencia_aluno rfa on rfa.id = caaa.registro_frequencia_aluno_id
							join aula a on a.id = rfa.aula_id
							inner join periodo_escolar p on a.tipo_calendario_id = p.tipo_calendario_id
							where not caaa.excluido 
							    and rfa.codigo_aluno = @codigoaluno
								and a.disciplina_id = @disciplinaid
								and p.bimestre = @bimestre
								and a.turma_id = @turmaid
								and rfa.valor = 2
							union all	
							select 
								a.id AulaId,
								a.data_aula as DataAula,
								'Aula ' || rfa.numero_aula as Descricao,
								caaa.registro_frequencia_aluno_id as RegistroFrequenciaAlunoId,
								false Sugestao
							from
								registro_frequencia_aluno rfa
							inner join aula a on
								rfa.aula_id = a.id
							inner join periodo_escolar p on
								a.tipo_calendario_id = p.tipo_calendario_id
							left join compensacao_ausencia_aluno_aula caaa on
												rfa.id = caaa.registro_frequencia_aluno_id
								and not caaa.excluido
							where not rfa.excluido
								and not a.excluido
								and rfa.codigo_aluno = @codigoaluno
								and a.turma_id = @turmaid
								and a.data_aula >= p.periodo_inicio
								and a.data_aula <= p.periodo_fim
								and rfa.valor = 2
								and p.bimestre = @bimestre
								and a.disciplina_id = @disciplinaid
								and rfa.numero_aula <= a.quantidade
							)
							select * from ausencias
							order by DataAula ";
            var parametros = new
            {
                turmaid = filtro.TurmaId,
                disciplinaid = filtro.DisciplinaId,
                codigoaluno = filtro.CodigoAluno,
                bimestre = filtro.Bimestre,
            };
            
            return await database.Conexao.QueryAsync<RegistroFaltasNaoCompensadaDto>(query,parametros);
        }
    }
}
