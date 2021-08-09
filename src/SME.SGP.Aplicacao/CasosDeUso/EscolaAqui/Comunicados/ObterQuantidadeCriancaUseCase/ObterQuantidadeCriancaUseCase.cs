using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeCriancaUseCase : AbstractUseCase, IObterQuantidadeCriancaUseCase
    {
        public ObterQuantidadeCriancaUseCase(IMediator mediator) : base(mediator) { }

        public async Task<QuantidadeCriancaDto> Executar(string anoTurma, string turma, string dreId, string ueId, int modalidade,int anoLetivo)
        {
            var quantidade = await mediator.Send(new ObterQuantidadeCriancaQuery(anoTurma,turma,dreId,ueId,modalidade,anoLetivo));

            if (quantidade == null)
                return default;

            return quantidade;
        }
    }
}
