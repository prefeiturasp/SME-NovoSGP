using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMarcadorAlunoQueryHandler : IRequestHandler<ObterMarcadorAlunoQuery, MarcadorFrequenciaDto>
    {
        public async Task<MarcadorFrequenciaDto> Handle(ObterMarcadorAlunoQuery request, CancellationToken cancellationToken)
        {
            MarcadorFrequenciaDto marcador = null;

            string dataSituacao = $"{request.Aluno.DataSituacao.Day}/{request.Aluno.DataSituacao.Month}/{request.Aluno.DataSituacao.Year}";

            switch (request.Aluno.CodigoSituacaoMatricula)
            {
                case SituacaoMatriculaAluno.Ativo:
                    // Macador "Novo" durante 15 dias se iniciou depois do inicio do bimestre
                    if ((request.Aluno.DataSituacao > request.DataReferencia) && (request.Aluno.DataSituacao.AddDays(15) >= DateTime.Now.Date))
                        marcador = new MarcadorFrequenciaDto()
                        {
                            Tipo = TipoMarcadorFrequencia.Novo,
                            Descricao = $"{(request.EhInfantil ? "Criança Nova" : "Estudante Novo")}: Data da matrícula {dataSituacao}"
                        };
                    break;

                case SituacaoMatriculaAluno.Transferido:
                    var detalheEscola = request.Aluno.Transferencia_Interna ?
                                        $"para escola {request.Aluno.EscolaTransferencia} e turma {request.Aluno.TurmaTransferencia}" :
                                        "para outras redes";

                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Transferido,
                        Descricao = $"{(request.EhInfantil ? "Criança Transferida" : "Estudante Transferido")}: {detalheEscola} em {dataSituacao}"
                    };

                    break;

                case SituacaoMatriculaAluno.RemanejadoSaida:
                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Remanejado,
                        Descricao = $"{(request.EhInfantil ? "Criança Remanejada" : "Estudante Remanejado")}: turma {request.Aluno.TurmaRemanejamento} em {dataSituacao}"
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
                        Descricao = $"{(request.EhInfantil ? "Criança Inativa" : "Estudante Inativo")} em {dataSituacao}"
                    };

                    break;

                default:
                    break;
            }

            return marcador;
        }
    }
}
