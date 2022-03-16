using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao
{
   public class ObterFechamentosTurmaComponentesQueryHandler : IRequestHandler<ObterFechamentosTurmaComponentesQuery, IEnumerable<Dominio.FechamentoTurmaDisciplina>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;

        public ObterFechamentosTurmaComponentesQueryHandler(IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<IEnumerable<Dominio.FechamentoTurmaDisciplina>> Handle(ObterFechamentosTurmaComponentesQuery request, CancellationToken cancellationToken)
            => await repositorioFechamentoTurmaDisciplina.ObterFechamentosTurmaDisciplinas(request.TurmaId, request.ComponentesCurricularesId, request.Bimestre);
    }
}
