using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaEscolaDados
{
    public class ObterProficienciaEscolaDadosQueryHandler : IRequestHandler<ObterProficienciaEscolaDadosQuery, PainelEducacionalProficienciaEscolaDadosDto>
    {
        private readonly IMediator mediator; 

        public ObterProficienciaEscolaDadosQueryHandler(
            IMediator mediator) 
        {          
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PainelEducacionalProficienciaEscolaDadosDto> Handle(ObterProficienciaEscolaDadosQuery request, CancellationToken cancellationToken)
        {
            var diretor = await mediator.Send(new ObterFuncionariosPorCargoUeQuery(request.CodigoUe, Convert.ToInt64(Cargo.Diretor)), cancellationToken);

            var informacoesEscola = await mediator.Send(new ObterDadosEscolaPorUeEolQuery(request.CodigoUe), cancellationToken);

            if (informacoesEscola == null)
                throw new NegocioException("Nenhum dado encontrado para a unidade escolar informada.");

            return new PainelEducacionalProficienciaEscolaDadosDto
            {
                NomeUe = informacoesEscola?.Nome ?? "",
                Diretor = diretor?.FirstOrDefault()?.NomeServidor ?? "",
                Telefone = informacoesEscola?.Telefone ?? "",
                Email = informacoesEscola?.Email ?? "",
                CodigoEol = informacoesEscola?.Codigo ?? "",
                CodigoInep = informacoesEscola?.CodigoINEP ?? ""
            };
        }
    }
}
