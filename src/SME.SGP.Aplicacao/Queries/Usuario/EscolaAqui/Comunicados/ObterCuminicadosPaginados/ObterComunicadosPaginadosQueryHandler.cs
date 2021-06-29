using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadosPaginadosQueryHandler : IRequestHandler<ObterComunicadosPaginadosQuery, PaginacaoResultadoDto<ComunicadoDto>>
    {
        private const string TODAS = "todas";
        private readonly IContextoAplicacao _contextoAplicacao;
        private readonly IRepositorioComunicado _repositorioComunicado;
        private readonly IConsultasAbrangencia _consultasAbrangencia;
        private readonly IServicoUsuario _servicoUsuario;

        public ObterComunicadosPaginadosQueryHandler(
              IContextoAplicacao contextoAplicacao
            , IRepositorioComunicado repositorioComunicado
            , IConsultasAbrangencia consultasAbrangencia
            , IServicoUsuario servicoUsuario)
        {
            this._contextoAplicacao = contextoAplicacao ?? throw new ArgumentNullException(nameof(contextoAplicacao));
            this._repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));
            this._consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this._servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<PaginacaoResultadoDto<ComunicadoDto>> Handle(ObterComunicadosPaginadosQuery request, CancellationToken cancellationToken)
        {
            var filtro = new FiltroComunicadoDto
            {
                AnoLetivo = request.AnoLetivo,
                CodigoDre = request.CodigoDre,
                CodigoUe = request.CodigoUe,
                DataEnvio = request.DataEnvio,
                DataExpiracao = request.DataExpiracao,
                GruposId = request.GruposId,
                Modalidades = request.Modalidades,
                Semestre = request.Semestre,
                Titulo = request.Titulo,
                Turmas = request.Turmas
            };

            var validacao = await ValidarAbrangenciaListagem(filtro);
            if (!validacao)
                return new PaginacaoResultadoDto<ComunicadoDto>();

            var comunicados = await _repositorioComunicado.ListarPaginado(filtro, Paginacao);

            return MapearParaDtoPaginado(comunicados);
        }

        private PaginacaoResultadoDto<ComunicadoDto> MapearParaDtoPaginado(PaginacaoResultadoDto<Comunicado> comunicado)
        {
            var itens = new List<ComunicadoDto>();

            var retornoPaginado = new PaginacaoResultadoDto<ComunicadoDto>
            {
                Items = new List<ComunicadoDto>(),
                TotalPaginas = comunicado.TotalPaginas,
                TotalRegistros = comunicado.TotalRegistros
            };

            foreach (var item in comunicado.Items)
            {
                var comunicadoDto = itens.FirstOrDefault(x => x.Id == item.Id);

                if (comunicadoDto == null)
                    itens.Add((ComunicadoDto)item);
                else
                    comunicadoDto.Grupos.AddRange(item.GruposComunicacao.Select(x => new GrupoComunicacaoDto
                    {
                        Id = x.Id,
                        Nome = x.Nome
                    }));
            }

            retornoPaginado.Items = itens;

            return retornoPaginado;
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
                Modalidades = filtroDto.Modalidades,
                Titulo = filtroDto.Titulo,
                Turmas = filtroDto.Turmas?.Select(x => new ComunicadoTurmaDto { CodigoTurma = x }),
                Semestre = filtroDto.Semestre
            };
        }

        private async Task ValidarAbrangenciaUsuario(ComunicadoDto filtroDto)
        {
            var usuarioLogado = await _servicoUsuario.ObterUsuarioLogado();

            if ((filtroDto.CodigoDre?.Equals(TODAS) ?? true) && !usuarioLogado.EhPerfilSME())
                throw new NegocioException("Apenas usuários SME podem visualizar comunicados de todas as DREs");

            if ((filtroDto.CodigoUe?.Equals(TODAS) ?? true) && !(usuarioLogado.EhPerfilDRE() || usuarioLogado.EhPerfilSME()))
                throw new NegocioException("Apenas usuários SME e DRE podem visualizar comunicados de todas as Escolas");

            if (usuarioLogado.EhPerfilDRE() && (!filtroDto.CodigoDre?.Equals(TODAS) ?? false))
                await ValidarAbrangenciaDre(filtroDto);

            if (usuarioLogado.EhPerfilUE() && (!filtroDto.CodigoUe?.Equals(TODAS) ?? false))
                await ValidarAbrangenciaUE(filtroDto);
        }

        private async Task ValidarAbrangenciaTurma(ComunicadoDto filtroDto)
        {
            foreach (var turma in filtroDto.Turmas)
            {
                var abrangenciaTurmas = await _consultasAbrangencia.ObterAbrangenciaTurma(turma.CodigoTurma);

                if (abrangenciaTurmas == null)
                    throw new NegocioException($"Usuário não possui permissão para visualizar comunicados da Turma com codigo {turma.CodigoTurma}");
            }
        }

        private async Task ValidarAbrangenciaUE(ComunicadoDto filtroDto)
        {
            var abrangenciaUes = await _consultasAbrangencia.ObterUes(filtroDto.CodigoDre, null);

            var ue = abrangenciaUes.FirstOrDefault(x => x.Codigo.Equals(filtroDto.CodigoUe));

            if (ue == null)
                throw new NegocioException($"Usuário não possui permissão para visualizar comunicados da UE com codigo {filtroDto.CodigoUe}");

            if (filtroDto.Turmas != null && filtroDto.Turmas.Any())
                await ValidarAbrangenciaTurma(filtroDto);
        }

        private async Task ValidarAbrangenciaDre(ComunicadoDto filtroDto)
        {
            var abrangenciaDres = await _consultasAbrangencia.ObterDres(null);

            var dre = abrangenciaDres.FirstOrDefault(x => x.Codigo.Equals(filtroDto.CodigoDre));

            if (dre == null)
                throw new NegocioException($"Usuário não possui permissão para visualizar comunicados da DRE com codigo {filtroDto.CodigoDre}");
        }

        public Paginacao Paginacao
        {
            get
            {
                var numeroPaginaQueryString = _contextoAplicacao.ObterVariavel<string>("NumeroPagina");
                var numeroRegistrosQueryString = _contextoAplicacao.ObterVariavel<string>("NumeroRegistros");

                if (string.IsNullOrWhiteSpace(numeroPaginaQueryString) || string.IsNullOrWhiteSpace(numeroRegistrosQueryString))
                    return new Paginacao(0, 0);

                var numeroPagina = int.Parse(numeroPaginaQueryString);
                var numeroRegistros = int.Parse(numeroRegistrosQueryString);

                return new Paginacao(numeroPagina, numeroRegistros);
            }
        }
    }
}
