using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.Relatorios;
using SME.SGP.Infra.Enumerados;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ImpressaoConselhoClasseTurmaUseCase : IImpressaoConselhoClasseTurmaUseCase
    {
        private readonly IMediator mediator;

        public ImpressaoConselhoClasseTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioConselhoClasseDto filtroRelatorioConselhoClasseDto)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioLogadoIdQuery());

            if (usuarioId == 0)
                throw new NegocioException("Não foi possível localizar o usuário.");
            

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ConselhoClasseTurma, filtroRelatorioConselhoClasseDto, usuarioId));
        }
    }
}
