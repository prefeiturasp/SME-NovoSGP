using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroColetivoNAAPACompletoPorIdQueryHandler : ConsultasBase, IRequestHandler<ObterRegistroColetivoNAAPACompletoPorIdQuery, RegistroColetivoCompletoDto>
    {
        private readonly IRepositorioRegistroColetivo repositorioRegistroColetivo;
        public ObterRegistroColetivoNAAPACompletoPorIdQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioRegistroColetivo repositorioRegistroColetivo) : base(contextoAplicacao)
        {
            this.repositorioRegistroColetivo = repositorioRegistroColetivo ?? throw new ArgumentNullException(nameof(repositorioRegistroColetivo));
        }

        public async Task<RegistroColetivoCompletoDto> Handle(ObterRegistroColetivoNAAPACompletoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioRegistroColetivo.ObterRegistroColetivoCompletoPorId(request.Id);
        }
    }
}
