using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConciliacaoFrequenciaTurmaCommandHandler : IRequestHandler<IncluirFilaConciliacaoFrequenciaTurmaCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaConciliacaoFrequenciaTurmaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaConciliacaoFrequenciaTurmaCommand request, CancellationToken cancellationToken)
        {
            var alunos = await ObterAlunosTurma(request.TurmaCodigo, (request.DataInicio, request.DataFim));

            if (!alunos?.Any() != true)
            {
                foreach(var componenteCurricularId in await ObterComponentesCurriculares(request.TurmaCodigo, request.ComponenteCurricularId))
                {
                    var alunosCodigo = alunos.Select(a => a.CodigoAluno);

                    var comando = new CalcularFrequenciaPorTurmaCommand(alunosCodigo, request.DataFim, request.TurmaCodigo, componenteCurricularId);
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaConciliacaoCalculoFrequenciaPorTurmaComponente, comando, Guid.NewGuid(), null), cancellationToken);
                }
            }

            return true;
        }

        private async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosTurma(string turmaCodigo, (DateTime DataInicio, DateTime DataFim) periodo)
        {
            try
            {
                var tempoCacheEmMinutos = 5;
                return await mediator.Send(new ObterAlunosDentroPeriodoQuery(turmaCodigo, periodo, tempoArmazenamentoCache: tempoCacheEmMinutos));
            }
            catch (NegocioException nex)
            {
                await RegistraExcecao(nex, LogNivel.Negocio);
            }
            catch (Exception ex)
            {
                await RegistraExcecao(ex);
            }

            return Enumerable.Empty<AlunoPorTurmaResposta>();
        }

        private async Task<IEnumerable<string>> ObterComponentesCurriculares(string turmaCodigo, string componenteCurricularId)
        {
            if (!string.IsNullOrEmpty(componenteCurricularId))
                return new List<string>() { componenteCurricularId };

            // Listar componentes da turma
            try
            {
                var componentesCurriculares = await mediator.Send(new ObterComponentesCurricularesEOLPorTurmasCodigoQuery(new string[] { turmaCodigo }));
                if (componentesCurriculares?.Any() != true)
                    throw new NegocioException("Não foi possível obter os componentes curriculares da turma para conciliação de frequência");

                return componentesCurriculares.Select(a => a.Codigo.ToString());
            }
            catch (NegocioException ex)
            {
                await RegistraExcecao(ex, LogNivel.Negocio);
            }
            catch (Exception ex)
            {
                await RegistraExcecao(ex);
            }

            return Enumerable.Empty<string>();
        }

        public Task RegistraExcecao(Exception ex, LogNivel nivel = LogNivel.Critico)
            => mediator.Send(new SalvarLogViaRabbitCommand(ex.Message,
                                                           LogNivel.Critico,
                                                           LogContexto.Frequencia,
                                                           rastreamento: ex.StackTrace,
                                                           excecaoInterna: ex.InnerException?.ToString()));
    };
}
