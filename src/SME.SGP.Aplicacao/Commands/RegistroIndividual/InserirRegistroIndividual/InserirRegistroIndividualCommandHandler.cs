﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirRegistroIndividualCommandHandler : IRequestHandler<InserirRegistroIndividualCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public InserirRegistroIndividualCommandHandler(IMediator mediator,
                                                       IRepositorioRegistroIndividual repositorioRegistroIndividual)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioRegistroIndividual));
        }

        public async Task<AuditoriaDto> Handle(InserirRegistroIndividualCommand request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));

            if (turma == null)
                throw new NegocioException("A turma informada não foi encontrada!");

            var componenteCurricular = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(new long[] { request.ComponenteCurricularId }));

            if (componenteCurricular == null || !componenteCurricular.Any())
                throw new NegocioException("O componente curricular não foi encontrado");

            var registroIndividual = MapearParaEntidade(request);

            await repositorioRegistroIndividual.SalvarAsync(registroIndividual);

            return (AuditoriaDto)registroIndividual;
        }

        private RegistroIndividual MapearParaEntidade(InserirRegistroIndividualCommand request)
            => new RegistroIndividual()
            {
                AlunoCodigo = request.AlunoCodigo,
                ComponenteCurricularId = request.ComponenteCurricularId,
                DataRegistro = request.DataRegistro,
                Registro = request.Registro,
                TurmaId = request.TurmaId
            };
    }
}
