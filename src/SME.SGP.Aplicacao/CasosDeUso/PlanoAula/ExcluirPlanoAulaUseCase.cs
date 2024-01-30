using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAulaUseCase : AbstractUseCase, IExcluirPlanoAulaUseCase
    {
        public ExcluirPlanoAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long aulaId)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var aula = await mediator.Send(new ObterAulaPorIdQuery(aulaId));

            await VerificaSeProfessorPodePersistirTurmaDisciplina(aula.TurmaId, aula.DisciplinaId, aula.DataAula, usuario);

            return await mediator.Send(new ExcluirPlanoAulaDaAulaCommand(aulaId));
        }

        private async Task VerificaSeProfessorPodePersistirTurmaDisciplina(string turmaId, string disciplinaId, DateTime dataAula, Usuario usuario)
        {
            if (!usuario.EhProfessorCj() && !await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(long.Parse(disciplinaId), turmaId, dataAula, usuario)))
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }
    }
}
