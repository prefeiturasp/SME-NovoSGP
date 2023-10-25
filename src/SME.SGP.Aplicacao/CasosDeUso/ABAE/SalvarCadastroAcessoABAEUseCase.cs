﻿using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao;

public class SalvarCadastroAcessoABAEUseCase: AbstractUseCase, ISalvarCadastroAcessoABAEUseCase
{
    public SalvarCadastroAcessoABAEUseCase(IMediator mediator) : base(mediator)
    {}

    public async Task<CadastroAcessoABAEDto> Executar(CadastroAcessoABAEDto cadastroAcessoABAEDto)
    {
        var cadastroAcessoABAE = cadastroAcessoABAEDto.Id.EhMaiorQueZero() ? await mediator.Send(new ObterCadastroAcessoABAEPorIdQuery(cadastroAcessoABAEDto.Id)): new CadastroAcessoABAE();

        if (cadastroAcessoABAEDto.Id.EhMaiorQueZero() && !cadastroAcessoABAE.Cpf.Equals(cadastroAcessoABAEDto.Cpf))
            throw new NegocioException(MensagemNegocioComuns.NAO_EH_PERMITIDO_ALTERACAO_CPF_POS_CADASTRO);
        
        if (cadastroAcessoABAEDto.Id.EhMaiorQueZero() && !cadastroAcessoABAE.UeId.Equals(cadastroAcessoABAEDto.UeId))
            throw new NegocioException(MensagemNegocioComuns.NAO_EH_PERMITIDO_ALTERACAO_UE_POS_CADASTRO);
        
        if (cadastroAcessoABAEDto.Id.EhIgualZero() && (await mediator.Send(new ExisteCadastroAcessoABAEPorCpfQuery(cadastroAcessoABAEDto.Cpf))))
            throw new NegocioException(string.Format(MensagemNegocioComuns.JA_EXISTE_CADASTRO_ACESSO_ABAR_PARA_ESSE_CPF, cadastroAcessoABAEDto.Cpf));
            
        cadastroAcessoABAE.Nome = cadastroAcessoABAEDto.Nome;
        cadastroAcessoABAE.Email = cadastroAcessoABAEDto.Email;
        cadastroAcessoABAE.Telefone = cadastroAcessoABAEDto.Telefone;
        cadastroAcessoABAE.Situacao = cadastroAcessoABAEDto.Situacao;
        cadastroAcessoABAE.Cep = cadastroAcessoABAEDto.Cep;
        cadastroAcessoABAE.Endereco = cadastroAcessoABAEDto.Endereco;
        cadastroAcessoABAE.Numero = cadastroAcessoABAEDto.Numero;
        cadastroAcessoABAE.Complemento = cadastroAcessoABAEDto.Complemento;
        cadastroAcessoABAE.Cidade = cadastroAcessoABAEDto.Cidade;
        cadastroAcessoABAE.Estado = cadastroAcessoABAEDto.Estado;
        cadastroAcessoABAE.Excluido = cadastroAcessoABAEDto.Excluido;

        if (cadastroAcessoABAEDto.Id.EhIgualZero())
        {
            cadastroAcessoABAE.UeId = cadastroAcessoABAEDto.UeId;
            cadastroAcessoABAE.Cpf = cadastroAcessoABAEDto.Cpf;
        }

        cadastroAcessoABAEDto.Id = await mediator.Send(new SalvarCadastroAcessoABAECommand(cadastroAcessoABAE));
        
        await mediator.Send(new PublicarFilaApiEOLCommand(RotasRabbitApiEOL.RotaManutencaoUsuarioABAECoreSSO, cadastroAcessoABAE));

        return cadastroAcessoABAEDto;
    }
}