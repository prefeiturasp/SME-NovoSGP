using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Informe.ServicosFake
{
    public class ObterRfsUsuariosPorPerfisDreUeQueryHandlerFake : IRequestHandler<ObterRfsUsuariosPorPerfisDreUeQuery, IEnumerable<UsuarioPerfilsAbrangenciaDto>>
    {
        public Task<IEnumerable<UsuarioPerfilsAbrangenciaDto>> Handle(ObterRfsUsuariosPorPerfisDreUeQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<UsuarioPerfilsAbrangenciaDto>>(
                new List<UsuarioPerfilsAbrangenciaDto>()
                {
                    new UsuarioPerfilsAbrangenciaDto() { 
                        UsuarioRf = "DIR999998" , 
                        Perfils = new List<PerfilsAbrangenciaDto>()
                        {
                            new PerfilsAbrangenciaDto()
                            {
                                Perfil = PerfilUsuario.AD.ObterNome().ToLower(),
                                Ues = new List<string>()
                                {
                                    "1"
                                }
                            }
                        }
                    },
                    new UsuarioPerfilsAbrangenciaDto() { 
                        UsuarioRf = "2222222",
                        Perfils = new List<PerfilsAbrangenciaDto>()
                        {
                            new PerfilsAbrangenciaDto()
                            {
                                Perfil = PerfilUsuario.ADMUE.ObterNome(),
                                Ues = new List<string>()
                                {
                                    "1"
                                }
                            }
                        }},
                    new UsuarioPerfilsAbrangenciaDto() { 
                        UsuarioRf = "3333333",
                        Perfils = new List<PerfilsAbrangenciaDto>()
                        {
                            new PerfilsAbrangenciaDto()
                            {
                                Perfil = PerfilUsuario.ADMDRE.ObterNome()
                            }
                        }
                    }
                });
        }
    }
}
