import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  setDadosModalAnotacao,
  setExibirModalAnotacao,
  setExpandirLinhaAusenciaEstudante,
} from '~/redux/modulos/encaminhamentoAEE/actions';
import { erros } from '~/servicos';
import ServicoEncaminhamentoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoEncaminhamentoAEE';
import {
  BtnVisualizarAnotacao,
  TabelaColunasFixas,
} from './indicativosEstudante.css';

const AusenciasEstudante = props => {
  const { indexLinha, dados } = props;

  const dispatch = useDispatch();

  const expandirLinhaAusenciaEstudante = useSelector(
    store => store.encaminhamentoAEE.expandirLinhaAusenciaEstudante
  );

  const dadosSecaoLocalizarEstudante = useSelector(
    store => store.encaminhamentoAEE.dadosSecaoLocalizarEstudante
  );

  const [ausencias, setAusencias] = useState([]);

  useEffect(() => {
    return () => {
      dispatch(setExpandirLinhaAusenciaEstudante([]));
    };
  }, [dispatch]);

  const obterAusenciaMotivoPorAlunoTurmaBimestreAno = useCallback(async () => {
    // TODO lOADER!
    const retorno = await ServicoEncaminhamentoAEE.obterAusenciaMotivoPorAlunoTurmaBimestreAno(
      dados.codigoAluno,
      dados.bimestre,
      dadosSecaoLocalizarEstudante.codigoTurma,
      dadosSecaoLocalizarEstudante.anoLetivo
    ).catch(e => erros(e));

    if (retorno?.data) {
      setAusencias(retorno.data);
    } else {
      setAusencias([]);
    }
  }, [dados, dadosSecaoLocalizarEstudante]);

  useEffect(() => {
    if (expandirLinhaAusenciaEstudante && dados) {
      obterAusenciaMotivoPorAlunoTurmaBimestreAno();
    } else {
      setAusencias([]);
    }
  }, [
    dados,
    dadosSecaoLocalizarEstudante,
    expandirLinhaAusenciaEstudante,
    obterAusenciaMotivoPorAlunoTurmaBimestreAno,
  ]);

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
};

AusenciasEstudante.propTypes = {
  indexLinha: null,
  dados: [],
};

export default AusenciasEstudante;
