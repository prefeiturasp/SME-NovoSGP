using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.NovoEncaminhamentoNAAPA.RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPA
{
    internal class RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPACommandHandler : IRequestHandler<RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPACommand, long>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes;

        public RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPACommandHandler(IMediator mediator, IRepositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes = repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes ?? throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes));
        }

        public async Task<long> Handle(RegistrarHistoricoDeAlteracaoDaSituacaoDoNovoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            var historicoAlteracao = await ObterHistoricoAlteracaoDaSituacao(request.encaminhamentoEscolar, request.SituacaoAlterada);

            if (historicoAlteracao.NaoEhNulo())
                return await repositorioNovoEncaminhamentoNAAPAHistoricoAlteracoes.SalvarAsync(historicoAlteracao);

            return 0;
        }

        private async Task<EncaminhamentoEscolarHistoricoAlteracoes> ObterHistoricoAlteracaoDaSituacao(EncaminhamentoEscolar encaminhamento, SituacaoNAAPA situacao)
        {
            if (encaminhamento.Situacao != situacao)
            {
                var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

                return new EncaminhamentoEscolarHistoricoAlteracoes()
                {
                    EncaminhamentoEscolarId = encaminhamento.Id,
                    DataHistorico = DateTimeExtension.HorarioBrasilia(),
                    TipoHistorico = TipoHistoricoAlteracoesAtendimentoNAAPA.Alteracao,
                    CamposAlterados = "Situação",
                    UsuarioId = usuarioLogado.Id
                };
            }

            return null;
        }
    }
}