using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaPorTurmaUseCase : AbstractUseCase, IConsolidarFrequenciaPorTurmaUseCase
    {
        public ConsolidarFrequenciaPorTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = new FiltroConsolidacaoFrequenciaTurma();
            try
            {
                filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaTurma>();
                var turma = !string.IsNullOrWhiteSpace(filtro.TurmaCodigo) ? 
                    await mediator.Send(new ObterTurmaPorCodigoQuery(filtro.TurmaCodigo)) : 
                    await mediator.Send(new ObterTurmaPorIdQuery(filtro.TurmaId));

                if (turma != null)
                {
                    var alunos = await mediator
                        .Send(new ObterAlunosPorTurmaQuery(turma.CodigoTurma, turma.AnoLetivo.Equals(DateTime.Today.Year))); 
                    
                    if (filtro.PercentualFrequenciaMinimo == 0)
                    {
                        var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(turma.ModalidadeCodigo == Dominio.Modalidade.EducacaoInfantil ? Dominio.TipoParametroSistema.PercentualFrequenciaMinimaInfantil : Dominio.TipoParametroSistema.PercentualFrequenciaCritico, turma.AnoLetivo));
                        filtro.PercentualFrequenciaMinimo = double.Parse(parametro.Valor);                            
                    }
                    
                    await ConsolidarFrequenciaAlunos(turma.Id, turma.CodigoTurma, filtro.PercentualFrequenciaMinimo, turma.AnoLetivo.Equals(DateTime.Today.Year) ? alunos : alunos.Where(a => !a.Inativo));
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Consolidar Frequencia Por Turma UseCase", LogNivel.Critico, LogContexto.Frequencia, ex.Message));
                throw;
            }            
        }

        private async Task ConsolidarFrequenciaAlunos(long turmaId, string turmaCodigo, double percentualFrequenciaMinimo, IEnumerable<AlunoPorTurmaResposta> alunos)
        {
            var frequenciaTurma = await mediator.Send(new ObterFrequenciaGeralPorTurmaQuery(turmaCodigo));
            var codigosAlunos = alunos.Select(a => a.CodigoAluno);
            var frequenciasConsideradas = frequenciaTurma?.Where(f => codigosAlunos.Contains(f.AlunoCodigo));
            var quantidadeReprovados = frequenciasConsideradas?.Where(c => c.PercentualFrequencia < percentualFrequenciaMinimo).Count() ?? 0;
            var quantidadeAprovados = frequenciasConsideradas?.Where(c => c.PercentualFrequencia >= percentualFrequenciaMinimo).Count() ?? 0;

            await RegistraConsolidacaoFrequenciaTurma(turmaId, quantidadeAprovados, quantidadeReprovados);
        }

        private async Task RegistraConsolidacaoFrequenciaTurma(long turmaId, int quantidadeAprovados, int quantidadeReprovados)
        {
            await mediator.Send(new RegistraConsolidacaoFrequenciaTurmaCommand(turmaId, quantidadeAprovados, quantidadeReprovados));
        }
    }
}
