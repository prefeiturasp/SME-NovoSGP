using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAulaUseCase : AbstractUseCase, IExcluirPlanoAulaUseCase
    {
        public ExcluirPlanoAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long aulaId)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var aula = await mediator.Send(new ObterAulaPorIdQuery(aulaId));

            await VerificaSeProfessorPodePersistirTurmaDisciplina(usuario.CodigoRf, aula.TurmaId, aula.DisciplinaId, aula.DataAula, usuario);

            return await mediator.Send(new ExcluirPlanoAulaDaAulaCommand(aulaId));
        }

        private async Task VerificaSeProfessorPodePersistirTurmaDisciplina(string codigoRf, string turmaId, string disciplinaId, DateTime dataAula, Usuario usuario)
        {
            if (!usuario.EhProfessorCj() && !await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(long.Parse(disciplinaId), turmaId, dataAula, usuario)))
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        }
    }
}
