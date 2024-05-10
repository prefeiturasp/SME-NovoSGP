import * as moment from 'moment';
import PropTypes from 'prop-types';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import Loader from '~/componentes/loader';
import {
  setDadosModalAnotacao,
  setExibirModalAnotacao,
  setExpandirLinhaAusenciaEstudante,
} from '~/redux/modulos/listaFrequenciaPorBimestre/actions';
import { erros } from '~/servicos';
import ServicoAcompanhamentoFrequencia from '~/servicos/Paginas/DiarioClasse/ServicoAcompanhamentoFrequencia';
import Paginacao from '../Paginacao/paginacao';
import {
  BtnVisualizarAnotacao,
  TabelaColunasFixas,
} from './listaFrequenciaPorBimestre.css';

const AusenciasEstudante = props => {
  const {
    indexLinha,
    bimestre,
    turmaId,
    codigoAluno,
    componenteCurricularId,
  } = props;

  const dispatch = useDispatch();

  const expandirLinhaAusenciaEstudante = useSelector(
    store => store.listaFrequenciaPorBimestre.expandirLinhaAusenciaEstudante
  );

  const [exibirLoader, setExibirLoader] = useState(false);
  const [ausencias, setAusencias] = useState([]);

  const REGISTROS_POR_PAGINA = 10;

  useEffect(() => {
    return () => {
      dispatch(setExpandirLinhaAusenciaEstudante([]));
    };
  }, [dispatch]);

  const obterAusenciaMotivoPorAlunoTurmaBimestreAno = useCallback(
    async numeroPagina => {
      setExibirLoader(true);
      const retorno = await ServicoAcompanhamentoFrequencia.obterJustificativaAcompanhamentoFrequenciaPaginacaoManual(
        turmaId,
        componenteCurricularId,
        codigoAluno,
        bimestre,
        numeroPagina || 1,
        REGISTROS_POR_PAGINA
      )
        .catch(e => erros(e))
        .finally(() => setExibirLoader(false));

      if (retorno?.data) {
        setAusencias(retorno.data);
      } else {
        setAusencias([]);
      }
    },
    [bimestre, turmaId, componenteCurricularId, codigoAluno]
  );

  useEffect(() => {
    if (expandirLinhaAusenciaEstudante[indexLinha]) {
      obterAusenciaMotivoPorAlunoTurmaBimestreAno();
    } else {
      setAusencias([]);
    }
  }, [
    indexLinha,
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
        <div>
          {item?.motivo?.substr(0, 100)}
          {item?.motivo?.length > 100 ? '...' : ''}
        </div>

        <BtnVisualizarAnotacao
          className={item.id > 0 ? 'btn-com-anotacao' : ''}
          onClick={() => {
            if (item.motivo) {
              onClickAnotacao(item);
            }
          }}
        >
          <i className="fas fa-eye" style={{ marginTop: '9px' }} />
        </BtnVisualizarAnotacao>
      </div>
    );
  };

  return (
    <>
      {expandirLinhaAusenciaEstudante[indexLinha] ? (
        <tr>
          <td colSpan="6">
            <Loader loading={exibirLoader}>
              <TabelaColunasFixas
                style={{ display: 'inline-grid', width: '100%' }}
              >
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
                        {ausencias?.items?.length ? (
                          ausencias?.items?.map(item => {
                            return (
                              <tr key={shortid.generate()}>
                                <td className="col-valor-linha-um">
                                  {moment(item.dataAusencia).format(
                                    'DD/MM/YYYY'
                                  )}
                                </td>
                                <td className="col-valor-linha-um">
                                  {item.registradoPor}
                                </td>
                                <td className="col-valor-linha-um">
                                  {visualizarAnotacao(item)}
                                </td>
                              </tr>
                            );
                          })
                        ) : (
                          <tr className="text-center">
                            <td colSpan="6">Sem dados</td>
                          </tr>
                        )}
                      </tbody>
                    </table>
                  </div>
                </div>
              </TabelaColunasFixas>
            </Loader>
            {ausencias?.items?.length && ausencias?.totalRegistros ? (
              <div className="col-md-12">
                <Paginacao
                  pageSize={REGISTROS_POR_PAGINA}
                  numeroRegistros={ausencias?.totalRegistros}
                  onChangePaginacao={
                    obterAusenciaMotivoPorAlunoTurmaBimestreAno
                  }
                />
              </div>
            ) : (
              ''
            )}
          </td>
        </tr>
      ) : (
        ''
      )}
    </>
  );
};

AusenciasEstudante.defaultProps = {
  indexLinha: PropTypes.oneOfType([PropTypes.any]),
  bimestre: PropTypes.oneOfType([PropTypes.any]),
  turmaId: PropTypes.oneOfType([PropTypes.any]),
  codigoAluno: PropTypes.oneOfType([PropTypes.any]),
  componenteCurricularId: PropTypes.oneOfType([PropTypes.any]),
};

AusenciasEstudante.propTypes = {
  indexLinha: null,
  bimestre: '',
  turmaId: '',
  codigoAluno: '',
  componenteCurricularId: 0,
};

export default AusenciasEstudante;
