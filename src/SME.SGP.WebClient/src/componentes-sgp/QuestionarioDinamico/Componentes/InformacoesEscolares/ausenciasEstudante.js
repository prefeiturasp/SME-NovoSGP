import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  setQuestionarioDinamicoDadosModalAnotacao,
  setQuestionarioDinamicoExibirModalAnotacao,
  setQuestionarioDinamicoExpandirLinhaAusenciaEstudante,
} from '~/redux/modulos/questionarioDinamico/actions';
import { erros, ServicoCalendarios } from '~/servicos';
import {
  BtnVisualizarAnotacao,
  TabelaColunasFixas,
} from './informacoesEscolares.css';

const AusenciasEstudante = props => {
  const { indexLinha, dados, anoLetivo, codigoTurma } = props;

  const dispatch = useDispatch();

  const expandirLinhaAusenciaEstudante = useSelector(
    store =>
      store.questionarioDinamico
        .questionarioDinamicoExpandirLinhaAusenciaEstudante
  );

  const [ausencias, setAusencias] = useState([]);

  useEffect(() => {
    return () => {
      dispatch(setQuestionarioDinamicoExpandirLinhaAusenciaEstudante([]));
    };
  }, [dispatch]);

  const obterAusenciaMotivoPorAlunoTurmaBimestreAno = useCallback(async () => {
    const retorno = await ServicoCalendarios.obterAusenciaMotivoPorAlunoTurmaBimestreAno(
      dados.codigoAluno,
      dados.bimestre,
      codigoTurma,
      anoLetivo
    ).catch(e => erros(e));

    if (retorno?.data) {
      setAusencias(retorno.data);
    } else {
      setAusencias([]);
    }
  }, [dados, codigoTurma, anoLetivo]);

  useEffect(() => {
    if (expandirLinhaAusenciaEstudante && dados) {
      obterAusenciaMotivoPorAlunoTurmaBimestreAno();
    } else {
      setAusencias([]);
    }
  }, [
    dados,
    expandirLinhaAusenciaEstudante,
    obterAusenciaMotivoPorAlunoTurmaBimestreAno,
  ]);

  const onClickAnotacao = item => {
    dispatch(setQuestionarioDinamicoDadosModalAnotacao(item));
    dispatch(setQuestionarioDinamicoExibirModalAnotacao(true));
  };

  const visualizarAnotacao = item => {
    return (
      <div
        className="d-flex"
        style={{ alignItems: 'center', justifyContent: 'space-between' }}
      >
        <div>{item.motivoAusencia}</div>

        <BtnVisualizarAnotacao
          className={item.justificativaAusencia ? 'btn-com-anotacao' : ''}
          onClick={() => {
            if (item.justificativaAusencia) {
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
      {expandirLinhaAusenciaEstudante[indexLinha] ? (
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
                      {ausencias.map(item => {
                        return (
                          <tr>
                            <td className="col-valor-linha-um">
                              {moment(item.dataAusencia).format('DD/MM/YYYY')}
                            </td>
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

AusenciasEstudante.defaultProps = {
  indexLinha: PropTypes.number,
  dados: PropTypes.oneOfType([PropTypes.array]),
  codigoTurma: PropTypes.oneOfType([PropTypes.any]),
  anoLetivo: PropTypes.oneOfType([PropTypes.any]),
};

AusenciasEstudante.propTypes = {
  indexLinha: null,
  dados: [],
  codigoTurma: '',
  anoLetivo: null,
};

export default AusenciasEstudante;
