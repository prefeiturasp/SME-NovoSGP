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

            var diarioBordo = await mediator.Send(new ObterDiarioDeBordoPorIdQuery(request.DiarioBordoId));
            if (diarioBordo is null)
                throw new NegocioException("O diário de bordo informado não foi encontrado.");

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (!diarioBordo.Auditoria.CriadoRF.Equals(usuarioLogado.CodigoRf))
                return await mediator.Send(new ObterUsuarioNotificarDiarioBordoObservacaoQuery(ObterProfessorTitular(diarioBordo), request.ObservacaoId));
            else
                return default;
        }

        private List<ProfessorTitularDisciplinaEol> ObterProfessorTitular(DiarioBordoDetalhesDto diarioBordo)
        {
            return new List<ProfessorTitularDisciplinaEol> { new ProfessorTitularDisciplinaEol { ProfessorRf = diarioBordo.Auditoria.CriadoRF, ProfessorNome = diarioBordo.Auditoria.CriadoPor } };
        }
    }
}
