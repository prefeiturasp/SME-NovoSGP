using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesComNotaDeFechamentoOuConselhoQueryHandler : IRequestHandler<ObterComponentesComNotaDeFechamentoOuConselhoQuery, IEnumerable<ComponenteCurricularDto>>
    {
        IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;

        public ObterComponentesComNotaDeFechamentoOuConselhoQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }
        public async Task<IEnumerable<ComponenteCurricularDto>> Handle(ObterComponentesComNotaDeFechamentoOuConselhoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioComponenteCurricular.ObterComponentesComNotaDeFechamentoOuConselhoPorAlunoEBimestre(request.AnoLetivo, request.TurmaId, request.Bimestre, request.CodigoAluno);
        }
    }
}