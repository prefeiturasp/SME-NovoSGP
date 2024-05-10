using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ConsultaComunicado : ConsultasBase, IConsultaComunicado
    {
        private readonly IRepositorioComunicado repositorio;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IRepositorioComunicadoTurma repositorioComunicadoTurma;
        private readonly IRepositorioComunicadoAluno repositorioComunicadoAluno;
        private readonly IMediator mediator;
        private const string Todas = "todas";

        public ConsultaComunicado(
            IRepositorioComunicado repositorio,
            IContextoAplicacao contextoAplicacao,
            IServicoUsuario servicoUsuario,
            IConsultasAbrangencia consultasAbrangencia,
            IRepositorioComunicadoTurma repositorioComunicadoTurma,
            IRepositorioComunicadoAluno repositorioComunicadoAluno,
            IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
            this.repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ComunicadoCompletoDto> BuscarPorIdAsync(long id)
        {
            var comunicado = await repositorio.ObterPorIdAsync(id);

            if (comunicado.Excluido)
                throw new NegocioException("Não é possivel acessar um registro excluido");

            comunicado.Alunos = (await repositorioComunicadoAluno.ObterPorComunicado(comunicado.Id)).ToList();

            comunicado.Turmas = (await repositorioComunicadoTurma.ObterPorComunicado(comunicado.Id)).ToList();

            var dto = (ComunicadoCompletoDto)comunicado;

            await ValidarAbrangenciaUsuario(dto);

            return dto;
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
                var abrangenciaTurmas = await mediator.Send(new ObterAbrangenciaPorTurmaEConsideraHistoricoQuery(turma.CodigoTurma));

                if (abrangenciaTurmas.EhNulo())
                    throw new NegocioException($"Usuário não possui permissão para visualizar comunicados da Turma com codigo {turma.CodigoTurma}");
            }
        }

        private async Task ValidarAbrangenciaUE(ComunicadoDto filtroDto)
        {
            var abrangenciaUes = await consultasAbrangencia.ObterUes(filtroDto.CodigoDre, null);

            var ue = abrangenciaUes.FirstOrDefault(x => x.Codigo.Equals(filtroDto.CodigoUe));

            if (ue.EhNulo())
                throw new NegocioException($"Usuário não possui permissão para visualizar comunicados da UE com codigo {filtroDto.CodigoUe}");

            if (filtroDto.Turmas.NaoEhNulo() && filtroDto.Turmas.Any())
                await ValidarAbrangenciaTurma(filtroDto);
        }

        private async Task ValidarAbrangenciaDre(ComunicadoDto filtroDto)
        {
            var abrangenciaDres = await consultasAbrangencia.ObterDres(null);

            var dre = abrangenciaDres.FirstOrDefault(x => x.Codigo.Equals(filtroDto.CodigoDre));

            if (dre.EhNulo())
                throw new NegocioException($"Usuário não possui permissão para visualizar comunicados da DRE com codigo {filtroDto.CodigoDre}");
        }
    }
}