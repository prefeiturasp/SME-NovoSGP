using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
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

                await mediator.Send(new InserirAtribuicaoCJCommand(atribuicao, professoresTitularesDisciplinasEol, atribuicoesAtuais, usuario, atribuicaoCJPersistenciaDto.Historico));

                Guid perfilCJ = atribuicao.Modalidade == Modalidade.EducacaoInfantil ? Perfis.PERFIL_CJ_INFANTIL : Perfis.PERFIL_CJ;

                atribuiuCj = await AtribuirPerfilCJ(atribuicaoCJPersistenciaDto, perfilCJ, atribuiuCj);

                if (DateTime.Now.Year == anoLetivo)
                    await PublicarAtribuicaoNoGoogleClassroomApiAsync(atribuicao);
            }
        }
        private async Task RemoverAtribuicaoAtual(AtribuicaoCJPersistenciaDto dto)
        {
            await mediator.Send(new RemoverAtribuicaoCJCommand(dto.DreId, dto.UeId, dto.TurmaId, dto.UsuarioRf));
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
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível publicar na fila {RotasRabbitSgpGoogleClassroomApi.FilaProfessorCursoIncluir}.", LogNivel.Negocio, LogContexto.CJ));                    
                }
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao publicar na fila do google  - PublicarAtribuicaoNoGoogleClassroomApiAsync", LogNivel.Critico, LogContexto.CJ, ex.Message));                
            }
        }
    }
}
