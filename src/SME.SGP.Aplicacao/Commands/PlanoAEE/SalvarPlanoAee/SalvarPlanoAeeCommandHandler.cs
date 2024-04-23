
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class SalvarPlanoAeeCommandHandler : IRequestHandler<SalvarPlanoAeeCommand, RetornoPlanoAEEDto>
    {

        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao;
        private readonly IRepositorioUsuarioConsulta repositorioUsuarioConsulta;
        private readonly IMediator mediator;

        public SalvarPlanoAeeCommandHandler(
            IRepositorioPlanoAEE repositorioPlanoAEE,
            IRepositorioPlanoAEEVersao repositorioPlanoAEEVersao,
            IRepositorioUsuarioConsulta repositorioUsuarioConsulta,
            IMediator mediator)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.repositorioPlanoAEEVersao = repositorioPlanoAEEVersao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEVersao));
            this.repositorioUsuarioConsulta = repositorioUsuarioConsulta ?? throw new ArgumentNullException(nameof(repositorioUsuarioConsulta));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<RetornoPlanoAEEDto> Handle(SalvarPlanoAeeCommand request, CancellationToken cancellationToken)
        {
            var plano = await MapearParaEntidade(request);

            var planoAeeDto = request.PlanoAEEDto;
            var planoId = planoAeeDto.Id.GetValueOrDefault();

            // Última versão plano
            int ultimaVersaoPlanoAee = planoId != 0 ? await ObterUltimaVersaoPlanoAEE(planoId) : 1;

            // Salva Plano
            if (plano?.Situacao == SituacaoPlanoAEE.Devolvido ||
               ((plano?.Situacao == SituacaoPlanoAEE.Expirado || plano?.Situacao == SituacaoPlanoAEE.Validado) && string.IsNullOrWhiteSpace(plano.ParecerCoordenacao)))
            {
                plano.Situacao = SituacaoPlanoAEE.ParecerCP;
            }
            else if (plano?.Situacao == SituacaoPlanoAEE.Expirado)
            {
                await mediator.Send(new ExcluirPendenciaPlanoAEECommand(planoId), cancellationToken);
                plano.Situacao = SituacaoPlanoAEE.Validado;
            }

            planoId = await repositorioPlanoAEE.SalvarAsync(plano);

            await TransfereResponsavelDaPendecia(plano);

            if (planoId > 0 && ultimaVersaoPlanoAee > 1)
                await mediator.Send(new ResolverPendenciaPlanoAEECommand(planoId), cancellationToken);

            // Salva Versao
            var planoAEEVersaoId = await SalvarPlanoAEEVersao(planoId, ultimaVersaoPlanoAee);

            // Salva Questoes
            foreach (var questao in planoAeeDto.Questoes)
            {
                if (await ValidaPersistenciaResposta(questao.Resposta, questao.QuestaoId))
                {
                    var planoAEEQuestaoId = await mediator.Send(new SalvarPlanoAEEQuestaoCommand(planoId, questao.QuestaoId, planoAEEVersaoId));
                    await mediator.Send(new SalvarPlanoAEERespostaCommand(planoId, planoAEEQuestaoId, questao.Resposta, questao.TipoQuestao), cancellationToken);
                }
            }

            if (await ParametroGeracaoPendenciaAtivo() && plano?.Situacao != SituacaoPlanoAEE.Validado)
                await mediator.Send(new GerarPendenciaValidacaoPlanoAEECommand(planoId, PerfilUsuario.CP), cancellationToken);

            return new RetornoPlanoAEEDto(planoId, planoAEEVersaoId);
        }

        private async Task TransfereResponsavelDaPendecia(PlanoAEE plano)
        {
            var command = new TransferirPendenciaParaNovoResponsavelCommand(plano.Id, plano.ResponsavelId);

            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.RotaTransferirPendenciaPlanoAEEParaNovoResponsavel, command, Guid.NewGuid()));
        }

        private async Task<bool> ValidaPersistenciaResposta(string resposta, long questaoId)
        {
            return !string.IsNullOrEmpty(resposta) ||
                await VerificaObrigatoriedadeQuestao(questaoId);
        }

        private async Task<bool> VerificaObrigatoriedadeQuestao(long questaoId)
        {
            var questaoObrigatoria = await mediator.Send(new VerificaObrigatoriedadeQuestaoQuery(questaoId));

            if (questaoObrigatoria)
                throw new NegocioException($"Questão Obrigatória [{questaoId}] não respondida.");

            return false;
        }

        private async Task<long> SalvarPlanoAEEVersao(long planoId, int ultimaVersaoPlanoAee)
        {
            var planoVersaoEntidade = new PlanoAEEVersao
            {
                PlanoAEEId = planoId,
                Numero = ultimaVersaoPlanoAee
            };
            return await repositorioPlanoAEEVersao.SalvarAsync(planoVersaoEntidade);
        }

        private async Task<int> ObterUltimaVersaoPlanoAEE(long planoId)
        {
            var versaoPlano = await repositorioPlanoAEEVersao.ObterUltimaVersaoPorPlanoId(planoId);

            return versaoPlano.NaoEhNulo() ?
                versaoPlano.Numero + 1 : 1;
        }

        private async Task<PlanoAEE> MapearParaEntidade(SalvarPlanoAeeCommand request)
        {
            var responsavelId = await repositorioUsuarioConsulta.ObterUsuarioIdPorCodigoRfAsync(request.PlanoAEEDto.ResponsavelRF);

            if (request.PlanoAEEDto.Id.HasValue && request.PlanoAEEDto.Id > 0)
            {
                var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(request.PlanoAEEDto.Id.Value));
                planoAEE.TurmaId = request.TurmaId;
                planoAEE.ResponsavelId = responsavelId;
                return planoAEE;
            }

            return new PlanoAEE()
            {
                TurmaId = request.TurmaId,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                AlunoCodigo = request.AlunoCodigo,
                AlunoNumero = request.AlunoNumero,
                AlunoNome = request.AlunoNome,
                Questoes = new System.Collections.Generic.List<PlanoAEEQuestao>(),
                ResponsavelId = responsavelId
            };
        }

        private async Task<bool> ParametroGeracaoPendenciaAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarPendenciasPlanoAEE, DateTime.Today.Year));

            return parametro.NaoEhNulo() && parametro.Ativo;
        }
    }
}
