using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSinteseAlunoQueryHandler : IRequestHandler<ObterSinteseAlunoQuery, SinteseDto>
    {
        private readonly IMediator mediator;
        public ObterSinteseAlunoQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<SinteseDto> Handle(ObterSinteseAlunoQuery request, CancellationToken cancellationToken)
        {
            var sintese = SinteseEnum.NaoFrequente;

            if (request.PercentualFrequencia.NaoEhNulo() && 
                request.PercentualFrequencia >= await mediator.Send(new ObterFrequenciaMediaQuery(request.Disciplina, request.AnoLetivo)))
                sintese = SinteseEnum.Frequente;

            return new SinteseDto()
            {
                Id = sintese,
                Valor = sintese.Name()
            };
        }
    }
}
