using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaAusenciaRegistroIndividualTurmaCommandHandler : IRequestHandler<GerarPendenciaAusenciaRegistroIndividualTurmaCommand, RetornoBaseDto>
    {
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IRepositorioPendenciaUsuario repositorioPendenciaUsuario;
        private readonly IRepositorioPendenciaRegistroIndividual repositorioPendenciaRegistroIndividual;
        private readonly IRepositorioPendenciaRegistroIndividualAluno repositorioPendenciaRegistroIndividualAluno;
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        private const string DescricaoBase = "As crianças abaixo estão sem registro individual a mais de 15 dias:";

        public GerarPendenciaAusenciaRegistroIndividualTurmaCommandHandler(IRepositorioPendencia repositorioPendencia, IRepositorioPendenciaUsuario repositorioPendenciaUsuario,
            IRepositorioPendenciaRegistroIndividual repositorioPendenciaRegistroIndividual, IRepositorioPendenciaRegistroIndividualAluno repositorioPendenciaRegistroIndividualAluno, 
            IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.repositorioPendenciaUsuario = repositorioPendenciaUsuario ?? throw new ArgumentNullException(nameof(repositorioPendenciaUsuario));
            this.repositorioPendenciaRegistroIndividual = repositorioPendenciaRegistroIndividual ?? throw new ArgumentNullException(nameof(repositorioPendenciaRegistroIndividual));
            this.repositorioPendenciaRegistroIndividualAluno = repositorioPendenciaRegistroIndividualAluno ?? throw new ArgumentNullException(nameof(repositorioPendenciaRegistroIndividualAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<RetornoBaseDto> Handle(GerarPendenciaAusenciaRegistroIndividualTurmaCommand request, CancellationToken cancellationToken)
        {
            var retorno = new RetornoBaseDto();

            var diasDeAusencia = await ObterDiasDeAusenciaParaPendenciaRegistroIndividualAsync();

            var periodos = await mediator.Send(new ObterPeriodosEscolaresPorAnoEModalidadeTurmaQuery(request.Turma.ModalidadeCodigo, request.Turma.AnoLetivo, request.Turma.Semestre));
            var diferencaInicioAnoLetivo = DateTime.Today.Subtract(periodos.Single(p => p.Bimestre.Equals(1)).PeriodoInicio);
            if (diferencaInicioAnoLetivo.Days < diasDeAusencia)
                return retorno;

            var query = new ListarAlunosTurmaComAusenciaRegistroIndividualPorDiasQuery(request.Turma.Id, request.Turma.CodigoTurma, diasDeAusencia);
            var alunosTurmaComAusenciaRegistroIndividualPorDias = await mediator.Send(query);
            if (!alunosTurmaComAusenciaRegistroIndividualPorDias?.Any() ?? true)
                return retorno;

            var pendenciaRegistroIndividual = await repositorioPendenciaRegistroIndividual.ObterPendenciaRegistroIndividualPorTurmaESituacao(request.Turma.Id, SituacaoPendencia.Pendente);
            unitOfWork.IniciarTransacao();
            try
            {
                if (pendenciaRegistroIndividual is null)
                    await CriarNovaPendenciaAusenciaRegistroIndividualAsync(request.Turma, alunosTurmaComAusenciaRegistroIndividualPorDias);
                else
                    await AlterarPendenciaAusenciaRegistroIndividualAsync(pendenciaRegistroIndividual, alunosTurmaComAusenciaRegistroIndividualPorDias);

                unitOfWork.PersistirTransacao();
            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                retorno.Mensagens.Add(ex?.InnerException.Message ?? ex.Message);
            }

            return retorno;
        }

        private async Task<int> ObterDiasDeAusenciaParaPendenciaRegistroIndividualAsync()
        {
            var parametroDiasSemRegistroIndividual = await mediator.Send(new ObterParametroSistemaPorTipoQuery(TipoParametroSistema.PendenciaPorAusenciaDeRegistroIndividual));
            if (string.IsNullOrEmpty(parametroDiasSemRegistroIndividual))
                throw new NegocioException($"Não foi possível obter o parâmetro {TipoParametroSistema.PendenciaPorAusenciaDeRegistroIndividual.Name()} ");

            return int.Parse(parametroDiasSemRegistroIndividual);
        }

        private async Task CriarNovaPendenciaAusenciaRegistroIndividualAsync(Turma turma, IEnumerable<AlunoPorTurmaResposta> alunosTurmaComAusenciaRegistroIndividualPorDias)
        {
            var listaProfessoresDaTurma = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(turma.CodigoTurma));
            var professoresDaTurma = listaProfessoresDaTurma?.Select(x => x.ProfessorRf);
            if (!professoresDaTurma?.Any() ?? true)
                throw new NegocioException($"Não foram encontrados professores para a turma {turma.CodigoTurma}.");

            var titulo = DefinirTituloDaPendenciaPorAusenciaDeRegistroIndividual(turma);
            var pendencia = new Pendencia(TipoPendencia.AusenciaDeRegistroIndividual, titulo, DescricaoBase, turma.Id);
            pendencia.Id = await repositorioPendencia.SalvarAsync(pendencia);


            var professoresRfsParaBusca = new List<string>();
            foreach (var professores in professoresDaTurma)
            {
                var listaProfessorComVirgulas = professores.Split(',');
                professoresRfsParaBusca.AddRange(listaProfessorComVirgulas.Select( a => a.Trim()));
            }


            await AdicionarUsuariosDaPendenciaAsync(pendencia, turma, professoresRfsParaBusca);
            await AdicionarPendenciaRegistroIndividualAsync(pendencia, turma, alunosTurmaComAusenciaRegistroIndividualPorDias);
        }

        private async Task AdicionarPendenciaRegistroIndividualAsync(Pendencia pendencia, Turma turma, IEnumerable<AlunoPorTurmaResposta> alunosTurmaComAusenciaRegistroIndividualPorDias)
        {
            var pendenciaRegistroIndividual = new PendenciaRegistroIndividual(pendencia, turma);
            pendenciaRegistroIndividual.Id = await repositorioPendenciaRegistroIndividual.SalvarAsync(pendenciaRegistroIndividual);

            var codigosDosAlunos = alunosTurmaComAusenciaRegistroIndividualPorDias.Select(x => long.Parse(x.CodigoAluno));
            pendenciaRegistroIndividual.AdicionarAlunos(codigosDosAlunos);

            foreach (var pendenciaRegistroIndividualAluno in pendenciaRegistroIndividual.Alunos)
            {
                pendenciaRegistroIndividualAluno.Id = await repositorioPendenciaRegistroIndividualAluno.SalvarAsync(pendenciaRegistroIndividualAluno);
            }
        }

        private async Task AdicionarUsuariosDaPendenciaAsync(Pendencia pendencia, Turma turma, IEnumerable<string> professoresRfs)
        {
            var usuariosIds = await mediator.Send(new ObterUsuariosIdPorCodigosRfQuery(professoresRfs.ToList()));
            if (!usuariosIds?.Any() ?? true)
                throw new NegocioException($"Não foram encontrados usuários para os professores da turma {turma.CodigoTurma}.");

            var pendenciasUsuarios = usuariosIds.Select(x => new PendenciaUsuario { PendenciaId = pendencia.Id, UsuarioId = x });
            foreach (var pendenciaUsuario in pendenciasUsuarios)
                await repositorioPendenciaUsuario.SalvarAsync(pendenciaUsuario);
        }

        private async Task AlterarPendenciaAusenciaRegistroIndividualAsync(PendenciaRegistroIndividual pendenciaRegistroIndividual, IEnumerable<AlunoPorTurmaResposta> alunosTurmaComAusenciaRegistroIndividualPorDias)
        {
            var alunosParaSeremAdicionadosNaPendencia = alunosTurmaComAusenciaRegistroIndividualPorDias
                .Where(x => !pendenciaRegistroIndividual.Alunos.Any(y => y.CodigoAluno.ToString() == x.CodigoAluno && y.Situacao == SituacaoPendenciaRegistroIndividualAluno.Pendente))
                .ToList();

            if (!alunosParaSeremAdicionadosNaPendencia.Any()) return;

            var codigosDosAlunos = alunosParaSeremAdicionadosNaPendencia.Select(x => long.Parse(x.CodigoAluno));
            pendenciaRegistroIndividual.AdicionarAlunos(codigosDosAlunos);

            foreach (var codigoAluno in codigosDosAlunos)
            {
                var pendenciaRegistroIndividualAluno = pendenciaRegistroIndividual.Alunos.First(x => x.CodigoAluno == codigoAluno);
                pendenciaRegistroIndividualAluno.Id = await repositorioPendenciaRegistroIndividualAluno.SalvarAsync(pendenciaRegistroIndividualAluno);
            }
        }

        private static string DefinirTituloDaPendenciaPorAusenciaDeRegistroIndividual(Turma turma)
            => $"Crianças com registro individual pendente - {turma.Nome} - {turma.Ue?.TipoEscola.ShortName()} {turma.Ue?.Nome} ({turma.Ue?.Dre?.Abreviacao})";
    }
}