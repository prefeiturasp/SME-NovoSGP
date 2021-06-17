using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarReestruturacaoPlanoAEEUseCase : AbstractUseCase, ISalvarReestruturacaoPlanoAEEUseCase
    {
        private readonly IUnitOfWork unitOfWork;

        public SalvarReestruturacaoPlanoAEEUseCase(IMediator mediator, IUnitOfWork unitOfWork) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<long> Executar(PlanoAEEReestrutucacaoPersistenciaDto param)
        {
            var reestruturacao = param.ReestruturacaoId.HasValue ?
                await mediator.Send(new ObterReestruturacaoPlanoAEEPorIdQuery(param.ReestruturacaoId.Value)) :
                new PlanoAEEReestruturacao();

            if (await ExisteReestruturacaoParaVersao(param.VersaoId, param.ReestruturacaoId))
                throw new NegocioException("Já existe uma reestruturação do plano cadastrada para esta versão");

            reestruturacao.PlanoAEEVersaoId = param.VersaoId;
            reestruturacao.Semestre = param.Semestre;
            reestruturacao.Descricao = param.Descricao;

            long reestruturacaoId = await SalvarReestruturacao(reestruturacao);

            if (await ParametroNotificacoesPlanoAtivo())
                await mediator.Send(new EnviarFilaNotificacaoReestruturacaoPlanoAEECommand(reestruturacaoId));

            return reestruturacaoId;
        }

        private async Task<long> SalvarReestruturacao(PlanoAEEReestruturacao reestruturacao)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var reestruturacaoId = await mediator.Send(new SalvarPlanoAEEReestruturacaoCommand(reestruturacao));
                    await mediator.Send(new AtualizarSituacaoPlanoAEEPorVersaoCommand(reestruturacao.PlanoAEEVersaoId, SituacaoPlanoAEE.Reestruturado));

                    unitOfWork.PersistirTransacao();
                    return reestruturacaoId;
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task<bool> ExisteReestruturacaoParaVersao(long versaoId, long? reestruturacaoId)
            => await mediator.Send(new VerificaExistenciaReestruturacaoPorVersaoPlanoAEEIdQuery(versaoId, reestruturacaoId.GetValueOrDefault()));

        private async Task<bool> ParametroNotificacoesPlanoAtivo()
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.GerarNotificacaoPlanoAEE, DateTime.Today.Year));

            return parametro != null && parametro.Ativo;
        }
    }
}
