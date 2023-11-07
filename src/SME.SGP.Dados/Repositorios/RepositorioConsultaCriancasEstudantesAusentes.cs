using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsultaCriancasEstudantesAusentes : IRepositorioConsultaCriancasEstudantesAusentes
    {
        private readonly ISgpContext contexto;

        public RepositorioConsultaCriancasEstudantesAusentes(ISgpContext contexto)
        {
            this.contexto = contexto;
        }

        public async Task<IEnumerable<AlunosAusentesDto>> ObterTurmasAlunosAusentes(FiltroObterAlunosAusentesDto filtro)
        {
            var parametro = new
            {
                tipoFalta = (int)TipoFrequencia.F,
                filtro.CodigoTurma,
                filtro.AnoLetivo,
                filtro.CodigoUe
            };

            var sql = new StringBuilder();
            sql.AppendLine(@";with sequencia_frequencia as
                            (select codigo_aluno, data_aula, 
                            row_number() over(partition by codigo_aluno order by data_aula) as linha
                            from (
                            select distinct rfa.codigo_aluno, a.data_aula
                               from registro_frequencia_aluno rfa
                                   inner join aula a on rfa.aula_id = a.id and not a.excluido   
                               where
   	                               a.turma_id = @CodigoTurma
	                               and a.ue_id = @CodigoUe
                                   and extract(year from a.data_aula) = @AnoLetivo                 
                                   and not rfa.excluido
                            order by rfa.codigo_aluno, a.data_aula) tab)");

            sql.AppendLine(@$",ausencias as (
                            select distinct rfa.codigo_aluno, a.data_aula, sq.linha
                               from registro_frequencia_aluno rfa
                               inner join aula a on rfa.aula_id = a.id and not a.excluido   
                               inner join sequencia_frequencia sq on sq.codigo_aluno = rfa.codigo_aluno and sq.data_aula = a.data_aula
                               where
   	                               a.turma_id = @CodigoTurma
	                               and a.ue_id = @CodigoUe
                                   and extract(year from a.data_aula) = @AnoLetivo                
                                   and not rfa.excluido
	                               and not exists(select 1 
	   				                               from registro_frequencia_aluno rfa_ext
				                                   inner join aula a_ext on rfa_ext.aula_id = a_ext.id and not a_ext.excluido  
				                                   where a_ext.data_aula = a.data_aula 
				                                   and a_ext.turma_id = a.turma_id
					                               and a_ext.ue_id = a.ue_id 
				                                   and rfa_ext.codigo_aluno = rfa.codigo_aluno
				                                   and extract(year from a_ext.data_aula) = @AnoLetivo
					                               and not rfa.excluido
				                                   and rfa_ext.valor <> @tipoFalta)
                                   { ObterQueryUltimasAulas(filtro.Ausencias) }
                                   order by rfa.codigo_aluno, a.data_aula)");

            sql.AppendLine(ObterQueryResultadoAlunosAusentes(filtro.Ausencias));

            return await contexto.Conexao.QueryAsync<AlunosAusentesDto>(sql.ToString(), parametro);
        }

        private string ObterQueryResultadoAlunosAusentes(EnumAusencias ausencias)
        {
            if (ausencias == EnumAusencias.TresAusenciasNosUltimos10Dias)
                return ObterQueryTresAusenciasNosUltimos10Dias();

            return ObterQueryResultadoAusentes(ausencias);
        }

        private string ObterQueryTresAusenciasNosUltimos10Dias()
        {
            return @$"{ObterQueryResultadoAusentes(EnumAusencias.TresAusenciasNosUltimos10Dias)}
                       having sum(DiasSeguidosComAusencia) >= 3";
        }

        private string ObterQueryResultadoAusentes(EnumAusencias ausencias)
        {
            return @$"select CodigoEol, Max(DiasSeguidosComAusencia) DiasSeguidosComAusencia
                      from(select distinct codigo_aluno as CodigoEol, count(1) DiasSeguidosComAusencia
                            from (select codigo_aluno, 
	                               linha - row_number() over(partition by codigo_aluno order by linha) as sequencia_frequencia
	                               from ausencias) tab
                            group by codigo_aluno, sequencia_frequencia
                            { ObterHavingAusencia(ausencias) }) tab
                     group by CodigoEol";
        }

        private string ObterQueryUltimasAulas(EnumAusencias ausencias)
        {
            if (ausencias == EnumAusencias.NoDiaDeHoje)
                return "and a.data_aula = now()::date";

            if (ausencias == EnumAusencias.TresAusenciasNosUltimos10Dias)
                return "and a.data_aula between now()::date - INTERVAL '9 DAYS' and now()::date ";

            return string.Empty;
        }

        private string ObterHavingAusencia(EnumAusencias ausencias)
        {
            const string HAVING = "having count(1) ";

            switch (ausencias)
            {
                case EnumAusencias.NoDiaDeHoje:
                case EnumAusencias.Ha2DiasSeguidos:
                case EnumAusencias.Ha3DiasSeguidos:
                case EnumAusencias.Ha4DiasSeguidos:
                case EnumAusencias.Ha5DiasSeguidos:
                    return $"{HAVING} >= {(int)ausencias}";

                case EnumAusencias.Entre6e10DiasSeguidos:
                    return $"{HAVING} >= 6";

                case EnumAusencias.Entre11e15DiasSeguidos:
                    return $"{HAVING} >= 11";

                case EnumAusencias.HaMaisDe15DiasSeguidos:
                    return $"{HAVING} > 15";

                default:
                    return string.Empty;
            }
        }
    }
}
