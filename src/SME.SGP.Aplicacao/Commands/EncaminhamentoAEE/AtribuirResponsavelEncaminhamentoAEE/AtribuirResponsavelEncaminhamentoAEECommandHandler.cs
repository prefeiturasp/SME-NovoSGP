using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelEncaminhamentoAEECommandHandler : IRequestHandler<AtribuirResponsavelEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE;

        public AtribuirResponsavelEncaminhamentoAEECommandHandler(IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE)
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

            await RemovePendenciaCEFAI(encaminhamentoAEE.TurmaId, encaminhamentoAEE.Id);

            await mediator.Send(new GerarPendenciaPAEEEncaminhamentoAEECommand(encaminhamentoAEE));

            return idEntidadeEncaminhamento != 0;
        }

        private async Task RemovePendenciaCEFAI(long turmaId, long encaminhamentoAEEId)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));
            var ehCEFAI = await EhCoordenadorCEFAI(usuarioLogado, turma);
            if (ehCEFAI)
            {
                var pendencia = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdQuery(encaminhamentoAEEId));
                await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendencia.Id));
            }                
        }

        private async Task<bool> EhCoordenadorCEFAI(Usuario usuarioLogado, Turma turma)
        {
            if (!usuarioLogado.EhCoordenadorCEFAI())
                return false;

            var codigoDre = await mediator.Send(new ObterCodigoDREPorUeIdQuery(turma.UeId));
            if (string.IsNullOrEmpty(codigoDre))
                return false;

            return await UsuarioTemFuncaoCEFAINaDRE(usuarioLogado, codigoDre);
        }

        private async Task<bool> UsuarioTemFuncaoCEFAINaDRE(Usuario usuarioLogado, string codigoDre)
        {
            var funcionarios = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoDre, new List<Guid>() { Perfis.PERFIL_CEFAI }));
            return funcionarios.Any(c => c == usuarioLogado.CodigoRf);
        }
    }
}
