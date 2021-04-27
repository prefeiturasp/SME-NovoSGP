import moment from 'moment';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { CampoData } from '~/componentes';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import { salvarDadosAulaFrequencia } from '~/redux/modulos/calendarioProfessor/actions';
import {
  limparDadosFrequenciaPlanoAula,
  setAtualizarDatas,
  setAulaIdFrequenciaPlanoAula,
  setComponenteCurricularFrequenciaPlanoAula,
  setDataSelecionadaFrequenciaPlanoAula,
  setExibirLoaderFrequenciaPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import { confirmar, erros } from '~/servicos';
import ServicoFrequencia from '~/servicos/Paginas/DiarioClasse/ServicoFrequencia';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import servicoSalvarFrequenciaPlanoAula from '../../servicoSalvarFrequenciaPlanoAula';
import ModalSelecionarAulaFrequenciaPlanoAula from '../ModalSelecionarAula/modalSelecionarAulaFrequenciaPlanoAula';

const CamposFiltrarDadosFrequenciaPlanoAula = () => {
  const dispatch = useDispatch();

  const [bloquearProximo, setBloquearProximo] = useState(true);
  const [veioCalendario, setVeioCalendario] = useState(false);

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const componenteCurricular = useSelector(
    state => state.frequenciaPlanoAula.componenteCurricular
  );

  const modoEdicaoFrequencia = useSelector(
    state => state.frequenciaPlanoAula.modoEdicaoFrequencia
  );

  const modoEdicaoPlanoAula = useSelector(
    state => state.frequenciaPlanoAula.modoEdicaoPlanoAula
  );

  const dataSelecionada = useSelector(
    state => state.frequenciaPlanoAula.dataSelecionada
  );

  const aulaId = useSelector(state => state.frequenciaPlanoAula.aulaId);

  const dadosAulaFrequencia = useSelector(
    state => state.calendarioProfessor.dadosAulaFrequencia
  );

  const atualizarDatas = useSelector(
    state => state.frequenciaPlanoAula.atualizarDatas
  );

  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [codigoComponenteCurricular, setCodigoComponenteCurricular] = useState(
    undefined
  );

  const [listaDatasAulas, setListaDatasAulas] = useState();
  const [diasParaHabilitar, setDiasParaHabilitar] = useState();
  const [diasParaSinalizar, setDiasParaSinalizar] = useState();
  const [aulasParaSelecionar, setAulasParaSelecionar] = useState([]);
  const [exibirModalSelecionarAula, setExibirModalSelecionarAula] = useState(
    false
  );

  const valorPadrao = useMemo(() => {
    const ano = turmaSelecionada.anoLetivo;
    const dataParcial = moment().format('MM-DD');
    const dataInteira = moment(`${dataParcial}-${ano}`);
    return dataInteira;
  }, [turmaSelecionada.anoLetivo]);

  const obterDatasDeAulasDisponiveis = useCallback(async () => {
    dispatch(setExibirLoaderFrequenciaPlanoAula(true));
    const datasDeAulas =
      turmaSelecionada && turmaSelecionada.turma
        ? await ServicoFrequencia.obterDatasDeAulasPorCalendarioTurmaEComponenteCurricular(
            turmaSelecionada.turma,
            codigoComponenteCurricular
          )
            .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
            .catch(e => erros(e))
        : [];

    if (datasDeAulas && datasDeAulas.data && datasDeAulas.data.length) {
      setListaDatasAulas(datasDeAulas.data);
      const habilitar = [];
      const sinalizar = [];
      datasDeAulas.data.forEach(itemDatas => {
        const dataFormatada = moment(itemDatas.data).format('YYYY-MM-DD');
        itemDatas.aulas.forEach(itemAulas => {
          if (itemAulas.possuiFrequenciaRegistrada) {
            sinalizar.push(moment(dataFormatada));
          }
        });
        habilitar.push(dataFormatada);
      });
      setDiasParaHabilitar(habilitar);
      setDiasParaSinalizar(sinalizar);
      dispatch(setAtualizarDatas(false));
    } else {
      setListaDatasAulas();
      setDiasParaHabilitar();
      dispatch(setExibirLoaderFrequenciaPlanoAula(false));
    }
  }, [turmaSelecionada, codigoComponenteCurricular, dispatch]);

  const obterListaComponenteCurricular = useCallback(async () => {
    dispatch(setExibirLoaderFrequenciaPlanoAula(true));
    const resposta =
      turmaSelecionada && turmaSelecionada.turma
        ? await ServicoDisciplina.obterDisciplinasPorTurma(
            turmaSelecionada.turma
          )
            .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
            .catch(e => erros(e))
        : [];

    if (resposta && resposta.data) {
      setListaComponenteCurricular(resposta.data);
      if (resposta.data.length === 1) {
        const componente = resposta.data[0];
        dispatch(setComponenteCurricularFrequenciaPlanoAula(componente));
      }
    } else {
      setListaComponenteCurricular([]);
      dispatch(setComponenteCurricularFrequenciaPlanoAula(undefined));
      dispatch(setExibirLoaderFrequenciaPlanoAula(false));
    }
  }, [turmaSelecionada, dispatch]);

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosFrequenciaPlanoAula());
  }, [dispatch]);

  // Inicio, quando tem turma selecionada realizar a consulta da lista de componentes curriculares!
  useEffect(() => {
    if (turmaSelecionada && turmaSelecionada.turma) {
      obterListaComponenteCurricular();
    } else {
      dispatch(setComponenteCurricularFrequenciaPlanoAula(undefined));
      setListaComponenteCurricular([]);
    }
  }, [obterListaComponenteCurricular, turmaSelecionada, dispatch]);

  // Quando selecionar o componente curricular vai realizar a consulta das das que tem aulas cadastrada para essa turma!
  useEffect(() => {
    if (
      (codigoComponenteCurricular && turmaSelecionada?.turma) ||
      atualizarDatas
    ) {
      obterDatasDeAulasDisponiveis();
    }
  }, [
    codigoComponenteCurricular,
    obterDatasDeAulasDisponiveis,
    turmaSelecionada,
    atualizarDatas,
  ]);

  // Quando tem valor do componente curricular no redux vai setar o id no componente select!
  useEffect(() => {
    if (
      listaComponenteCurricular &&
      listaComponenteCurricular.length &&
      componenteCurricular
    ) {
      setCodigoComponenteCurricular(
        String(componenteCurricular.codigoComponenteCurricular)
      );
    } else {
      setCodigoComponenteCurricular(undefined);
    }
  }, [componenteCurricular, listaComponenteCurricular]);

  const pergutarParaSalvar = () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const onChangeComponenteCurricular = useCallback(
    async codigoComponenteCurricularId => {
      if (!codigoComponenteCurricularId) {
        dispatch(salvarDadosAulaFrequencia());
      }

      const aposValidarSalvar = () => {
        resetarInfomacoes();
        if (codigoComponenteCurricularId) {
          const componente = listaComponenteCurricular.find(
            item =>
              String(item.codigoComponenteCurricular) ===
              codigoComponenteCurricularId
          );
          dispatch(setComponenteCurricularFrequenciaPlanoAula(componente));
        } else {
          dispatch(setComponenteCurricularFrequenciaPlanoAula(undefined));
        }

        if (!codigoComponenteCurricularId) {
          setListaDatasAulas();
          setDiasParaHabilitar();
          dispatch(setDataSelecionadaFrequenciaPlanoAula());
        }
      };

      if (modoEdicaoFrequencia || modoEdicaoPlanoAula) {
        const confirmarParaSalvar = await pergutarParaSalvar();
        if (confirmarParaSalvar) {
          const salvou = await servicoSalvarFrequenciaPlanoAula.validarSalvarFrequenciPlanoAula();

          if (salvou) {
            aposValidarSalvar();
          }
        } else {
          aposValidarSalvar();
          // TODO
          // resetarPlanoAula();
        }
      } else {
        aposValidarSalvar();
      }
    },
    [
      dispatch,
      modoEdicaoFrequencia,
      modoEdicaoPlanoAula,
      listaComponenteCurricular,
      resetarInfomacoes,
    ]
  );

  const obterAulaSelecionada = useCallback(
    async data => {
      if (listaDatasAulas) {
        const aulaDataSelecionada = listaDatasAulas.find(item => {
          return (
            window.moment(item.data).format('DD/MM/YYYY') ===
            window.moment(data).format('DD/MM/YYYY')
          );
        });

        return aulaDataSelecionada;
      }
      return null;
    },
    [listaDatasAulas]
  );

  useEffect(() => {
    if (aulaId) {
      ServicoFrequencia.obterListaFrequencia();
    }
  }, [aulaId]);

  const validaSeTemIdAula = useCallback(
    async data => {
      if (!veioCalendario && dadosAulaFrequencia?.aulaId) {
        // Quando for Professor ou CJ podem visualizar somente uma aula por data selecionada!
        dispatch(setAulaIdFrequenciaPlanoAula(dadosAulaFrequencia.aulaId));
        setVeioCalendario(true);
      } else {
        const aulaDataSelecionada = await obterAulaSelecionada(data);
        if (aulaDataSelecionada && aulaDataSelecionada.aulas.length === 1) {
          // Quando for Professor ou CJ podem visualizar somente uma aula por data selecionada!
          dispatch(
            setAulaIdFrequenciaPlanoAula(aulaDataSelecionada.aulas[0].aulaId)
          );
          // Após setar o id vai disparar evento para buscar lista de frequencia!
        } else if (
          aulaDataSelecionada &&
          aulaDataSelecionada.aulas.length > 1
        ) {
          // Quando for CP, Diretor ou usuários da DRE e SME podem visualizar mais aulas por data selecionada!
          setAulasParaSelecionar(aulaDataSelecionada.aulas);
          setExibirModalSelecionarAula(true);
        }
      }
    },
    [obterAulaSelecionada, dispatch, dadosAulaFrequencia, veioCalendario]
  );

  const onChangeData = useCallback(
    async data => {
      let salvou = true;
      if (modoEdicaoFrequencia || modoEdicaoPlanoAula) {
        const confirmarParaSalvar = await pergutarParaSalvar();
        if (confirmarParaSalvar) {
          salvou = await servicoSalvarFrequenciaPlanoAula.validarSalvarFrequenciPlanoAula();
        }
      }

      if (salvou) {
        resetarInfomacoes();
        await validaSeTemIdAula(data);
        dispatch(setDataSelecionadaFrequenciaPlanoAula(data));
      }
    },
    [
      modoEdicaoFrequencia,
      modoEdicaoPlanoAula,
      validaSeTemIdAula,
      dispatch,
      resetarInfomacoes,
    ]
  );

  const onClickFecharModal = () => {
    setExibirModalSelecionarAula(false);
  };

  const onClickSelecionarAula = aulaDataSelecionada => {
    setExibirModalSelecionarAula(false);
    if (aulaDataSelecionada) {
      // Após setar o id vai disparar evento para buscar lista de frequencia!
      dispatch(setAulaIdFrequenciaPlanoAula(aulaDataSelecionada.aulaId));
    }
  };

  // Executa quando vir da tela do calendario!
  useEffect(() => {
    if (
      dadosAulaFrequencia &&
      Object.entries(dadosAulaFrequencia).length &&
      dadosAulaFrequencia.disciplinaId &&
      listaComponenteCurricular &&
      listaComponenteCurricular.length &&
      !codigoComponenteCurricular &&
      !veioCalendario
    ) {
      onChangeComponenteCurricular(String(dadosAulaFrequencia.disciplinaId));
    }
    if (
      dadosAulaFrequencia &&
      Object.entries(dadosAulaFrequencia).length &&
      dadosAulaFrequencia.dia &&
      diasParaHabilitar &&
      diasParaHabilitar.length &&
      !dataSelecionada &&
      !veioCalendario
    ) {
      onChangeData(window.moment(dadosAulaFrequencia.dia));
    }
  }, [
    dadosAulaFrequencia,
    listaComponenteCurricular,
    dataSelecionada,
    diasParaHabilitar,
    onChangeComponenteCurricular,
    onChangeData,
    codigoComponenteCurricular,
    veioCalendario,
  ]);

  const onClickProximaAula = async () => {
    const datasOrdenadas = diasParaHabilitar.sort(
      (a, b) => Date.parse(new Date(a)) - Date.parse(new Date(b))
    );
    const proximoIndice =
      datasOrdenadas.findIndex(
        data =>
          window.moment(data).format('DD/MM/YYYY') ===
          window.moment(dataSelecionada).format('DD/MM/YYYY')
      ) + 1;
    await onChangeData(window.moment(datasOrdenadas[proximoIndice]));
  };

  useEffect(() => {
    if (diasParaHabilitar && dataSelecionada) {
      const datasOrdenadas = diasParaHabilitar.sort(
        (a, b) => Date.parse(new Date(a)) - Date.parse(new Date(b))
      );
      const indiceAtual = datasOrdenadas.findIndex(
        data =>
          window.moment(data).format('DD/MM/YYYY') ===
          window.moment(dataSelecionada).format('DD/MM/YYYY')
      );
      const qtdeItems = datasOrdenadas.length - 1;
      setBloquearProximo(indiceAtual >= qtdeItems);
    }
  }, [diasParaHabilitar, dataSelecionada]);

  return (
    <>
      <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
        <SelectComponent
          id="componente-curricular"
          lista={listaComponenteCurricular}
          valueOption="codigoComponenteCurricular"
          valueText="nome"
          valueSelect={codigoComponenteCurricular}
          onChange={onChangeComponenteCurricular}
          placeholder="Selecione um componente curricular"
          disabled={
            !turmaSelecionada.turma ||
            (listaComponenteCurricular &&
              listaComponenteCurricular.length === 1)
          }
        />
      </div>
      <div className="col-sm-12 col-md-4 col-lg-3 col-xl-3 mb-3">
        <CampoData
          valor={dataSelecionada}
          onChange={onChangeData}
          placeholder="DD/MM/AAAA"
          formatoData="DD/MM/YYYY"
          desabilitado={
            !listaComponenteCurricular ||
            !listaComponenteCurricular.length ||
            !codigoComponenteCurricular ||
            !diasParaHabilitar
          }
          diasParaHabilitar={diasParaHabilitar}
          diasParaSinalizar={diasParaSinalizar}
          valorPadrao={valorPadrao}
        />
      </div>
      <div className="col-sm-12 col-md-4 col-lg-3 col-xl-3 mb-3">
        <Button
          id="btn-proximo"
          label="Próxima aula"
          icon="arrow-right"
          color={Colors.Azul}
          border
          className="mr-3"
          disabled={bloquearProximo}
          onClick={onClickProximaAula}
        />
      </div>
      <ModalSelecionarAulaFrequenciaPlanoAula
        visivel={exibirModalSelecionarAula}
        aulasParaSelecionar={aulasParaSelecionar}
        onClickFecharModal={onClickFecharModal}
        onClickSelecionarAula={onClickSelecionarAula}
      />
    </>
  );
};

export default CamposFiltrarDadosFrequenciaPlanoAula;
