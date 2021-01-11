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

  const { turmaSelecionada } = useSelector(state => state.usuario);
  const turmaId = turmaSelecionada?.turma || 0;
  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const obterComponentesCurriculares = useCallback(async () => {
    setCarregandoGeral(true);
    const componentes = await ServicoDisciplina.obterDisciplinasPorTurma(
      turmaId
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
  }, [turmaId]);

  const headers = [];
  // eslint-disable-next-line no-plusplus
  for (let i = 0; i < 5; i++) {
    const arr = [
      {
        id: shortid.generate(),
        header: '23/11/2020 - VERA LUCIA DA SILVA SOUZA',
        planejamento: `<p>teste VERA LUCIA ${i}</p>`,
      },
      {
        id: shortid.generate(),
        header: '17/11/2020 - CARMEN FERREIRA MENDES',
        planejamento: `<p>teste CARMEN FERREIRA ${i}</p>`,
      },
    ];
    headers.push(...arr);
  }

  const ehMostrarPaginacao = headers.length === 10;

  const resetarTela = useCallback(() => {}, []);

  useEffect(() => {
    if (turmaId && turmaInfantil) {
      obterComponentesCurriculares();
    } else {
      setListaComponenteCurriculares([]);
      setComponenteCurricularSelecionado(undefined);
      resetarTela();
    }
  }, [turmaId, obterComponentesCurriculares, resetarTela, turmaInfantil]);

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
    if (!valor) {
      // setDiasParaHabilitar([]);
    }
    // setDataSelecionada('');
    setComponenteCurricularSelecionado(valor);
  };

  const onChangePaginacao = pagina => {
    // eslint-disable-next-line no-console
    console.log('pagina ===> ', pagina);
  };

  const onClickNotificarUsuarios = () => setModalVisivel(true);

  const onClickConsultarDiario = () => {
    history.push(`${RotasDto.DIARIO_BORDO}/detalhes`);
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
            {headers.map(({ header, id, planejamento }) => (
              <div className="col-sm-12 mb-3" key={id}>
                <PainelCollapse corFundo={Base.Branco}>
                  <PainelCollapse.Painel
                    key={id}
                    espacoPadrao
                    corBorda={Base.AzulBordaCollapse}
                    temBorda
                    header={header}
                    altura={44}
                  >
                    <div className="row ">
                      <div className="col-sm-12 mb-3">
                        <JoditEditor
                          id={`${id}-editor-planejamento`}
                          name="planejamento"
                          value={planejamento}
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
                    </div>
                  </PainelCollapse.Painel>
                </PainelCollapse>
              </div>
            ))}
          </div>
          {ehMostrarPaginacao && (
            <div className="row">
              <div className="col-12 d-flex justify-content-center mt-4">
                <Paginacao
                  numeroRegistros={10}
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
