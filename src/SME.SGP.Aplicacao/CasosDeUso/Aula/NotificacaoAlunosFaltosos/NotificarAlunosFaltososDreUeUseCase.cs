using System;
using System.Threading.Tasks;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class NotificarAlunosFaltososDreUeUseCase : INotificarAlunosFaltososDreUeUseCase
    {
        private readonly IServicoNotificacaoFrequencia servicoNotificacaoFrequencia;

        public NotificarAlunosFaltososDreUeUseCase(IServicoNotificacaoFrequencia servicoNotificacaoFrequencia)
        {
            this.servicoNotificacaoFrequencia = servicoNotificacaoFrequencia ?? throw new ArgumentNullException(nameof(servicoNotificacaoFrequencia));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<DreUeDto>();

            if (filtro.EhNulo()) 
                return false;            
            
            await servicoNotificacaoFrequencia.NotificarAlunosFaltosos(filtro.UeId);
            return true;
        }
    }
}