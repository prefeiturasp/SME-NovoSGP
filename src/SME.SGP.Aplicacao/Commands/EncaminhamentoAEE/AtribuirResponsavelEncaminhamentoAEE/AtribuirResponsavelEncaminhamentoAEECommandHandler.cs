using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelEncaminhamentoAEECommandHandler : IRequestHandler<AtribuirResponsavelEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public AtribuirResponsavelEncaminhamentoAEECommandHandler(IMediator mediator, 
            IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE
            )
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<bool> Handle(AtribuirResponsavelEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEComTurmaPorIdQuery(request.EncaminhamentoId));

            if (encaminhamentoAEE == null)
                throw new NegocioException("O encaminhamento informado não foi encontrado");

            if (encaminhamentoAEE.Situacao == Dominio.Enumerados.SituacaoAEE.Finalizado
             || encaminhamentoAEE.Situacao == Dominio.Enumerados.SituacaoAEE.Encerrado)
                throw new NegocioException("A situação do encaminhamento não permite a remoção do responsável");

            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.Analise;
            encaminhamentoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(request.RfResponsavel));

            var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);

            await RemovePendencias(encaminhamentoAEE.TurmaId, encaminhamentoAEE.Id);

            if (await ParametroGeracaoPendenciaAtivo())
                await mediator.Send(new GerarPendenciaPAEEEncaminhamentoAEECommand(encaminhamentoAEE));

            return idEntidadeEncaminhamento != 0;
        }

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasEncaminhamentoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }

        private async Task RemovePendencias(long turmaId, long encaminhamentoAEEId)
        {
            await mediator.Send(new ExcluirPendenciasEncaminhamentoAEECEFAICommand(turmaId, encaminhamentoAEEId));
            await mediator.Send(new ExcluirPendenciasEncaminhamentoAEECPCommand(turmaId, encaminhamentoAEEId));
        }
    }
}
