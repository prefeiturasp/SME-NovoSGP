using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.Relatorios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class RelatorioNotasEConceitosFinaisUseCase : IRelatorioNotasEConceitosFinaisUseCase
    {
        private readonly IMediator mediator;

        public RelatorioNotasEConceitosFinaisUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioNotasEConceitosFinaisDto filtroRelatorioNotasEConceitosFinaisDto)
        {
            await mediator.Send(new ValidaSeExisteDrePorCodigoQuery(filtroRelatorioNotasEConceitosFinaisDto.DreCodigo));
            await mediator.Send(new ValidaSeExisteUePorCodigoQuery(filtroRelatorioNotasEConceitosFinaisDto.UeCodigo));

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            filtroRelatorioNotasEConceitosFinaisDto.UsuarioNome = usuarioLogado.Nome;
            filtroRelatorioNotasEConceitosFinaisDto.UsuarioRf = usuarioLogado.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.FechamentoPendencias, filtroRelatorioNotasEConceitosFinaisDto, usuarioLogado));
        }
    }
}
