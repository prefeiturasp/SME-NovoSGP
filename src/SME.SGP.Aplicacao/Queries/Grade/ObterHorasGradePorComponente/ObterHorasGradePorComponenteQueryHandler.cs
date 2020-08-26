using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHorasGradePorComponenteQueryHandler : IRequestHandler<ObterHorasGradePorComponenteQuery, int>
    {
        private readonly IRepositorioGrade repositorioGrade;
        public ObterHorasGradePorComponenteQueryHandler(IRepositorioGrade repositorioGrade)
        {
            this.repositorioGrade = repositorioGrade ?? throw new ArgumentNullException(nameof(repositorioGrade));
        }

        public async Task<int> Handle(ObterHorasGradePorComponenteQuery request, CancellationToken cancellationToken)
            => await repositorioGrade.ObterHorasComponente(request.GradeId, request.ComponenteCurricular, request.Ano);
    }
}
