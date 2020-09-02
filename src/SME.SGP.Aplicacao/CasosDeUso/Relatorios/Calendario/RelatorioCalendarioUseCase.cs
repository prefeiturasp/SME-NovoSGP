using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioCalendarioUseCase : IRelatorioCalendarioUseCase
    {
        private readonly IMediator mediator;

        public RelatorioCalendarioUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(FiltroRelatorioCalendarioDto filtroRelatorioCalendarioDto)
        {

            await mediator.Send(new ValidaSeExisteTipoCalendarioPorIdQuery(filtroRelatorioCalendarioDto.TipoCalendarioId));
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            filtroRelatorioCalendarioDto.SetarDadosUsuario(usuario);

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Calendario, filtroRelatorioCalendarioDto, usuario));
        }
    }
}
