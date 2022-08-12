﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacaoFrequenciaConsulta : RepositorioBase<NotificacaoFrequencia>, IRepositorioNotificacaoFrequenciaConsulta
	{
        public RepositorioNotificacaoFrequenciaConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<RegistroFrequenciaFaltanteDto>> ObterTurmasSemRegistroDeFrequencia(TipoNotificacaoFrequencia tipoNotificacao)
        {
            var query = @"select
	                        distinct a.turma_id as CodigoTurma,
	                        t.nome as NomeTurma ,
							t.modalidade_codigo  as ModalidadeTurma,
	                        ue.ue_id as CodigoUe,
							ue.tipo_escola as TipoEscola,
	                        ue.nome as NomeUe ,
	                        dre.dre_id as CodigoDre,
	                        dre.nome as NomeDre ,
	                        a.disciplina_id as DisciplinaId
                        from
	                        aula a
                        inner join turma t on
	                        t.turma_id = a.turma_id
                        inner join ue on
	                        ue.id = t.ue_id
                        inner join dre on
	                        dre.id = ue.dre_id
                        where
	                        not a.excluido
	                        and not a.migrado
	                        and not exists (
	                        select
		                        1
	                        from
		                        notificacao_frequencia n
	                        where
		                        n.aula_id = a.id
		                        and n.tipo = @tipoNotificacao)
	                        and not exists (
	                        select
		                        1
	                        from
		                        registro_frequencia r
	                        where
		                        r.aula_id = a.id)
	                        and a.data_aula < date(now())
							and extract(year from a.data_aula) = extract(year from now())
                        order by
	                        dre.dre_id,
	                        ue.ue_id,
	                        a.turma_id";

            return await database.Conexao.QueryAsync<RegistroFrequenciaFaltanteDto>(query, new { tipoNotificacao }, commandTimeout: 600);
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