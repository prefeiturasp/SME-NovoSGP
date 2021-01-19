import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  setDadosModalAnotacao,
  setExibirModalAnotacao,
} from '~/redux/modulos/acompanhamentoFrequencia/actions';
import { erros } from '~/servicos';
import ServicoAcompanhamentoFrequencia from '~/servicos/Paginas/DiarioClasse/ServicoAcompanhamentoFrequencia';
import { BtnVisualizarAnotacao, TabelaColunasFixas } from './listaAlunos.css';

const AusenciasAluno = props => {
  const { indexLinha, componenteCurricularId, codigoAluno, turmaId } = props;
  const [dados, setDados] = useState([]);

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
        const retorno = await ServicoAcompanhamentoFrequencia.obterJustificativaAcompanhamentoFrequencia(
          turmaId,
          componenteCurricularId,
          codigoAluno
        ).catch(e => erros(e));

        if (retorno?.data) {
          setDados(retorno.data);
        }
      } else {
        setDados([]);
      }
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
        <div>{item.motivo}</div>

        <BtnVisualizarAnotacao
          className={item.anotacao ? 'btn-com-anotacao' : ''}
          onClick={() => {
            if (item.anotacao) {
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
  componenteCurricularId: PropTypes.string,
  turmaId: PropTypes.string,
  codigoAluno: PropTypes.string,
  indexLinha: PropTypes.number,
};

AusenciasAluno.propTypes = {
  componenteCurricularId: PropTypes.string,
  turmaId: PropTypes.string,
  codigoAluno: PropTypes.string,
  indexLinha: null,
};

export default AusenciasAluno;
