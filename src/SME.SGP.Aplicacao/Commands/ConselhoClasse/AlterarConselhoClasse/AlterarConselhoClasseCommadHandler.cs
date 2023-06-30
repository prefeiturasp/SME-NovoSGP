using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarConselhoClasseCommadHandler : InserirAlterarConselhoClasseAbstrato, IRequestHandler<AlterarConselhoClasseCommad, ConselhoClasseNotaRetornoDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta;
        private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNotaConsulta;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;

        public AlterarConselhoClasseCommadHandler(
                            IMediator mediator,
                            IUnitOfWork unitOfWork,
                            IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta,
                            IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNotaConsulta,
                            IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                            IRepositorioConselhoClasseNota repositorioConselhoClasseNota) : base(mediator, repositorioConselhoClasseNota)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioConselhoClasseAlunoConsulta = repositorioConselhoClasseAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAlunoConsulta));
            this.repositorioConselhoClasseNotaConsulta = repositorioConselhoClasseNotaConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNotaConsulta));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }

        public async Task<ConselhoClasseNotaRetornoDto> Handle(AlterarConselhoClasseCommad request, CancellationToken cancellationToken)
        {
            AuditoriaDto auditoria = null;
            long conselhoClasseAlunoId = 0;
            var enviarAprovacao = false;

            var conselhoClasseAluno = await repositorioConselhoClasseAlunoConsulta
                .ObterPorConselhoClasseAlunoCodigoAsync(request.ConselhoClasseId, request.CodigoAluno);

            unitOfWork.IniciarTransacao();
            try
            {
                conselhoClasseAlunoId = conselhoClasseAluno?.Id ?? await SalvarConselhoClasseAlunoResumido(request.ConselhoClasseId, request.CodigoAluno);

                await mediator.Send(new InserirTurmasComplementaresCommand(request.Turma.Id, conselhoClasseAlunoId, request.CodigoAluno), cancellationToken);

                var conselhoClasseNota = await repositorioConselhoClasseNotaConsulta
                    .ObterPorConselhoClasseAlunoComponenteCurricularAsync(conselhoClasseAlunoId, request.ConselhoClasseNotaDto.CodigoComponenteCurricular);

                double? notaAnterior = null;
                long? conceitoIdAnterior = null;

                await MoverJustificativaConselhoClasseNota(request.ConselhoClasseNotaDto, conselhoClasseNota == null ? string.Empty : conselhoClasseNota.Justificativa);

                if (conselhoClasseNota == null)
                {
                    conselhoClasseNota = ObterConselhoClasseNota(request.ConselhoClasseNotaDto, conselhoClasseAlunoId);
                }
                else
                {
                    notaAnterior = conselhoClasseNota.Nota;
                    conceitoIdAnterior = conselhoClasseNota.ConceitoId;

                    conselhoClasseNota.Justificativa = request.ConselhoClasseNotaDto.Justificativa;
                    if (request.ConselhoClasseNotaDto.Nota.HasValue)
                    {
                        // Gera histórico de alteração
                        if (conselhoClasseNota.Nota != null && conselhoClasseNota.Nota != request.ConselhoClasseNotaDto.Nota.Value)
                            await mediator.Send(new SalvarHistoricoNotaConselhoClasseCommand(conselhoClasseNota.Id, conselhoClasseNota.Nota.Value, request.ConselhoClasseNotaDto.Nota.Value), cancellationToken);

                        conselhoClasseNota.Nota = request.ConselhoClasseNotaDto.Nota.Value;
                    }
                    else conselhoClasseNota.Nota = null;

                    // Gera histórico de alteração
                    if (request.ConselhoClasseNotaDto.Conceito.HasValue)
                    {
                        if (conselhoClasseNota.ConceitoId != null && conselhoClasseNota.ConceitoId != request.ConselhoClasseNotaDto.Conceito.Value)
                            await mediator.Send(new SalvarHistoricoConceitoConselhoClasseCommand(conselhoClasseNota.Id, conselhoClasseNota.ConceitoId, request.ConselhoClasseNotaDto.Conceito.Value), cancellationToken);
                    }
                    else
                    {
                        if (conselhoClasseNota.ConceitoId != null && request.ConselhoClasseNotaDto.Conceito == null)
                            await mediator.Send(new SalvarHistoricoConceitoConselhoClasseCommand(conselhoClasseNota.Id, conselhoClasseNota.ConceitoId, null), cancellationToken);
                    }
                    conselhoClasseNota.ConceitoId = request.ConselhoClasseNotaDto.Conceito.HasValue ? request.ConselhoClasseNotaDto.Conceito.Value : null;
                }

                if (request.Turma.AnoLetivo == 2020)
                    ValidarNotasFechamentoConselhoClasse2020(conselhoClasseNota);

                if (conselhoClasseNota.Id > 0 || conselhoClasseAluno is { AlteradoEm: { } })
                    await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);

                enviarAprovacao = await EnviarParaAprovacao(request.Turma, request.UsuarioLogado);

                if (enviarAprovacao)
                    await GerarWFAprovacao(conselhoClasseNota, request.Turma, request.Bimestre, request.UsuarioLogado, request.CodigoAluno, notaAnterior, conceitoIdAnterior);
                else
                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);

                auditoria = (AuditoriaDto)conselhoClasseAluno;

                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
            }
            
            await RemoverCache(string.Format(NomeChaveCache.CHAVE_NOTA_CONCEITO_FECHAMENTO_TURMA_TODOS_BIMESTRES_E_FINAL, request.Turma.CodigoTurma, request.CodigoAluno), cancellationToken);
            await RemoverCache(string.Format(NomeChaveCache.CHAVE_NOTA_CONCEITO_CONSELHO_CLASSE_TURMA_BIMESTRE, request.Turma.CodigoTurma, request.Bimestre, request.CodigoAluno), cancellationToken);
                        
            //Tratar após o fechamento da transação - ano letivo e turmaId
            if (!enviarAprovacao)
            {
                var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(request.Turma.CodigoTurma, request.CodigoAluno, consideraInativos: true), cancellationToken);

                if (aluno == null)
                    throw new NegocioException($"Não foram encontrados alunos para a turma {request.Turma.CodigoTurma} no Eol");

                var consolidacaoNotaAlunoDto = new ConsolidacaoNotaAlunoDto()
                {
                    AlunoCodigo = request.CodigoAluno,
                    TurmaId = request.Turma.Id,
                    Bimestre = ObterBimestre(request.Bimestre),
                    AnoLetivo = request.Turma.AnoLetivo,
                    Nota = request.ConselhoClasseNotaDto.Nota,
                    ConceitoId = request.ConselhoClasseNotaDto.Conceito,
                    ComponenteCurricularId = request.ConselhoClasseNotaDto.CodigoComponenteCurricular,
                    Inativo = aluno.Inativo
                };
                
                await mediator.Send(new ConsolidacaoNotaAlunoCommand(consolidacaoNotaAlunoDto), cancellationToken);
            }

            var conselhoClasseNotaRetorno = new ConselhoClasseNotaRetornoDto()
            {
                ConselhoClasseId = request.ConselhoClasseId,
                FechamentoTurmaId = request.FechamentoTurmaId,
                Auditoria = auditoria,
                ConselhoClasseAlunoId = conselhoClasseAlunoId,
                EmAprovacao = enviarAprovacao
            };

            return conselhoClasseNotaRetorno;
        }
        
        private async Task RemoverCache(string nomeChave, CancellationToken cancellationToken)
        {
            await mediator.Send(new RemoverChaveCacheCommand(nomeChave), cancellationToken);
        }        

        private async Task<long> SalvarConselhoClasseAlunoResumido(long conselhoClasseId, string alunoCodigo)
        {
            var conselhoClasseAluno = new ConselhoClasseAluno()
            {
                AlunoCodigo = alunoCodigo,
                ConselhoClasseId = conselhoClasseId
            };

            return await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);
        }
    }
}
