using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitoraUe
{
    public class ObterFluenciaLeitoraUeQueryHandler : IRequestHandler<ObterFluenciaLeitoraUeQuery, IEnumerable<PainelEducacionalFluenciaLeitoraUeDto>>
    {
        private readonly IRepositorioConsultaFluenciaLeitoraUe repositorio;

        public ObterFluenciaLeitoraUeQueryHandler(IRepositorioConsultaFluenciaLeitoraUe repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<PainelEducacionalFluenciaLeitoraUeDto>> Handle(ObterFluenciaLeitoraUeQuery request, CancellationToken cancellationToken)
        {
            var registros = await repositorio.ObterFluenciaLeitoraUe(request.Filtro);
            return registros
                .GroupBy(r => new { r.Turma, r.AlunosPrevistos, r.AlunosAvaliados, r.PreLeitorTotal })
                .Select(g => new PainelEducacionalFluenciaLeitoraUeDto
                {
                    Turma = g.Key.Turma,
                    AlunosPrevistos = g.Key.AlunosPrevistos,
                    AlunosAvaliados = g.Key.AlunosAvaliados,
                    TotalPreLeitor = g.Key.PreLeitorTotal,
                    Indicadores = g
                        .GroupBy(f => new { f.Fluencia, f.QuantidadeAlunoFluencia, f.PercentualFluencia })
                        .Select(a => new IndicadorPreLeitorDto
                        {
                            Fluencia = (int)a.Key.Fluencia,
                            QuantidadeAlunos = a.Sum(z => z.QuantidadeAlunoFluencia),
                            PercentualFluencia = a.Sum(z => z.PercentualFluencia),
                        })
                        .OrderBy(x => x.Fluencia)
                        .ToList()
                })
                .OrderBy(x => x.Turma)
                .ToList();
        }
    }
}
