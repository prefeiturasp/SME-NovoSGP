using System;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageProcessor;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.ComprimirArquivos.Worker
{
    public class OtimizarImagemCommandHandler : IRequestHandler<OtimizarImagemCommand, bool>
    {
        private readonly IMediator mediator;
        
        public OtimizarImagemCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        
        public async Task<bool> Handle(OtimizarImagemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!request.NomeArquivo.EhArquivoImagemParaOtimizar())
                    return false;
            
                var input = Path.Combine(UtilArquivo.ObterDiretorioCompletoArquivos(), request.NomeArquivo);               

                var output = Path.Combine(UtilArquivo.ObterDiretorioCompletoTemporario(), request.NomeArquivo);

                using ( var imageFactory = new ImageFactory(preserveExifData: false))
                {
                    imageFactory.Load(input)
                        .Quality(50)
                        .Save(output);
                };

                await mediator.Send(new MoverExcluirArquivoFisicoCommand(input, output));

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao comprimir arquivo imagem", LogNivel.Critico, LogContexto.ComprimirArquivos, ex.Message));
                return false;
            }
        }
    }
}
