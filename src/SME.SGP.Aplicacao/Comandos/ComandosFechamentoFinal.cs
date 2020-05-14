using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoFinal : IComandosFechamentoFinal
    {
        private readonly IRepositorioConceito repositorioConceito;
        private readonly IRepositorioFechamentoNota repositorioFechamentoNota;
        private readonly IRepositorioFechamentoAluno repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoTurma repositorioFechamentoTurma;
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IServicoFechamentoFinal servicoFechamentoFinal;

        public ComandosFechamentoFinal(
            IRepositorioConceito repositorioConceito,
            IServicoFechamentoFinal servicoFechamentoFinal,
            IRepositorioTurma repositorioTurma,
            IRepositorioFechamentoNota repositorioFechamentoNota,
            IRepositorioFechamentoAluno repositorioFechamentoAluno,
            IRepositorioFechamentoTurma repositorioFechamentoTurma,
            IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioConceito = repositorioConceito ?? throw new System.ArgumentNullException(nameof(repositorioConceito));
            this.servicoFechamentoFinal = servicoFechamentoFinal ?? throw new System.ArgumentNullException(nameof(servicoFechamentoFinal));
            this.repositorioTurma = repositorioTurma ?? throw new System.ArgumentNullException(nameof(repositorioTurma));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.repositorioFechamentoNota = repositorioFechamentoNota ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoNota));
        }

        public async Task<string[]> SalvarAsync(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var turma = ObterTurma(fechamentoFinalSalvarDto.TurmaCodigo);
            await servicoFechamentoFinal.VerificaPersistenciaGeral(turma);

            var fechamentoTurmaDisciplina = await TransformarDtoSalvarEmEntidade(fechamentoFinalSalvarDto, turma);
            
            var mensagensDeErro = await servicoFechamentoFinal.SalvarAsync(fechamentoTurmaDisciplina);

            return mensagensDeErro.ToArray();
        }

        private Turma ObterTurma(string turmaCodigo)
        {
            var turma = repositorioTurma.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
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

                foreach(var fechamentoItemDto in agrupamentoAluno)
                {
                    var fechamentoNota = fechamentoAluno.FechamentoNotas.FirstOrDefault(c => c.DisciplinaId == fechamentoItemDto.ComponenteCurricularCodigo);

                    MapearParaEntidade(fechamentoNota, fechamentoItemDto, fechamentoAluno);
                }

                fechamentoTurmaDisciplina.FechamentoAlunos.Add(fechamentoAluno);
            }
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