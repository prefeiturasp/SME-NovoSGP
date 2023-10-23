using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInformeUseCase : AbstractUseCase, IObterInformeUseCase
    {
        public ObterInformeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<InformesRespostaDto> Executar(long informeId)
        {
            var informe = await mediator.Send(new ObterInformesPorIdQuery(informeId));

            if (informe.EhNulo())
                throw new NegocioException(MensagemNegocioInformes.INFORMES_NAO_ENCONTRADO);

            var perfils = await mediator.Send(new ObterGruposDeUsuariosQuery((int)TipoPerfil.SME));

            return new InformesRespostaDto()
            {
                Id = informe.Id,
                DreId = informe.DreId,
                DreNome = informe.Dre.NaoEhNulo() ? informe.Dre.Abreviacao : string.Empty,
                UeId = informe.UeId,
                UeNome = informe.Ue.NaoEhNulo() ? $"{ informe.Ue.TipoEscola.ShortName() } { informe.Ue.Nome }" : string.Empty,
                Texto = informe.Texto,
                Titulo = informe.Titulo,
                Perfis = ObterPerfils(perfils, informe.Perfis),
                Auditoria = new AuditoriaDto()
                {
                    CriadoEm = informe.CriadoEm,
                    CriadoPor = informe.CriadoPor,
                    CriadoRF = informe.CriadoRF,
                    AlteradoEm = informe.AlteradoEm,
                    AlteradoPor = informe.AlteradoPor,
                    AlteradoRF = informe.AlteradoRF
                }
            };

        }

        private List<GruposDeUsuariosDto> ObterPerfils(
                                            IEnumerable<GruposDeUsuariosDto> perfils, 
                                            IEnumerable<InformativoPerfil> informativoPerfis)
        {
            var codigoPerfis = informativoPerfis.Select(ip => ip.CodigoPerfil);

            return perfils.Where(p => codigoPerfis.Contains(p.Id)).ToList();
        }
    }
}
