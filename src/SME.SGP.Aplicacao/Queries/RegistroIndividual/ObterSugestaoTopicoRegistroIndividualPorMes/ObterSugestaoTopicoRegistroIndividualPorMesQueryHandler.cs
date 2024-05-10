using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSugestaoTopicoRegistroIndividualPorMesQueryHandler : IRequestHandler<ObterSugestaoTopicoRegistroIndividualPorMesQuery, SugestaoTopicoRegistroIndividualDto>
    {
        private readonly IRepositorioRegistroIndividual repositorioRegistroIndividual;

        public ObterSugestaoTopicoRegistroIndividualPorMesQueryHandler(IRepositorioRegistroIndividual repositorioRegistroIndividual)
        {
            this.repositorioRegistroIndividual = repositorioRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioRegistroIndividual));
        }

        public async Task<SugestaoTopicoRegistroIndividualDto> Handle(ObterSugestaoTopicoRegistroIndividualPorMesQuery request, CancellationToken cancellationToken)
        {
            var susgestaoTopico = await repositorioRegistroIndividual.ObterSugestaoTopicoPorMes(request.Mes);
            return susgestaoTopico;
        }
    }
}
