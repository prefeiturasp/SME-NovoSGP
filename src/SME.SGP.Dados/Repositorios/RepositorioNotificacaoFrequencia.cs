using Dapper;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacaoFrequencia : RepositorioBase<NotificacaoFrequencia>, IRepositorioNotificacaoFrequencia
    {
        public RepositorioNotificacaoFrequencia(ISgpContext database) : base(database)
        {
        }

        public IEnumerable<RegistroFrequenciaFaltanteDto> ObterTurmasSemRegistroDeFrequencia(TipoNotificacaoFrequencia tipoNotificacao, string ueId)
        {
            var query = @"select distinct a.turma_id as CodigoTurma, t.nome as NomeTurma
	                        , ue.ue_id as CodigoUe, ue.nome as NomeUe
	                        , dre.dre_id as CodigoDre, dre.nome as NomeDre
	                        , a.disciplina_id as DisciplinaId
                           from aula a
                          inner join turma t on t.turma_id = a.turma_id
                          inner join ue on ue.id = t.ue_id
                          inner join dre on dre.id = ue.dre_id
                          left join registro_frequencia r on r.aula_id = a.id
                          left join notificacao_frequencia n on n.aula_id = a.id and n.tipo = @tipoNotificacao
                         where not a.excluido
                           and r.id is null
                           and n.id is null
                           and a.data_aula < DATE(now())
                           and ue.ue_id = @ueId
                        order by dre.dre_id, ue.ue_id, a.turma_id";
            var lista = new List<RegistroFrequenciaFaltanteDto>();
            try
            {
                lista = database.Conexao.Query<RegistroFrequenciaFaltanteDto>(query, new { tipoNotificacao, ueId }).ToList();
            }
            catch (Exception ex)
            {
                using (SentrySdk.Init("https://09eed44e9e8e4f2387b5e24b35aabc5b@sentry.sme.prefeitura.sp.gov.br/2"))
                {
                    SentrySdk.CaptureException(ex);
                    var evento = new SentryEvent(ex);
                    evento.Message = $"{query} - parametros :{tipoNotificacao} - {ueId}";
                    SentrySdk.CaptureEvent(evento);
                    SentrySdk.CaptureMessage($"{query} - parametros :{tipoNotificacao} - {ueId}");
                }
            }
            return lista;
        }

        public bool UsuarioNotificado(long usuarioId, TipoNotificacaoFrequencia tipo)
        {
            var query = @"select 0
                          from notificacao_frequencia f
                         inner join notificacao n on n.codigo = f.notificacao_codigo
                         where n.usuario_id = @usuarioId
                           and f.tipo = @tipo";

            return database.Conexao.Query<int>(query, new { usuarioId, tipo }).Any();
        }
    }
}