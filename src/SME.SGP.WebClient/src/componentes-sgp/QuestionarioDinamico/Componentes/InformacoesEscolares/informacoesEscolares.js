import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { erros } from '~/servicos';
import ServicoEstudante from '~/servicos/Paginas/Estudante/ServicoEstudante';
import AusenciasEstudante from './ausenciasEstudante';
import BtnExpandirAusenciaEstudante from './btnExpandirAusenciaEstudante';
import { TabelaColunasFixas } from './informacoesEscolares.css';
import ModalAnotacoesQuestionarioDinamico from './modalAnotacoesQuestionarioDinamico';

const InformacoesEscolares = props => {
  const [dados, setDados] = useState([]);
  const { codigoAluno, codigoTurma, anoLetivo } = props;

  const obterInformacoesEscolaresDoAluno = useCallback(async () => {
    const resposta = await ServicoEstudante.obterInformacoesEscolaresDoAluno(
      codigoAluno,
      codigoTurma
    ).catch(e => erros(e));

    if (resposta?.data) {
      setDados(resposta.data);
    } else {
      setDados([]);
    }
  }, [codigoAluno, codigoTurma]);

  useEffect(() => {
    obterInformacoesEscolaresDoAluno();
  }, [obterInformacoesEscolaresDoAluno]);

  return (
    <>
      <ModalAnotacoesQuestionarioDinamico />
      <TabelaColunasFixas>
        <div className="wrapper">
          <div className="header-fixo">
            <table className="table">
              <thead className="tabela-um-thead">
                <tr>
                  <th className="col-linha-um">
                    Indicativo de deficiência (EOL)
                  </th>
                  <th className="col-linha-um">Recursos utilizados (EOL)</th>
                  <th className="col-linha-um">Frequência Global</th>
                </tr>
              </thead>
              <tbody className="tabela-um-tbody">
                <tr>
                  <td className="col-valor-linha-um">
                    {dados.descricaoNecessidadeEspecial || '-'}
                  </td>
                  <td className="col-valor-linha-um">
                    {dados.descricaoRecurso || '-'}
                  </td>
                  <td className="col-valor-linha-um">
                    {dados.frequenciaGlobal}
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </TabelaColunasFixas>

      <TabelaColunasFixas>
        <div className="wrapper">
          <div className="header-fixo">
            <table className="table">
              <thead className="tabela-um-thead">
                <tr>
                  <th className="col-linha-dois">Bimestre</th>
                  <th className="col-linha-dois">Ausências no Bimestre</th>
                  <th className="col-linha-dois">Compensações de ausência</th>
                  <th className="col-linha-dois">Frequência</th>
                </tr>
              </thead>
              <tbody className="tabela-um-tbody">
                {dados?.frequenciaAlunoPorBimestres?.length ? (
                  dados?.frequenciaAlunoPorBimestres?.map((data, index) => {
                    return (
                      <>
                        <tr id={index}>
                          <td className="col-valor-linha-dois">
                            {data.bimestre}°
                          </td>
                          <td className="col-valor-linha-dois">
                            {data.quantidadeAusencias}
                          </td>
                          <td className="col-valor-linha-dois">
                            {data.quantidadeCompensacoes}
                          </td>
                          <td className="col-valor-linha-dois">
                            {data.frequencia}
                            <BtnExpandirAusenciaEstudante indexLinha={index} />
                          </td>
                        </tr>
                        <AusenciasEstudante
                          indexLinha={index}
                          dados={data}
                          codigoTurma={codigoTurma}
                          anoLetivo={anoLetivo}
                        />
                      </>
                    );
                  })
                ) : (
                  <tr>
                    <td colSpan="4">Sem dados</td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </TabelaColunasFixas>
    </>
  );
};

InformacoesEscolares.propTypes = {
  codigoAluno: PropTypes.oneOfType([PropTypes.any]),
  codigoTurma: PropTypes.oneOfType([PropTypes.any]),
  anoLetivo: PropTypes.oneOfType([PropTypes.any]),
};

InformacoesEscolares.defaultProps = {
  codigoAluno: '',
  codigoTurma: '',
  anoLetivo: null,
};

export default InformacoesEscolares;
