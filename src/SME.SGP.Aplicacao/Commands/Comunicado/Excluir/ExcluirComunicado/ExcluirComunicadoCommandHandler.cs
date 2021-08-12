using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirComunicadoCommandHandler : IRequestHandler<ExcluirComunicadoCommand, bool>
    {
        private readonly IRepositorioComunicado repositorioComunicado;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ExcluirComunicadoCommandHandler(IRepositorioComunicado repositorioComunicado, IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirComunicadoCommand request, CancellationToken cancellationToken)
        {
            var comunicados = await mediator.Send(new ObterComunicadosPorIdsQuery(request.Ids));

            if (comunicados == null || !comunicados.Any())
                throw new NegocioException("Comunicados não localizados");

            var erros = new StringBuilder();

            var idsNaoEncontrados = request.Ids.Select(id =>
            {
                var comunicado = comunicados.FirstOrDefault(c => c.Id == id);
                if (comunicado == null)
                {
                    erros.Append($"<li>{id} - comunicado não encontrado</li>");
                }
                return id;
            });

            if (string.IsNullOrEmpty(erros.ToString()))
                try
                {
                    unitOfWork.IniciarTransacao();

                    foreach (var comunicado in comunicados)
                    {                        
                        await mediator.Send(new ExcluirTodosAlunosComunicadoCommand(comunicado.Id));

                        await mediator.Send(new ExcluirTodasTurmasComunicadoCommand(comunicado.Id));

                        await mediator.Send(new ExcluirComunicadoModalidadesCommand(comunicado.Id));

                        await mediator.Send(new ExcluirComunicadoModalidadesCommand(comunicado.Id));

                        comunicado.MarcarExcluido();

                        await repositorioComunicado.SalvarAsync(comunicado);
                    }

                    await mediator.Send(new ExcluirNotificacaoEscolaAquiCommand(request.Ids));

                    unitOfWork.PersistirTransacao();                    
                }
                catch(Exception ex)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            else
                SentrySdk.CaptureMessage(erros.ToString());

            return true;
        }
    }
}
