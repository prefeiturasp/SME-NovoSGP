using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase : AbstractUseCase, IObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase
    {
        public ObterSemestresPorAnoLetivoModalidadeEUeCodigoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<int>> Executar(bool consideraHistorico, int modalidade, int anoLetivo, string ueCodigo)
        {
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            if (usuario == null)
                throw new NegocioException("Usuário não encontrado");

            var semestres = await mediator.Send(new ObterSemestresPorAnoLetivoModalidadeEUeCodigoQuery(usuario.Login,
                                                                                                       usuario.PerfilAtual,
                                                                                                       modalidade,
                                                                                                       consideraHistorico,
                                                                                                       anoLetivo,
                                                                                                       ueCodigo));
            if (semestres == null || !semestres.Any())
                return default;

            return semestres;
        }
    }
}
