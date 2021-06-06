using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarAlunosFaltososUseCase : INotificarAlunosFaltososUseCase
    {
        private readonly IServicoNotificacaoFrequencia servicoNotificacaoFrequencia;

        public NotificarAlunosFaltososUseCase(IServicoNotificacaoFrequencia servicoNotificacaoFrequencia)
        {
            this.servicoNotificacaoFrequencia = servicoNotificacaoFrequencia ?? throw new ArgumentNullException(nameof(servicoNotificacaoFrequencia));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            await servicoNotificacaoFrequencia.NotificarAlunosFaltosos();
            return true;
        }
    }
}
