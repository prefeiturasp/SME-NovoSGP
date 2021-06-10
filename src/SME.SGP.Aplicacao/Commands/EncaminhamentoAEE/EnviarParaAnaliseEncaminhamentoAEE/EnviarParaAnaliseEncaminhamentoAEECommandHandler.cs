using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarParaAnaliseEncaminhamentoAEECommandHandler : IRequestHandler<EnviarParaAnaliseEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public EnviarParaAnaliseEncaminhamentoAEECommandHandler(IMediator mediator,
            IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<bool> Handle(EnviarParaAnaliseEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEComTurmaPorIdQuery(request.EncaminhamentoId));

            if (encaminhamentoAEE == null)
                throw new NegocioException("O encaminhamento informado não foi encontrado");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId));

            if (turma == null)
                throw new NegocioException("turma não encontrada");


            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.AtribuicaoResponsavel;

            var funcionarioPAEE = await mediator.Send(new ObterPAEETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));

            if (funcionarioPAEE != null && funcionarioPAEE.Count() == 1)
            {
                encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.Analise;
                encaminhamentoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(funcionarioPAEE.FirstOrDefault().CodigoRf));
                if (await ParametroGeracaoPendenciaAtivo())
                    await mediator.Send(new GerarPendenciaPAEEEncaminhamentoAEECommand(encaminhamentoAEE));
            }

            var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);

            await mediator.Send(new ExcluirPendenciasEncaminhamentoAEECPCommand(encaminhamentoAEE.TurmaId, encaminhamentoAEE.Id));

            await GerarPendenciasEncaminhamentoAEE(encaminhamentoAEE, funcionarioPAEE);

            return idEntidadeEncaminhamento != 0;
        }
        private async Task GerarPendenciasEncaminhamentoAEE(EncaminhamentoAEE encaminhamentoAEE, IEnumerable<UsuarioEolRetornoDto> funcionarioPAEE)
        {
            if (!await ParametroGeracaoPendenciaAtivo())
                return;

            if (!funcionarioPAEE.Any())
            {
                await mediator.Send(new GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand(encaminhamentoAEE, true));
            }

            if (funcionarioPAEE.Count() > 1)
            {
                await mediator.Send(new GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand(encaminhamentoAEE, false));
            }
        }

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasEncaminhamentoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }
    }
}
