using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPorFechamentoTurmaDisciplinaIdAlunoCodigoQueryHandler : IRequestHandler<ObterPorFechamentoTurmaDisciplinaIdAlunoCodigoQuery, IEnumerable<FechamentoNotaAlunoAprovacaoDto>>
    {
        private readonly IRepositorioFechamentoNotaConsulta repositorioFechamentoNota;
        public ObterPorFechamentoTurmaDisciplinaIdAlunoCodigoQueryHandler(IRepositorioFechamentoNotaConsulta repositorioFechamentoNota)
        {
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new ArgumentNullException(nameof(repositorioFechamentoNota));
        }
        public async Task<IEnumerable<FechamentoNotaAlunoAprovacaoDto>> Handle(ObterPorFechamentoTurmaDisciplinaIdAlunoCodigoQuery request, CancellationToken cancellationToken)
             => await repositorioFechamentoNota.ObterPorFechamentosTurmaAlunoCodigo(request.Ids,request.AlunoCodigo);
    }
}