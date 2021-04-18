using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosMatriculadosSRMPAEEUseCase : AbstractUseCase, IObterAlunosMatriculadosSRMPAEEUseCase
    {
        public ObterAlunosMatriculadosSRMPAEEUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<IEnumerable<AEEAlunosMatriculadosDto>> Executar(FiltroDashboardAEEDto param)
        {
            if (param.AnoLetivo == 0)
                param.AnoLetivo = DateTime.Now.Year;
            
            var alunosMatriculadosEol = await mediator.Send(new ObterAlunosMatriculadosPorAnoLetivoECCEolQuery(param.AnoLetivo, param.DreCodigo, param.UeCodigo, new int[] { 1030, 1310 }));
            if(alunosMatriculadosEol.Any())
            {
                return param.UeCodigo != null ? MapearParaDtoTurmas(alunosMatriculadosEol.GroupBy(a => $"{a.Modalidade} - {a.Turma}")) : MapearParaDto(alunosMatriculadosEol.GroupBy(a => $"{a.Modalidade} - {a.Ano}"));
            }

            return null;
        }

        private static IEnumerable<AEEAlunosMatriculadosDto> MapearParaDto(IEnumerable<IGrouping<string, AlunosMatriculadosEolDto>> alunosMatriculadosEol)
        {
            List<AEEAlunosMatriculadosDto> alunos = new List<AEEAlunosMatriculadosDto>();

            foreach (var alunoMatriculadoEol in alunosMatriculadosEol)
            {
                AEEAlunosMatriculadosDto aluno = new AEEAlunosMatriculadosDto()
                {
                    Ordem = alunoMatriculadoEol.First().Ordem,
                    Descricao = alunoMatriculadoEol.First().Ano != "" ? $"{alunoMatriculadoEol.First().Modalidade} - {alunoMatriculadoEol.First().Ano}" : alunoMatriculadoEol.First().Modalidade,
                    LegendaPAEE = "Qtd. de matriculados PAEE colaborativo",
                    LegendaSRM = "Qtd. de matriculados SRM",
                    QuantidadePAEE = alunoMatriculadoEol.Any(a => a.ComponenteCurricularId == 1310) ? alunoMatriculadoEol.Where(a => a.ComponenteCurricularId == 1310).Sum(a => a.Quantidade) : 0,
                    QuantidadeSRM = alunoMatriculadoEol.Any(a => a.ComponenteCurricularId == 1030) ? alunoMatriculadoEol.Where(a => a.ComponenteCurricularId == 1030).Sum(a => a.Quantidade) : 0,
                };
                alunos.Add(aluno);

            }
            return alunos
                .OrderBy(a => a.Ordem)
                .ThenBy(a => a.Descricao);
        }


        private static IEnumerable<AEEAlunosMatriculadosDto> MapearParaDtoTurmas(IEnumerable<IGrouping<string, AlunosMatriculadosEolDto>> alunosMatriculadosEol)
        {
            List<AEEAlunosMatriculadosDto> alunos = new List<AEEAlunosMatriculadosDto>();

            foreach (var alunoMatriculadoEol in alunosMatriculadosEol)
            {
                AEEAlunosMatriculadosDto aluno = new AEEAlunosMatriculadosDto()
                {
                    Ordem = alunoMatriculadoEol.First().Ordem,
                    Descricao = alunoMatriculadoEol.First().Turma != "" ? $"{alunoMatriculadoEol.First().Modalidade} - {alunoMatriculadoEol.First().Turma}" : alunoMatriculadoEol.First().Modalidade,
                    LegendaPAEE = "Qtd. de matriculados PAEE colaborativo",
                    LegendaSRM = "Qtd. de matriculados SRM",
                    QuantidadePAEE = alunoMatriculadoEol.Any(a => a.ComponenteCurricularId == 1310) ? alunoMatriculadoEol.FirstOrDefault(a => a.ComponenteCurricularId == 1310).Quantidade : 0,
                    QuantidadeSRM = alunoMatriculadoEol.Any(a => a.ComponenteCurricularId == 1030) ? alunoMatriculadoEol.FirstOrDefault(a => a.ComponenteCurricularId == 1030).Quantidade : 0,
                };
                alunos.Add(aluno);

            }
            return alunos
                .OrderBy(a => a.Ordem)
                .ThenBy(a => a.Descricao);
        }
    }
}
