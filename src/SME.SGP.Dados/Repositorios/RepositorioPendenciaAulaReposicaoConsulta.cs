using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPendenciaAulaReposicaoConsulta : IRepositorioPendenciaAulaReposicaoConsulta
    {
        private readonly ISgpContextConsultas sgpContextConsultas;

        public RepositorioPendenciaAulaReposicaoConsulta(ISgpContextConsultas sgpContextConsultas)
        {
            this.sgpContextConsultas = sgpContextConsultas ?? throw new ArgumentNullException(nameof(sgpContextConsultas));
        }
        public async Task<long[]> ObterAulasReposicaoComPendenciaCriada(long[] aulasId)
        {
            var sqlQuery = @"select a.id
                                from aula a
                                    inner join turma t
                                        on a.turma_id = t.turma_id
                                    inner join periodo_escolar pe
                                        on a.tipo_calendario_id = pe.tipo_calendario_id
                             where a.id = any(@aulasId)
                                and not exists (select 1
                                                     from fechamento_turma ft
                                                         inner join fechamento_turma_disciplina ftd
                                                             on ft.id = ftd.fechamento_turma_id 
                                                          inner join pendencia_fechamento pf
                                                              on ftd.id = pf.fechamento_turma_disciplina_id
                                                          inner join pendencia p
                                                              on pf.pendencia_id = p.id
                                                  where not ft.excluido
                                                      and not ftd.excluido
                                                      and not p.excluido
                                                      and ft.turma_id = t.id
                                                      and ft.periodo_escolar_id = pe.id
                                                      and ftd.disciplina_id = a.disciplina_id::int8
                                                      and p.tipo = @tipoPendencia
                                                      and p.descricao like ('%' || to_char(a.data_aula, 'dd/mm/yyyy') || '%'));";

            return (await sgpContextConsultas.Conexao.QueryAsync<long>(sqlQuery, new { aulasId, tipoPendencia = TipoPendencia.AulasReposicaoPendenteAprovacao })).ToArray();
        }
    }
}
