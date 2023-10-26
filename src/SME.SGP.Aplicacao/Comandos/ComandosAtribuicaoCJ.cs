using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAtribuicaoCJ : IComandosAtribuicaoCJ
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IMediator mediator;
        private readonly IServicoAtribuicaoCJ servicoAtribuicaoCJ;
        private readonly IServicoUsuario servicoUsuario;

        public ComandosAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IServicoAtribuicaoCJ servicoAtribuicaoCJ, IServicoUsuario servicoUsuario, IMediator mediator)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.servicoAtribuicaoCJ = servicoAtribuicaoCJ ?? throw new ArgumentNullException(nameof(servicoAtribuicaoCJ));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Salvar(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto)
        {
            var atribuicoesAtuais = await repositorioAtribuicaoCJ.ObterPorFiltros(atribuicaoCJPersistenciaDto.Modalidade, atribuicaoCJPersistenciaDto.TurmaId,
               atribuicaoCJPersistenciaDto.UeId, 0, atribuicaoCJPersistenciaDto.UsuarioRf, string.Empty, null);

            bool atribuiuCj = false;

            var professorValidoNoEol = await mediator.Send(new ValidarProfessorEOLQuery(atribuicaoCJPersistenciaDto.UsuarioRf));
            if (!professorValidoNoEol)
                throw new NegocioException("Este professor não é válido para ser CJ.");

            var professoresTitularesDisciplinasEol = await mediator.Send(new ObterProfessoresTitularesDisciplinasEolQuery(atribuicaoCJPersistenciaDto.TurmaId));

            foreach (var atribuicaoDto in atribuicaoCJPersistenciaDto.Disciplinas)
            {
                var atribuicao = TransformaDtoEmEntidade(atribuicaoCJPersistenciaDto, atribuicaoDto);

                await servicoAtribuicaoCJ.Salvar(atribuicao, professoresTitularesDisciplinasEol, atribuicoesAtuais);

                Guid perfilCJ = atribuicao.Modalidade == Modalidade.EducacaoInfantil ? Perfis.PERFIL_CJ_INFANTIL : Perfis.PERFIL_CJ;

                atribuiuCj = await AtribuirPerfilCJ(atribuicaoCJPersistenciaDto, perfilCJ, atribuiuCj);

                await PublicarAtribuicaoNoGoogleClassroomApiAsync(atribuicao);
            }
        }

        private async Task<bool> AtribuirPerfilCJ(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto, Guid perfil, bool atribuiuCj)
        {
            if (atribuiuCj)
                return atribuiuCj;

            await mediator.Send(new AtribuirPerfilCommand(atribuicaoCJPersistenciaDto.UsuarioRf, perfil));

            servicoUsuario.RemoverPerfisUsuarioAtual();

            return true;
        }

        private AtribuicaoCJ TransformaDtoEmEntidade(AtribuicaoCJPersistenciaDto dto, AtribuicaoCJPersistenciaItemDto itemDto)
        {
            return new AtribuicaoCJ()
            {
                DreId = dto.DreId,
                Modalidade = dto.Modalidade,
                ProfessorRf = dto.UsuarioRf,
                Substituir = itemDto.Substituir,
                TurmaId = dto.TurmaId,
                UeId = dto.UeId,
                DisciplinaId = itemDto.DisciplinaId
            };
        }

        private async Task PublicarAtribuicaoNoGoogleClassroomApiAsync(AtribuicaoCJ atribuicaoCJ)
        {
            try
            {
                if (!long.TryParse(atribuicaoCJ.ProfessorRf, out var rf))
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível publicar a atribuição CJ no Google Classroom Api. O RF informado é inválido.", LogNivel.Negocio, LogContexto.CJ));
                    return;
                }

                if (!long.TryParse(atribuicaoCJ.TurmaId, out var turmaId))
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Não foi possível publicar a atribuição CJ no Google Classroom Api. A turma informada é inválida.", LogNivel.Negocio, LogContexto.CJ));
                    return;
                }

                var dto = new AtribuicaoCJGoogleClassroomApiDto(rf, turmaId, atribuicaoCJ.DisciplinaId);

                var publicacaoConcluida = atribuicaoCJ.Substituir
                    ? await mediator.Send(new PublicarFilaGoogleClassroomCommand(RotasRabbitSgpGoogleClassroomApi.FilaProfessorCursoIncluir, dto))
                    : await mediator.Send(new PublicarFilaGoogleClassroomCommand(RotasRabbitSgpGoogleClassroomApi.FilaProfessorCursoRemover, dto));
                if (!publicacaoConcluida)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível publicar na fila {RotasRabbitSgpGoogleClassroomApi.FilaProfessorCursoIncluir}.", LogNivel.Negocio, LogContexto.CJ, "Google Classroom Api"));
                }
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao publicar fila de atribuição de CJ Google Classroom.", LogNivel.Negocio, LogContexto.CJ, ex.Message));
            }
        }
    }
}