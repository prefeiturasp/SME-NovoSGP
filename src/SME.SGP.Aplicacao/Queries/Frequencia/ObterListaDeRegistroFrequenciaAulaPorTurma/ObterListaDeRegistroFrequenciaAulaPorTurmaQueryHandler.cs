using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaDeRegistroFrequenciaAulaPorTurmaQueryHandler : IRequestHandler<ObterListaDeRegistroFrequenciaAulaPorTurmaQuery, IEnumerable<RegistroFrequenciaAulaParcialDto>>
    {
        private readonly IRepositorioAulaConsulta repositorioAulaConsulta;
        public ObterListaDeRegistroFrequenciaAulaPorTurmaQueryHandler(
                                IRepositorioAulaConsulta repositorioAulaConsulta)
        {
            this.repositorioAulaConsulta = repositorioAulaConsulta;
        }
        public async Task<IEnumerable<RegistroFrequenciaAulaParcialDto>> Handle(ObterListaDeRegistroFrequenciaAulaPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAulaConsulta.ObterListaDeRegistroFrequenciaAulaPorTurma(request.CodigoTurma);
        }
    }
}
