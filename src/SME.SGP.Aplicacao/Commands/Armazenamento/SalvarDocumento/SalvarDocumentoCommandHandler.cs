using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarDocumentoCommandHandler : AbstractUseCase, IRequestHandler<SalvarDocumentoCommand, bool>
    {
        private readonly IRepositorioDocumento repositorioDocumento;

        public SalvarDocumentoCommandHandler(IRepositorioDocumento repositorioDocumento, IMediator mediator) :base(mediator)
        {
            this.repositorioDocumento = repositorioDocumento ?? throw new ArgumentNullException(nameof(repositorioDocumento));
        }


        public async Task<bool> Handle(SalvarDocumentoCommand request, CancellationToken cancellationToken)
        {
            var arquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(request.SalvarDocumentoDto.ArquivoCodigo));
            if (arquivo == null)
                throw new NegocioException("Não foi encontrado um arquivo com este código");

            var existeArquivo = await mediator.Send(new VerificaUsuarioPossuiArquivoQuery(request.SalvarDocumentoDto.TipoDocumentoId, request.SalvarDocumentoDto.ClassificacaoId, request.SalvarDocumentoDto.UsuarioId, request.SalvarDocumentoDto.UeId));
            if (existeArquivo)
                throw new NegocioException("Este usuário já possui um arquivo");

            var documento = new Documento()
            {
                ClassificacaoDocumentoId = request.SalvarDocumentoDto.ClassificacaoId,
                UsuarioId = request.SalvarDocumentoDto.UsuarioId,
                UeId = request.SalvarDocumentoDto.UeId,
                ArquivoId = arquivo.Id,
                AnoLetivo = request.SalvarDocumentoDto.AnoLetivo
            };

            await repositorioDocumento.SalvarAsync(documento);

            return true;
        }
    }
}
