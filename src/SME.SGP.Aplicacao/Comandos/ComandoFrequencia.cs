using MediatR;
using Sentry;
using SME.Background.Core;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandoFrequencia : IComandoFrequencia
    {
        private readonly IConsultasAula consultasAula;
        private readonly IServicoFrequencia servicoFrequencia;
        private readonly IMediator mediator;

        public ComandoFrequencia(IServicoFrequencia servicoFrequencia,
                                 IConsultasAula consultasAula,
                                 IMediator mediator)
        {
            this.servicoFrequencia = servicoFrequencia ?? throw new System.ArgumentNullException(nameof(servicoFrequencia));
            this.consultasAula = consultasAula ?? throw new System.ArgumentNullException(nameof(consultasAula));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));

        }

        //TODO REMOVER
        //public async Task Registrar(FrequenciaDto frequenciaDto)
        //{

        //    List<RegistroAusenciaAluno> registrosAusenciaAlunos = ObtemListaDeAusencias(frequenciaDto);
        //    await servicoFrequencia.Registrar(frequenciaDto.AulaId, registrosAusenciaAlunos);

        //    var alunos = frequenciaDto.ListaFrequencia.Select(a => a.CodigoAluno).ToList();
        //    if (alunos == null || !alunos.Any())
        //    {
        //        throw new NegocioException("A lista de alunos da turma e o componente curricular devem ser informados para calcular a frequência.");
        //    }

        //    var aula = await consultasAula.BuscarPorId(frequenciaDto.AulaId);

        //    var bimestre = await mediator.Send(new ObterBimestrePorTurmaCodigoQuery(aula.TurmaId, aula.DataAula));

        //    await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunos, aula.DataAula, aula.TurmaId, aula.DisciplinaId, bimestre));

        //    await mediator.Send(new ExcluirPendenciaAulaCommand(aula.Id, TipoPendencia.Frequencia));

        //}

        private static List<RegistroAusenciaAluno> ObtemListaDeAusencias(FrequenciaDto frequenciaDto)
        {
            var registrosAusenciaAlunos = new List<RegistroAusenciaAluno>();

            foreach (var frequencia in frequenciaDto.ListaFrequencia.Where(c => c.Aulas.Any(a => !a.Compareceu)))
            {
                foreach (var ausenciaNaAula in frequencia.Aulas.Where(c => !c.Compareceu))
                {
                    registrosAusenciaAlunos.Add(new RegistroAusenciaAluno(frequencia.CodigoAluno, ausenciaNaAula.NumeroAula));
                }
            }

            return registrosAusenciaAlunos;
        }
    }
}