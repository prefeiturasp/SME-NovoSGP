using System;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase : AbstractUseCase, IConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase
    {
        private readonly IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidadoConsulta;

        public ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresAlunoUseCase(IMediator mediator, IRepositorioConselhoClasseConsolidadoConsulta repositorioConselhoClasseConsolidadoConsulta) : base(mediator)
        {
            this.repositorioConselhoClasseConsolidadoConsulta = repositorioConselhoClasseConsolidadoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseConsolidadoConsulta));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemConsolidarTurmaConselhoClasseAlunoPorAlunoDto>();

            try
            {
                var alunoConsolidacoesPorAluno = await repositorioConselhoClasseConsolidadoConsulta.ObterConselhoClasseConsolidadoPorTurmaAlunoAsync(filtro.TurmaId, filtro.AlunoCodigo);

                foreach (var alunoNota in filtro.AlunoNotas)
                {
                    if (alunoConsolidacoesPorAluno == 0)
                    {
                        var conselhoClasseAlunoTurma = new ConselhoClasseConsolidadoTurmaAluno()
                        {
                            AlunoCodigo = filtro.AlunoCodigo,
                            CriadoEm = alunoNota.CriadoEm,
                            CriadoPor = alunoNota.CriadoPor,
                            CriadoRF = alunoNota.CriadoRf,
                            DataAtualizacao = DateTimeExtension.HorarioBrasilia(),
                            ParecerConclusivoId = filtro.ParecerConclusivo,
                            Status = 0,
                            TurmaId = filtro.TurmaId
                        };
                        alunoConsolidacoesPorAluno = await repositorioConselhoClasseConsolidadoConsulta.SalvarAsync(conselhoClasseAlunoTurma);
                    }

                    var migracaoConsolidacaoTurmaCommand = new ConsolidacaoTurmaConselhoClasseAlunoAnosAnterioresCommand()
                    {
                        ConsolidacaoId = alunoConsolidacoesPorAluno,
                        AlunoNota = alunoNota
                    };
                    await mediator.Send(migracaoConsolidacaoTurmaCommand);
                }
                
                return true;
            }
            catch (System.Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível executar a consolidacao turma conselho classe aluno anos anteriores por aluno.", LogNivel.Critico, LogContexto.ConselhoClasse, ex.Message));
                return false;
            }
        }
    }
}
