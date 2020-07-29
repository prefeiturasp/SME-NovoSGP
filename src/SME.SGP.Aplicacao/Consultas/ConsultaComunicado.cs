using SME.SGP.Aplicacao.Integracoes;
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
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultaComunicado : ConsultasBase, IConsultaComunicado
    {
        private readonly IRepositorioComunicado repositorio;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IRepositorioComunicadoTurma repositorioComunicadoTurma;
        private readonly IRepositorioComunicadoAluno repositorioComunicadoAluno;
        private readonly IRepositorioComunicadoGrupo repositorioComunicadoGrupo;
        private readonly IConsultaGrupoComunicacao consultaGrupoComunicacao;
        private readonly IServicoEol servicoEol;
        private const string Todas = "todas";

        public ConsultaComunicado(
            IRepositorioComunicado repositorio,
            IContextoAplicacao contextoAplicacao,
            IServicoUsuario servicoUsuario,
            IConsultasAbrangencia consultasAbrangencia,
            IRepositorioComunicadoTurma repositorioComunicadoTurma,
            IRepositorioComunicadoAluno repositorioComunicadoAluno,
            IRepositorioComunicadoGrupo repositorioComunicadoGrupo,
            IConsultaGrupoComunicacao consultaGrupoComunicacao,
            IServicoEol servicoEol) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
            this.repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
            this.repositorioComunicadoGrupo = repositorioComunicadoGrupo ?? throw new ArgumentNullException(nameof(repositorioComunicadoGrupo));
            this.consultaGrupoComunicacao = consultaGrupoComunicacao ?? throw new ArgumentNullException(nameof(consultaGrupoComunicacao));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
        }

        public async Task<ComunicadoCompletoDto> BuscarPorIdAsync(long id)
        {
            var comunicado = await repositorio.ObterPorIdAsync(id);

            if (comunicado.Excluido)
                throw new NegocioException("Não é possivel acessar um registro excluido");

            comunicado.Alunos = (await repositorioComunicadoAluno.ObterPorComunicado(comunicado.Id)).ToList();

            comunicado.Turmas = (await repositorioComunicadoTurma.ObterPorComunicado(comunicado.Id)).ToList();

            comunicado.Grupos = (await repositorioComunicadoGrupo.ObterPorComunicado(comunicado.Id)).ToList();

            var dto = (ComunicadoCompletoDto)comunicado;

            dto.Grupos = (await consultaGrupoComunicacao.Listar(comunicado.Grupos.Select(x => x.GrupoComunicadoId))).ToList();

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

        public async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorTurma(string codigoTurma, int anoLetivo)
        {
            var alunos = await servicoEol.ObterAlunosPorTurma(codigoTurma);

            if (alunos == null || !alunos.Any())
                throw new NegocioException($"Não foi encontrado alunos para a turma {codigoTurma} e ano letivo {anoLetivo}");

            return alunos.Where(x => x.DeveMostrarNaChamada(DateTime.Now)).OrderBy(x => x.NumeroAlunoChamada);
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
                Modalidade = filtroDto.Modalidade,
                Titulo = filtroDto.Titulo,
                Turmas = filtroDto.Turmas?.Select(x => new ComunicadoTurmaDto { CodigoTurma = x }),
                Semestre = filtroDto.Semestre
            };
        }

        private async Task ValidarAbrangenciaUsuario(ComunicadoDto filtroDto)
        {
            var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            if ((filtroDto.CodigoDre?.Equals(Todas) ?? true) && !usuarioLogado.EhPerfilSME())
                throw new NegocioException("Apenas usuários SME podem visualizar comunicados de todas as DREs");

            if ((filtroDto.CodigoUe?.Equals(Todas) ?? true) && !(usuarioLogado.EhPerfilDRE() || usuarioLogado.EhPerfilSME()))
                throw new NegocioException("Apenas usuários SME e DRE podem visualizar comunicados de todas as Escolas");

            if (usuarioLogado.EhPerfilDRE() && (!filtroDto.CodigoDre?.Equals(Todas) ?? false))
                await ValidarAbrangenciaDre(filtroDto);

            if (usuarioLogado.EhPerfilUE() && (!filtroDto.CodigoUe?.Equals(Todas) ?? false))
                await ValidarAbrangenciaUE(filtroDto);
        }

        private async Task ValidarAbrangenciaTurma(ComunicadoDto filtroDto)
        {
            foreach (var turma in filtroDto.Turmas)
            {
                var abrangenciaTurmas = await consultasAbrangencia.ObterAbrangenciaTurma(turma.CodigoTurma);

                if (abrangenciaTurmas == null)
                    throw new NegocioException($"Usuário não possui permissão para visualizar comunicados da Turma com codigo {turma.CodigoTurma}");
            }
        }

        private async Task ValidarAbrangenciaUE(ComunicadoDto filtroDto)
        {
            var abrangenciaUes = await consultasAbrangencia.ObterUes(filtroDto.CodigoDre, null);

            var ue = abrangenciaUes.FirstOrDefault(x => x.Codigo.Equals(filtroDto.CodigoUe));

            if (ue == null)
                throw new NegocioException($"Usuário não possui permissão para visualizar comunicados da UE com codigo {filtroDto.CodigoUe}");

            if (filtroDto.Turmas != null && filtroDto.Turmas.Any())
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