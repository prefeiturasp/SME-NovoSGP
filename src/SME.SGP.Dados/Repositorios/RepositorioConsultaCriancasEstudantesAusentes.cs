using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.ConsultaCriancasEstudantesAusentes;
using System;
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
            var dataInicioAno = new DateTime(filtro.AnoLetivo, 1, 1);

            var parametro = new
            {
                tipoFalta = (int)TipoFrequencia.F,
                filtro.CodigoTurma,
                filtro.CodigoUe,
                dataInicioAno,
                dataFimAno = dataInicioAno.AddYears(1)
            };

            var sql = new StringBuilder();
            sql.AppendLine(@"with dados_diarios as materialized (
                                select rfa.codigo_aluno,
                                       a.data_aula,
                                       bool_or(not rfa.excluido) as possui_registro_ativo,
                                       bool_or(rfa.valor <> @tipoFalta) as possui_valor_diferente_falta
                                  from aula a
                                 inner join registro_frequencia_aluno rfa on rfa.aula_id = a.id
                                 where a.turma_id = @CodigoTurma
                                   and a.ue_id = @CodigoUe
                                   and a.data_aula >= @dataInicioAno
                                   and a.data_aula < @dataFimAno
                                   and not a.excluido
                                 group by rfa.codigo_aluno, a.data_aula)
                            ,sequencia_frequencia as (
                                select codigo_aluno,
                                       data_aula,
                                       possui_valor_diferente_falta,
                                       row_number() over(partition by codigo_aluno order by data_aula) as linha
                                  from dados_diarios
                                 where possui_registro_ativo)");

            sql.AppendLine(@$",ausencias as (
                                select codigo_aluno, linha
                                  from sequencia_frequencia a
                                 where not possui_valor_diferente_falta
                                 {ObterQueryUltimasAulas(filtro.Ausencias)})");

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
                    return $"{HAVING} between 6 and 10";

                case EnumAusencias.Entre11e15DiasSeguidos:
                    return $"{HAVING} between 11 and 15";

                case EnumAusencias.HaMaisDe15DiasSeguidos:
                    return $"{HAVING} > 15";

                default:
                    return string.Empty;
            }
        }
    }
}
