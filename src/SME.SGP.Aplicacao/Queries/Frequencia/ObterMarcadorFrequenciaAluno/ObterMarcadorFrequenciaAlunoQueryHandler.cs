using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Frequencia.ObterMarcadorFrequenciaAluno
{
    public class ObterMarcadorFrequenciaAlunoQueryHandler : IRequestHandler<ObterMarcadorFrequenciaAlunoQuery, MarcadorFrequenciaDto>
    {
        private const int DiasDesdeInicioBimestreParaMarcadorNovo = 15;

        public Task<MarcadorFrequenciaDto> Handle(ObterMarcadorFrequenciaAlunoQuery request, CancellationToken cancellationToken)
        {
            MarcadorFrequenciaDto marcador = null;

            var ehInfantil = request.Modalidade == Modalidade.InfantilPreEscola;
            var dataSituacao = $"{request.Aluno.DataSituacao.Day}/{request.Aluno.DataSituacao.Month}/{request.Aluno.DataSituacao.Year}";

            switch (request.Aluno.CodigoSituacaoMatricula)
            {
                case SituacaoMatriculaAluno.Ativo:
                    if ((request.Aluno.DataSituacao > request.PeriodoEscolar.PeriodoInicio) && (request.Aluno.DataSituacao.AddDays(DiasDesdeInicioBimestreParaMarcadorNovo) >= DateTime.Now.Date))
                        marcador = new MarcadorFrequenciaDto()
                        {
                            Tipo = TipoMarcadorFrequencia.Novo,
                            Descricao = $"{(ehInfantil ? "Criança Nova" : "Estudante Novo")}: Data da matrícula {dataSituacao}"
                        };
                    break;

                case SituacaoMatriculaAluno.Transferido:
                    var detalheEscola = request.Aluno.Transferencia_Interna ?
                                        $"para escola {request.Aluno.EscolaTransferencia} e turma {request.Aluno.TurmaTransferencia}" :
                                        "para outras redes";

                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Transferido,
                        Descricao = $"{(ehInfantil ? "Criança Transferida" : "Estudante Transferido")}: {detalheEscola} em {dataSituacao}"
                    };

                    break;

                case SituacaoMatriculaAluno.RemanejadoSaida:
                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Remanejado,
                        Descricao = $"{(ehInfantil ? "Criança Remanejada" : "Estudante Remanejado")}: turma {request.Aluno.TurmaRemanejamento} em {dataSituacao}"
                    };

                    break;

                case SituacaoMatriculaAluno.Desistente:
                case SituacaoMatriculaAluno.VinculoIndevido:
                case SituacaoMatriculaAluno.Falecido:
                case SituacaoMatriculaAluno.NaoCompareceu:
                case SituacaoMatriculaAluno.Deslocamento:
                case SituacaoMatriculaAluno.Cessado:
                case SituacaoMatriculaAluno.ReclassificadoSaida:
                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Inativo,
                        Descricao = $"{(ehInfantil ? "Criança Inativa" : "Estudante Inativo")} em {dataSituacao}"
                    };

                    break;

                default:
                    break;
            }

            return Task.FromResult(marcador);
        }
    }
}