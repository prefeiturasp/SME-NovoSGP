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
    public class ObterGradePorTipoEscolaModalidadeDuracaoQueryHandler : IRequestHandler<ObterGradePorTipoEscolaModalidadeDuracaoQuery, GradeDto>
    {
        private readonly IRepositorioGrade repositorioGrade;
        public ObterGradePorTipoEscolaModalidadeDuracaoQueryHandler(IRepositorioGrade repositorioGrade)
        {
            this.repositorioGrade = repositorioGrade ?? throw new ArgumentNullException(nameof(repositorioGrade));
        }

        public async Task<GradeDto> Handle(ObterGradePorTipoEscolaModalidadeDuracaoQuery request, CancellationToken cancellationToken)
        {
            var grade = await repositorioGrade.ObterGradeTurma(request.TipoEscola, request.Modalidade, request.Duracao);

            return MapearParaDto(grade);
        }

        private GradeDto MapearParaDto(Grade grade)
            => grade == null ? null : new GradeDto
            {
                Id = grade.Id,
                Nome = grade.Nome
            };
    }
}
