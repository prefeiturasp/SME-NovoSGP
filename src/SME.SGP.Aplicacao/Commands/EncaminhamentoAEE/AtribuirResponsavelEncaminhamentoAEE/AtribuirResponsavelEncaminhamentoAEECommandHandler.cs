using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Interfaces;
using System;
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

            if (encaminhamentoAEE.EhNulo())
                throw new NegocioException("O encaminhamento informado não foi encontrado");

            if (encaminhamentoAEE.Situacao == Dominio.Enumerados.SituacaoAEE.Finalizado
             || encaminhamentoAEE.Situacao == Dominio.Enumerados.SituacaoAEE.Encerrado)
                throw new NegocioException("A situação do encaminhamento não permite a remoção do responsável");

            var rfResponsavel = request.RfResponsavel.Trim();
            await ValidarResponsavelPAEE(encaminhamentoAEE, rfResponsavel, cancellationToken);

            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.Analise;
            encaminhamentoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(rfResponsavel));

            var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);

            var dadosPendencia = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdQuery(encaminhamentoAEE.Id));
            
            if (dadosPendencia.NaoEhNulo() && dadosPendencia.PendenciaId > 0)
                await RemovePendencias(dadosPendencia.PendenciaId);
            
            if (await ParametroGeracaoPendenciaAtivo())
                await mediator.Send(new GerarPendenciaPAEEEncaminhamentoAEECommand(encaminhamentoAEE));

            return idEntidadeEncaminhamento != 0;
        }

        private async Task ValidarResponsavelPAEE(EncaminhamentoAEE encaminhamentoAEE, string rfResponsavel, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId), cancellationToken);

            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var funcionariosPAEE = await mediator.Send(new ObterPAEETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe), cancellationToken);
            var rfsPAEE = funcionariosPAEE?
                .Where(funcionario => !string.IsNullOrWhiteSpace(funcionario.CodigoRf))
                .Select(funcionario => funcionario.CodigoRf.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (rfsPAEE.NaoEhNulo() && rfsPAEE.Any()
                && !rfsPAEE.Contains(rfResponsavel.Trim(), StringComparer.OrdinalIgnoreCase))
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.RESPONSAVEL_ATRIBUIDO_DEVE_SER_PAEE_DA_UE);
        }

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasEncaminhamentoAEE, DateTime.Today.Year));

            return parametro.NaoEhNulo() && parametro.Ativo;
        }

        private async Task RemovePendencias(long pendenciaEncaminhamentoId)
        {
            await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendenciaEncaminhamentoId));
        }
    }
}
