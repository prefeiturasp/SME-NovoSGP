import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import * as moment from 'moment';
import { useDispatch, useSelector } from 'react-redux';
import {
  setDadosModalAnotacao,
  setExibirModalAnotacao,
} from '~/redux/modulos/acompanhamentoFrequencia/actions';
import { erros } from '~/servicos';
import ServicoAcompanhamentoFrequencia from '~/servicos/Paginas/DiarioClasse/ServicoAcompanhamentoFrequencia';
import { BtnVisualizarAnotacao, TabelaColunasFixas } from './listaAlunos.css';
import { Loader } from '~/componentes';

const AusenciasAluno = props => {
  const {
    indexLinha,
    componenteCurricularId,
    codigoAluno,
    turmaId,
    bimestre,
  } = props;
  const [dados, setDados] = useState([]);
  const [carregandoListaAusencias, setCarregandoListaAusencias] = useState(
    false
  );
  const [semDados, setSemDados] = useState(false);

  const dispatch = useDispatch();

  const expandirLinhaFrequenciaAluno = useSelector(
    store => store.acompanhamentoFrequencia.expandirLinhaFrequenciaAluno
  );

  const frequenciaAlunoCodigo = useSelector(
    store => store.acompanhamentoFrequencia.frequenciaAlunoCodigo
  );

  useEffect(() => {
    const obterMotivos = async () => {
      if (
        expandirLinhaFrequenciaAluno.includes(true) &&
        codigoAluno === frequenciaAlunoCodigo
      ) {
        setCarregandoListaAusencias(true);
        const retorno = await ServicoAcompanhamentoFrequencia.obterJustificativaAcompanhamentoFrequencia(
          turmaId,
          componenteCurricularId,
          codigoAluno,
          bimestre
        ).catch(e => {
          erros(e);
          setSemDados(true);
        });

        if (retorno?.data) {
          setSemDados(false);
          setDados(retorno.data);
        } else {
          setSemDados(true);
        }
      }
      setCarregandoListaAusencias(false);
    };
    obterMotivos();
  }, [frequenciaAlunoCodigo]);

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
          {item.motivo.substr(0, 100)}
          {item.motivo.length > 100 ? '...' : ''}
        </div>

        <BtnVisualizarAnotacao
          className={item.id ? 'btn-com-anotacao' : ''}
          onClick={() => {
            if (item.id) {
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
      {expandirLinhaFrequenciaAluno[indexLinha] ? (
        <tr>
          <td colSpan="5">
            <Loader loading={carregandoListaAusencias} />
            {dados.length > 0 && !semDados && (
              <>
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
                                  {moment(item.dataAnotacao).format('L')}
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
              </>
            )}
            {semDados && (
              <>
                <p>
                  {carregandoListaAusencias
                    ? ''
                    : 'Não foram encontrados os motivos de ausência do aluno.'}
                </p>
              </>
            )}
          </td>
        </tr>
      ) : (
        ''
      )}
    </>
  );
};

AusenciasAluno.defaultProps = {
  componenteCurricularId: PropTypes.string,
  turmaId: PropTypes.string,
  codigoAluno: PropTypes.string,
  indexLinha: PropTypes.number,
  bimestre: PropTypes.number,
};

AusenciasAluno.propTypes = {
  componenteCurricularId: PropTypes.string,
  turmaId: PropTypes.string,
  codigoAluno: PropTypes.string,
  indexLinha: null,
  bimestre: null,
};

export default AusenciasAluno;
