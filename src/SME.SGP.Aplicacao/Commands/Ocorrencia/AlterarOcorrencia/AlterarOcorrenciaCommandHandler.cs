using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class AlterarOcorrenciaCommandHandler : IRequestHandler<AlterarOcorrenciaCommand, AuditoriaDto>
    {
        private readonly IRepositorioOcorrencia repositorioOcorrencia;
        private readonly IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo;
        private readonly IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno;
        private readonly IRepositorioOcorrenciaServidor repositorioOcorrenciaServidor;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions;

        public AlterarOcorrenciaCommandHandler(IRepositorioOcorrencia repositorioOcorrencia, IRepositorioOcorrenciaTipo repositorioOcorrenciaTipo,
            IRepositorioOcorrenciaAluno repositorioOcorrenciaAluno, IUnitOfWork unitOfWork, IMediator mediator,
            IRepositorioOcorrenciaServidor repositorioOcorrenciaServidor,
            IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions)
        {
            this.repositorioOcorrencia = repositorioOcorrencia ?? throw new ArgumentNullException(nameof(repositorioOcorrencia));
            this.repositorioOcorrenciaServidor = repositorioOcorrenciaServidor ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaServidor));
            this.repositorioOcorrenciaTipo = repositorioOcorrenciaTipo ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaTipo)); ;
            this.repositorioOcorrenciaAluno = repositorioOcorrenciaAluno ?? throw new ArgumentNullException(nameof(repositorioOcorrenciaAluno)); ;
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }

        public async Task<AuditoriaDto> Handle(AlterarOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var ocorrencia = await repositorioOcorrencia.ObterPorIdAsync(request.Id);
                    if (ocorrencia.EhNulo())
                        throw new NegocioException($"Ocorrencia {request.Id} não encontrada!");

                    var ocorrenciaTipo = await repositorioOcorrenciaTipo.ObterPorIdAsync(request.OcorrenciaTipoId);
                    if (ocorrenciaTipo is null)
                        throw new NegocioException("O tipo da ocorrência informado não foi encontrado.");

                    var descricaoAtual = ocorrencia.Descricao;
                    await MapearAlteracoes(ocorrencia, request, ocorrenciaTipo);
                    await repositorioOcorrencia.SalvarAsync(ocorrencia);

                    var alunosParaSeremDeletados = ocorrencia.Alunos.Where(x => !request.CodigosAlunos.Contains(x.CodigoAluno)).Select(x => x.Id);
                    await repositorioOcorrenciaAluno.ExcluirAsync(alunosParaSeremDeletados);
                    
                    var novosAlunos = request.CodigosAlunos.Where(x => !ocorrencia.Alunos.Any(y => y.CodigoAluno == x)).ToList();
                    ocorrencia.AdiconarAlunos(novosAlunos);
                    foreach (var novoAluno in novosAlunos)
                    {
                        var ocorrenciaAluno = ocorrencia.Alunos.FirstOrDefault(x => x.CodigoAluno == novoAluno);
                        await repositorioOcorrenciaAluno.SalvarAsync(ocorrenciaAluno);
                    }

                    var servidoresParaSeremDeletados = ocorrencia.Servidores.Where(s => !request.CodigosServidores.Contains(s.CodigoServidor)).Select(d => d.Id);
                    await repositorioOcorrenciaServidor.ExcluirPoIds(servidoresParaSeremDeletados);
                    
                    var novosServidores = request.CodigosServidores.Where(x => !ocorrencia.Servidores.Any(y => y.CodigoServidor == x)).ToList();
                    ocorrencia.AdicionarServidores(novosServidores);
                    foreach (var novoServidor in novosServidores)
                    {
                        var ocorrenciaServidor = ocorrencia.Servidores.FirstOrDefault(x => x.CodigoServidor == novoServidor);
                        await repositorioOcorrenciaServidor.SalvarAsync(ocorrenciaServidor);
                    }
                    
                    
                    unitOfWork.PersistirTransacao();
                    await MoverRemoverExcluidos(request.Descricao, descricaoAtual);
                    return (AuditoriaDto)ocorrencia;
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }
        private async Task MoverRemoverExcluidos(string novo, string atual)
        {
            var caminho = string.Empty;

            if (!string.IsNullOrEmpty(novo))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.Ocorrencia, atual, novo));
            }
            if (!string.IsNullOrEmpty(atual))
            {
                var deletarArquivosNaoUtilziados = await mediator.Send(new RemoverArquivosExcluidosCommand(atual, novo, TipoArquivo.Ocorrencia.Name()));
            }
        }
        private async Task MapearAlteracoes(Ocorrencia entidade, AlterarOcorrenciaCommand request, OcorrenciaTipo ocorrenciaTipo)
        {
            entidade.DataOcorrencia = request.DataOcorrencia;
            entidade.SetHoraOcorrencia(request.HoraOcorrencia);
            entidade.Titulo = request.Titulo;
            entidade.Descricao = request.Descricao.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos);
            entidade.UeId = request.UeId;
            entidade.SetOcorrenciaTipo(ocorrenciaTipo);
        }
    }
}

