using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarCadastroAcessoABAEUseCase : AbstractUseCase, ISalvarCadastroAcessoABAEUseCase
    {
        public SalvarCadastroAcessoABAEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<CadastroAcessoABAEDto> Executar(CadastroAcessoABAEDto param)
        {
            if (param.Cpf.NaoEhCpfValido())
                throw new NegocioException(MensagemNegocioComuns.CPF_INFORMADO_EH_INVALIDO);

            if (param.Telefone.NaoEhTelefoneValido())
                throw new NegocioException(MensagemNegocioComuns.TELEFONE_DEVE_ESTAR_COM_A_SEGUINTE_MASCARA);

            var cadastroAcessoABAE = param.Id.EhMaiorQueZero()
                ? await mediator.Send(new ObterCadastroAcessoABAEPorIdQuery(param.Id))
                : new CadastroAcessoABAE();

            if (param.Id.EhMaiorQueZero() && !cadastroAcessoABAE.Cpf.Equals(param.Cpf))
                throw new NegocioException(MensagemNegocioComuns.NAO_EH_PERMITIDO_ALTERACAO_CPF_POS_CADASTRO);

            if (param.Id.EhMaiorQueZero() &&
                !cadastroAcessoABAE.UeId.Equals(param.UeId))
                throw new NegocioException(MensagemNegocioComuns.NAO_EH_PERMITIDO_ALTERACAO_UE_POS_CADASTRO);

            if (param.Id.EhIgualZero() &&
                (await mediator.Send(new ExisteCadastroAcessoABAEPorCpfQuery(param.Cpf, param.UeId))))
                throw new NegocioException(string.Format(
                    MensagemNegocioComuns.JA_EXISTE_CADASTRO_ACESSO_ABAR_PARA_ESSE_CPF, param.Cpf));

            cadastroAcessoABAE.Nome = param.Nome;
            cadastroAcessoABAE.Email = param.Email;
            cadastroAcessoABAE.Telefone = param.Telefone;
            cadastroAcessoABAE.Situacao = param.Situacao;
            cadastroAcessoABAE.Cep = param.Cep;
            cadastroAcessoABAE.Endereco = param.Endereco;
            cadastroAcessoABAE.Numero = param.Numero;
            cadastroAcessoABAE.Complemento = param.Complemento;
            cadastroAcessoABAE.Cidade = param.Cidade;
            cadastroAcessoABAE.Bairro = param.Bairro;
            cadastroAcessoABAE.Estado = param.Estado;
            cadastroAcessoABAE.Excluido = param.Excluido;

            if (param.Id.EhIgualZero())
            {
                cadastroAcessoABAE.UeId = param.UeId;
                cadastroAcessoABAE.Cpf = param.Cpf;
            }

            param.Id = await mediator.Send(new SalvarCadastroAcessoABAECommand(cadastroAcessoABAE));

            await mediator.Send(new PublicarFilaApiEOLCommand(RotasRabbitApiEOL.RotaManutencaoUsuarioABAECoreSSO,
                cadastroAcessoABAE.toManutencaoUsuarioABAECoreSSOAPIEolDto(await mediator.Send(new ObterUeCodigoPorIdQuery(cadastroAcessoABAE.UeId)))));

            return param;
        }
    }
}