using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComunicadoPorIdQueryHandler : IRequestHandler<ObterComunicadoPorIdQuery, ComunicadoCompletoDto>
    {
        private const string TODAS = "todas";
        private readonly IRepositorioComunicado _repositorioComunicado;        
        private readonly IRepositorioComunicadoTurma _repositorioComunicadoTurma;
        private readonly IRepositorioComunicadoAluno _repositorioComunicadoAluno;
        private readonly IConsultasAbrangencia _consultasAbrangencia;        

        public ObterComunicadoPorIdQueryHandler(
              IRepositorioComunicado repositorioComunicado            
            , IRepositorioComunicadoTurma repositorioComunicadoTurma
            , IRepositorioComunicadoAluno repositorioComunicadoAluno
            , IConsultasAbrangencia consultasAbrangencia)
        {
            this._repositorioComunicado = repositorioComunicado ?? throw new ArgumentNullException(nameof(repositorioComunicado));            
            this._repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
            this._repositorioComunicadoAluno = repositorioComunicadoAluno ?? throw new ArgumentNullException(nameof(repositorioComunicadoAluno));
            this._consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));            
        }

        public async Task<ComunicadoCompletoDto> Handle(ObterComunicadoPorIdQuery request, CancellationToken cancellationToken)
        {
            var comunicado = await _repositorioComunicado.ObterPorIdAsync(request.Id);

            if (comunicado.Excluido)
                throw new NegocioException("Não é possivel acessar um registro excluido");

            comunicado.Alunos = (await _repositorioComunicadoAluno.ObterPorComunicado(comunicado.Id)).ToList();

            comunicado.Turmas = (await _repositorioComunicadoTurma.ObterPorComunicado(comunicado.Id)).ToList();            

            var dto = (ComunicadoCompletoDto)comunicado;            

            await ValidarAbrangenciaUsuario(dto);

            return dto;
        }

        private async Task ValidarAbrangenciaUsuario(ComunicadoDto filtroDto)
        {
            //var usuarioLogado = await servicoUsuario.ObterUsuarioLogado();

            //if ((filtroDto.CodigoDre?.Equals(TODAS) ?? true) && !usuarioLogado.EhPerfilSME())
            //    throw new NegocioException("Apenas usuários SME podem visualizar comunicados de todas as DREs");

            //if ((filtroDto.CodigoUe?.Equals(TODAS) ?? true) && !(usuarioLogado.EhPerfilDRE() || usuarioLogado.EhPerfilSME()))
            //    throw new NegocioException("Apenas usuários SME e DRE podem visualizar comunicados de todas as Escolas");

            //if (usuarioLogado.EhPerfilDRE() && (!filtroDto.CodigoDre?.Equals(TODAS) ?? false))
            //    await ValidarAbrangenciaDre(filtroDto);

            //if (usuarioLogado.EhPerfilUE() && (!filtroDto.CodigoUe?.Equals(TodTODASas) ?? false))
            //    await ValidarAbrangenciaUE(filtroDto);
        }

        private async Task ValidarAbrangenciaUE(ComunicadoCompletoDto comunicadoCompletoDto)
        {
            var abrangenciaUes = await _consultasAbrangencia.ObterUes(comunicadoCompletoDto.CodigoDre, null);

            var ue = abrangenciaUes.FirstOrDefault(x => x.Codigo.Equals(comunicadoCompletoDto.CodigoUe));

            if (ue == null)
                throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a UE com codigo {comunicadoCompletoDto.CodigoUe}");

            if (comunicadoCompletoDto.Turmas != null && comunicadoCompletoDto.Turmas.Any())
                await ValidarAbrangenciaTurma(comunicadoCompletoDto);
        }

        private async Task ValidarAbrangenciaDre(ComunicadoCompletoDto comunicadoCompletoDto)
        {
            var abrangenciaDres = await _consultasAbrangencia.ObterDres(null);

            var dre = abrangenciaDres.FirstOrDefault(x => x.Codigo.Equals(comunicadoCompletoDto.CodigoDre));

            if (dre == null)
                throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a DRE com codigo {comunicadoCompletoDto.CodigoDre}");
        }

        private async Task ValidarAbrangenciaTurma(ComunicadoCompletoDto comunicadoCompletoDto)
        {
            foreach (var turma in comunicadoCompletoDto.Turmas)
            {
                var abrangenciaTurmas = await _consultasAbrangencia.ObterAbrangenciaTurma(turma.CodigoTurma);

                if (abrangenciaTurmas == null)
                    throw new NegocioException($"Usuário não possui permissão para enviar comunicados para a Turma com codigo {turma}");
            }
        }
    }
}
