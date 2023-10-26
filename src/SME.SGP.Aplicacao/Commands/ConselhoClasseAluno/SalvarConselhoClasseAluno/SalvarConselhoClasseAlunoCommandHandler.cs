using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using Npgsql;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseAlunoCommandHandler : IRequestHandler<SalvarConselhoClasseAlunoCommand, long>
    {
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        private readonly IMediator mediator;
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        private readonly IConsultasPeriodoFechamento consultasPeriodoFechamento;
        private readonly IConsultasConselhoClasse consultasConselhoClasse;

        public SalvarConselhoClasseAlunoCommandHandler(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,IMediator mediator,IRepositorioConselhoClasse repositorioConselhoClasse,
            IConsultasPeriodoFechamento consultasPeriodoFechamento,IConsultasConselhoClasse consultasConselhoClasse)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasPeriodoFechamento = consultasPeriodoFechamento ?? throw new ArgumentNullException(nameof(consultasPeriodoFechamento));
            this.consultasConselhoClasse = consultasConselhoClasse ?? throw new ArgumentNullException(nameof(consultasConselhoClasse));
        }

        public async Task<long> Handle(SalvarConselhoClasseAlunoCommand request,CancellationToken cancellationToken)
        {
            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(request.ConselhoClasseAluno.ConselhoClasse.FechamentoTurmaId, request.ConselhoClasseAluno.AlunoCodigo), cancellationToken);

            // Se não existir conselho de classe para o fechamento gera
            if (request.ConselhoClasseAluno.ConselhoClasse.Id == 0)
            {
                await GerarConselhoClasse(request.ConselhoClasseAluno.ConselhoClasse, fechamentoTurma);
                request.ConselhoClasseAluno.ConselhoClasseId = request.ConselhoClasseAluno.ConselhoClasse.Id;
            }
            else
                await repositorioConselhoClasse.SalvarAsync(request.ConselhoClasseAluno.ConselhoClasse);

            long conselhoClasseAlunoId;
            try
            {
                conselhoClasseAlunoId = await repositorioConselhoClasseAluno.SalvarAsync(request.ConselhoClasseAluno);
            }
            catch (PostgresException ex)
            {
                await LogarExcecao(ex);
                throw new Exception("Erro ao salvar o conselho de classe do aluno.");
            }

            await mediator.Send(new InserirTurmasComplementaresCommand(fechamentoTurma.TurmaId, conselhoClasseAlunoId, request.ConselhoClasseAluno.AlunoCodigo), cancellationToken);

            return conselhoClasseAlunoId;
        }

        private Task LogarExcecao(PostgresException ex)
        {
            var mensagem = $"ConselhoClasseAluno: Coluna[{ex.ColumnName}] Restrição[{ex.ConstraintName}] Erro:{ex.Message}";
            return mediator.Send(new SalvarLogViaRabbitCommand(mensagem,
                                                               LogNivel.Critico,
                                                               LogContexto.Fechamento,
                                                               ex.Detail,
                                                               rastreamento: ex.StackTrace,
                                                               excecaoInterna: ex.InnerException?.Message));
        }

        public async Task<AuditoriaDto> GerarConselhoClasse(ConselhoClasse conselhoClasse, FechamentoTurma fechamentoTurma)
        {
            var conselhoClasseExistente = await mediator.Send(new ObterConselhoClassePorTurmaEPeriodoQuery(fechamentoTurma.TurmaId, fechamentoTurma.PeriodoEscolarId));

            if (conselhoClasseExistente.NaoEhNulo())
               throw new NegocioException(string.Format(MensagemNegocioConselhoClasse.JA_EXISTE_CONSELHO_CLASSE_GERADO_PARA_TURMA, fechamentoTurma.Turma.Nome));

            if (fechamentoTurma.PeriodoEscolarId.HasValue)
            {
                // Fechamento Bimestral
                if (!await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(fechamentoTurma.Turma, DateTime.Today, fechamentoTurma.PeriodoEscolar.Bimestre))
                    throw new NegocioException(string.Format(MensagemNegocioFechamentoTurma.TURMA_NAO_ESTA_EM_PERIODO_FECHAMENTO_PARA_BIMESTRE, fechamentoTurma.Turma.Nome, fechamentoTurma.PeriodoEscolar.Bimestre));
            }
            else
            {
                // Fechamento Final
                if (fechamentoTurma.Turma.AnoLetivo >= DateTime.Now.Year)
                {
                    var validacaoConselhoFinal = await mediator.Send(new ObterUltimoBimestreTurmaQuery(fechamentoTurma.Turma));
                    if (!validacaoConselhoFinal.possuiConselho)
                        throw new NegocioException(string.Format(MensagemNegocioConselhoClasse.NAO_PERMITE_ACESSO_ABA_SEM_REGISTRAR_CONSELHO_BIMESTRE, validacaoConselhoFinal.bimestre));
                }
            }

            await repositorioConselhoClasse.SalvarAsync(conselhoClasse);

            return (AuditoriaDto)conselhoClasse;
        }
    }
}
