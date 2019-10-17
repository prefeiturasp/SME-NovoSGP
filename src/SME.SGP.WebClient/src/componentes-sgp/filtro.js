import React, { useState, useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import shortid from 'shortid';
import { store } from '../redux';
import {
  selecionarTurma,
  removerTurma,
} from '../redux/modulos/usuario/actions';
import Grid from '../componentes/grid';
import Button from '../componentes/button';
import { Base, Colors } from '../componentes/colors';
import SelectComponent from '../componentes/select';
import api from '../servicos/api';
import {
  salvarAnosLetivos,
  salvarModalidades,
  salvarPeriodos,
  salvarDres,
  salvarUnidadesEscolares,
  salvarTurmas,
} from '~/redux/modulos/filtro/actions';

const Filtro = () => {
  const [alternarFocoCampo, setAlternarFocoCampo] = useState(false);
  const [alternarFocoBusca, setAlternarFocoBusca] = useState(false);

  const Container = styled.div`
    width: 568px !important;
    z-index: 100;
    @media (max-width: 575.98px) {
      max-width: 80%;
    }
  `;

  const Campo = styled.input`
    background: ${Base.CinzaFundo};
    font-weight: bold;
    height: 45px;
    &::placeholder {
      font-weight: normal;
    }
    &:focus {
      background: ${Base.Branco};
      box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
      color ${Base.Preto};
      font-weight: normal;
      &:read-only {
        background: ${Base.CinzaFundo};
        font-weight: bold;
        box-shadow: none;
      }
    }
  `;

  const Icone = styled.i`
    color: ${Base.CinzaMako};
    cursor: pointer;
  `;

  const Busca = styled(Icone)`
    left: 0;
    max-height: 23px;
    max-width: 14px;
    padding: 1rem;
    right: 0;
    top: 0;
  `;

  const Fechar = styled(Icone)`
    right: 50px;
    top: 15px;
  `;

  const Seta = styled(Icone)`
    background: ${Base.CinzaDesabilitado};
    max-height: 36px;
    max-width: 36px;
    padding: 0.7rem 0.9rem;
    right: 5px;
    top: 5px;
    transition: transform 0.3s;
    ${alternarFocoBusca && 'transform: rotate(180deg);'}
  `;

  const ItemLista = styled.li`
    cursor: pointer;
    &:hover,
    &:focus,
    &.selecionado {
      background: ${Base.Roxo};
      color: ${Base.Branco};
    }
  `;

  const divBuscaRef = useRef();
  const campoBuscaRef = useRef();

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
  const [anoLetivoSelecionado, setAnoLetivoSelecionado] = useState();

  useEffect(() => {
    const anosLetivos = [];
    api.get('v1/abrangencia/anos-letivos').then(resposta => {
      if (resposta.data) {
        resposta.data.forEach(ano => {
          anosLetivos.push({ desc: ano, valor: ano });
        });
        store.dispatch(salvarAnosLetivos(anosLetivos));
        setAnosLetivos(anosLetivos);
        setCampoAnoLetivoDesabilitado(false);
      }
    });
  }, []);

  useEffect(() => {
    if (anosLetivos && anosLetivos.length === 1)
      setAnoLetivoSelecionado(anosLetivos[0].valor);
  }, [anosLetivos]);

  const modalidadesStore = useSelector(state => state.filtro.modalidades);
  const [modalidades, setModalidades] = useState(modalidadesStore);
  const [modalidadeSelecionada, setModalidadeSelecionada] = useState();

  useEffect(() => {
    if (anoLetivoSelecionado) {
      const modalidades = [];
      api.get('v1/abrangencia/modalidades').then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(modalidade => {
            modalidades.push({
              desc: modalidade.descricao,
              valor: modalidade.id,
            });
          });
          store.dispatch(salvarModalidades(modalidades));
          setModalidades(modalidades);
          setCampoModalidadeDesabilitado(false);
        }
      });
    } else {
      setModalidadeSelecionada();
      setCampoModalidadeDesabilitado(true);
    }
  }, [anoLetivoSelecionado]);

  useEffect(() => {
    if (modalidades && modalidades.length === 1)
      setModalidadeSelecionada(modalidades[0].valor);
  }, [modalidades]);

  const periodosStore = useSelector(state => state.filtro.periodos);
  const [periodos, setPeriodos] = useState(periodosStore);
  const [periodoSelecionado, setPeriodoSelecionado] = useState();

  const dresStore = useSelector(state => state.filtro.dres);
  const [dres, setDres] = useState(dresStore);
  const [dreSelecionada, setDreSelecionada] = useState();

  useEffect(() => {
    if (modalidadeSelecionada) {
      const periodos = [];
      api.get('v1/abrangencia/semestres').then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(periodo => {
            periodos.push({ desc: periodo, valor: periodo });
          });
          store.dispatch(salvarPeriodos(periodos));
          setPeriodos(periodos);
          setCampoPeriodoDesabilitado(false);
        }
      });

      const dres = [];
      api.get('v1/abrangencia/dres').then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(dre => {
            dres.push({
              desc: dre.nome,
              valor: dre.codigo,
              abrev: dre.abreviacao,
            });
          });
          store.dispatch(salvarDres(dres));
          setDres(dres);
          setCampoDreDesabilitado(false);
        }
      });
    } else {
      setPeriodoSelecionado();
      setCampoPeriodoDesabilitado(true);
      setDreSelecionada();
      setCampoDreDesabilitado(true);
    }
  }, [modalidadeSelecionada]);

  useEffect(() => {
    if (dres && dres.length === 1) setDreSelecionada(dres[0].valor);
  }, [dres]);

  const unidadesEscolaresStore = useSelector(
    state => state.filtro.unidadesEscolares
  );
  const [unidadesEscolares, setUnidadesEscolares] = useState(
    unidadesEscolaresStore
  );
  const [unidadeEscolarSelecionada, setUnidadeEscolarSelecionada] = useState();

  useEffect(() => {
    if (dreSelecionada) {
      const unidadesEscolares = [];
      api.get(`v1/abrangencia/dres/${dreSelecionada}/ues`).then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(unidade => {
            unidadesEscolares.push({
              desc: unidade.nome,
              valor: unidade.codigo,
            });
          });
          store.dispatch(salvarUnidadesEscolares(unidadesEscolares));
          setUnidadesEscolares(unidadesEscolares);
          setCampoUnidadeEscolarDesabilitado(false);
        }
      });
    } else {
      setUnidadeEscolarSelecionada();
      setCampoUnidadeEscolarDesabilitado(true);
    }
  }, [dreSelecionada]);

  useEffect(() => {
    if (unidadesEscolares && unidadesEscolares.length === 1)
      setUnidadeEscolarSelecionada(unidadesEscolares[0].valor);
  }, [unidadesEscolares]);

  const turmasStore = useSelector(state => state.filtro.turmas);
  const [turmas, setTurmas] = useState(turmasStore);
  const [turmaSelecionada, setTurmaSelecionada] = useState();

  useEffect(() => {
    if (unidadeEscolarSelecionada) {
      const turmas = [];
      api
        .get(`v1/abrangencia/dres/ues/${unidadeEscolarSelecionada}/turmas`)
        .then(resposta => {
          if (resposta.data) {
            resposta.data.forEach(turma => {
              turmas.push({ desc: turma.nome, valor: turma.codigo });
            });
            store.dispatch(salvarTurmas(turmas));
            setTurmas(turmas);
            setCampoTurmaDesabilitado(false);
          }
        });
    } else {
      setTurmaSelecionada();
      setCampoTurmaDesabilitado(true);
    }
  }, [unidadeEscolarSelecionada]);

  useEffect(() => {
    if (turmas && turmas.length === 1) setTurmaSelecionada(turmas[0].valor);
  }, [turmas]);

  const usuarioStore = useSelector(state => state.usuario);
  const [turmaUsuarioSelecionada, setTurmaUsuarioSelecionada] = useState(
    usuarioStore.turmaSelecionada
  );

  useEffect(() => {
    if (typeof turmaUsuarioSelecionada === 'object') {
      setAnoLetivoSelecionado(turmaUsuarioSelecionada.anoLetivo);
      setModalidadeSelecionada(turmaUsuarioSelecionada.modalidade.toString());
      setDreSelecionada(turmaUsuarioSelecionada.dre);
      setUnidadeEscolarSelecionada(turmaUsuarioSelecionada.unidadeEscolar);
      setTurmaSelecionada(turmaUsuarioSelecionada.turma);
      setTurmaUsuarioSelecionada(turmaUsuarioSelecionada.desc);
    }
  }, [turmaUsuarioSelecionada]);

  const aplicarFiltro = () => {
    if (
      anoLetivoSelecionado &&
      modalidadeSelecionada &&
      dreSelecionada &&
      unidadeEscolarSelecionada &&
      turmaSelecionada
    ) {
      const modalidadeDesc = modalidades.filter(
        modalidade => modalidade.valor.toString() === modalidadeSelecionada
      );
      const turmaDesc = turmas.filter(
        turma => turma.valor === turmaSelecionada
      );
      const unidadeEscolarDesc = unidadesEscolares.filter(
        unidade => unidade.valor === unidadeEscolarSelecionada
      );
      setTurmaUsuarioSelecionada(
        `${modalidadeDesc[0].desc} - ${turmaDesc[0].desc} - ${unidadeEscolarDesc[0].desc}`
      );
      setAlternarFocoBusca(false);
      const turma = {
        anoLetivo: anoLetivoSelecionado,
        modalidade: modalidadeSelecionada,
        dre: dreSelecionada,
        unidadeEscolar: unidadeEscolarSelecionada,
        turma: turmaSelecionada,
        desc: `${modalidadeDesc[0].desc} - ${turmaDesc[0].desc} - ${unidadeEscolarDesc[0].desc}`,
      };
      store.dispatch(selecionarTurma(turma));
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
    if (!alternarFocoBusca && alternarFocoCampo) campoBuscaRef.current.focus();
    if (alternarFocoBusca)
      document.addEventListener('click', controlaClickFora);
    return () => document.removeEventListener('click', controlaClickFora);
  }, [alternarFocoBusca, alternarFocoCampo]);

  const [textoAutocomplete, setTextoAutocomplete] = useState();
  const [resultadosFiltro, setResultadosFiltro] = useState([]);

  useEffect(() => {
    campoBuscaRef.current.focus();
    if (!textoAutocomplete) setResultadosFiltro([]);
  }, [textoAutocomplete]);

  useEffect(() => {
    campoBuscaRef.current.focus();
  }, [resultadosFiltro]);

  const onChangeAutocomplete = () => {
    const texto = campoBuscaRef.current.value;
    setTextoAutocomplete(texto);

    if (texto.length >= 2) {
      api.get(`v1/abrangencia/${texto}`).then(resposta => {
        if (resposta.data) {
          setResultadosFiltro(resposta.data);
        }
      });
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
    setAnoLetivoSelecionado(ano);
  };

  const aoTrocarModalidade = modalidade => {
    setModalidadeSelecionada(modalidade);
  };

  const aoTrocarPeriodo = periodo => {
    setPeriodoSelecionado(periodo);
  };

  const aoTrocarDre = dre => {
    setDreSelecionada(dre);
  };

  const aoTrocarUnidadeEscolar = unidade => {
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
            value={turmaUsuarioSelecionada || textoAutocomplete}
          />
          {turmaUsuarioSelecionada && (
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
