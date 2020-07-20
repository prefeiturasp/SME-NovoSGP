using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
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
        private readonly IServicoUsuario servicoUsuario;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private const string Todas = "todas";

        public ConsultaComunicado(
            IRepositorioComunicado repositorio,
            IContextoAplicacao contextoAplicacao,
            IServicoUsuario servicoUsuario,
            IConsultasAbrangencia consultasAbrangencia) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
        }

        public async Task<ComunicadoCompletoDto> BuscarPorIdAsync(long id)
        {
            var comunicado = await repositorio.ObterResultadoPorComunicadoIdAsync(id);

            var dto = MapearPorIdParaDto(comunicado);

            await ValidarAbrangenciaUsuario(dto);

            return dto;
        }

        public async Task<PaginacaoResultadoDto<ComunicadoDto>> ListarPaginado(FiltroComunicadoDto filtro)
        {
            var validacao = await ValidarAbrangenciaListagem(filtro);

            if (!validacao)
                return new PaginacaoResultadoDto<ComunicadoDto>();

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
                            s.Semestre,
                            s.Modalidade,
                            s.TipoComunicado,
                            s.CodigoDre,
                            s.CodigoUe,
                            s.AnoLetivo,
                            s.Turma,
                            s.Alunos,
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
                AnoLetivo = c.AnoLetivo,
                CodigoDre = c.CodigoDre,
                CodigoUe = c.CodigoUe,
                Semestre = c.Semestre,                
                Turma = c.Turma,
                Alunos = c.Alunos,
                Modalidade = c.Modalidade,
                TipoComunicado = c.TipoComunicado,
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
            var comunicadosDistintos = items.Select(s => new { s.Id, s.Titulo, s.Descricao, s.DataEnvio, s.DataExpiracao, s.TipoComunicado, s.AlunoEspecificado, s.Alunos, s.AnoLetivo, s.CodigoDre, s.CodigoUe, s.Semestre, s.Modalidade, s.Turma }).Distinct();

            return comunicadosDistintos.Select(s => new ComunicadoDto
            {
                Id = s.Id,
                Titulo = s.Titulo,
                DataEnvio = s.DataEnvio,
                DataExpiracao = s.DataExpiracao,
                Descricao = s.Descricao,
                TipoComunicado = s.TipoComunicado,
                CodigoUe = s.CodigoUe,
                Turma = s.Turma,
                Modalidade = s.Modalidade ?? 0,
                Semestre = s.Semestre ?? 0,
                AnoLetivo = s.AnoLetivo,
                CodigoDre = s.CodigoDre,
                Alunos = s.Alunos.Select(x => (ComunicadoAlunoDto)x),
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

        private async Task<bool> ValidarAbrangenciaListagem(FiltroComunicadoDto filtroDto)
        {
            try
            {
                ComunicadoDto comunicado = MapearFiltroDtoValidacao(filtroDto);

                await ValidarAbrangenciaUsuario(comunicado);

                return true;
            }
            catch (NegocioException)
            {
                return false;
            }
        }

        private static ComunicadoDto MapearFiltroDtoValidacao(FiltroComunicadoDto filtroDto)
        {
            return new ComunicadoDto
            {
                AnoLetivo = filtroDto.AnoLetivo,
                CodigoDre = filtroDto.CodigoDre,
                CodigoUe = filtroDto.CodigoUe,
                DataEnvio = filtroDto.DataEnvio ?? DateTime.Now,
                DataExpiracao = filtroDto.DataExpiracao,
                Modalidade = filtroDto.Modalidade,
                Titulo = filtroDto.Titulo,
                Turma = filtroDto.Turma,
                Semestre = filtroDto.Semestre
            };
        }

        private async Task ValidarAbrangenciaUsuario(ComunicadoDto filtroDto)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            if (filtroDto.CodigoDre.Equals(Todas) && !usuarioLogado.EhPerfilSME())
                throw new NegocioException("Apenas usuários SME podem visualizar comunicados de todas as DREs");

            if (filtroDto.CodigoUe.Equals(Todas) && !(usuarioLogado.EhPerfilDRE() || usuarioLogado.EhPerfilSME()))
                throw new NegocioException("Apenas usuários SME e DRE podem visualizar comunicados de todas as Escolas");

            if (usuarioLogado.EhPerfilDRE() && !filtroDto.CodigoDre.Equals(Todas))
                await ValidarAbrangenciaDre(filtroDto);

            if (usuarioLogado.EhPerfilUE() && !filtroDto.CodigoUe.Equals(Todas))
                await ValidarAbrangenciaUE(filtroDto);
        }

        private async Task ValidarAbrangenciaTurma(ComunicadoDto filtroDto)
        {
            var abrangenciaTurmas = await consultasAbrangencia.ObterAbrangenciaTurma(filtroDto.Turma);

            if (abrangenciaTurmas == null)
                throw new NegocioException($"Usuário não possui permissão para visualizar comunicados da Turma com codigo {filtroDto.Turma}");
        }

        private async Task ValidarAbrangenciaUE(ComunicadoDto filtroDto)
        {
            var abrangenciaUes = await consultasAbrangencia.ObterUes(filtroDto.CodigoDre, null);

            var ue = abrangenciaUes.FirstOrDefault(x => x.Codigo.Equals(filtroDto.CodigoUe));

            if (ue == null)
                throw new NegocioException($"Usuário não possui permissão para visualizar comunicados da UE com codigo {filtroDto.CodigoUe}");

            if (!filtroDto.Turma.Equals(Todas))
                await ValidarAbrangenciaTurma(filtroDto);
        }

        private async Task ValidarAbrangenciaDre(ComunicadoDto filtroDto)
        {
            var abrangenciaDres = await consultasAbrangencia.ObterDres(null);

            var dre = abrangenciaDres.FirstOrDefault(x => x.Codigo.Equals(filtroDto.CodigoDre));

            if (dre == null)
                throw new NegocioException($"Usuário não possui permissão para visualizar comunicados da DRE com codigo {filtroDto.CodigoDre}");
        }
    }
}