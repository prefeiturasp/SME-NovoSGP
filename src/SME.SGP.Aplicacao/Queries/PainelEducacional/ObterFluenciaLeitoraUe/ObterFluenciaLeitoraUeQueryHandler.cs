using MediatR;
using SME.SGP.Dominio.Enumerados;
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
                .Select(g =>
                {
                    int alunosPrevistos = g.FirstOrDefault().AlunosPrevistos;
                    int alunosAvaliados = g.FirstOrDefault().AlunosAvaliados;
                    int preLeitorTotal = g.FirstOrDefault().PreLeitorTotal;

                    var fluencias = g
                        .GroupBy(x => (int)x.Fluencia)
                        .ToDictionary(
                            f => f.Key,
                            f => f.FirstOrDefault().QuantidadeAlunoFluencia
                        );

                    return new PainelEducacionalFluenciaLeitoraUeDto
                    {
                        Turma = g.Key.Turma,
                        AlunosPrevistos = alunosPrevistos,
                        AlunosAvaliados = IndicadorQuantidadePercentual(alunosAvaliados, alunosPrevistos),
                        TotalPreLeitor = IndicadorQuantidadePercentual(preLeitorTotal, alunosAvaliados),
                        PreLeitor1 = IndicadorQuantidadePercentual(fluencias[(int)NivelFluenciaLeitoraEnum.Fluencia1], alunosAvaliados),
                        PreLeitor2 = IndicadorQuantidadePercentual(fluencias[(int)NivelFluenciaLeitoraEnum.Fluencia2], alunosAvaliados),
                        PreLeitor3 = IndicadorQuantidadePercentual(fluencias[(int)NivelFluenciaLeitoraEnum.Fluencia3], alunosAvaliados),
                        PreLeitor4 = IndicadorQuantidadePercentual(fluencias[(int)NivelFluenciaLeitoraEnum.Fluencia4], alunosAvaliados),
                        LeitorIniciante = IndicadorQuantidadePercentual(fluencias[(int)NivelFluenciaLeitoraEnum.Fluencia5], alunosAvaliados),
                        LeitorFluente = IndicadorQuantidadePercentual(fluencias[(int)NivelFluenciaLeitoraEnum.Fluencia6], alunosAvaliados)
                    };
                })
                .OrderBy(x => x.Turma)
                .ToList();
        }

        string IndicadorQuantidadePercentual(int qtd, int total)
        {
            if (qtd == 0 || total == 0) return "0";
            var pct = Math.Round((decimal)qtd / total * 100, 2);
            return $"{qtd} ({pct}%)";
        }
    }
}
