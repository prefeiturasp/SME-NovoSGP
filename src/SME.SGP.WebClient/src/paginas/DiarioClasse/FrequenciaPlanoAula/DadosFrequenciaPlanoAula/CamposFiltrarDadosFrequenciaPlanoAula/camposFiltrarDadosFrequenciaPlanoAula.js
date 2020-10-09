import { Switch } from 'antd';
import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { CampoData } from '~/componentes';
import SelectComponent from '~/componentes/select';
import { salvarDadosAulaFrequencia } from '~/redux/modulos/calendarioProfessor/actions';
import {
  limparDadosFrequenciaPlanoAula,
  setAulaIdFrequenciaPlanoAula,
  setComponenteCurricularFrequenciaPlanoAula,
  setDataSelecionadaFrequenciaPlanoAula,
  setExibirLoaderFrequenciaPlanoAula,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import { confirmar, erros, ServicoCalendarios } from '~/servicos';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import ServicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import servicoSalvarFrequenciaPlanoAula from '../../servicoSalvarFrequenciaPlanoAula';

const CamposFiltrarDadosFrequenciaPlanoAula = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    state => state.filtro.modalidades
  );

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

  const [listaComponenteCurricular, setListaComponenteCurricular] = useState(
    []
  );
  const [codigoComponenteCurricular, setCodigoComponenteCurricular] = useState(
    undefined
  );

  const [listaDatasAulas, setListaDatasAulas] = useState();
  const [diasParaHabilitar, setDiasParaHabilitar] = useState();
  const [exibeEscolhaAula, setExibeEscolhaAula] = useState(false);
  const [ehAulaCj, setEhAulaCj] = useState(false);
  const [possuiPlanoAnual, setPossuiPlanoAnual] = useState(true);
  const [aula, setAula] = useState();

  const obterDatasDeAulasDisponiveis = useCallback(async () => {
    dispatch(setExibirLoaderFrequenciaPlanoAula(true));
    const datasDeAulas = await ServicoCalendarios.obterDatasDeAulasDisponiveis(
      turmaSelecionada.anoLetivo,
      turmaSelecionada.turma,
      codigoComponenteCurricular
    )
      .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
      .catch(e => erros(e));

    if (datasDeAulas && datasDeAulas.data && datasDeAulas.data.length) {
      setListaDatasAulas(datasDeAulas.data);
      const habilitar = datasDeAulas.data.map(item =>
        window.moment(item.data).format('YYYY-MM-DD')
      );
      setDiasParaHabilitar(habilitar);
    } else {
      setListaDatasAulas();
      setDiasParaHabilitar();
    }
  }, [turmaSelecionada, codigoComponenteCurricular, dispatch]);

  const obterListaComponenteCurricular = useCallback(async () => {
    dispatch(setExibirLoaderFrequenciaPlanoAula(true));
    const resposta = await ServicoDisciplina.obterDisciplinasPorTurma(
      turmaSelecionada.turma
    )
      .finally(() => dispatch(setExibirLoaderFrequenciaPlanoAula(false)))
      .catch(e => erros(e));

    if (resposta && resposta.data) {
      setListaComponenteCurricular(resposta.data);
      if (resposta.data.length === 1) {
        const componente = resposta.data[0];
        dispatch(setComponenteCurricularFrequenciaPlanoAula(componente));
      }
    } else {
      setListaComponenteCurricular([]);
      dispatch(setComponenteCurricularFrequenciaPlanoAula(undefined));
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
    if (codigoComponenteCurricular) {
      obterDatasDeAulasDisponiveis();
    }
  }, [codigoComponenteCurricular, obterDatasDeAulasDisponiveis]);

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

  const validaSePossuiPlanoAnual = useCallback(() => {
    if (!ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)) {
      ServicoPlanoAnual.obter(
        turmaSelecionada.anoLetivo,
        codigoComponenteCurricular,
        turmaSelecionada.unidadeEscolar,
        turmaSelecionada.turma
      )
        .then(resposta => {
          const planoAnualCadastrado =
            resposta?.data &&
            !!resposta.data.filter(
              plano => plano.id && plano.criadoEm && plano.descricao.length
            ).length;

          setPossuiPlanoAnual(planoAnualCadastrado);
        })
        .catch(e => erros(e));
    }
  }, [
    turmaSelecionada,
    codigoComponenteCurricular,
    modalidadesFiltroPrincipal,
  ]);

  useEffect(() => {
    if (
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) &&
      turmaSelecionada.anoLetivo &&
      codigoComponenteCurricular &&
      turmaSelecionada.unidadeEscolar &&
      turmaSelecionada.turma
    ) {
      validaSePossuiPlanoAnual();
    }
  }, [
    turmaSelecionada,
    codigoComponenteCurricular,
    modalidadesFiltroPrincipal,
    validaSePossuiPlanoAnual,
  ]);

  const obterAulaSelecionada = useCallback(
    async data => {
      if (listaDatasAulas) {
        const aulaDataSelecionada = listaDatasAulas.filter(item => {
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
      ServicoCalendarios.obterListaFrequencia();
    }
  }, [aulaId]);

  const validaSeTemIdAula = useCallback(
    async data => {
      const aulaDataSelecionada = await obterAulaSelecionada(data);

      if (aulaDataSelecionada && aulaDataSelecionada.length) {
        if (
          usuario &&
          !usuario.ehProfessor &&
          !usuario.ehProfessorCj &&
          !usuario.ehProfessorPoa &&
          aulaDataSelecionada.length > 1
        ) {
          setExibeEscolhaAula(true);
        } else {
          const aulaSelecionada = aulaDataSelecionada.find(
            item => item.aulaCJ === usuario.ehProfessorCj
          );

          if (aulaSelecionada) {
            setAula(aulaSelecionada);
            if (aulaSelecionada && aulaSelecionada.idAula) {
              // Após setar o id vai disparar evento para buscar lista de frequencia!
              dispatch(setAulaIdFrequenciaPlanoAula(aulaSelecionada.idAula));
            }
          }
        }
      }
    },
    [obterAulaSelecionada, usuario, dispatch]
  );

  const onChangeData = useCallback(
    async data => {
      // TODO
      // resetarPlanoAula();
      // setAula();
      // TODO VER SE DA PARA REMOVER DAQUI!

      // if (planoAulaExpandido) onClickPlanoAula();

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
        />
      </div>
      <div className="col-sm-12 col-md-4 col-lg-3 col-xl-3 mb-3">
        {exibeEscolhaAula && (
          <>
            <Switch
              onChange={() => setEhAulaCj(!ehAulaCj)}
              checked={ehAulaCj}
              size="small"
              className="mr-2"
            />
            Aula CJ
          </>
        )}
      </div>
    </>
  );
};

export default CamposFiltrarDadosFrequenciaPlanoAula;
