import { Switch, Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';

import { Lista, MarcadorSituacao, BtbAnotacao } from './listaFrequencia.css';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import tipoIndicativoFrequencia from '~/dtos/tipoIndicativoFrequencia';
import ModalAnotacoesFrequencia from '~/paginas/DiarioClasse/FrequenciaPlanoAula/ModalAnotacoes/modalAnotacoes';
import SinalizacaoAEE from '../SinalizacaoAEE/sinalizacaoAEE';

const ListaFrequencia = props => {
  const {
    onChangeFrequencia,
    permissoesTela,
    frequenciaId,
    temPeriodoAberto,
    ehInfantil,
    aulaId,
    componenteCurricularId,
    setDataSource,
  } = props;

  const dataSource = useSelector(
    state => state.frequenciaPlanoAula.listaDadosFrequencia?.listaFrequencia
  );

  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const [exibirModalAnotacao, setExibirModalAnotacao] = useState(false);
  const [dadosModalAnotacao, setDadosModalAnotacao] = useState({});

  useEffect(() => {
    const somenteConsulta = verificaSomenteConsulta(permissoesTela);
    const desabilitar =
      frequenciaId > 0
        ? somenteConsulta || !permissoesTela.podeAlterar
        : somenteConsulta || !permissoesTela.podeIncluir;
    setDesabilitarCampos(desabilitar);
    if (!temPeriodoAberto) setDesabilitarCampos(!temPeriodoAberto);
  }, [frequenciaId, permissoesTela, temPeriodoAberto]);

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

  const onClickAnotacao = aluno => {
    setDadosModalAnotacao(aluno);
    setExibirModalAnotacao(true);
  };

  const onCloseModalAnotacao = () => {
    setExibirModalAnotacao(false);
    setDadosModalAnotacao({});
  };

  const btnAnotacao = item => {
    const podeAbrirModal =
      (item.permiteAnotacao && !desabilitarCampos) ||
      (item.possuiAnotacao && desabilitarCampos);
    return (
      <Tooltip
        title={
          item.possuiAnotacao
            ? `${ehInfantil ? 'Criança' : 'Estudante'} com anotações`
            : ''
        }
        placement="top"
      >
        <div className=" d-flex justify-content-end">
          <BtbAnotacao
            podeAbrirModal={podeAbrirModal}
            className={item.possuiAnotacao ? 'btn-com-anotacao' : ''}
            onClick={() => {
              if (podeAbrirModal) {
                onClickAnotacao(item);
              }
            }}
          >
            <i className="fas fa-pen" />
          </BtbAnotacao>
        </div>
      </Tooltip>
    );
  };

  return (
    <>
      {exibirModalAnotacao ? (
        <ModalAnotacoesFrequencia
          exibirModal={exibirModalAnotacao}
          onCloseModal={onCloseModalAnotacao}
          dadosModalAnotacao={dadosModalAnotacao}
          dadosListaFrequencia={dataSource}
          ehInfantil={ehInfantil}
          aulaId={aulaId}
          componenteCurricularId={componenteCurricularId}
          desabilitarCampos={desabilitarCampos}
        />
      ) : (
        ''
      )}
      {dataSource && dataSource.length > 0 ? (
        <Lista className="mt-4 table-responsive">
          <div className="scroll-tabela-frequencia-thead">
            <table className="table mb-0 ">
              <thead className="tabela-frequencia-thead">
                <tr>
                  <th className="width-60" />
                  <th className="text-left">
                    Lista de {ehInfantil ? 'crianças' : 'estudantes'}
                  </th>
                  {dataSource[0].aulas.length > 0 ? (
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
                          {aluno.marcador ? (
                            <Tooltip
                              title={aluno.marcador.descricao}
                              placement="top"
                            >
                              <MarcadorSituacao className="fas fa-circle" />
                            </Tooltip>
                          ) : (
                            ''
                          )}
                        </td>
                        <td>
                          <div
                            className="d-flex"
                            style={{ justifyContent: 'space-between' }}
                          >
                            <div className=" d-flex justify-content-start">
                              {aluno.nomeAluno}
                            </div>
                            <div className=" d-flex justify-content-end">
                              <div className="mr-3">
                                <SinalizacaoAEE
                                  exibirSinalizacao={aluno.ehAtendidoAEE}
                                />
                              </div>
                              {btnAnotacao(aluno)}
                            </div>
                          </div>
                        </td>
                        {dataSource[0].aulas.length > 0 ? (
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
                        ) : (
                          <></>
                        )}

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
                    </React.Fragment>
                  );
                })}
              </tbody>
            </table>
          </div>
        </Lista>
      ) : (
        <></>
      )}
    </>
  );
};

ListaFrequencia.propTypes = {
  onChangeFrequencia: PropTypes.oneOfType([PropTypes.func]),
  permissoesTela: PropTypes.oneOfType([PropTypes.any]),
  frequenciaId: PropTypes.oneOfType([PropTypes.any]),
  temPeriodoAberto: PropTypes.oneOfType([PropTypes.bool]),
  ehInfantil: PropTypes.oneOfType([PropTypes.bool]),
  aulaId: PropTypes.oneOfType([PropTypes.any]),
  componenteCurricularId: PropTypes.oneOfType([PropTypes.any]),
  setDataSource: PropTypes.oneOfType([PropTypes.func]),
};

ListaFrequencia.defaultProps = {
  onChangeFrequencia: () => {},
  permissoesTela: {},
  frequenciaId: 0,
  temPeriodoAberto: false,
  ehInfantil: false,
  aulaId: '',
  componenteCurricularId: '',
  setDataSource: () => {},
};

export default ListaFrequencia;
