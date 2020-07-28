using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioParecerConclusivoUseCase : IRelatorioParecerConclusivoUseCase
    {
        private readonly IMediator mediator;

        public RelatorioParecerConclusivoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioParecerConclusivoDto filtroRelatorioParecerConclusivoDto)
        {
            await mediator.Send(new ValidaSeExisteDrePorCodigoQuery(filtroRelatorioParecerConclusivoDto.DreCodigo));
            await mediator.Send(new ValidaSeExisteUePorCodigoQuery(filtroRelatorioParecerConclusivoDto.UeCodigo));

            if (filtroRelatorioParecerConclusivoDto.Modalidade == Modalidade.Infantil)
            {
                throw new NegocioException("Não é possível gerar este relatório para a modalidade infantil!");
            }

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ParecerConclusivo, filtroRelatorioParecerConclusivoDto, usuarioLogado));
        }
    }
}
