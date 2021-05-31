using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CalculoFrequenciaTurmaDisciplinaUseCase : AbstractUseCase, ICalculoFrequenciaTurmaDisciplinaUseCase
    {
        public CalculoFrequenciaTurmaDisciplinaUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var comando = mensagemRabbit.ObterObjetoMensagem<CalcularFrequenciaPorTurmaOldCommand>();
            if (comando != null)
            {
                await mediator.Send(comando);
                return true;
            }
            return false;
        }

        public async Task IncluirCalculoFila(CalcularFrequenciaDto calcularFrequenciaDto)
        {
            var alunosConsiderados = await ValidarDadosCalculo(calcularFrequenciaDto);

            await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunosConsiderados
                .Select(a => a.CodigoAluno), calcularFrequenciaDto.DataReferencia, calcularFrequenciaDto.CodigoTurma, calcularFrequenciaDto.CodigoComponenteCurricular));
        }

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ValidarDadosCalculo(CalcularFrequenciaDto calcularFrequenciaDto)
        {
            if (calcularFrequenciaDto == null)
                throw new ArgumentNullException(nameof(calcularFrequenciaDto));

            if (string.IsNullOrWhiteSpace(calcularFrequenciaDto.CodigoTurma))
                throw new NegocioException("Informe o código da turma.");

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(calcularFrequenciaDto.CodigoTurma));

            if (turma == null)
                throw new NegocioException($"A turma com o código {calcularFrequenciaDto.CodigoTurma} não foi localizada.");

            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(turma.CodigoTurma));

            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foram localizados alunos para a turma informada.");

            if (calcularFrequenciaDto.CodigosAlunos != null && calcularFrequenciaDto.CodigosAlunos.Any() && !alunos.Select(a => a.CodigoAluno).Intersect(calcularFrequenciaDto.CodigosAlunos).Any())
                throw new NegocioException("Nenhum dos códigos de aluno informados pertence aos alunos da turma.");

            if (calcularFrequenciaDto.CodigosAlunos?.Length > 0)
            {
                alunos = from a in alunos
                         where calcularFrequenciaDto.CodigosAlunos.Contains(a.CodigoAluno)
                         select a;
            }

            if (calcularFrequenciaDto.DataReferencia == DateTime.MinValue)
                throw new NegocioException($"A data de referência deve ser informada.");

            var componenteCurricular = await mediator.Send(new ObterComponentesCurricularesQuery());

            if (string.IsNullOrWhiteSpace(calcularFrequenciaDto.CodigoComponenteCurricular) || !componenteCurricular.Any(cc => cc.Codigo.Equals(calcularFrequenciaDto.CodigoComponenteCurricular)))
                throw new NegocioException("O código de componente curricular informado é inválido.");

            if (!new int[] { 1, 2, 3, 4 }.Contains(calcularFrequenciaDto.Bimestre))
                throw new NegocioException("Informe um bimestre válido.");

            return alunos;
        }
    }
}
