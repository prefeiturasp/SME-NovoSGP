using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoAulasPrevistrasSyncUseCase : AbstractUseCase, INotificacaoAulasPrevistrasSyncUseCase
    {
        public NotificacaoAulasPrevistrasSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var quantidadeDias = await ObterQuantidadeDiasFimBimestre();

            if (quantidadeDias > 0)
            {
                // Busca registro de aula sem frequencia e sem notificação do tipo
                var turmasAulasPrevistasDivergentes = await ObterTurmasDivergentes(quantidadeDias);

                foreach (var turma in turmasAulasPrevistasDivergentes)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaNotificacaoAulasPrevistas, turma, Guid.NewGuid(), null));

                return true;
            }

            return false;
        }

        private async Task<IEnumerable<RegistroAulaPrevistaDivergenteDto>> ObterTurmasDivergentes(int quantidadeDias)
            => await mediator.Send(new ObterTurmasAulasPrevistasDivergentesQuery(quantidadeDias));

        private async Task<int> ObterQuantidadeDiasFimBimestre()
        {
            var valor = await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(TipoParametroSistema.QuantidadeDiasNotificarProfessor, DateTime.Today.Year));

            if (string.IsNullOrEmpty(valor))
                return 0;

            return int.Parse(valor);
        }
    }
}
