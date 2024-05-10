using MediatR;
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
        private readonly IRepositorioFechamentoAlunoConsulta repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;        
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;        
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;
        private readonly IServicoFechamentoFinal servicoFechamentoFinal;
        private readonly IMediator mediator;

        public ComandosFechamentoFinal(
            IServicoFechamentoFinal servicoFechamentoFinal,
            IRepositorioTurmaConsulta repositorioTurmaConsulta,
            IRepositorioFechamentoAlunoConsulta repositorioFechamentoAluno,
            IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma,            
            IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina,
            IMediator mediator)
        {
            this.servicoFechamentoFinal = servicoFechamentoFinal ?? throw new ArgumentNullException(nameof(servicoFechamentoFinal));
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaPersistenciaDto> SalvarAsync(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var usuarioLogado = await ObterUsuarioLogado();
            var turma = await ObterTurma(fechamentoFinalSalvarDto.TurmaCodigo);
            var emAprovacao = await ExigeAprovacao(turma, usuarioLogado);

            var fechamentoTurmaDisciplina = await TransformarDtoSalvarEmEntidade(fechamentoFinalSalvarDto, turma);

            var auditoria = await servicoFechamentoFinal.SalvarAsync(fechamentoTurmaDisciplina, turma, usuarioLogado,
                fechamentoFinalSalvarDto.Itens, emAprovacao);

            if (!auditoria.EmAprovacao)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoSync,
                    new ConsolidacaoTurmaDto(turma.Id, 0),
                    Guid.NewGuid()));
            }

            return auditoria;
        }

        private Task<Usuario> ObterUsuarioLogado()
            => mediator.Send(ObterUsuarioLogadoQuery.Instance);

        private async Task<Turma> ObterTurma(string turmaCodigo)
        {
            var turma = await repositorioTurmaConsulta.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            
            if (turma.EhNulo())
                throw new NegocioException("Não foi possível localizar a turma.");
            
            return turma;
        }

        private async Task<bool> ExigeAprovacao(Turma turma, Usuario usuarioLogado)
        {
            return turma.AnoLetivo < DateTime.Today.Year
                && !usuarioLogado.EhGestorEscolar()
                && await ParametroAprovacaoAtivo(turma.AnoLetivo);
        }

        private async Task<bool> ParametroAprovacaoAtivo(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaFechamento, anoLetivo));
            
            if (parametro.EhNulo())
                throw new NegocioException($"Não foi possível localizar o parametro 'AprovacaoAlteracaoNotaFechamento' para o ano {anoLetivo}");

            return parametro.Ativo;
        }

        private async Task<FechamentoTurmaDisciplina> TransformarDtoSalvarEmEntidade(FechamentoFinalSalvarDto fechamentoFinalSalvarDto, Turma turma)
        {
            var disciplinaId = fechamentoFinalSalvarDto.EhRegencia ? long.Parse(fechamentoFinalSalvarDto.DisciplinaId) :
                fechamentoFinalSalvarDto.Itens.First().ComponenteCurricularCodigo;

            FechamentoTurmaDisciplina fechamentoTurmaDisciplina = null;

            var fechamentoFinalTurma = await repositorioFechamentoTurma.ObterPorTurmaPeriodo(turma.Id);

            if (fechamentoFinalTurma.EhNulo())
                fechamentoFinalTurma = new FechamentoTurma(0, turma.Id);
            else
                fechamentoTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(fechamentoFinalSalvarDto.TurmaCodigo, disciplinaId);

            fechamentoTurmaDisciplina ??= new FechamentoTurmaDisciplina
            {
                DisciplinaId = disciplinaId,
                Situacao = SituacaoFechamento.ProcessadoComSucesso
            };

            fechamentoTurmaDisciplina.FechamentoTurma = fechamentoFinalTurma;

            foreach (var agrupamentoAluno in fechamentoFinalSalvarDto.Itens.GroupBy(a => a.AlunoRf))
            {
                var fechamentoAluno = await repositorioFechamentoAluno.ObterFechamentoAlunoENotas(fechamentoTurmaDisciplina.Id, agrupamentoAluno.Key) ??
                    new FechamentoAluno { AlunoCodigo = agrupamentoAluno.Key };

                fechamentoTurmaDisciplina.FechamentoAlunos.Add(fechamentoAluno);
            }

            return fechamentoTurmaDisciplina;
        }
    }
}