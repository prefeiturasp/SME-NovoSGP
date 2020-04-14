using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultaComunicado : ConsultasBase, IConsultaComunicado
    {
        private readonly IRepositorioComunicado repositorio;

        public ConsultaComunicado(
            IRepositorioComunicado repositorio,
            IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<ComunicadoCompletoDto> BuscarPorIdAsync(long id)
        {
            var comunicado = await repositorio.ObterPorIdAsync(id);
            return MapearPorIdParaDto(comunicado);
        }

        public async Task<PaginacaoResultadoDto<ComunicadoDto>> ListarPaginado(FiltroComunicadoDto filtro)
        {
            var comunicados = await repositorio.ListarPaginado(filtro, Paginacao);
            return MapearParaDtoPaginado(comunicados);
        }

        private static IEnumerable<ComunicadoCompletoDto> ConverterParaDto(IEnumerable<ComunicadoResultadoDto> comunicados)
        {
            var comunicadosDistintos = comunicados
                        .Select(s => new
                        {
                            s.Titulo,
                            s.Descricao,
                            s.Id,
                            s.AlteradoEm,
                            s.AlteradoPor,
                            s.AlteradoRF,
                            s.CriadoEm,
                            s.CriadoPor,
                            s.CriadoRF,
                            s.DataExpiracao,
                            s.DataEnvio
                        })
                        .Distinct();

            return comunicadosDistintos.Select(c => new ComunicadoCompletoDto
            {
                Id = c.Id,
                Titulo = c.Titulo,
                Descricao = c.Descricao,
                AlteradoEm = c.AlteradoEm,
                AlteradoPor = c.AlteradoPor,
                AlteradoRF = c.AlteradoRF,
                CriadoEm = c.CriadoEm,
                CriadoPor = c.CriadoPor,
                CriadoRF = c.CriadoRF,
                DataEnvio = c.DataEnvio,
                DataExpiracao = c.DataExpiracao,
                Grupos = comunicados.Where(w => w.Id == c.Id).Select(s => new GrupoComunicacaoDto { Id = s.GrupoId, Nome = s.Grupo }).ToList()
            }); ;
        }

        private IEnumerable<ComunicadoDto> MapearParaDto(IEnumerable<Comunicado> items)
        {
            var comunicadosDistintos = items.Select(s => new { s.Id, s.Titulo, s.Descricao, s.DataEnvio, s.DataExpiracao }).Distinct();

            return comunicadosDistintos.Select(s => new ComunicadoDto
            {
                Id = s.Id,
                Titulo = s.Titulo,
                DataEnvio = s.DataEnvio,
                DataExpiracao = s.DataExpiracao,
                Descricao = s.Descricao,
                Grupos = items.Where(w => w.Id == s.Id)
                        .Select(x => x.Grupos
                                      .Select(g =>
                                        new GrupoComunicacaoDto
                                        {
                                            Id = g.Id,
                                            Nome = g.Nome
                                        })
                                      .FirstOrDefault()
                                )
                        .ToList()
            });
        }

        private PaginacaoResultadoDto<ComunicadoDto> MapearParaDtoPaginado(PaginacaoResultadoDto<Comunicado> comunicado)
        {
            return new PaginacaoResultadoDto<ComunicadoDto>
            {
                Items = MapearParaDto(comunicado.Items),
                TotalPaginas = comunicado.TotalPaginas,
                TotalRegistros = comunicado.TotalRegistros
            };
        }

        private ComunicadoCompletoDto MapearPorIdParaDto(IEnumerable<ComunicadoResultadoDto> comunicado)
        {
            return ConverterParaDto(comunicado).FirstOrDefault();
        }
    }
}