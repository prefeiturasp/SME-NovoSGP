using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasPorRegistroUseCase : IRegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasPorRegistroUseCase
    {
        private readonly IMediator mediator;

        public RegularizarFrequenciaAlunoPresencasMaiorQuantidadeAulasPorRegistroUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var frequenciaAlunoId = long.Parse(param.Mensagem.ToString());

            return await mediator
                .Send(new RegularizarFrequenciaPresencasMaiorQuantidadeAulasCommand(frequenciaAlunoId));
        }
    }
}
