using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.Informes;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInformesPorFiltroQueryHandler : ConsultasBase, IRequestHandler<ObterInformesPorFiltroQuery, PaginacaoResultadoDto<InformeResumoDto>>
    {
        private const string TODAS = "Todas";
        private readonly IRepositorioInformativo repositorio;
        private readonly IMediator mediator;

        public ObterInformesPorFiltroQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioInformativo repositorio, IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<InformeResumoDto>> Handle(ObterInformesPorFiltroQuery request, CancellationToken cancellationToken)
        {
            var perfils = await mediator.Send(new ObterGruposDeUsuariosQuery((int)TipoPerfil.SME));
            var paginacao = await this.repositorio.ObterInformesPaginado(request.Filtro, Paginacao);

            return MapearParaDto(paginacao, perfils);
        }

        private PaginacaoResultadoDto<InformeResumoDto> MapearParaDto(PaginacaoResultadoDto<Informativo> informativo, IEnumerable<GruposDeUsuariosDto> perfils)
        {
            return new PaginacaoResultadoDto<InformeResumoDto>()
            {
                TotalPaginas = informativo.TotalPaginas,
                TotalRegistros = informativo.TotalRegistros,
                Items = MapearParaDto(informativo.Items.OrderByDescending(info => info.DataEnvio), perfils)
            };
        }

        private IEnumerable<InformeResumoDto> MapearParaDto(IEnumerable<Informativo> informativos, IEnumerable<GruposDeUsuariosDto> perfils)
        {
            var informes = new List<InformeResumoDto>();

            foreach (var informativo in informativos)
            {
                informes.Add(new InformeResumoDto()
                {
                    Id = informativo.Id,
                    UeNome = informativo.Ue.NaoEhNulo() ? $"{informativo.Ue.TipoEscola.ShortName()} {informativo.Ue.Nome}" : TODAS,
                    DataEnvio = informativo.DataEnvio.ToString("dd/MM/yyyy"),
                    DreNome = informativo.Dre.NaoEhNulo() ? informativo.Dre.Abreviacao : TODAS,
                    Perfis = ObterPerfils(perfils, informativo.Perfis),
                    Titulo = informativo.Titulo,
                    EnviadoPor = $"{informativo.CriadoPor} ({informativo.CriadoRF})"
                }); 
            }

            return informes;
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
