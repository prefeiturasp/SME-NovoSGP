using MediatR;
using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirConselhoClasseNotaCommadHandler : InserirAlterarConselhoClasseAbstrato, IRequestHandler<InserirConselhoClasseNotaCommad, ConselhoClasseNotaRetornoDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioConselhoClasse repositorioConselhoClasse;
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;

        public InserirConselhoClasseNotaCommadHandler(
                            IMediator mediator,
                            IUnitOfWork unitOfWork,
                            IRepositorioConselhoClasse repositorioConselhoClasse,
                            IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno,
                            IRepositorioConselhoClasseNota repositorioConselhoClasseNota) : base(mediator, repositorioConselhoClasseNota)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }

        public async Task<ConselhoClasseNotaRetornoDto> Handle(InserirConselhoClasseNotaCommad request, CancellationToken cancellationToken)
        {
            AuditoriaDto auditoria = null;
            var enviarAprovacao = false;
            ConselhoClasseNota conselhoClasseNota = null;

            var conselhoClasse = new ConselhoClasse { FechamentoTurmaId = request.FechamentoTurma.Id };
            var conselhoClasseAluno = new ConselhoClasseAluno() { AlunoCodigo = request.CodigoAluno };

            unitOfWork.IniciarTransacao();
            try
            {
                await repositorioConselhoClasse.SalvarAsync(conselhoClasse);

                conselhoClasseAluno.ConselhoClasseId = conselhoClasse.Id;

                conselhoClasseAluno.Id = await repositorioConselhoClasseAluno.SalvarAsync(conselhoClasseAluno);
                
                await mediator.Send(new InserirTurmasComplementaresCommand(request.FechamentoTurma.TurmaId, conselhoClasseAluno.Id, request.CodigoAluno), cancellationToken);

                await MoverJustificativaConselhoClasseNota(request.ConselhoClasseNotaDto, string.Empty);

                conselhoClasseNota = ObterConselhoClasseNota(request.ConselhoClasseNotaDto, conselhoClasseAluno.Id);

                if (request.FechamentoTurma.Turma.AnoLetivo == 2020)
                    ValidarNotasFechamentoConselhoClasse2020(conselhoClasseNota);

                enviarAprovacao = await EnviarParaAprovacao(request.FechamentoTurma.Turma, request.UsuarioLogado);

                if (enviarAprovacao)
                    await GerarWFAprovacao(conselhoClasseNota, request.FechamentoTurma.Turma, request.Bimestre, request.UsuarioLogado, request.CodigoAluno, null, null);
                else
                    await repositorioConselhoClasseNota.SalvarAsync(conselhoClasseNota);

                unitOfWork.PersistirTransacao();

                auditoria = (AuditoriaDto)conselhoClasseNota;
            }
            catch(PostgresException ex)
            {
                unitOfWork.Rollback();

                await LogarExcecao(ex);
                throw new ErroInternoException("Erro ao inserir o conselho de classe");
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw;
            }

            //Tratar após o fechamento da transação
            if (!enviarAprovacao)
            {
                var aluno = await mediator.Send(new ObterAlunoPorTurmaAlunoCodigoQuery(request.FechamentoTurma.Turma.CodigoTurma, request.CodigoAluno, consideraInativos: true), cancellationToken);

                if (aluno.EhNulo())
                    throw new NegocioException($"Não foram encontrados alunos para a turma {request.FechamentoTurma.Turma.CodigoTurma} no Eol");

                var consolidacaoNotaAlunoDto = new ConsolidacaoNotaAlunoDto()
                {
                    AlunoCodigo = request.CodigoAluno,
                    TurmaId = request.FechamentoTurma.TurmaId,
                    Bimestre = ObterBimestre(request.Bimestre),
                    AnoLetivo = request.FechamentoTurma.Turma.AnoLetivo,
                    Nota = request.ConselhoClasseNotaDto.Nota,
                    ConceitoId = request.ConselhoClasseNotaDto.Conceito,
                    ComponenteCurricularId = conselhoClasseNota.ComponenteCurricularCodigo,
                    Inativo = aluno.Inativo,
                    ConselhoClasse = true
                };

                await mediator.Send(new ConsolidacaoNotaAlunoCommand(consolidacaoNotaAlunoDto));
            }

            var conselhoClasseNotaRetorno = new ConselhoClasseNotaRetornoDto()
            {
                ConselhoClasseId = conselhoClasse.Id,
                FechamentoTurmaId = request.FechamentoTurma.Id,
                Auditoria = auditoria,
                ConselhoClasseAlunoId = conselhoClasseAluno.Id,
                EmAprovacao = enviarAprovacao
            };
            return conselhoClasseNotaRetorno;
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
    }
}
