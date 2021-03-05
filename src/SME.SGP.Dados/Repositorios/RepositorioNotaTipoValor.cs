using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotaTipoValor : RepositorioBase<NotaTipoValor>, IRepositorioNotaTipoValor
    {
        public RepositorioNotaTipoValor(ISgpContext database) : base(database)
        {
        }

        public NotaTipoValor ObterPorCicloIdDataAvalicacao(long cicloId, DateTime dataAvalicao)
        {
            var sql = @"select ntv.* from notas_tipo_valor ntv
                        inner join notas_conceitos_ciclos_parametos nccp
                        on nccp.tipo_nota = ntv.id
                        where nccp.ciclo = @cicloId and @dataAvalicao >= nccp.inicio_vigencia
                        and (nccp.ativo = true or @dataAvalicao <= nccp.fim_vigencia)
                        order by nccp.id asc";

            var parametros = new { cicloId, dataAvalicao };

            return database.QueryFirstOrDefault<NotaTipoValor>(sql, parametros);
        }

        public NotaTipoValor ObterPorTurmaId(long turmaId, TipoTurma tipoTurma = TipoTurma.Regular)
        {
            var sql = tipoTurma == TipoTurma.EdFisica ? 
                    $@"select *
	                      from notas_conceitos_ciclos_parametos
                       where tipo_nota = {(int)TipoNota.Nota}
                       limit 1;" :
                    @"select
	                    nccp.*
                    from
	                    turma t
                    inner join tipo_ciclo_ano tca on
	                    tca.ano = t.ano
	                    and tca.modalidade = t.modalidade_codigo
                    inner join tipo_ciclo tc on
	                    tca.tipo_ciclo_id = tc.id
                    inner join notas_conceitos_ciclos_parametos nccp on
	                    nccp.ciclo = tc.id
                    where
	                    t.id = @turmaId";

            return database.QueryFirstOrDefault<NotaTipoValor>(sql, new { turmaId });
        }
    }
}