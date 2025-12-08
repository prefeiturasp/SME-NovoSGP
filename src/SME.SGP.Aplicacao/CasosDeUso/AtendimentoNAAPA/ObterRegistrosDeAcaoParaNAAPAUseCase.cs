using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterRegistrosDeAcaoParaNAAPAUseCase : IObterRegistrosDeAcaoParaNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterRegistrosDeAcaoParaNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public Task<PaginacaoResultadoDto<RegistroAcaoBuscaAtivaNAAPADto>> Executar(string param)
        {
            return mediator.Send(new ObterRegistrosDeAcaoParaNAAPAQuery(param));
        }
    }
}
