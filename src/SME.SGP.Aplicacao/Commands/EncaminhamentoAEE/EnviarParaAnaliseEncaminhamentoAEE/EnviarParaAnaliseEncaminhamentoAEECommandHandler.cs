using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

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
            var encaminhamentoAEE = await mediator.Send(new ObterEncaminhamentoAEEComTurmaPorIdQuery(request.EncaminhamentoId), cancellationToken);

            if (encaminhamentoAEE.EhNulo())
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.ENCAMINHAMENTO_NAO_ENCONTRADO);

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId), cancellationToken);

            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.AtribuicaoResponsavel;           

            var funcionarioPAEE = await mediator.Send(new ObterPAEETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe), cancellationToken);

            var funcionarioPAAI = await mediator.Send(new ObterResponsavelAtribuidoUePorUeTipoQuery(turma.Ue.CodigoUe, TipoResponsavelAtribuicao.PAAI), cancellationToken);

            if (funcionarioPAEE.NaoEhNulo() && funcionarioPAEE.Count() == 1)
                await AtribuirResponsavelPAEEPAAI(encaminhamentoAEE, funcionarioPAEE.FirstOrDefault().CodigoRf);
            else if (funcionarioPAAI.NaoEhNulo() && funcionarioPAAI.Count() == 1)
                await AtribuirResponsavelPAEEPAAI(encaminhamentoAEE, funcionarioPAAI.FirstOrDefault().CodigoRf);

            var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);

            await mediator.Send(new ExcluirPendenciasEncaminhamentoAEECPCommand(encaminhamentoAEE.TurmaId, encaminhamentoAEE.Id), cancellationToken);            

            await GerarPendenciasEncaminhamentoAEE(encaminhamentoAEE, funcionarioPAEE);

            return idEntidadeEncaminhamento != 0;
        }

        private async Task AtribuirResponsavelPAEEPAAI(EncaminhamentoAEE encaminhamentoAEE, string CodigoRf)
        {
            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.Analise;
            encaminhamentoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(CodigoRf));
        }

        private async Task GerarPendenciasEncaminhamentoAEE(EncaminhamentoAEE encaminhamentoAEE, IEnumerable<UsuarioEolRetornoDto> funcionarioPAEE)
        {
            if (!await ParametroGeracaoPendenciaAtivo())
                return;

            var ehCEFAI = ((!funcionarioPAEE.Any()) && ((encaminhamentoAEE.ResponsavelId == 0) || (encaminhamentoAEE.ResponsavelId.EhNulo())));

            if (ehCEFAI)
            {
                await mediator.Send(new GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand(encaminhamentoAEE, true));
            }
            else
            {
                if ((encaminhamentoAEE.ResponsavelId.NaoEhNulo()) && (encaminhamentoAEE.ResponsavelId > 0))
                {
                    await mediator.Send(new GerarPendenciaPAEEEncaminhamentoAEECommand(encaminhamentoAEE));
                    return;
                }

                if (funcionarioPAEE.Count() > 1)
                {
                    await mediator.Send(new GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand(encaminhamentoAEE, false));
                    return;
                }
            }
        }

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasEncaminhamentoAEE, DateTime.Today.Year));

            return parametro.NaoEhNulo() && parametro.Ativo;
        }
    }
}
