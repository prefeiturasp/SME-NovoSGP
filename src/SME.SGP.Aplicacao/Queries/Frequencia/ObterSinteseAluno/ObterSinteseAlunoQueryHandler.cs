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
            var sintese = request.PercentualFrequencia.EhNulo() ?
                SinteseEnum.NaoFrequente :
                request.PercentualFrequencia >= await mediator.Send(new ObterFrequenciaMediaQuery(request.Disciplina, request.AnoLetivo)) ?
                SinteseEnum.Frequente :
                SinteseEnum.NaoFrequente;

            return new SinteseDto()
            {
                Id = sintese,
                Valor = sintese.Name()
            };
        }
    }
}
