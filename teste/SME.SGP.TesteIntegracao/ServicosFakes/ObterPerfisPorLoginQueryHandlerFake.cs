using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ObterPerfisPorLoginQueryHandlerFake : IRequestHandler<ObterPerfisPorLoginQuery, PerfisApiEolDto>
    {
        public async Task<PerfisApiEolDto> Handle(ObterPerfisPorLoginQuery request, CancellationToken cancellationToken)
        {
            var listaUsuarios = new List<PerfisApiEolDto>
            {
                new PerfisApiEolDto
                {
                    CodigoRf = TesteBaseComuns.USUARIO_LOGADO_RF,
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_PROFESSOR,
                        Perfis.PERFIL_CJ
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = "6737544",
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_PROFESSOR,
                        Perfis.PERFIL_CJ
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = "1111111",
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_CP,
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = TesteBaseComuns.USUARIO_LOGIN_CP,
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_CP,
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = TesteBaseComuns.USUARIO_LOGIN_DIRETOR,
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_DIRETOR,
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = TesteBaseComuns.USUARIO_LOGIN_AD,
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_AD,
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = TesteBaseComuns.USUARIO_LOGIN_PAAI,
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_PAAI,
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = TesteBaseComuns.USUARIO_LOGIN_COOD_NAAPA,
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_COORDENADOR_NAAPA
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = TesteBaseComuns.USUARIO_LOGIN_ADM_DRE,
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_ADMDRE
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = TesteBaseComuns.USUARIO_LOGIN_ADM_SME,
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_ADMSME
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = "1",
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_ADMUE
                    }
                },
                new PerfisApiEolDto
                {
                    CodigoRf = "5555555",
                    Perfis = new List<Guid>
                    {
                        Perfis.PERFIL_PAEE,
                    }
                }
            };
            return await Task.FromResult(listaUsuarios.FirstOrDefault(x => x.CodigoRf == request.Login.ToUpper()));
        }
    }
}
