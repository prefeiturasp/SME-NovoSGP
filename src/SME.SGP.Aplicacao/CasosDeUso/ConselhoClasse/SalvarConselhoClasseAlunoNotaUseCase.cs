using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao;

public class SalvarConselhoClasseAlunoNotaUseCase : ISalvarConselhoClasseAlunoNotaUseCase
{
    private readonly IMediator mediator;

    public SalvarConselhoClasseAlunoNotaUseCase(IMediator mediator)
    {
        this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<ConselhoClasseNotaRetornoDto> Executar(SalvarConselhoClasseAlunoNotaDto salvarConselhoClasseAlunoNota)
    {
        var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(salvarConselhoClasseAlunoNota.CodigoTurma));
        
        if (turma == null) 
            throw new NegocioException("Turma não encontrada");

        var ehAnoAnterior = turma.AnoLetivo != DateTime.Now.Year;

        var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(salvarConselhoClasseAlunoNota.FechamentoTurmaId,
            salvarConselhoClasseAlunoNota.CodigoAluno, ehAnoAnterior));

        var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina();
        var periodoEscolar = new PeriodoEscolar();

        if (fechamentoTurma == null)
        {
            if (!ehAnoAnterior) 
                throw new NegocioException("Não existe fechamento de turma para o conselho de classe");

            var ue = repositorioUe.ObterPorId(turma.UeId);
            ue.AdicionarDre(repositorioDre.ObterPorId(ue.DreId));
            turma.AdicionarUe(ue);

            periodoEscolar = await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, salvarConselhoClasseAlunoNota.Bimestre));

            if (periodoEscolar == null && salvarConselhoClasseAlunoNota.Bimestre > 0) 
                throw new NegocioException("Período escolar não encontrado");

            fechamentoTurma = new FechamentoTurma()
            {
                TurmaId = turma.Id,
                Migrado = false,
                PeriodoEscolarId = periodoEscolar?.Id,
                Turma = turma,
                PeriodoEscolar = periodoEscolar
            };

            fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina()
            {
                DisciplinaId = salvarConselhoClasseAlunoNota.ConselhoClasseNotaDto.CodigoComponenteCurricular
            };
        }
        else
        {
            if (fechamentoTurma.PeriodoEscolarId != null)
                periodoEscolar = await mediator.Send(new ObterPeriodoEscolarePorIdQuery(fechamentoTurma.PeriodoEscolarId.Value));
        }

        await GravarFechamentoTurma(fechamentoTurma, fechamentoTurmaDisciplina, turma, periodoEscolar);

        return await GravarConselhoClasse(fechamentoTurma, conselhoClasseId, alunoCodigo, turma, conselhoClasseNotaDto, periodoEscolar?.Bimestre);
    }
    
        private async Task GravarFechamentoTurma(FechamentoTurma fechamentoTurma, FechamentoTurmaDisciplina fechamentoTurmaDisciplina, Turma turma, PeriodoEscolar periodoEscolar)
        {
            if (fechamentoTurma.PeriodoEscolarId.HasValue)
            {
                // Fechamento Bimestral
                if (!await consultasPeriodoFechamento.TurmaEmPeriodoDeFechamento(fechamentoTurma.Turma, DateTime.Today, fechamentoTurma.PeriodoEscolar.Bimestre))
                    throw new NegocioException($"Turma {fechamentoTurma.Turma.Nome} não esta em período de fechamento para o {fechamentoTurma.PeriodoEscolar.Bimestre}º Bimestre!");
            }
            else
            {
                // Fechamento Final
                if (fechamentoTurma.Turma.AnoLetivo != 2020 && !fechamentoTurma.Turma.Historica)
                {
                    var validacaoConselhoFinal = await consultasConselhoClasse.ValidaConselhoClasseUltimoBimestre(fechamentoTurma.Turma);
                    if (!validacaoConselhoFinal.Item2 && fechamentoTurma.Turma.AnoLetivo == DateTime.Today.Year)
                        throw new NegocioException($"Para salvar a nota final você precisa registrar o conselho de classe do {validacaoConselhoFinal.Item1}º bimestre");
                }
            }
            var consolidacaoTurma = new ConsolidacaoTurmaDto(turma.Id, fechamentoTurma.PeriodoEscolarId != null ? periodoEscolar.Bimestre : 0);
            var mensagemParaPublicar = JsonConvert.SerializeObject(consolidacaoTurma);

            try
            {
                unitOfWork.IniciarTransacao();
                if (fechamentoTurmaDisciplina.DisciplinaId > 0)
                {
                    await repositorioFechamentoTurma.SalvarAsync(fechamentoTurma);
                    fechamentoTurmaDisciplina.FechamentoTurmaId = fechamentoTurma.Id;
                    await repositorioFechamentoTurmaDisciplina.SalvarAsync(fechamentoTurmaDisciplina);
                }
                unitOfWork.PersistirTransacao();

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoSync, mensagemParaPublicar, Guid.NewGuid(), null));
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw e;
            }
        }    
}