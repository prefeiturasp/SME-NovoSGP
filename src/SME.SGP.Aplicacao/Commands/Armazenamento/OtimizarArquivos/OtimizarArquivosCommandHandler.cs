using MediatR;
using SME.SGP.Dominio;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Aplicacao
{
    public class OtimizarArquivosCommandHandler : IRequestHandler<OtimizarArquivosCommand, bool>
    {
        private readonly IMediator mediator;
        
        public OtimizarArquivosCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(OtimizarArquivosCommand request, CancellationToken cancellationToken)
        {
            string extensao = Path.GetExtension(request.NomeArquivo);
            
            var ehImagem = extensao.EhExtensaoImagemParaOtimizar();
            
            var ehVideo = extensao.EhExtensaoVideoParaOtimizar();

            if (ehImagem || ehVideo)
            {
                var nomeFila = ehImagem ? RotasRabbitSgp.OtimizarArquivoImagem : RotasRabbitSgp.OtimizarArquivoVideo;
                await mediator.Send(new PublicarFilaSgpCommand(nomeFila,request.NomeArquivo,Guid.NewGuid(),null));
            }
            
            return true;
        }
    }
}
