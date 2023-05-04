using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class ObterFuncionarioCoreSSOPorPerfilDreQueryHandlerFake : IRequestHandler<ObterFuncionarioCoreSSOPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        public async Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterFuncionarioCoreSSOPorPerfilDreQuery request, CancellationToken cancellationToken)
        {
            if (request.CodigoPerfil == Perfis.PERFIL_PSICOLOGO_ESCOLAR)
                return RetornarPsicologoUsuarioEOL();

            if (request.CodigoPerfil == Perfis.PERFIL_PSICOPEDAGOGO)
                return RetornarPsicoPedagogoUsuarioEOL();
            if (request.CodigoPerfil == Perfis.PERFIL_ASSISTENTE_SOCIAL)
                return RetornarAssistenteSocialUsuarioEOL();

            return Enumerable.Empty<UsuarioEolRetornoDto>();
        }

        private static IEnumerable<UsuarioEolRetornoDto> RetornarPsicologoUsuarioEOL()
        {
            return new List<UsuarioEolRetornoDto>
            { new UsuarioEolRetornoDto()
            {   UsuarioId = 3,
                CodigoFuncaoAtividade = 0,
                CodigoRf = "3",
                Login = "3",
                EstaAfastado = false,
                NomeServidor = "Psicologo Teste" }
            };
        }
        private static IEnumerable<UsuarioEolRetornoDto> RetornarPsicoPedagogoUsuarioEOL()
        {
            return new List<UsuarioEolRetornoDto>
            { new UsuarioEolRetornoDto()
            {   UsuarioId = 4,
                CodigoFuncaoAtividade = 0,
                CodigoRf = "4",
                Login = "4",
                EstaAfastado = false,
                NomeServidor = "PsicoPedagogo Teste"}
            };
        }
        private static IEnumerable<UsuarioEolRetornoDto> RetornarAssistenteSocialUsuarioEOL()
        {
            return new List<UsuarioEolRetornoDto>
            { new UsuarioEolRetornoDto()
            {   UsuarioId = 5,
                CodigoFuncaoAtividade = 0,
                CodigoRf = "5",
                Login = "5",
                EstaAfastado = false,
                NomeServidor = "Assistente Social Teste"}
            };
        }

    }
}
