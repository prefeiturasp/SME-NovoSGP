import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import AusenciasEstudante from '~/componentes-sgp/ListaFrequenciaPorBimestre/ausenciasEstudante';
import BtnExpandirAusenciaEstudante from '~/componentes-sgp/ListaFrequenciaPorBimestre/btnExpandirAusenciaEstudante';
import ModalAnotacoes from '~/componentes-sgp/ListaFrequenciaPorBimestre/modalAnotacoes';
import NomeEstudanteLista from '~/componentes-sgp/NomeEstudanteLista/nomeEstudanteLista';
import Ordenacao from '~/componentes-sgp/Ordenacao/ordenacao';
import { Base } from '~/componentes/colors';
import { setExpandirLinhaFrequenciaAluno } from '~/redux/modulos/acompanhamentoFrequencia/actions';
import { erros } from '~/servicos';
import ServicoAcompanhamentoFrequencia from '~/servicos/Paginas/DiarioClasse/ServicoAcompanhamentoFrequencia';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import {
  MarcadorAulas,
  Marcadores,
  TabelaColunasFixas,
} from './listaAlunos.css';

const ListaAlunos = props => {
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const { componenteCurricularId, bimestreLista } = props;
  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const exibirModalAnotacao = useSelector(
    store => store.listaFrequenciaPorBimestre.exibirModalAnotacao
  );

  const { bimestreSelecionado } = useSelector(
    store => store.acompanhamentoFrequencia
  );

  const { id: turmaId } = turmaSelecionada;

  const dispatch = useDispatch();

  const [carregandoListaAlunos, setCarregandoListaAlunos] = useState(false);
  const [dadosBimestre, setDadosBimestre] = useState(null);

  useEffect(() => {
    const obterAlunos = async () => {
      setCarregandoListaAlunos(true);
      const retorno = await ServicoAcompanhamentoFrequencia.obterAcompanhamentoFrequenciaPorBimestre(
        turmaSelecionada?.id,
        componenteCurricularId,
        bimestreSelecionado
      ).catch(e => erros(e));

      if (retorno?.data) {
        setDadosBimestre(retorno?.data);
      }
      setCarregandoListaAlunos(false);
    };
    if (
      componenteCurricularId &&
      turmaId &&
      bimestreSelecionado &&
      String(bimestreLista) === String(bimestreSelecionado)
    ) {
      dispatch(setExpandirLinhaFrequenciaAluno([]));
      obterAlunos();
    }
  }, [componenteCurricularId, turmaId, bimestreSelecionado]);

  const onChangeOrdenacao = alunosOrdenados => {
    dispatch(setExpandirLinhaFrequenciaAluno([]));
    setDadosBimestre({ ...dadosBimestre, frequenciaAlunos: alunosOrdenados });
  };

  return (
    <>
      <Loader loading={carregandoListaAlunos} />
      {dadosBimestre ? (
        <>
          {exibirModalAnotacao ? <ModalAnotacoes /> : ''}
          <TabelaColunasFixas>
            <div className="row">
              <div className="col-md-6 col-sm-12">
                <Ordenacao
                  className="mb-2"
                  conteudoParaOrdenar={dadosBimestre?.frequenciaAlunos}
                  ordenarColunaNumero="numeroChamada"
                  ordenarColunaTexto="nome"
                  retornoOrdenado={retorno => {
                    onChangeOrdenacao(retorno);
                  }}
                />
              </div>

              <Marcadores className="col-md-6 col-sm-12 d-flex justify-content-end">
                {!ehTurmaInfantil(
                  modalidadesFiltroPrincipal,
                  turmaSelecionada
                ) ? (
                  <MarcadorAulas className="ml-2">
                    <span>Aulas previstas </span>
                    <span className="numero">
                      {dadosBimestre?.aulasPrevistas
                        ? dadosBimestre.aulasPrevistas
                        : 0}
                    </span>
                  </MarcadorAulas>
                ) : (
                  <></>
                )}
                <MarcadorAulas className="ml-2">
                  <span>Aulas dadas </span>
                  <span className="numero">
                    {dadosBimestre?.aulasDadas ? dadosBimestre.aulasDadas : 0}
                  </span>
                </MarcadorAulas>
              </Marcadores>
            </div>
            <div className="wrapper">
              <div className="header-fixo">
                <table className="table">
                  <thead className="tabela-dois-thead">
                    <tr>
                      <th className="col-linha-quatro" colSpan="2">
                        Nome
                      </th>
                      <th className="col-linha-dois">Ausências no Bimestre</th>
                      {!ehTurmaInfantil(
                        modalidadesFiltroPrincipal,
                        turmaSelecionada
                      ) ? (
                        <th className="col-linha-dois">
                          Compensações de ausência
                        </th>
                      ) : (
                        <></>
                      )}
                      <th className="col-linha-dois">Frequência</th>
                    </tr>
                  </thead>
                  <tbody className="tabela-um-tbody">
                    {dadosBimestre?.frequenciaAlunos?.map((data, index) => {
                      return (
                        <>
                          <tr
                            id={index}
                            style={{
                              background: data?.marcadorFrequencia
                                ? Base.CinzaDesabilitado
                                : '',
                              borderRight: data?.marcadorFrequencia
                                ? `solid 1px ${Base.CinzaBotao}`
                                : `solid 1px ${Base.CinzaDesabilitado}`,
                            }}
                          >
                            <td
                              className="col-valor-linha-tres"
                              style={{
                                borderRight: data?.marcadorFrequencia
                                  ? `solid 1px ${Base.CinzaBotao}`
                                  : `solid 1px ${Base.CinzaDesabilitado}`,
                              }}
                            >
                              <strong>{data?.numeroChamada}</strong>
                              {data?.marcadorFrequencia ? (
                                <div className="divIconeSituacao">
                                  <Tooltip
                                    title={data.marcadorFrequencia?.descricao}
                                  >
                                    <span className="iconeSituacao" />
                                  </Tooltip>
                                </div>
                              ) : (
                                ''
                              )}
                            </td>
                            <td
                              className="col-valor-linha-quatro"
                              style={{
                                borderRight: data?.marcadorFrequencia
                                  ? `solid 1px ${Base.CinzaBotao}`
                                  : `solid 1px ${Base.CinzaDesabilitado}`,
                              }}
                            >
                              <NomeEstudanteLista
                                nome={data?.nome}
                                exibirSinalizacao={data?.ehAtendidoAEE}
                              />
                            </td>
                            <td
                              className="col-valor-linha-dois"
                              style={{
                                borderRight: data?.marcadorFrequencia
                                  ? `solid 1px ${Base.CinzaBotao}`
                                  : `solid 1px ${Base.CinzaDesabilitado}`,
                              }}
                            >
                              {data.ausencias}
                            </td>
                            {!ehTurmaInfantil(
                              modalidadesFiltroPrincipal,
                              turmaSelecionada
                            ) ? (
                              <td
                                className="col-valor-linha-dois"
                                style={{
                                  borderRight: data?.marcadorFrequencia
                                    ? `solid 1px ${Base.CinzaBotao}`
                                    : `solid 1px ${Base.CinzaDesabilitado}`,
                                }}
                              >
                                {data.compensacoes}
                              </td>
                            ) : (
                              <></>
                            )}
                            <td className="col-valor-linha-dois">
                              {data.frequencia}%
                              {data.ausencias > 0 &&
                                bimestreSelecionado > 0 && (
                                  <BtnExpandirAusenciaEstudante
                                    indexLinha={index}
                                  />
                                )}
                            </td>
                          </tr>
                          <AusenciasEstudante
                            indexLinha={index}
                            bimestre={bimestreSelecionado}
                            turmaId={turmaId}
                            codigoAluno={data.alunoRf}
                            componenteCurricularId={componenteCurricularId}
                          />
                        </>
                      );
                    })}
                  </tbody>
                </table>
              </div>
            </div>
          </TabelaColunasFixas>
        </>
      ) : (
        <></>
      )}
    </>
  );
};

ListaAlunos.propTypes = {
  componenteCurricularId: PropTypes.string,
  bimestreLista: PropTypes.number,
};

ListaAlunos.defaultProps = {
  componenteCurricularId: PropTypes.string,
  bimestreLista: '1',
};

export default ListaAlunos;
