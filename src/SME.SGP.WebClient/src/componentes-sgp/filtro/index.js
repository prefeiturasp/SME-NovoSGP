import React, { useState, useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { store } from '~/redux';
import {
  selecionarTurma,
  turmasUsuario,
  removerTurma,
} from '~/redux/modulos/usuario/actions';
import Grid from '~/componentes/grid';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import api from '~/servicos/api';
import {
  Container,
  Campo,
  Busca,
  Fechar,
  SetaFunction,
  ItemLista,
} from './index.css';
import {
  salvarAnosLetivos,
  salvarModalidades,
  salvarPeriodos,
  salvarDres,
  salvarUnidadesEscolares,
  salvarTurmas,
} from '~/redux/modulos/filtro/actions';
import FiltroHelper from './helper';
import { erro } from '~/servicos/alertas';

const Filtro = () => {
  const [alternarFocoCampo, setAlternarFocoCampo] = useState(false);
  const [alternarFocoBusca, setAlternarFocoBusca] = useState(false);

  const Seta = SetaFunction(alternarFocoBusca);

  const divBuscaRef = useRef();
  const campoBuscaRef = useRef();

  const usuarioStore = useSelector(state => state.usuario);
  const [turmaUsuarioSelecionada, setTurmaUsuarioSelecionada] = useState(
    usuarioStore.turmaSelecionada
  );

  const [campoAnoLetivoDesabilitado, setCampoAnoLetivoDesabilitado] = useState(
    true
  );
  const [
    campoModalidadeDesabilitado,
    setCampoModalidadeDesabilitado,
  ] = useState(true);
  const [campoPeriodoDesabilitado, setCampoPeriodoDesabilitado] = useState(
    true
  );
  const [campoDreDesabilitado, setCampoDreDesabilitado] = useState(true);
  const [
    campoUnidadeEscolarDesabilitado,
    setCampoUnidadeEscolarDesabilitado,
  ] = useState(true);
  const [campoTurmaDesabilitado, setCampoTurmaDesabilitado] = useState(true);

  const anosLetivoStore = useSelector(state => state.filtro.anosLetivos);
  const [anosLetivos, setAnosLetivos] = useState(anosLetivoStore);
  const [anoLetivoSelecionado, setAnoLetivoSelecionado] = useState(
    turmaUsuarioSelecionada.anoLetivo
  );

  const modalidadesStore = useSelector(state => state.filtro.modalidades);
  const [modalidades, setModalidades] = useState(modalidadesStore);
  const [modalidadeSelecionada, setModalidadeSelecionada] = useState(
    turmaUsuarioSelecionada.modalidade
  );

  const periodosStore = useSelector(state => state.filtro.periodos);
  const [periodos, setPeriodos] = useState(periodosStore);
  const [periodoSelecionado, setPeriodoSelecionado] = useState();

  const dresStore = useSelector(state => state.filtro.dres);
  const [dres, setDres] = useState(dresStore);
  const [dreSelecionada, setDreSelecionada] = useState(
    turmaUsuarioSelecionada.dre
  );

  const unidadesEscolaresStore = useSelector(
    state => state.filtro.unidadesEscolares
  );
  const [unidadesEscolares, setUnidadesEscolares] = useState(
    unidadesEscolaresStore
  );
  const [unidadeEscolarSelecionada, setUnidadeEscolarSelecionada] = useState(
    turmaUsuarioSelecionada.unidadeEscolar
  );

  const turmasStore = useSelector(state => state.filtro.turmas);
  const [turmas, setTurmas] = useState(turmasStore);
  const [turmaSelecionada, setTurmaSelecionada] = useState(
    turmaUsuarioSelecionada.turma
  );

  const [textoAutocomplete, setTextoAutocomplete] = useState(
    turmaUsuarioSelecionada.desc
  );
  const [resultadosFiltro, setResultadosFiltro] = useState([]);

  useEffect(() => {
    let estado = true;

    ObtenhaAnosLetivos(estado);

    return () => (estado = false);
  }, []);

  useEffect(() => {
    let estado = true;
    if (
      estado &&
      typeof turmaUsuarioSelecionada === 'object' &&
      Object.keys(turmaUsuarioSelecionada).length > 0
    ) {
      setAnoLetivoSelecionado(turmaUsuarioSelecionada.anoLetivo);
      setModalidadeSelecionada(turmaUsuarioSelecionada.modalidade.toString());
      setDreSelecionada(turmaUsuarioSelecionada.dre);
      setUnidadeEscolarSelecionada(turmaUsuarioSelecionada.unidadeEscolar);
      setTurmaSelecionada(turmaUsuarioSelecionada.turma);
      setTextoAutocomplete(turmaUsuarioSelecionada.desc);
    }
    return () => (estado = false);
  }, [turmaUsuarioSelecionada]);

  useEffect(() => {
    if (anosLetivos && anosLetivos.length === 1) {
      setAnoLetivoSelecionado(anosLetivos[0].valor);
    }
  }, [anosLetivos]);

  useEffect(() => {
    let estado = true;

    if (!anoLetivoSelecionado) {
      setModalidadeSelecionada();
      setCampoModalidadeDesabilitado(true);
      return;
    }

    ObtenhaModalidades(estado);

    return () => (estado = false);
  }, [anoLetivoSelecionado]);

  useEffect(() => {
    if (modalidades && modalidades.length === 1)
      setModalidadeSelecionada(modalidades[0].valor);
  }, [modalidadesStore]);

  useEffect(() => {
    let estado = true;

    if (!modalidadeSelecionada) {
      setPeriodoSelecionado();
      setDreSelecionada();
      setCampoPeriodoDesabilitado(true);
      setCampoDreDesabilitado(true);
      return;
    }

    ObtenhaPeriodos(estado);
    ObtenhaDres(estado);

    return () => (estado = false);
  }, [modalidadeSelecionada]);

  useEffect(() => {
    if (dres && dres.length === 1) setDreSelecionada(dres[0].valor);
  }, [dres]);

  useEffect(() => {
    let estado = true;

    if (!dreSelecionada) {
      setUnidadeEscolarSelecionada();
      setCampoUnidadeEscolarDesabilitado(true);
      return;
    }

    ObtenhaUnidadesEscolares(estado);

    return () => (estado = false);
  }, [dreSelecionada]);

  useEffect(() => {
    if (unidadesEscolares && unidadesEscolares.length === 1)
      setUnidadeEscolarSelecionada(unidadesEscolares[0].valor);
  }, [unidadesEscolares]);

  useEffect(() => {
    let estado = true;

    if (!unidadeEscolarSelecionada) {
      setTurmaSelecionada();
      setCampoTurmaDesabilitado(true);
      return;
    }

    ObtenhaTurmas(estado);

    return () => (estado = false);
  }, [unidadeEscolarSelecionada]);

  useEffect(() => {
    if (turmas && turmas.length === 1) setTurmaSelecionada(turmas[0].valor);
  }, [turmas]);

  const aplicarFiltro = () => {
    if (
      anoLetivoSelecionado &&
      modalidadeSelecionada &&
      dreSelecionada &&
      unidadeEscolarSelecionada &&
      turmaSelecionada
    ) {
      const modalidadeDesc = modalidades.find(
        modalidade => modalidade.valor.toString() === `${modalidadeSelecionada}`
      );

      const turmaDesc = turmas.find(turma => turma.valor === turmaSelecionada);

      const unidadeEscolarDesc = unidadesEscolares.find(
        unidade => unidade.valor === unidadeEscolarSelecionada
      );
      setTurmaUsuarioSelecionada(
        `${modalidadeDesc.desc} - ${turmaDesc.desc} - ${unidadeEscolarDesc.desc}`
      );
      setAlternarFocoBusca(false);

      const turmaSelecionadaCompleta = turmas.find(
        t => t.valor == turmaSelecionada
      );

      const turma = {
        anoLetivo: anoLetivoSelecionado,
        modalidade: modalidadeSelecionada,
        dre: dreSelecionada,
        unidadeEscolar: unidadeEscolarSelecionada,
        turma: turmaSelecionada,
        ano: turmaSelecionadaCompleta.ano,
        desc: `${modalidadeDesc.desc} - ${turmaDesc.desc} - ${unidadeEscolarDesc.desc}`,
      };

      store.dispatch(turmasUsuario(turmas));
      store.dispatch(selecionarTurma(turma));

      setTextoAutocomplete(turma.desc);
    }
  };

  const mostrarEsconderBusca = () => {
    setAlternarFocoBusca(!alternarFocoBusca);
    setAlternarFocoCampo(false);
  };

  const controlaClickFora = evento => {
    if (
      !evento.target.nodeName === 'svg' &&
      !evento.target.nodeName === 'path' &&
      !evento.target.classList.contains('fa-caret-down') &&
      !evento.target.classList.contains('ant-select-dropdown-menu-item') &&
      !evento.target.classList.contains(
        'ant-select-dropdown-menu-item-active'
      ) &&
      !evento.target.classList.contains('ant-select-selection__placeholder') &&
      !evento.target.classList.contains(
        'ant-select-selection-selected-value'
      ) &&
      !evento.target.classList.contains(
        'ant-select-dropdown-menu-item-selected'
      ) &&
      divBuscaRef.current &&
      !divBuscaRef.current.contains(evento.target)
    )
      mostrarEsconderBusca();
  };

  useEffect(() => {
    if (!turmaUsuarioSelecionada && !alternarFocoBusca && alternarFocoCampo)
      campoBuscaRef.current.focus();
    if (alternarFocoBusca)
      document.addEventListener('click', controlaClickFora);
    return () => document.removeEventListener('click', controlaClickFora);
  }, [alternarFocoBusca, alternarFocoCampo]);

  useEffect(() => {
    if (!turmaUsuarioSelecionada) campoBuscaRef.current.focus();
    if (!textoAutocomplete) setResultadosFiltro([]);
  }, [textoAutocomplete]);

  useEffect(() => {
    if (!turmaUsuarioSelecionada) campoBuscaRef.current.focus();
  }, [resultadosFiltro]);

  const onChangeAutocomplete = () => {
    const texto = campoBuscaRef.current.value;
    setTextoAutocomplete(texto);

    if (texto.length >= 2) {
      api.get(`v1/abrangencias/${texto}`).then(resposta => {
        if (resposta.data) {
          setResultadosFiltro(resposta.data);
        }
      });
    }
  };

  const ObtenhaAnosLetivos = async estado => {
    const anosLetivos = await FiltroHelper.ObtenhaAnosLetivos();

    if (estado) {
      store.dispatch(salvarAnosLetivos(anosLetivos));
      setAnosLetivos(anosLetivos);
      setCampoAnoLetivoDesabilitado(false);
    }
  };

  const ObtenhaModalidades = async estado => {
    const modalidadesLista = await FiltroHelper.ObtenhaModalidades();

    if (estado) {
      setModalidades([...modalidadesLista]);
      store.dispatch(salvarModalidades(modalidadesLista));
      setCampoModalidadeDesabilitado(false);
    }
  };

  const ObtenhaPeriodos = async estado => {
    const periodos = await FiltroHelper.ObtenhaPeriodos(modalidadeSelecionada);

    if (estado) {
      store.dispatch(salvarPeriodos(periodos));
      setPeriodos(periodos);
      setCampoPeriodoDesabilitado(false);
    }
  };

  const ObtenhaDres = async estado => {
    const dres = await FiltroHelper.ObtenhaDres(modalidadeSelecionada);

    if (estado) {
      store.dispatch(salvarDres(dres));
      setDres(dres);
      setCampoDreDesabilitado(false);
    }
  };

  const ObtenhaUnidadesEscolares = async estado => {
    const unidadesEscolares = await FiltroHelper.ObtenhaUnidadesEscolares(
      modalidadeSelecionada,
      dreSelecionada
    );

    if (!unidadesEscolares) {
      setDreSelecionada();
      setCampoDreDesabilitado(true);
      erro('Esta DRE não possui unidades escolares da modalidade escolhida');
      return;
    }

    if (estado) {
      store.dispatch(salvarUnidadesEscolares(unidadesEscolares));
      setUnidadesEscolares(unidadesEscolares);
      setCampoUnidadeEscolarDesabilitado(false);
    }
  };

  const ObtenhaTurmas = async estado => {
    const turmas = await FiltroHelper.ObtenhaTurmas(
      modalidadeSelecionada,
      unidadeEscolarSelecionada
    );

    if (!turmas || turmas.length === 0) {
      setUnidadeEscolarSelecionada();
      setCampoUnidadeEscolarDesabilitado(true);
      erro('Esta unidade escolar não possui turmas da modalidade escolhida');
      return;
    }

    if (estado) {
      store.dispatch(salvarTurmas(turmas));
      setTurmas(turmas);
      setCampoTurmaDesabilitado(false);
    }
  };

  const selecionaTurmaAutocomplete = resultado => {
    setTurmaUsuarioSelecionada(resultado.descricaoFiltro);
    const turma = {
      anoLetivo: resultado.anoLetivo,
      modalidade: resultado.codigoModalidade,
      dre: resultado.codigoDre,
      unidadeEscolar: resultado.codigoUe,
      turma: resultado.codigoTurma,
      desc: resultado.descricaoFiltro,
    };
    store.dispatch(selecionarTurma(turma));
    store.dispatch(turmasUsuario(turmas));
    setResultadosFiltro([]);
  };

  let selecionado = -1;

  const aoPressionarTeclaBaixoAutocomplete = evento => {
    if (resultadosFiltro && resultadosFiltro.length > 0) {
      const resultados = document.querySelectorAll('.list-group-item');
      if (resultados && resultados.length > 0) {
        if (evento.key === 'ArrowUp') {
          if (selecionado > 0) selecionado -= 1;
        } else if (evento.key === 'ArrowDown') {
          if (selecionado < resultados.length - 1) selecionado += 1;
        }
        resultados.forEach(resultado =>
          resultado.classList.remove('selecionado')
        );
        if (resultados[selecionado]) {
          resultados[selecionado].classList.add('selecionado');
          campoBuscaRef.current.focus();
        }
      }
    }
  };

  const aoSubmeterAutocomplete = evento => {
    evento.preventDefault();
    if (resultadosFiltro) {
      if (resultadosFiltro.length === 1) {
        setModalidadeSelecionada(
          resultadosFiltro[0].codigoModalidade.toString()
        );
        setDreSelecionada(resultadosFiltro[0].codigoDre);
        setUnidadeEscolarSelecionada(resultadosFiltro[0].codigoUe);
        setTimeout(() => {
          setTurmaSelecionada(resultadosFiltro[0].codigoTurma);
        }, 1000);
        selecionaTurmaAutocomplete(resultadosFiltro[0]);
      } else {
        const itemSelecionado = document.querySelector(
          '.list-group-item.selecionado'
        );
        if (itemSelecionado) {
          const indice = itemSelecionado.getAttribute('tabindex');
          if (indice) {
            const resultado = resultadosFiltro[indice];
            if (resultado) {
              setModalidadeSelecionada(resultado.codigoModalidade.toString());
              setDreSelecionada(resultado.codigoDre);
              setUnidadeEscolarSelecionada(resultado.codigoUe);
              setTimeout(() => {
                setTurmaSelecionada(resultado.codigoTurma);
              }, 1000);
              selecionaTurmaAutocomplete(resultado);
            }
          }
        }
      }
    }
  };

  const aoFocarBusca = () => {
    if (alternarFocoBusca) {
      setAlternarFocoBusca(false);
      setAlternarFocoCampo(true);
    }
  };

  const aoTrocarAnoLetivo = ano => {
    if (ano !== anoLetivoSelecionado) setModalidadeSelecionada();

    setAnoLetivoSelecionado(ano);
  };

  const aoTrocarModalidade = modalidade => {
    if (modalidade !== modalidadeSelecionada) {
      setDreSelecionada();
      setPeriodoSelecionado();
    }

    setModalidadeSelecionada(modalidade);
  };

  const aoTrocarPeriodo = periodo => {
    setPeriodoSelecionado(periodo);
  };

  const aoTrocarDre = dre => {
    if (dre !== dreSelecionada) setUnidadeEscolarSelecionada();

    setDreSelecionada(dre);
  };

  const aoTrocarUnidadeEscolar = unidade => {
    if (unidade !== unidadeEscolarSelecionada) setTurmaSelecionada();

    setUnidadeEscolarSelecionada(unidade);
  };

  const aoTrocarTurma = turma => {
    setTurmaSelecionada(turma);
  };

  const removerTurmaSelecionada = () => {
    store.dispatch(removerTurma());
    setTextoAutocomplete();
    setModalidadeSelecionada();
    setPeriodoSelecionado();
    setDreSelecionada();
    setUnidadeEscolarSelecionada();
    setTurmaSelecionada();
    setTurmaUsuarioSelecionada();
  };

  return (
    <Container className="position-relative w-100">
      <form className="w-100" onSubmit={aoSubmeterAutocomplete}>
        <div className="form-group mb-0 w-100 position-relative">
          <Busca className="fa fa-search fa-lg bg-transparent position-absolute text-center" />
          <Campo
            type="text"
            className="form-control form-control-lg rounded d-flex px-5 border-0 fonte-14"
            placeholder="Pesquisar Turma"
            ref={campoBuscaRef}
            onFocus={aoFocarBusca}
            onChange={onChangeAutocomplete}
            onKeyDown={aoPressionarTeclaBaixoAutocomplete}
            readOnly={!!turmaUsuarioSelecionada}
            value={textoAutocomplete}
          />
          {turmaUsuarioSelecionada && turmaUsuarioSelecionada.length > 0 && (
            <Fechar
              className="fa fa-times position-absolute"
              onClick={removerTurmaSelecionada}
            />
          )}
          <Seta
            className="fa fa-caret-down rounded-circle position-absolute text-center"
            onClick={mostrarEsconderBusca}
          />
        </div>
        {resultadosFiltro.length > 0 && (
          <div className="container position-absolute bg-white shadow rounded mt-1 p-0">
            <div className="list-group">
              {resultadosFiltro.map((resultado, indice) => {
                return (
                  <ItemLista
                    key={shortid.generate()}
                    className="list-group-item list-group-item-action border-0 rounded-0"
                    onClick={() => selecionaTurmaAutocomplete(resultado)}
                    tabIndex={indice}
                  >
                    {resultado.descricaoFiltro}
                  </ItemLista>
                );
              })}
            </div>
          </div>
        )}
        {alternarFocoBusca && (
          <div
            ref={divBuscaRef}
            className="container position-absolute bg-white shadow rounded mt-1 px-3 pt-5 pb-1"
          >
            <div className="form-row">
              <Grid cols={3} className="form-group">
                <SelectComponent
                  className="fonte-14"
                  onChange={aoTrocarAnoLetivo}
                  lista={anosLetivos}
                  valueOption="valor"
                  valueText="desc"
                  valueSelect={anoLetivoSelecionado}
                  placeholder="Ano"
                  disabled={campoAnoLetivoDesabilitado}
                />
              </Grid>
              <Grid
                cols={modalidadeSelecionada === '3' ? 5 : 9}
                className="form-group"
              >
                <SelectComponent
                  className="fonte-14"
                  onChange={aoTrocarModalidade}
                  lista={modalidades}
                  valueOption="valor"
                  valueText="desc"
                  valueSelect={modalidadeSelecionada}
                  placeholder="Modalidade"
                  disabled={campoModalidadeDesabilitado}
                />
              </Grid>
              {modalidadeSelecionada === '3' && (
                <Grid cols={4} className="form-group">
                  <SelectComponent
                    className="fonte-14"
                    onChange={aoTrocarPeriodo}
                    lista={periodos}
                    valueOption="valor"
                    valueText="desc"
                    valueSelect={periodoSelecionado}
                    placeholder="Período"
                    disabled={campoPeriodoDesabilitado}
                  />
                </Grid>
              )}
            </div>
            <div className="form-group">
              <SelectComponent
                className="fonte-14"
                onChange={aoTrocarDre}
                lista={dres}
                valueOption="valor"
                valueText="desc"
                valueSelect={dreSelecionada}
                placeholder="Diretoria Regional De Educação (DRE)"
                disabled={campoDreDesabilitado}
              />
            </div>
            <div className="form-group">
              <SelectComponent
                className="fonte-14"
                onChange={aoTrocarUnidadeEscolar}
                lista={unidadesEscolares}
                valueOption="valor"
                valueText="desc"
                valueSelect={unidadeEscolarSelecionada}
                placeholder="Unidade Escolar (UE)"
                disabled={campoUnidadeEscolarDesabilitado}
              />
            </div>
            <div className="form-row d-flex justify-content-between">
              <Grid cols={3} className="form-group">
                <SelectComponent
                  className="fonte-14"
                  onChange={aoTrocarTurma}
                  lista={turmas}
                  valueOption="valor"
                  valueText="desc"
                  valueSelect={turmaSelecionada}
                  placeholder="Turma"
                  disabled={campoTurmaDesabilitado}
                />
              </Grid>
              <Grid cols={3} className="form-group text-right">
                <Button
                  label="Aplicar filtro"
                  color={Colors.Roxo}
                  bold
                  onClick={aplicarFiltro}
                />
              </Grid>
            </div>
          </div>
        )}
      </form>
    </Container>
  );
};

export default Filtro;
