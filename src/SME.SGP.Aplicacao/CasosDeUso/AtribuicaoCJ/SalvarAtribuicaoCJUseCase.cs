using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtribuicaoCJUseCase : AbstractUseCase, ISalvarAtribuicaoCJUseCase
    {
        public SalvarAtribuicaoCJUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto)
        {
            var anoLetivo = int.Parse(atribuicaoCJPersistenciaDto.AnoLetivo);

            var atribuicoesAtuais = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(atribuicaoCJPersistenciaDto.Modalidade, atribuicaoCJPersistenciaDto.TurmaId,
              atribuicaoCJPersistenciaDto.UeId, 0, atribuicaoCJPersistenciaDto.UsuarioRf, string.Empty, null, "", null, anoLetivo));

            bool atribuiuCj = false;

            await RemoverDisciplinasCache(atribuicaoCJPersistenciaDto);

            var professorValidoNoEol = await mediator.Send(new ValidarProfessorEOLQuery(atribuicaoCJPersistenciaDto.UsuarioRf));
            if (!professorValidoNoEol)
                throw new NegocioException("Este professor não é válido para ser CJ.");

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(atribuicaoCJPersistenciaDto.TurmaId));

            var professoresTitularesDisciplinasEol = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(turma.Id));

            foreach (var atribuicaoDto in atribuicaoCJPersistenciaDto.Disciplinas)
            {
                var atribuicao = TransformaDtoEmEntidade(atribuicaoCJPersistenciaDto, atribuicaoDto);

                var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

                await mediator.Send(new InserirAtribuicaoCJCommand(atribuicao, professoresTitularesDisciplinasEol, atribuicoesAtuais, usuario));

                Guid perfilCJ = atribuicao.Modalidade == Modalidade.InfantilPreEscola ? Perfis.PERFIL_CJ_INFANTIL : Perfis.PERFIL_CJ;

                atribuiuCj = await AtribuirPerfilCJ(atribuicaoCJPersistenciaDto, perfilCJ, atribuiuCj);

                if(DateTime.Now.Year != anoLetivo)
                    await PublicarAtribuicaoNoGoogleClassroomApiAsync(atribuicao);
            }
        }

        private async Task<bool> AtribuirPerfilCJ(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto, Guid perfil, bool atribuiuCj)
        {
            if (atribuiuCj)
                return atribuiuCj;

            await mediator.Send(new AtribuirPerfilCommand(atribuicaoCJPersistenciaDto.UsuarioRf, perfil));

            var codigoRf = await mediator.Send(new ObterUsuarioLogadoRFQuery());
            await mediator.Send(new RemoverPerfisUsuarioAtualCommand(codigoRf));

            return true;
        }

        private async Task RemoverDisciplinasCache(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto)
        {
            var chaveCache = $"Disciplinas-{atribuicaoCJPersistenciaDto.TurmaId}-{atribuicaoCJPersistenciaDto.UsuarioRf}--{Perfis.PERFIL_CJ}";
            await mediator.Send(new RemoverChaveCacheCommand(chaveCache));
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
                    SentrySdk.CaptureMessage("Não foi possível publicar a atribuição CJ no Google Classroom Api. O RF informado é inválido.");
                    return;
                }

                if (!long.TryParse(atribuicaoCJ.TurmaId, out var turmaId))
                {
                    SentrySdk.CaptureMessage("Não foi possível publicar a atribuição CJ no Google Classroom Api. A turma informada é inválida.");
                    return;
                }

                var dto = new AtribuicaoCJGoogleClassroomApiDto(rf, turmaId, atribuicaoCJ.DisciplinaId);

                var publicacaoConcluida = atribuicaoCJ.Substituir
                    ? await mediator.Send(new PublicarFilaGoogleClassroomCommand(RotasRabbitSgpGoogleClassroomApi.FilaProfessorCursoIncluir, dto))
                    : await mediator.Send(new PublicarFilaGoogleClassroomCommand(RotasRabbitSgpGoogleClassroomApi.FilaProfessorCursoRemover, dto));
                if (!publicacaoConcluida)
                {
                    SentrySdk.AddBreadcrumb("Atribuição CJ", "Google Classroom Api");
                    SentrySdk.CaptureMessage($"Não foi possível publicar na fila {RotasRabbitSgpGoogleClassroomApi.FilaProfessorCursoIncluir}."); ;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.AddBreadcrumb("Atribuição CJ", "Google Classroom Api");
                SentrySdk.CaptureException(ex);
            }
        }
    }
}
