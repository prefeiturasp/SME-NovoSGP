using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasUseCase : IRegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasUseCase
    {
        private readonly IMediator mediator;

        public RegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var frequenciasAlunoIds = param.Mensagem
                .ToString().Replace("[", string.Empty).Replace("]", string.Empty)
                .Split(',')
                .Select(long.Parse)
                .ToArray();

            foreach (var frequenciaAlunoId in frequenciasAlunoIds)
            {
                await mediator
                    .Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RegularizarFrequenciaAlunoPresencasMaiorTotalAulasPorRegistro, frequenciaAlunoId, param.CodigoCorrelacao));
            }

            return true;
        }
    }
}
