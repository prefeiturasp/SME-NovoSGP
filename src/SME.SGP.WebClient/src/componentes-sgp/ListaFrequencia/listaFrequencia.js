import { Switch, Icon } from 'antd';
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import shortid from 'shortid';

import {
  Lista,
  CaixaMarcadores,
  IconePlusMarcadores,
} from './listaFrequencia.css';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import tipoIndicativoFrequencia from '~/dtos/tipoIndicativoFrequencia';

const ListaFrequencia = props => {
  const { dados, onChangeFrequencia, permissoesTela, frequenciaId } = props;

  const [dataSource, setDataSource] = useState(dados);
  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [expandirLinha, setExpandirLinha] = useState([]);

  useEffect(() => {
    const somenteConsulta = verificaSomenteConsulta(permissoesTela);
    const desabilitar =
      frequenciaId > 0
        ? somenteConsulta || !permissoesTela.podeAlterar
        : somenteConsulta || !permissoesTela.podeIncluir;
    setDesabilitarCampos(desabilitar);
  }, [frequenciaId, permissoesTela]);

  const renderSwitch = (i, aula, aluno) => {
    return (
      <div
        id={`switch-${i}`}
        className={aula.compareceu ? 'presenca' : 'falta'}
      >
        <Switch
          disabled={desabilitarCampos || aluno.desabilitado}
          checkedChildren="C"
          unCheckedChildren="F"
          onChange={faltou => {
            if (!desabilitarCampos || !aluno.desabilitado) {
              aula.compareceu = !faltou;
              setDataSource([...dataSource]);
              onChangeFrequencia(true);
            }
          }}
          checked={!aula.compareceu}
        />
      </div>
    );
  };

  const validaSeFaltouTodasAulas = aluno => {
    const totalAulas = aluno.aulas.length;
    const totalAulasFaltou = aluno.aulas.filter(aula => !aula.compareceu);
    return totalAulas === totalAulasFaltou.length;
  };

  const validaSeCompareceuTodasAulas = aluno => {
    const totalAulas = aluno.aulas.length;
    const totalAulasCompareceu = aluno.aulas.filter(aula => aula.compareceu);
    return totalAulas === totalAulasCompareceu.length;
  };

  const marcaPresencaFaltaTodasAulas = (aluno, marcarPresenca) => {
    if (!desabilitarCampos && !aluno.desabilitado) {
      aluno.aulas.forEach(aula => {
        aula.compareceu = marcarPresenca;
      });
      setDataSource([...dataSource]);
      onChangeFrequencia(true);
    }
  };

  const marcarPresencaFaltaTodosAlunos = marcarPresenca => {
    if (!desabilitarCampos) {
      dataSource.forEach(aluno => {
        if (!aluno.desabilitado) {
          aluno.aulas.forEach(aula => {
            aula.compareceu = marcarPresenca;
          });
        }
      });
      setDataSource([...dataSource]);
      onChangeFrequencia(true);
    }
  };

  const onClickExpandir = index => {
    expandirLinha[index] = !expandirLinha[index];
    setExpandirLinha([...expandirLinha]);
  };

  return (
    <>
      {dataSource && dataSource.length > 0 ? (
        <Lista className="mt-4 table-responsive">
          <div className="scroll-tabela-frequencia-thead">
            <table className="table mb-0 ">
              <thead className="tabela-frequencia-thead">
                <tr>
                  <th className="width-60" />
                  <th className="text-left">Lista de estudantes</th>
                  {dataSource[0].aulas.length > 1 ? (
                    <>
                      <th
                        className="width-50 cursor-pointer"
                        onClick={() => marcarPresencaFaltaTodosAlunos(true)}
                      >
                        <div className="marcar-todas-frequencia">
                          Marcar todas
                        </div>
                        <div className="margin-marcar-todos">C</div>
                      </th>
                      <th
                        className="width-50 cursor-pointer"
                        onClick={() => marcarPresencaFaltaTodosAlunos(false)}
                      >
                        <div className="margin-marcar-todos">F</div>
                      </th>
                    </>
                  ) : null}
                  {dataSource[0].aulas.map((aula, i) => {
                    return (
                      <th
                        key={shortid.generate()}
                        className={
                          dataSource[0].aulas.length - 1 === i
                            ? 'width-70'
                            : 'border-right-none width-70'
                        }
                      >
                        {aula.numeroAula}
                      </th>
                    );
                  })}
                  <th className="width-70">
                    <i className="fas fa-exclamation-triangle" />
                  </th>
                </tr>
              </thead>
            </table>
          </div>
          <div className="scroll-tabela-frequencia-tbody">
            {dataSource && dataSource.length && (
              <table className="table mb-0">
                <tbody className="tabela-frequencia-tbody">
                  {dataSource.map((aluno, i) => {
                    return (
                      <React.Fragment key={shortid.generate()}>
                        <tr
                          className={
                            desabilitarCampos || aluno.desabilitado
                              ? 'desabilitar-aluno'
                              : ''
                          }
                        >
                          <td className="width-60 text-center font-weight-bold">
                            {aluno.numeroAlunoChamada}
                          </td>
                          <td className="text-left">
                            {aluno.nomeAluno}
                            {aluno.marcador && (
                              <>
                                <CaixaMarcadores>
                                  {aluno.marcador.nome}
                                </CaixaMarcadores>
                                <IconePlusMarcadores
                                  onClick={() => onClickExpandir(i)}
                                  className={
                                    expandirLinha[i]
                                      ? 'fas fa-minus fa-minus-linha-expandida '
                                      : 'fas fa-plus-circle'
                                  }
                                />
                              </>
                            )}
                          </td>
                          {dataSource[0].aulas.length > 1 ? (
                            <>
                              <td className="width-50">
                                <button
                                  type="button"
                                  onClick={() =>
                                    marcaPresencaFaltaTodasAulas(aluno, true)
                                  }
                                  className={`ant-btn ant-btn-circle ant-btn-sm btn-falta-presenca ${
                                    validaSeCompareceuTodasAulas(aluno)
                                      ? 'btn-compareceu'
                                      : ''
                                  } `}
                                  disabled={
                                    desabilitarCampos || aluno.desabilitado
                                  }
                                >
                                  <i className="fas fa-check fa-sm" />
                                </button>
                              </td>
                              <td className="width-50">
                                <button
                                  type="button"
                                  onClick={() =>
                                    marcaPresencaFaltaTodasAulas(aluno, false)
                                  }
                                  className={`ant-btn ant-btn-circle ant-btn-sm btn-falta-presenca ${
                                    validaSeFaltouTodasAulas(aluno)
                                      ? 'btn-falta'
                                      : ''
                                  } `}
                                  disabled={
                                    desabilitarCampos || aluno.desabilitado
                                  }
                                >
                                  <i className="fas fa-times fa-sm" />
                                </button>
                              </td>
                            </>
                          ) : null}
                          {aluno.aulas.map((aula, a) => {
                            return (
                              <td
                                key={shortid.generate()}
                                className={
                                  dataSource[0].aulas.length - 1 === a
                                    ? 'width-70'
                                    : 'border-right-none width-70'
                                }
                              >
                                {renderSwitch(a, aula, aluno)}
                              </td>
                            );
                          })}
                          <td className="width-70">
                            <span
                              className={`width-70 ${
                                aluno.indicativoFrequencia &&
                                tipoIndicativoFrequencia.Alerta ===
                                  aluno.indicativoFrequencia.tipo
                                  ? 'indicativo-alerta'
                                  : ''
                              } ${
                                aluno.indicativoFrequencia &&
                                tipoIndicativoFrequencia.Critico ===
                                  aluno.indicativoFrequencia.tipo
                                  ? 'indicativo-critico'
                                  : ''
                              } `}
                            >
                              {aluno.indicativoFrequencia
                                ? `${aluno.indicativoFrequencia.percentual}%`
                                : ''}
                            </span>
                          </td>
                        </tr>
                        {expandirLinha[i] && (
                          <>
                            <tr className="linha-expandida">
                              <td colSpan="1" className="text-center">
                                <Icon type="double-right" />
                              </td>
                              <td colSpan={dataSource[0].aulas.length + 4}>
                                {aluno.marcador.descricao}
                              </td>
                            </tr>
                          </>
                        )}
                      </React.Fragment>
                    );
                  })}
                </tbody>
              </table>
            )}
          </div>
        </Lista>
      ) : null}
    </>
  );
};

ListaFrequencia.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.array]),
  onChangeFrequencia: PropTypes.oneOfType([PropTypes.func]),
  permissoesTela: PropTypes.oneOfType([PropTypes.any]),
  frequenciaId: PropTypes.oneOfType([PropTypes.any]),
};

ListaFrequencia.defaultProps = {
  dados: [],
  onChangeFrequencia: () => {},
  permissoesTela: [],
  frequenciaId: 0,
};

export default ListaFrequencia;
