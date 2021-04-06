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
            
            var alunosMatriculadosEol = await mediator.Send(new ObterAlunosMatriculadosPorAnoLetivoECCEolQuery(param.AnoLetivo, param.DreId, param.UeId, new int[] { 1030, 1310 }));
            if(alunosMatriculadosEol.Any())
            {
                return MapearParaDto(alunosMatriculadosEol);
            }

            return null;
        }

        private static IEnumerable<AEEAlunosMatriculadosDto> MapearParaDto(IEnumerable<AlunosMatriculadosEolDto> alunosMatriculadosEol)
        {
            List<AEEAlunosMatriculadosDto> alunos = new();

            foreach (var alunoMatriculadoEol in alunosMatriculadosEol.GroupBy(a => $"{a.Modalidade} - {a.Ano}"))
            {
                AEEAlunosMatriculadosDto aluno = new AEEAlunosMatriculadosDto()
                {
                    Descricao = alunoMatriculadoEol.Key,
                    LegendaPAEE = "Qtd. de matriculados PAEE colaborativo",
                    LegendaSRM = "Qtd. de matriculados SRM",
                    QuantidadePAEE = alunoMatriculadoEol.Where(a => a.ComponenteCurricularId == 1310).Any() ? alunoMatriculadoEol.FirstOrDefault(a => a.ComponenteCurricularId == 1310).Quantidade : 0,
                    QuantidadeSRM = alunoMatriculadoEol.Where(a => a.ComponenteCurricularId == 1030).Any() ? alunoMatriculadoEol.FirstOrDefault(a => a.ComponenteCurricularId == 1030).Quantidade : 0,
                };
                alunos.Add(aluno);

            }
            return alunos;
        }
    }
}
