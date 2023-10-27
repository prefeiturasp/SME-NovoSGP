using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterCadastroAcessoABAEUseCase : AbstractUseCase, IObterCadastroAcessoABAEUseCase
    {
        public ObterCadastroAcessoABAEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<CadastroAcessoABAEDto> Executar(long id)
        {
            var cadastroAcessoABAE = await mediator.Send(new ObterCadastroAcessoABAEPorIdQuery(id));

            return new CadastroAcessoABAEDto()
            {
                Id = cadastroAcessoABAE.Id,
                DreCodigo = cadastroAcessoABAE.DreCodigo,
                UeCodigo = cadastroAcessoABAE.UeCodigo,
                UeId = cadastroAcessoABAE.UeId,
                Nome = cadastroAcessoABAE.Nome,
                Cpf = cadastroAcessoABAE.Cpf,
                Email = cadastroAcessoABAE.Email,
                Telefone = cadastroAcessoABAE.Telefone,
                Situacao = cadastroAcessoABAE.Situacao,
                Cep = cadastroAcessoABAE.Cep,
                Endereco = cadastroAcessoABAE.Endereco,
                Numero = cadastroAcessoABAE.Numero,
                Complemento = cadastroAcessoABAE.Complemento,
                Bairro = cadastroAcessoABAE.Bairro,
                Cidade = cadastroAcessoABAE.Cidade,
                Estado = cadastroAcessoABAE.Estado,
                Excluido = cadastroAcessoABAE.Excluido,
                CriadoEm = cadastroAcessoABAE.CriadoEm,
                CriadoPor = cadastroAcessoABAE.CriadoPor,
                CriadoRF = cadastroAcessoABAE.CriadoRF,
                AlteradoEm = cadastroAcessoABAE.AlteradoEm,
                AlteradoPor = cadastroAcessoABAE.AlteradoPor,
                AlteradoRF = cadastroAcessoABAE.AlteradoRF
            };
        }
    }
}