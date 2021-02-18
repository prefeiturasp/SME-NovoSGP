using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarReestruturacaoPlanoAEEUseCase : AbstractUseCase, ISalvarReestruturacaoPlanoAEEUseCase
    {
        public SalvarReestruturacaoPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
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

            return await mediator.Send(new SalvarPlanoAEEReestruturacaoCommand(reestruturacao));
        }

        private async Task<bool> ExisteReestruturacaoParaVersao(long versaoId, long? reestruturacaoId)
            => await mediator.Send(new VerificaExistenciaReestruturacaoPorVersaoPlanoAEEIdQuery(versaoId, reestruturacaoId.GetValueOrDefault()));

    }
}
