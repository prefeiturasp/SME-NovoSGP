using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommandHandler : 
        IRequestHandler<ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand, 
            IEnumerable<PendenciaDiarioBordoParaExcluirDto>>
    {
        private readonly IRepositorioPendenciaDiarioBordoConsulta _repositorioPendenciaDiarioBordoConsulta;
        public ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommandHandler(
            IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordoConsulta)
        {
            _repositorioPendenciaDiarioBordoConsulta = repositorioPendenciaDiarioBordoConsulta;
        }
        public async Task<IEnumerable<PendenciaDiarioBordoParaExcluirDto>> Handle(
            ObterAulasComPendenciaDiarioBordoResolvidaPorTurmaCommand request, 
            CancellationToken cancellationToken)
        {
            return await _repositorioPendenciaDiarioBordoConsulta
                .ListarPendenciaDiarioBordoParaExcluirPorIdTurma(request.TurmaId);
        }
    }
}