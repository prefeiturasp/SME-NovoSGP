using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiarioBordoNotificacaoQueryHandler : IRequestHandler<ObterDiarioBordoNotificacaoQuery, IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>>
    {
        private readonly IMediator mediator;

        public ObterDiarioBordoNotificacaoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<UsuarioNotificarDiarioBordoObservacaoDto>> Handle(ObterDiarioBordoNotificacaoQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            if (turma is null)
                throw new NegocioException("A turma informada não foi encontrada.");

            var professorRf = string.Empty;
            var professorNome = string.Empty;

            if (request.DiarioBordoId > 0)
            {
                var diarioBordo = await mediator.Send(new ObterDiarioDeBordoPorIdQuery(request.DiarioBordoId));

                if (diarioBordo is null)
                    throw new NegocioException("O diário de bordo informado não foi encontrado.");

                professorRf = diarioBordo.Auditoria.CriadoRF;
                professorNome = diarioBordo.Auditoria.CriadoPor;
            }
            else if (request.ObservacaoId.HasValue)
            {
                var diarioBordoObs = await mediator.Send(new ObterDiarioBordoObservacaoPorObservacaoIdQuery(request.ObservacaoId.Value));

                if (diarioBordoObs is null)
                    throw new NegocioException("O diário de bordo observação informado não foi encontrado.");

                professorRf = diarioBordoObs.UsuarioCodigoRfDiarioBordo;
                professorNome = diarioBordoObs.UsuarioNomeDiarioBordo;
            }

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (professorRf.Equals(usuarioLogado.CodigoRf))
                return await mediator.Send(new ObterUsuarioNotificarDiarioBordoObservacaoQuery(ObterProfessorTitular(professorRf, professorNome)));

            else
                return default;
        }

        private List<ProfessorTitularDisciplinaEol> ObterProfessorTitular(string codigoRf, string nome)
        {
            return new List<ProfessorTitularDisciplinaEol> { new ProfessorTitularDisciplinaEol { ProfessorRf = codigoRf, ProfessorNome = nome } };
        }
    }
}
