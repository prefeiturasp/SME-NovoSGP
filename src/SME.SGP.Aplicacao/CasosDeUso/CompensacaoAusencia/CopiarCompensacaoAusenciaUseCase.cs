using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class CopiarCompensacaoAusenciaUseCase : AbstractUseCase, ICopiarCompensacaoAusenciaUseCase
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;
        private readonly IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia;
        private readonly ISalvarCompensasaoAusenciaUseCase salvarCompensasaoAusenciaUseCase;

        public CopiarCompensacaoAusenciaUseCase(IMediator mediator, IRepositorioCompensacaoAusencia compensacaoAusencia,
            IRepositorioCompensacaoAusenciaDisciplinaRegencia compensacaoAusenciaDisciplinaRegencia            ,
            ISalvarCompensasaoAusenciaUseCase salvarCompensasaoAusencia
            ) : base(mediator)
        {
            repositorioCompensacaoAusencia = compensacaoAusencia ?? throw new ArgumentNullException(nameof(compensacaoAusencia));
            repositorioCompensacaoAusenciaDisciplinaRegencia = compensacaoAusenciaDisciplinaRegencia ?? throw new ArgumentNullException(nameof(compensacaoAusenciaDisciplinaRegencia));
            salvarCompensasaoAusenciaUseCase = salvarCompensasaoAusencia ?? throw new ArgumentNullException(nameof(salvarCompensasaoAusencia));
        }

        public async Task<string> Executar(CompensacaoAusenciaCopiaDto param)
        {
            var compensacaoOrigem = repositorioCompensacaoAusencia.ObterPorId(param.CompensacaoOrigemId);
            if (compensacaoOrigem == null)
                throw new NegocioException("Compensação de origem não localizada com o identificador informado.");

            var turmasCopiadas = new StringBuilder("");
            var turmasComErro = new StringBuilder("");
            foreach (var turmaId in param.TurmasIds)
            {
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaId));
                var compensacaoDto = new CompensacaoAusenciaDto()
                {
                    TurmaId = turmaId,
                    Bimestre = param.Bimestre,
                    DisciplinaId = compensacaoOrigem.DisciplinaId,
                    Atividade = compensacaoOrigem.Nome,
                    Descricao = compensacaoOrigem.Descricao,
                    DisciplinasRegenciaIds = new List<string>(),
                    Alunos = new List<CompensacaoAusenciaAlunoDto>()
                };

                var disciplinasRegencia =
                    await repositorioCompensacaoAusenciaDisciplinaRegencia.ObterPorCompensacao(compensacaoOrigem.Id);
                if (disciplinasRegencia != null && disciplinasRegencia.Any())
                    compensacaoDto.DisciplinasRegenciaIds = disciplinasRegencia.Select(s => s.DisciplinaId);

                try
                {
                    await salvarCompensasaoAusenciaUseCase.Executar(0, compensacaoDto);
                    turmasCopiadas.Append(turmasCopiadas.ToString().Length > 0 ? ", " + turma.Nome : turma.Nome);
                }
                catch (Exception e)
                {
                    turmasComErro.AppendLine($"A cópia para a turma {turma.Nome} não foi realizada: {e.Message}\n");
                }
            }

            var respTurmasCopiadas = turmasCopiadas.ToString();
            var textoTurmas = respTurmasCopiadas.Contains(",") ? "as turmas" : "a turma";
            var respostaSucesso = respTurmasCopiadas.Length > 0
                ? $"A cópia para {textoTurmas} {respTurmasCopiadas} foi realizada com sucesso"
                : "";
            var respTurmasComErro = turmasComErro.ToString();
            if (respTurmasComErro.Length > 0)
                throw new NegocioException($"{respTurmasComErro} {respostaSucesso}");

            return respostaSucesso;
        }
    }
}