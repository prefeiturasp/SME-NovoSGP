using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAtribuicaoCJUseCase : AbstractUseCase, ISalvarAtribuicaoCJUseCase
    {
        public SalvarAtribuicaoCJUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar(AtribuicaoCJPersistenciaDto persistenciaDto)
        {
            var anoLetivo = int.Parse(persistenciaDto.AnoLetivo);

            var atribuicoesAtuais = await mediator.Send(new ObterAtribuicoesPorTurmaEProfessorQuery(persistenciaDto.Modalidade, persistenciaDto.TurmaId,
                persistenciaDto.UeId, 0, persistenciaDto.UsuarioRf, string.Empty, null, "", null, anoLetivo));

            var atribuiuCj = false;

            await RemoverDisciplinasCache(persistenciaDto);
            await RemoverAtribuicaoAtivaCache(persistenciaDto.UsuarioRf);

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(persistenciaDto.TurmaId));

            var professoresTitularesDisciplinasEol = await mediator.Send(new ObterProfessoresTitularesPorTurmaIdQuery(turma.Id));
            var excluiAbrangencia = persistenciaDto.Disciplinas.All(a => a.Substituir == false);

            foreach (var atribuicaoDto in persistenciaDto.Disciplinas)
            {
                var atribuicao = TransformaDtoEmEntidade(persistenciaDto, atribuicaoDto);

                var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
                await mediator.Send(new InserirAtribuicaoCJCommand(atribuicao, professoresTitularesDisciplinasEol, atribuicoesAtuais, usuario, persistenciaDto.Historico,excluiAbrangencia));

                var perfilCJ = atribuicao.Modalidade == Modalidade.EducacaoInfantil ? Perfis.PERFIL_CJ_INFANTIL : Perfis.PERFIL_CJ;

                atribuiuCj = await AtribuirPerfilCJ(persistenciaDto, perfilCJ, atribuiuCj);

                if (DateTime.Now.Year == anoLetivo)
                    await PublicarAtribuicaoNoGoogleClassroomApiAsync(atribuicao);
            }
        }

        private async Task<bool> AtribuirPerfilCJ(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto, Guid perfil, bool atribuiuCj)
        {
            if (atribuiuCj)
                return atribuiuCj;

            await mediator.Send(new AtribuirPerfilCommand(atribuicaoCJPersistenciaDto.UsuarioRf, perfil));

            var codigoRf = await mediator.Send(ObterUsuarioLogadoRFQuery.Instance);
            await mediator.Send(new RemoverPerfisUsuarioAtualCommand(codigoRf));

            return true;
        }

        private async Task RemoverDisciplinasCache(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto)
        {
            
            var chaveCache = string.Format(NomeChaveCache.COMPONENTES_TURMA_PROFESSOR_PERFIL, atribuicaoCJPersistenciaDto.TurmaId, atribuicaoCJPersistenciaDto.UsuarioRf, Perfis.PERFIL_CJ);
            await mediator.Send(new RemoverChaveCacheCommand(chaveCache));
        }

        private async Task RemoverAtribuicaoAtivaCache(string codigoRf)
        {
            var chaveCache = string.Format(NomeChaveCache.ATRIBUICAO_CJ_PROFESSOR, codigoRf);
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
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível publicar na fila {RotasRabbitSgpGoogleClassroomApi.FilaProfessorCursoIncluir}.", LogNivel.Negocio, LogContexto.CJ));
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao publicar na fila do google  - PublicarAtribuicaoNoGoogleClassroomApiAsync", LogNivel.Critico, LogContexto.CJ, ex.Message));
            }
        }
    }
}