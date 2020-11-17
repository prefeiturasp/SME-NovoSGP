using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarDocumentoCommandHandler : AbstractUseCase, IRequestHandler<SalvarDocumentoCommand, bool>
    {
        private readonly IRepositorioDocumento repositorioDocumento;

        public SalvarDocumentoCommandHandler(IRepositorioDocumento repositorioDocumento, IMediator mediator) : base(mediator)
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

            if(request.SalvarDocumentoDto.TipoDocumentoId == 1)
            {
                var usuario = await mediator.Send(new ObterUsuarioPorIdQuery(request.SalvarDocumentoDto.UsuarioId));
                var tiposDocumentos = await mediator.Send(new ObterTipoDocumentoClassificacaoQuery());

                var classificacao = tiposDocumentos.FirstOrDefault(t => t.Id == request.SalvarDocumentoDto.TipoDocumentoId).Classificacoes.FirstOrDefault(c => c.Id == request.SalvarDocumentoDto.ClassificacaoId);

                if (!usuario.Perfis.Where(u => u.NomePerfil == classificacao.Classificacao).Any())
                    throw new NegocioException("O usuário vinculado a este documento não possui o perfil que corresponde ao tipo de plano selecionado.");
            }            


            if (await mediator.Send(new ValidarTipoDocumentoDaClassificacaoQuery(request.SalvarDocumentoDto.ClassificacaoId, Dominio.Enumerados.TipoDocumento.PlanoTrabalho))
                && await mediator.Send(new ValidarUeEducacaoInfantilQuery(request.SalvarDocumentoDto.UeId)))
                throw new NegocioException("Escolas de educação infantíl não podem fazer upload de Plano de Trabalho!");

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
