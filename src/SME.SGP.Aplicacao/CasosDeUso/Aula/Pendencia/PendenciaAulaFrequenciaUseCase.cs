using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaFrequenciaUseCase : AbstractUseCase, IPendenciaAulaFrequenciaUseCase
    {
        public PendenciaAulaFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<DreUeDto>();

            var aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.Frequencia,
                                                                                 "registro_frequencia",
                                                                                 new long[] { (int)Modalidade.EducacaoInfantil, (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio },
                                                                                 filtro.DreId));

            var aulasRegistramFrequencia = aulas.Where(a => a.PermiteRegistroFrequencia());
            if (aulasRegistramFrequencia.Any())
                await RegistraPendencia(aulasRegistramFrequencia, TipoPendencia.Frequencia);

            return true;
        }

        private async Task RegistraPendencia(IEnumerable<Aula> aulas, TipoPendencia tipoPendenciaAula)
        {
            await mediator.Send(new SalvarPendenciaAulasPorTipoCommand(aulas, tipoPendenciaAula));
        }

    }
}
