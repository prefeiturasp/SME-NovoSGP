using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarRegistroIndividualCommandHandler : IRequestHandler<AlterarRegistroIndividualCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public AlterarRegistroIndividualCommandHandler(IMediator mediator,
                                                       IRepositorioRegistroIndividual repositorioRegistroIndividual)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioRegistroIndividual));
        }

        public async Task<AuditoriaDto> Handle(AlterarRegistroIndividualCommand request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));

            if (turma == null)
                throw new NegocioException("A turma informada não foi encontrada!");

            var componenteCurricular = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { request.ComponenteCurricularId }));

            if (componenteCurricular == null || !componenteCurricular.Any())
                throw new NegocioException("O componente curricular não foi encontrado");

            var registroIndividual = await repositorioRegistroIndividual.ObterPorIdAsync(request.Id);
            if (registroIndividual == null)
                throw new NegocioException($"Registro individual {request.Id} não encontrado!");

            MapearAlteracoes(registroIndividual, request);

            await repositorioRegistroIndividual.SalvarAsync(registroIndividual);

            return (AuditoriaDto)registroIndividual;
        }

        private void MapearAlteracoes(RegistroIndividual entidade, AlterarRegistroIndividualCommand request)
        {
            entidade.AlunoCodigo = request.AlunoCodigo;
            entidade.ComponenteCurricularId = request.ComponenteCurricularId;
            entidade.DataRegistro = request.DataRegistro;
            entidade.Registro = request.Registro;
            entidade.TurmaId = request.TurmaId;
        }
    }
}
