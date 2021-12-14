using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciasDetalhadasPorDataUseCase : AbstractUseCase, IObterFrequenciasDetalhadasPorDataUseCase
    {
        public ObterFrequenciasDetalhadasPorDataUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<FrequenciaDetalhadaDto>> Executar(FiltroFrequenciaDetalhadaDto filtro)
        {
            var frequenciaRetorno = new List<FrequenciaDetalhadaDto>();

            var frequenciasAluno = await mediator.Send(new ObterFrequenciasDetalhadasPorDataQuery(filtro.CodigoAluno, filtro.DataInicio, filtro.DataFim));

            foreach (var frequenciaAluno in frequenciasAluno)
            {
                var frequenciaDetalhadaALuno = new FrequenciaDetalhadaDto
                {
                    DataAula = frequenciaAluno.DataAula,
                    NumeroAula = frequenciaAluno.NumeroAula
                };

                var aula = await mediator.Send(new ObterAulaPorIdQuery(frequenciaAluno.AulaId));
                if (aula == null)
                    throw new NegocioException("Aula não encontrada.");

                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
                if (turma == null)
                    throw new NegocioException("Não foi encontrada uma turma com o id informado. Verifique se você possui abrangência para essa turma.");

                var periodoEscolar = await mediator.Send(new ObterPeriodosEscolaresPorTipoCalendarioIdEDataQuery(frequenciaAluno.TipoCalendario, frequenciaAluno.DataAula));
                if (periodoEscolar == null)
                    throw new NegocioException("Ocorreu um erro, esta aula está fora do período escolar.");

                var parametroPercentualCritico = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PercentualFrequenciaCritico, periodoEscolar.PeriodoInicio.Year));
                if (parametroPercentualCritico == null)
                    throw new NegocioException("Parâmetro de percentual de frequência em nível crítico não encontrado contate a SME.");
                var percentualCritico = int.Parse(parametroPercentualCritico.Valor);

                var parametroPercentualAlerta = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.PercentualFrequenciaAlerta, periodoEscolar.PeriodoInicio.Year));
                if (parametroPercentualAlerta == null)
                    throw new NegocioException("Parâmetro de percentual de frequência em alerta não encontrado contate a SME.");
                var percentualAlerta = int.Parse(parametroPercentualAlerta.Valor);

                var frequenciaTurmaRegistrada = await mediator.Send(new ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolarQuery(turma, long.Parse(aula.DisciplinaId), periodoEscolar.Id));

                var frequenciaAlunoRegistrada = frequenciaTurmaRegistrada.FirstOrDefault(a => a.CodigoAluno == filtro.CodigoAluno && a.DisciplinaId == aula.DisciplinaId && a.PeriodoEscolarId == periodoEscolar.Id);

                var turmaPossuiFrequenciaRegistrada = await mediator.Send(new ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery(turma.CodigoTurma, aula.DisciplinaId, periodoEscolar.Id));

                frequenciaDetalhadaALuno.IndicativoFrequencia = ObterIndicativoFrequencia(frequenciaAlunoRegistrada, percentualAlerta, percentualCritico, turmaPossuiFrequenciaRegistrada, turma);

                frequenciaRetorno.Add(frequenciaDetalhadaALuno);
            }

            return frequenciaRetorno;
        }

        private IndicativoFrequenciaDto ObterIndicativoFrequencia(FrequenciaAluno frequenciaAluno, int percentualAlerta, int percentualCritico, bool turmaPossuiFrequenciaRegistrada, Turma turma)
        {
            var percentualFrequencia = "0";
            if (turma.AnoLetivo.Equals(2020))
                percentualFrequencia = frequenciaAluno == null || frequenciaAluno.TotalAusencias == 0
                    ?
                    "100"
                    :
                    frequenciaAluno?.PercentualFrequencia.ToString();
            else
                percentualFrequencia = frequenciaAluno == null && turmaPossuiFrequenciaRegistrada
                ?
                "100"
                :
                frequenciaAluno != null
                ?
                frequenciaAluno?.PercentualFrequencia.ToString()
                :
                "";

            var percentualFrequenciaCauculada = (int)Math.Round(Convert.ToDouble(percentualFrequencia));


            var percentualFrequenciaLabel = percentualFrequenciaCauculada < 0 ? null : percentualFrequencia.ToString();

            // Critico
            if (percentualFrequenciaCauculada <= percentualCritico)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Critico, Percentual = percentualFrequenciaLabel };

            // Alerta
            if (percentualFrequenciaCauculada <= percentualAlerta)
                return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Alerta, Percentual = percentualFrequenciaLabel };

            return new IndicativoFrequenciaDto() { Tipo = TipoIndicativoFrequencia.Info, Percentual = percentualFrequenciaLabel };
        }
    }
}
