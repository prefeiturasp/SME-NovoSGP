using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ExisteNovoEncaminhamentoNAAPAAtivoParaAluno
{
    public class ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoQueryHandler : IRequestHandler<ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoQuery, bool>
    {
        private readonly IRepositorioNovoEncaminhamentoNAAPA repositorio;
        public ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoQueryHandler(IRepositorioNovoEncaminhamentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }
        public Task<bool> Handle(ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ExisteEncaminhamentoNAAPAAtivoParaAluno(request.CodigoAluno);
        }
    }
}