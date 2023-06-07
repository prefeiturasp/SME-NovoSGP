using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosAlunosComAnotacaoNoFechamentoQueryHandler : IRequestHandler<ObterCodigosAlunosComAnotacaoNoFechamentoQuery, IEnumerable<string>>
    {
        private readonly IRepositorioAnotacaoFechamentoAlunoConsulta repositorioAnotacaoFechamentoAlunoConsulta;

        public ObterCodigosAlunosComAnotacaoNoFechamentoQueryHandler(IRepositorioAnotacaoFechamentoAlunoConsulta repositorioAnotacaoFechamentoAlunoConsulta)
        {
            this.repositorioAnotacaoFechamentoAlunoConsulta = repositorioAnotacaoFechamentoAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioAnotacaoFechamentoAlunoConsulta));
        }

        public Task<IEnumerable<string>> Handle(ObterCodigosAlunosComAnotacaoNoFechamentoQuery request, CancellationToken cancellationToken)
            => repositorioAnotacaoFechamentoAlunoConsulta.ObterAlunosComAnotacaoNoFechamento(request.FechamentoTurmaDisciplinaId);
    }
}
