using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentosTurmaDisciplinasDuplicadosQueryHandler : IRequestHandler<ObterFechamentosTurmaDisciplinasDuplicadosQuery, IEnumerable<long>>
    {
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;

        public ObterFechamentosTurmaDisciplinasDuplicadosQueryHandler(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }

        public async Task<IEnumerable<long>> Handle(ObterFechamentosTurmaDisciplinasDuplicadosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurmaDisciplina
                .ObterFechamentosTurmaDisciplinaEmDuplicidade(request.DataInicio);
        }
    }
}
