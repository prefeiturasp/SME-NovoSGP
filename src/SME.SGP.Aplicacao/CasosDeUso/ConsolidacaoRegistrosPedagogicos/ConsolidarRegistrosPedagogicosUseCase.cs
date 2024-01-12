using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarRegistrosPedagogicosUseCase : AbstractUseCase, IConsolidarRegistrosPedagogicosUseCase
    {
        public ConsolidarRegistrosPedagogicosUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var parametrosConsolidacao = await mediator.Send(new ObterParametrosSistemaPorTiposQuery() { Tipos = new long[] { (long)TipoParametroSistema.ExecucaoConsolidacaoRegistrosPedagogicos } });
            var ues = await mediator.Send(ObterTodasUesIdsQuery.Instance);

            foreach (var parametro in parametrosConsolidacao)
            {
                if (parametro.Ano < DateTime.Now.Year && await mediator.Send(new ExisteConsolidacaoRegistroPedagogicoPorAnoQuery(parametro.Ano.GetValueOrDefault())))
                    continue;

                foreach (var ue in ues)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarRegistrosPedagogicosPorUeTratar, new FiltroConsolidacaoRegistrosPedagogicosPorUeDto(ue, parametro.Ano.GetValueOrDefault()), Guid.NewGuid(), null));

                await AtualizarDataExecucao(parametro);
            }

            return true;
        }

        private async Task AtualizarDataExecucao(ParametrosSistema parametroConsolidacao)
        {
            parametroConsolidacao.Valor = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            await mediator.Send(new AtualizarParametroSistemaCommand(parametroConsolidacao));
        }
    }
}
