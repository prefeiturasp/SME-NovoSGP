import { data } from 'jquery';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';

import {
  Base,
  Button,
  CampoData,
  Card,
  Colors,
  JoditEditor,
  Loader,
  PainelCollapse,
  SelectComponent,
} from '~/componentes';
import { Cabecalho, Paginacao } from '~/componentes-sgp';
import ObservacoesUsuario from '~/componentes-sgp/ObservacoesUsuario/observacoesUsuario';

import { RotasDto } from '~/dtos';

import { ehTurmaInfantil, erros, history, ServicoDisciplina } from '~/servicos';
import ServicoDiarioBordo from '~/servicos/Paginas/DiarioClasse/ServicoDiarioBordo';

import { BotoesAcoes, Mensagens, ModalNotificarUsuarios } from './componentes';

const NovoDiarioBordo = () => {
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [turmaInfantil, setTurmaInfantil] = useState(false);
  const [
    listaComponenteCurriculares,
    setListaComponenteCurriculares,
  ] = useState();
  const [
    componenteCurricularSelecionado,
    setComponenteCurricularSelecionado,
  ] = useState();
  const [dataInicial, setDataInicial] = useState();
  const [dataFinal, setDataFinal] = useState();
  const [modalVisivel, setModalVisivel] = useState(false);
  const [numeroPagina, setNumeroPagina] = useState(1);

  const { turmaSelecionada } = useSelector(state => state.usuario);
  const turmaId = turmaSelecionada?.id || 0;
  const turma = turmaSelecionada?.turma || 0;
  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );
  const [listaTitulos, setListaTitulos] = useState();
  const [diarioBordoAtual, setDiarioBordoAtual] = useState();

  const obterComponentesCurriculares = useCallback(async () => {
    setCarregandoGeral(true);
    const componentes = await ServicoDisciplina.obterDisciplinasPorTurma(
      turma
    ).catch(e => erros(e));

    if (componentes?.data?.length) {
      setListaComponenteCurriculares(componentes.data);

      if (componentes.data.length === 1) {
        const componente = componentes.data[0];
        setComponenteCurricularSelecionado(
          String(componente.codigoComponenteCurricular)
        );
      }
    }

    setCarregandoGeral(false);
  }, [turma]);

  const numeroRegistros = 10;
  const numeroTotalRegistros = listaTitulos?.totalRegistros;
  const mostrarPaginacao = numeroTotalRegistros > numeroRegistros;

  const resetarTela = useCallback(() => {}, []);

  useEffect(() => {
    if (turma && turmaInfantil) {
      obterComponentesCurriculares();
      return;
    }
    setListaComponenteCurriculares([]);
    setComponenteCurricularSelecionado(undefined);
    resetarTela();
  }, [turma, obterComponentesCurriculares, resetarTela, turmaInfantil]);

  useEffect(() => {
    const infantil = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setTurmaInfantil(infantil);

    if (!turmaInfantil) {
      resetarTela();
    }
  }, [
    turmaSelecionada,
    modalidadesFiltroPrincipal,
    resetarTela,
    turmaInfantil,
  ]);

  const onChangeComponenteCurricular = valor => {
    setComponenteCurricularSelecionado(valor);
  };

  const onClickNotificarUsuarios = () => setModalVisivel(true);

  const onClickConsultarDiario = () => {
    history.push(`${RotasDto.DIARIO_BORDO}/detalhes`);
  };

  const obterTitulos = useCallback(
    async (dataInicio, dataFim) => {
      setCarregandoGeral(true);
      const retorno = await ServicoDiarioBordo.obterTitulosDiarioBordo({
        turmaId,
        componenteCurricularId: componenteCurricularSelecionado,
        dataInicio,
        dataFim,
        numeroPagina,
        numeroRegistros,
      })
        .catch(e => erros(e))
        .finally(() => setCarregandoGeral(false));

      if (retorno?.status === 200) {
        setListaTitulos(retorno.data);
      }
    },
    [componenteCurricularSelecionado, turmaId, numeroPagina]
  );

  useEffect(() => {
    if (componenteCurricularSelecionado && numeroPagina) {
      obterTitulos();
    }
  }, [componenteCurricularSelecionado, obterTitulos, numeroPagina]);

  useEffect(() => {
    if (dataInicial && dataFinal) {
      const dataIncialFormatada = dataInicial?.format('MM-DD-YYYY');
      const dataFinalFormatada = dataFinal?.format('MM-DD-YYYY');
      obterTitulos(dataIncialFormatada, dataFinalFormatada);
    }
  }, [dataInicial, dataFinal, obterTitulos]);

  const onChangePaginacao = pagina => {
    // eslint-disable-next-line no-console
    console.log('pagina ===> ', pagina);
    setNumeroPagina(pagina);
  };

  const onColapse = async id => {
    const dados = await ServicoDiarioBordo.obterDiarioBordoDetalhes(id);
    if (dados?.data) {
      setDiarioBordoAtual(dados.data);
      if (dados.data.observacoes.length) {
        console.log(dados.data.observacoes);
      }
    }
  };

  return (
    <Loader loading={carregandoGeral} className="w-100">
      <Mensagens />
      <Cabecalho pagina="Diário de bordo" />
      <Card>
        <div className="col-md-12 p-0">
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-4">
              <SelectComponent
                id="disciplina"
                name="disciplinaId"
                lista={listaComponenteCurriculares || []}
                valueOption="codigoComponenteCurricular"
                valueText="nome"
                valueSelect={componenteCurricularSelecionado}
                onChange={onChangeComponenteCurricular}
                placeholder="Selecione um componente curricular"
                disabled={
                  !turmaInfantil ||
                  (listaComponenteCurriculares &&
                    listaComponenteCurriculares.length === 1)
                }
              />
            </div>
            <div className="col-sm-12 col-lg-8 col-md-8 d-flex justify-content-end pb-4">
              <BotoesAcoes turmaInfantil={turmaInfantil} />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-3 col-xl-3 mb-4 pr-0">
              <CampoData
                valor={dataInicial}
                onChange={data => setDataInicial(data)}
                name="dataInicial"
                placeholder="DD/MM/AAAA"
                formatoData="DD/MM/YYYY"
                desabilitado={
                  !turmaInfantil ||
                  !listaComponenteCurriculares ||
                  !componenteCurricularSelecionado
                  // || !diasParaHabilitar
                }
                // diasParaHabilitar={diasParaHabilitar}
              />
            </div>
            <div className="col-sm-12 col-md-4 col-lg-3 col-xl-3 mb-4">
              <CampoData
                valor={dataFinal}
                onChange={data => setDataFinal(data)}
                name="dataFinal"
                placeholder="DD/MM/AAAA"
                formatoData="DD/MM/YYYY"
                desabilitado={
                  !turmaInfantil ||
                  !listaComponenteCurriculares ||
                  !componenteCurricularSelecionado
                  // ||  !diasParaHabilitar
                }
                // diasParaHabilitar={diasParaHabilitar}
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 mb-3">
              <PainelCollapse accordion onChange={onColapse}>
                {listaTitulos?.items?.map(({ id, titulo }) => (
                  <PainelCollapse.Painel
                    key={id}
                    accordion
                    espacoPadrao
                    corBorda={Base.AzulBordaCollapse}
                    temBorda
                    header={titulo}
                    // altura={44}
                  >
                    <div className="row ">
                      <div className="col-sm-12 mb-3">
                        <JoditEditor
                          id={`${id}-editor-planejamento`}
                          name="planejamento"
                          value={diarioBordoAtual?.planejamento}
                          desabilitar
                        />
                      </div>
                      <div className="col-sm-12 d-flex justify-content-end mb-4">
                        <Button
                          id={shortid.generate()}
                          label="Consultar diário completo"
                          icon="book"
                          color={Colors.Azul}
                          border
                          onClick={onClickConsultarDiario}
                        />
                      </div>
                      <div className="col-sm-12 p-0 position-relative">
                        <ObservacoesUsuario
                          esconderLabel
                          esconderCaixaExterna
                          // salvarObservacao={obs => salvarEditarObservacao(obs)}
                          // editarObservacao={obs => salvarEditarObservacao(obs)}
                          // excluirObservacao={obs => excluirObservacao(obs)}
                        />
                        <div
                          className="position-absolute"
                          style={{ left: 16, bottom: 24 }}
                        >
                          <Button
                            id={shortid.generate()}
                            label="Notificar usuários (2)"
                            icon="bell"
                            color={Colors.Azul}
                            border
                            onClick={onClickNotificarUsuarios}
                          />
                        </div>
                      </div>
                      {diarioBordoAtual?.observacoes && (
                        <div className="col-sm-12 p-0 position-relative">
                          {diarioBordoAtual.observacoes.map(observacao => {
                            return (
                              <ObservacoesUsuario
                                esconderLabel
                                esconderCaixaExterna
                                salvarObservacao={() => {}}
                                editarObservacao={() => {}}
                                excluirObservacao={() => {}}
                              />
                            );
                          })}
                        </div>
                      )}
                    </div>
                  </PainelCollapse.Painel>
                ))}
              </PainelCollapse>
            </div>
          </div>
          {mostrarPaginacao && (
            <div className="row">
              <div className="col-12 d-flex justify-content-center mt-4">
                <Paginacao
                  numeroRegistros={numeroTotalRegistros}
                  pageSize={10}
                  onChangePaginacao={onChangePaginacao}
                />
              </div>
            </div>
          )}
        </div>
        <ModalNotificarUsuarios
          modalVisivel={modalVisivel}
          setModalVisivel={setModalVisivel}
        />
      </Card>
    </Loader>
  );
};

export default NovoDiarioBordo;
