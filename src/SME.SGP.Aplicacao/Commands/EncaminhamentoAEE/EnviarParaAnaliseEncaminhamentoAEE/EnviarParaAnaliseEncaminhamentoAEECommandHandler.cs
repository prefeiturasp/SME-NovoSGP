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

            if (encaminhamentoAEE.Situacao == Dominio.Enumerados.SituacaoAEE.Analise
                && encaminhamentoAEE.ResponsavelId.GetValueOrDefault() > 0)
                return true;

            if (!SituacaoPermiteEnvioParaAnalise(encaminhamentoAEE.Situacao))
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.ENCAMINHAMENTO_NAO_PERMITE_ENVIO_PARA_ANALISE);

            if (encaminhamentoAEE.ResponsavelId.GetValueOrDefault() > 0)
                throw new NegocioException(MensagemNegocioEncaminhamentoAee.ENCAMINHAMENTO_JA_POSSUI_RESPONSAVEL);

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(encaminhamentoAEE.TurmaId), cancellationToken);

            if (turma.EhNulo())
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.AtribuicaoResponsavel;
            encaminhamentoAEE.ResponsavelId = null;

            var funcionariosPAEE = NormalizarFuncionarios(await mediator.Send(new ObterPAEETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe), cancellationToken));

            var funcionariosPAAI = NormalizarFuncionarios(await mediator.Send(new ObterResponsavelAtribuidoUePorUeTipoQuery(turma.Ue.CodigoUe, TipoResponsavelAtribuicao.PAAI), cancellationToken));

            if (funcionariosPAEE.Count == 1)
                await AtribuirResponsavelPAEEPAAI(encaminhamentoAEE, funcionariosPAEE.First().CodigoRf);
            else if (funcionariosPAEE.Count == 0 && funcionariosPAAI.Count == 1)
                await AtribuirResponsavelPAEEPAAI(encaminhamentoAEE, funcionariosPAAI.First().CodigoRf);

            var idEntidadeEncaminhamento = await repositorioEncaminhamentoAEE.SalvarAsync(encaminhamentoAEE);

            await mediator.Send(new ExcluirPendenciasEncaminhamentoAEECPCommand(encaminhamentoAEE.TurmaId, encaminhamentoAEE.Id), cancellationToken);            

            await GerarPendenciasEncaminhamentoAEE(encaminhamentoAEE, funcionariosPAEE);

            return idEntidadeEncaminhamento != 0;
        }

        private static bool SituacaoPermiteEnvioParaAnalise(Dominio.Enumerados.SituacaoAEE situacao)
            => situacao == Dominio.Enumerados.SituacaoAEE.Encaminhado
               || situacao == Dominio.Enumerados.SituacaoAEE.AtribuicaoResponsavel
               || situacao == Dominio.Enumerados.SituacaoAEE.AtribuicaoPAAI;

        private async Task AtribuirResponsavelPAEEPAAI(EncaminhamentoAEE encaminhamentoAEE, string codigoRf)
        {
            encaminhamentoAEE.Situacao = Dominio.Enumerados.SituacaoAEE.Analise;
            encaminhamentoAEE.ResponsavelId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(codigoRf));
        }

        private static IReadOnlyList<UsuarioEolRetornoDto> NormalizarFuncionarios(IEnumerable<UsuarioEolRetornoDto> funcionarios)
            => funcionarios?
                .Where(funcionario => !string.IsNullOrWhiteSpace(funcionario.CodigoRf))
                .GroupBy(funcionario => funcionario.CodigoRf.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(grupo => grupo.First())
                .ToList()
               ?? new List<UsuarioEolRetornoDto>();

        private async Task GerarPendenciasEncaminhamentoAEE(EncaminhamentoAEE encaminhamentoAEE, IReadOnlyCollection<UsuarioEolRetornoDto> funcionariosPAEE)
        {
            if (!await ParametroGeracaoPendenciaAtivo())
                return;

            if (funcionariosPAEE.Count > 1)
            {
                await mediator.Send(new GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand(encaminhamentoAEE, false));
                return;
            }

            var ehCEFAI = !funcionariosPAEE.Any()
                && ((encaminhamentoAEE.ResponsavelId == 0) || encaminhamentoAEE.ResponsavelId.EhNulo());

            if (ehCEFAI)
            {
                await mediator.Send(new GerarPendenciaAtribuirResponsavelEncaminhamentoAEECommand(encaminhamentoAEE, true));
            }
            else if (encaminhamentoAEE.ResponsavelId.NaoEhNulo() && encaminhamentoAEE.ResponsavelId > 0)
            {
                await mediator.Send(new GerarPendenciaPAEEEncaminhamentoAEECommand(encaminhamentoAEE));
            }
        }

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasEncaminhamentoAEE, DateTime.Today.Year));

            return parametro.NaoEhNulo() && parametro.Ativo;
        }
    }
}
