using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnosLetivosAtribuicaoCJUseCase : AbstractUseCase, IObterAnosLetivosAtribuicaoCJUseCase
    {
        public ObterAnosLetivosAtribuicaoCJUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<int[]> Executar()
        {
            var codigoRf = await mediator.Send(new ObterUsuarioLogadoRFQuery());

            var listaRetorno = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(null, null, null, 0,
                codigoRf, null, true));

            if (listaRetorno == null || !listaRetorno.Any())
                return null;

            var anosLetivos = listaRetorno.Select(a => a.Turma.AnoLetivo).Distinct().OrderByDescending(a => a);

            return anosLetivos.ToArray();
        }
    }
}
