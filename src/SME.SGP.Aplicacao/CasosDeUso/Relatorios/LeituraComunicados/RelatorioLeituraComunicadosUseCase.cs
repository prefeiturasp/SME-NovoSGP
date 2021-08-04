using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioLeituraComunicadosUseCase : AbstractUseCase, IRelatorioLeituraComunicadosUseCase
    {
        public RelatorioLeituraComunicadosUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(FiltroRelatorioLeituraComunicados filtro)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            filtro.NomeUsuario = usuarioLogado.Nome;

            if (filtro.DataInicio == null)
                filtro.DataInicio = new DateTime(filtro.Ano, 1, 1);

            if (filtro.DataFim == null)
                filtro.DataFim = new DateTime(filtro.Ano, 12, 31);

            if (filtro.DataInicio.GetValueOrDefault().Date > filtro.DataFim.GetValueOrDefault().Date)
                throw new NegocioException("A data de início não pode ser maior que a data fim");

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.Leitura, filtro, usuarioLogado, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosEscolaAquiLeitura));
        }

    }
}
