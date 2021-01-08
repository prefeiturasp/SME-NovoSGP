import PropTypes from 'prop-types';
import React, { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  setDadosModalAnotacao,
  setExibirModalAnotacao,
  setExpandirLinhaAusenciaAluno,
} from '~/redux/modulos/encaminhamentoAEE/actions';
import {
  BtnVisualizarAnotacao,
  TabelaColunasFixas,
} from './informacoesEscolares.css';

const AusenciasAluno = props => {
  const { indexLinha, dados } = props;

  const dispatch = useDispatch();

  const expandirLinhaAusenciaAluno = useSelector(
    store => store.encaminhamentoAEE.expandirLinhaAusenciaAluno
  );

  useEffect(() => {
    return () => {
      dispatch(setExpandirLinhaAusenciaAluno([]));
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
      {expandirLinhaAusenciaAluno[indexLinha] ? (
        <tr>
          <td colSpan="4">
            <TabelaColunasFixas>
              <div className="wrapper">
                <div className="header-fixo">
                  <table className="table">
                    <thead className="tabela-dois-thead">
                      <tr>
                        <th className="col-linha-um">Data</th>
                        <th className="col-linha-um">Registrado por</th>
                        <th className="col-linha-um">Motivo da ausência</th>
                      </tr>
                    </thead>
                    <tbody className="tabela-dois-tbody">
                      {dados?.ausencias.map(item => {
                        return (
                          <tr>
                            <td className="col-valor-linha-um">{item.data}</td>
                            <td className="col-valor-linha-um">
                              {item.registradoPor}
                            </td>
                            <td className="col-valor-linha-um">
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
  dados: [],
};

export default AusenciasAluno;
