using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarParaAnaliseEncaminhamentoAEECommandHandler : IRequestHandler<EnviarParaAnaliseEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;
        private readonly IServicoEncaminhamentoAEE servicoEncaminhamentoAEE;

        public EnviarParaAnaliseEncaminhamentoAEECommandHandler(IMediator mediator,
            IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE,
            IServicoEncaminhamentoAEE servicoEncaminhamentoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
            this.servicoEncaminhamentoAEE = servicoEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(servicoEncaminhamentoAEE));
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

            var funciorarioPAEE = await servicoEncaminhamentoAEE.ObterPAEETurma(turma);

            if (funciorarioPAEE != null && funciorarioPAEE.Count() == 1)
            {
                encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.Analise;
                encaminhamentoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(funciorarioPAEE.FirstOrDefault().CodigoRf));
                await mediator.Send(new GerarPendenciaPAEEEncaminhamentoAEECommand(encaminhamentoAEE));
            }

            var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);

            await mediator.Send(new ExcluirPendenciasEncaminhamentoAEECPCommand(encaminhamentoAEE.TurmaId, encaminhamentoAEE.Id));

            if (!funciorarioPAEE.Any())
            {
                await mediator.Send(new GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand(encaminhamentoAEE, true));
            }

            if (funciorarioPAEE.Count() > 1)
            {
                await mediator.Send(new GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand(encaminhamentoAEE, false));
            }

            return idEntidadeEncaminhamento != 0;
        }

    }
}
