using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGradePorTipoEscolaModalidadeDuracaoAnoQueryHandler : IRequestHandler<ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery, Grade>
    {
        private readonly IRepositorioGrade repositorioGrade;
        public ObterGradePorTipoEscolaModalidadeDuracaoAnoQueryHandler(IRepositorioGrade repositorioGrade)
        {
            this.repositorioGrade = repositorioGrade ?? throw new ArgumentNullException(nameof(repositorioGrade));
        }

        public async Task<Grade> Handle(ObterGradePorTipoEscolaModalidadeDuracaoAnoQuery request, CancellationToken cancellationToken)
            => await repositorioGrade.ObterGradeTurmaAno(request.TipoEscola, request.Modalidade, request.Duracao, request.Ano);
    }
}
