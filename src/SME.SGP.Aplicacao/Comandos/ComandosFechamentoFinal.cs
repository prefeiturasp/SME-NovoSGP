﻿using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoFinal : IComandosFechamentoFinal
    {
        private readonly IRepositorioFechamentoAluno repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoFechamentoFinal servicoFechamentoFinal;
        private readonly IMediator mediator;

        public ComandosFechamentoFinal(
            IServicoFechamentoFinal servicoFechamentoFinal,
            IRepositorioTurma repositorioTurma,
            IRepositorioFechamentoAluno repositorioFechamentoAluno,
            IRepositorioFechamentoTurma repositorioFechamentoTurma,
            IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina,
            IMediator mediator)
        {
            this.servicoFechamentoFinal = servicoFechamentoFinal ?? throw new System.ArgumentNullException(nameof(servicoFechamentoFinal));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<string[]> SalvarAsync(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var turma = await ObterTurma(fechamentoFinalSalvarDto.TurmaCodigo);

            var fechamentoTurmaDisciplina = await TransformarDtoSalvarEmEntidade(fechamentoFinalSalvarDto, turma);

            var mensagensDeErro = await servicoFechamentoFinal.SalvarAsync(fechamentoTurmaDisciplina, turma);

            return mensagensDeErro.ToArray();
        }

        private async Task<Turma> ObterTurma(string turmaCodigo)
        {
            var turma = await repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma.");
            return turma;
        }

        private async Task<FechamentoTurmaDisciplina> TransformarDtoSalvarEmEntidade(FechamentoFinalSalvarDto fechamentoFinalSalvarDto, Turma turma)
        {
            var disciplinaId = fechamentoFinalSalvarDto.EhRegencia ? long.Parse(fechamentoFinalSalvarDto.DisciplinaId) : fechamentoFinalSalvarDto.Itens.First().ComponenteCurricularCodigo;

            FechamentoTurmaDisciplina fechamentoTurmaDisciplina = null;
            var fechamentoFinalTurma = await repositorioFechamentoTurma.ObterPorTurmaPeriodo(turma.Id);
            if (fechamentoFinalTurma == null)
                fechamentoFinalTurma = new FechamentoTurma(0, turma.Id);
            else
                fechamentoTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(fechamentoFinalSalvarDto.TurmaCodigo, disciplinaId);

            if (fechamentoTurmaDisciplina == null)
                fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina() { DisciplinaId = disciplinaId, Situacao = SituacaoFechamento.ProcessadoComSucesso };

            fechamentoTurmaDisciplina.FechamentoTurma = fechamentoFinalTurma;

            foreach (var agrupamentoAluno in fechamentoFinalSalvarDto.Itens.GroupBy(a => a.AlunoRf))
            {
                var fechamentoAluno = await repositorioFechamentoAluno.ObterFechamentoAlunoENotas(fechamentoTurmaDisciplina.Id, agrupamentoAluno.Key);
                if (fechamentoAluno == null)
                    fechamentoAluno = new FechamentoAluno() { AlunoCodigo = agrupamentoAluno.Key };

                foreach (var fechamentoItemDto in agrupamentoAluno)
                {
                    var fechamentoNota = fechamentoAluno.FechamentoNotas.FirstOrDefault(c => c.DisciplinaId == fechamentoItemDto.ComponenteCurricularCodigo);

                    if (fechamentoNota != null)
                    {
                        if (fechamentoItemDto.Nota.HasValue)
                        {
                            if (fechamentoNota.Nota.Value != fechamentoItemDto.Nota.Value)
                                await mediator.Send(new SalvarHistoricoNotaFechamentoCommand(fechamentoNota.Nota.Value, fechamentoItemDto.Nota.Value, fechamentoNota.Id));
                        }
                        else
                        if (fechamentoNota.ConceitoId != fechamentoItemDto.ConceitoId)
                            await mediator.Send(new SalvarHistoricoConceitoFechamentoCommand(fechamentoNota.ConceitoId, fechamentoItemDto.ConceitoId, fechamentoNota.Id));
                    }

                    MapearParaEntidade(fechamentoNota, fechamentoItemDto, fechamentoAluno);
                }

                fechamentoTurmaDisciplina.FechamentoAlunos.Add(fechamentoAluno);
            }

            var consolidacaoTurma = new ConsolidacaoTurmaDto(turma.Id, 0);
            var mensagemParaPublicar = JsonConvert.SerializeObject(consolidacaoTurma);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaFechamentoSync, mensagemParaPublicar, Guid.NewGuid(), null));

            return fechamentoTurmaDisciplina;
        }

        private void MapearParaEntidade(FechamentoNota fechamentoNota, FechamentoFinalSalvarItemDto fechamentoItemDto, FechamentoAluno fechamentoAluno)
        {
            // Verifica se tem nota atribuida para a disciplina
            if (fechamentoNota == null)
            {
                fechamentoNota = new FechamentoNota()
                {
                    DisciplinaId = fechamentoItemDto.ComponenteCurricularCodigo,
                    Nota = fechamentoItemDto.Nota,
                    ConceitoId = fechamentoItemDto.ConceitoId,
                    // TODO implementar sintese para fechamento final (não tem o atributo no DTO)
                    //SinteseId = fechamentoItemDto.sin
                };
                fechamentoAluno.FechamentoNotas.Add(fechamentoNota);
            }
            else
            {
                fechamentoNota.Nota = fechamentoItemDto.Nota;
                fechamentoNota.ConceitoId = fechamentoItemDto.ConceitoId;
                // TODO implementar sintese para fechamento final (não tem o atributo no DTO)
                //SinteseId = fechamentoItemDto.sin
            }
        }
    }
}