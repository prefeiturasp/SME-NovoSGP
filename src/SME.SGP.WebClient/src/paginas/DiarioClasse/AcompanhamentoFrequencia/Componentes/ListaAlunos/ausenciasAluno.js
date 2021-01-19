import PropTypes from 'prop-types';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  setDadosModalAnotacao,
  setExibirModalAnotacao,
  setExpandirLinhaFrequenciaAluno,
} from '~/redux/modulos/acompanhamentoFrequencia/actions';
import { BtnVisualizarAnotacao, TabelaColunasFixas } from './listaAlunos.css';

const AusenciasAluno = props => {
  const { indexLinha, dados } = props;

  const dispatch = useDispatch();

  const expandirLinhaFrequenciaAluno = useSelector(
    store => store.acompanhamentoFrequencia.expandirLinhaFrequenciaAluno
  );

  useEffect(() => {
    return () => {
      dispatch(setExpandirLinhaFrequenciaAluno([]));
    };
  }, [dispatch]);

  const onClickAnotacao = item => {
    dispatch(setDadosModalAnotacao(item));
    dispatch(setExibirModalAnotacao(true));
  };

  const visualizarAnotacao = item => {
    return (
      <div
        className="d-flex"
        style={{ alignItems: 'center', justifyContent: 'space-between' }}
      >
        <div>{item.motivo}</div>

        <BtnVisualizarAnotacao
          className={item.anotacao ? 'btn-com-anotacao' : ''}
          onClick={() => {
            if (item.anotacao) {
              onClickAnotacao(item);
            }
          }}
        >
          <i className="fas fa-eye" />
        </BtnVisualizarAnotacao>
      </div>
    );
  };

  return (
    <>
      {expandirLinhaFrequenciaAluno[indexLinha] ? (
        <tr>
          <td colSpan="5">
            <TabelaColunasFixas>
              <div className="wrapper">
                <div className="header-fixo">
                  <table className="table">
                    <thead className="tabela-dois-thead">
                      <tr>
                        <th className="col-linha-tres">Data</th>
                        <th className="col-linha-quatro">Motivo</th>
                      </tr>
                    </thead>
                    <tbody className="tabela-dois-tbody">
                      {dados.map((item, index) => {
                        return (
                          <tr id={index}>
                            <td className="col-valor-linha-tres">
                              {item.data}
                            </td>
                            <td className="col-valor-linha-quatro">
                              {visualizarAnotacao(item)}
                            </td>
                          </tr>
                        );
                      })}
                    </tbody>
                  </table>
                </div>
              </div>
            </TabelaColunasFixas>
          </td>
        </tr>
      ) : (
        ''
      )}
    </>
  );
};

AusenciasAluno.defaultProps = {
  indexLinha: PropTypes.number,
  dados: PropTypes.oneOfType([PropTypes.array]),
};

AusenciasAluno.propTypes = {
  indexLinha: null,
  dados: PropTypes.oneOfType([PropTypes.array]),
};

export default AusenciasAluno;
