import React, { useState, useEffect, useLayoutEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import shortid from 'shortid';
import { store } from '../redux';
import {
  turmasUsuario,
  selecionarTurma,
  removerTurma,
} from '../redux/modulos/usuario/actions';
import Grid from '../componentes/grid';
import Button from '../componentes/button';
import { Base, Colors } from '../componentes/colors';
import SelectComponent from '../componentes/select';
import { sucesso, erro } from '../servicos/alertas';
import api from '../servicos/api';
import modalidade from '~/dtos/modalidade';

const Filtro = () => {
  const [dados, setDados] = useState([]);

  const [anosLetivosFiltro, setAnosLetivosFiltro] = useState([]);
  const [
    anoLetivoFiltroSelecionado,
    setAnoLetivoFiltroSelecionado,
  ] = useState();

  const [modalidadesFiltro, setModalidadesFiltro] = useState([]);
  const [
    modalidadeFiltroSelecionada,
    setModalidadeFiltroSelecionada,
  ] = useState();

  const [periodosFiltro, setPeriodosFiltro] = useState([]);
  const [periodoFiltroSelecionado, setPeriodoFiltroSelecionado] = useState();

  const [dresFiltro, setDresFiltro] = useState([]);
  const [dreFiltroSelecionada, setDreFiltroSelecionada] = useState();

  const [unidadesEscolaresFiltro, setUnidadesEscolaresFiltro] = useState([]);
  const [
    unidadeEscolarFiltroSelecionada,
    setUnidadeEscolarFiltroSelecionada,
  ] = useState();

  const [turmaFiltroSelecionada, setTurmaFiltroSelecionada] = useState();

  const [resultadosFiltro, setResultadosFiltro] = useState([]);

  const [toggleInputFocus, setToggleInputFocus] = useState(false);
  const [toggleBusca, setToggleBusca] = useState(false);

  const [turmaUeSelecionada, setTurmaUeSelecionada] = useState();

  const [desabilitarTurma, setDesabilitarTurma] = useState(true);

  const Container = styled.div`
    width: 568px !important;
    @media (max-width: 575.98px) {
      max-width: 80% !important;
    }
  `;

  const Input = styled.input`
    background: ${Base.CinzaFundo} !important;
    font-weight: bold !important;
    height: 45px !important;
    &::placeholder {
      font-weight: normal !important;
    }
    &:focus {
      background: ${Base.Branco} !important;
      box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075) !important;
      color ${Base.Preto} !important;
      font-weight: normal !important;
      &:read-only {
        background: ${Base.CinzaFundo} !important;
        font-weight: bold !important;
        box-shadow: none !important;
      }
    }
  `;

  const Icon = styled.i`
    color: ${Base.CinzaMako} !important;
    cursor: pointer !important;
  `;

  const Search = styled(Icon)`
    left: 0 !important;
    max-height: 23px !important;
    max-width: 14px !important;
    padding: 1rem !important;
    right: 0 !important;
    top: 0 !important;
  `;

  const Times = styled(Icon)`
    right: 50px !important;
    top: 15px !important;
  `;

  const Caret = styled(Icon)`
    background: ${Base.CinzaDesabilitado} !important;
    max-height: 36px !important;
    max-width: 36px !important;
    padding: 0.7rem 0.9rem !important;
    right: 5px !important;
    top: 5px !important;
    ${toggleBusca && 'transform: rotate(180deg) !important;'}
  `;

  const ListItem = styled.li`
    cursor: pointer !important;
    &:hover,
    &:focus,
    &.selecionado {
      background: ${Base.Roxo} !important;
      color: ${Base.Branco} !important;
    }
  `;

  const inputBuscaRef = useRef();
  const [textoAutocomplete, setTextoAutocomplete] = useState();

  const divBuscaRef = useRef();

  const usuario = useSelector(state => state.usuario);

  const buscaDadosPoRf = async rf => {
    await api.get(`v1/professores/${rf}/turmas`).then(res => {
      if (res.data.length > 0) setDados(res.data);
      else erro('Usuário sem turmas atribuídas!');
    });
  };

  useEffect(() => {
    if (usuario.rf.length > 0) {
      if (dados.length === 0) buscaDadosPoRf(usuario.rf);
    }
    if (usuario.turmaSelecionada.length > 0) {
      const {
        modalidade,
        nomeTurma,
        tipoEscola,
        ue,
      } = usuario.turmaSelecionada[0];
      const selecionada = `${modalidade.trim()} - ${nomeTurma.trim()} - ${tipoEscola.trim()} - ${ue.trim()}`;
      setTurmaUeSelecionada(selecionada);
    }
  }, [usuario.turmaSelecionada, usuario.rf]);

  const ordenaTurmas = (x, y) => {
    const a = x.turma.toLowerCase();
    const b = y.turma.toLowerCase();

    if (a > b) return 1;
    if (a < b) return -1;

    return 0;
  };

  useEffect(() => {
    const anosLetivos = [];
    const modalidades = [];
    const periodos = [];
    const dres = [];
    const unidadesEscolares = [];
    const turmas = [];

    dados.forEach(dado => {
      if (anosLetivos.findIndex(ano => ano.ano === dado.anoLetivo) < 0) {
        anosLetivos.push({
          ano: dado.anoLetivo,
        });
      }

      if (
        modalidades.findIndex(
          modalidade => modalidade.codigo === dado.codModalidade
        ) < 0
      ) {
        modalidades.push({
          codigo: dado.codModalidade,
          modalidade: dado.modalidade,
        });
      }

      if (dado.semestre === 2) {
        for (let semestre = 1; semestre <= dado.semestre; semestre += 1) {
          if (periodos.findIndex(periodo => periodo.codigo === semestre) < 0) {
            periodos.push({
              codigo: semestre,
              periodo: `${semestre}º Semestre`,
            });
          }
        }
      }

      if (dres.findIndex(dre => dre.codigo === dado.codDre) < 0) {
        dres.push({
          codigo: dado.codDre,
          dre: dado.dre,
        });
      }

      if (
        unidadesEscolares.findIndex(
          unidade => unidade.codigo === dado.codEscola
        ) < 0
      ) {
        unidadesEscolares.push({
          codigo: dado.codEscola,
          unidade: dado.ue,
        });
      }

      if (turmas.findIndex(turma => turma.codigo === dado.codTurma) < 0) {
        turmas.push({
          codigo: dado.codTurma,
          ano: dado.ano,
          turma: dado.nomeTurma,
          codEscola: dado.codEscola,
        });
      }
    });

    setAnosLetivosFiltro([...anosLetivos]);
    if (anosLetivos.length > 0) setAnoLetivoFiltroSelecionado('2019');

    setModalidadesFiltro([...modalidades]);
    setPeriodosFiltro([...periodos]);
    setDresFiltro([...dres]);
    setUnidadesEscolaresFiltro([...unidadesEscolares]);

    store.dispatch(turmasUsuario(turmas.sort(ordenaTurmas)));
    selecionaTurmaCasosEspecificos();
  }, [dados]);

  useEffect(() => {
    if (modalidadesFiltro.length === 1)
      setModalidadeFiltroSelecionada(modalidadesFiltro[0].codigo.toString());
    if (dresFiltro.length === 1)
      setDreFiltroSelecionada(dresFiltro[0].codigo.toString());
    if (unidadesEscolaresFiltro.length === 1)
      setUnidadeEscolarFiltroSelecionada(
        unidadesEscolaresFiltro[0].codigo.toString()
      );
    if (usuario.turmasUsuario.length === 1)
      setTurmaFiltroSelecionada(usuario.turmasUsuario[0].codigo.toString());
  }, [
    modalidadesFiltro,
    dresFiltro,
    unidadesEscolaresFiltro,
    usuario.turmasUsuario,
  ]);

  useEffect(() => {
    inputBuscaRef.current.focus();
    if (!textoAutocomplete) setResultadosFiltro([]);
  }, [textoAutocomplete]);

  useLayoutEffect(() => {
    if (!toggleBusca && toggleInputFocus) inputBuscaRef.current.focus();
    if (toggleBusca) document.addEventListener('click', handleClickFora);
  }, [toggleBusca, toggleInputFocus]);

  useEffect(() => {
    const dres = [];
    const unidadesEscolares = [];
    const turmas = [];

    dados
      .filter(dado => {
        if (modalidadeFiltroSelecionada)
          return dado.codModalidade.toString() === modalidadeFiltroSelecionada;
        return true;
      })
      .filter(dado => {
        if (periodoFiltroSelecionado)
          return dado.semestre.toString() === periodoFiltroSelecionado;
        return true;
      })
      .filter(dado => {
        if (dreFiltroSelecionada)
          return dado.codDre.toString() === dreFiltroSelecionada;
        return true;
      })
      .filter(dado => {
        if (unidadeEscolarFiltroSelecionada)
          return dado.codEscola.toString() === unidadeEscolarFiltroSelecionada;
        return true;
      })
      .forEach(dado => {
        if (dres.findIndex(dre => dre.codigo === dado.codDre) < 0) {
          dres.push({
            codigo: dado.codDre,
            dre: dado.dre,
          });
        }
        if (
          unidadesEscolares.findIndex(
            unidade => unidade.codigo === dado.codEscola
          ) < 0
        ) {
          unidadesEscolares.push({
            codigo: dado.codEscola,
            unidade: dado.ue,
          });
        }
        if (turmas.findIndex(turma => turma.codigo === dado.codTurma) < 0) {
          turmas.push({
            codigo: dado.codTurma,
            ano: dado.ano,
            turma: dado.nomeTurma,
            codEscola: dado.codEscola,
          });
        }
      });

    if (dres.filter(dre => dre.codigo === dreFiltroSelecionada).length === 0)
      setDreFiltroSelecionada();
    setDresFiltro([...dres]);

    if (
      unidadesEscolares.filter(
        unidade => unidade.codigo === unidadeEscolarFiltroSelecionada
      ).length === 0
    )
      setUnidadeEscolarFiltroSelecionada();
    setUnidadesEscolaresFiltro([...unidadesEscolares]);

    if (
      turmas.filter(turma => turma.codigo === turmaFiltroSelecionada).length ===
      0
    )
      setTurmaFiltroSelecionada();
    store.dispatch(turmasUsuario(turmas.sort(ordenaTurmas)));
  }, [
    modalidadeFiltroSelecionada,
    periodoFiltroSelecionado,
    dreFiltroSelecionada,
    unidadeEscolarFiltroSelecionada,
  ]);

  useEffect(() => {
    if (modalidadeFiltroSelecionada) {
      if (
        modalidade.EJA == modalidadeFiltroSelecionada &&
        !periodoFiltroSelecionado
      ) {
        setDesabilitarTurma(true);
      } else {
        setDesabilitarTurma(false);
      }
    } else {
      setDesabilitarTurma(true);
    }
  }, [modalidadeFiltroSelecionada, turmaFiltroSelecionada]);

  useEffect(() => {
    if (
      modalidadeFiltroSelecionada &&
      periodoFiltroSelecionado &&
      modalidade.EJA == modalidadeFiltroSelecionada
    ) {
      setDesabilitarTurma(false);
    } else {
      setDesabilitarTurma(true);
    }
  }, [periodoFiltroSelecionado]);

  const handleClickFora = event => {
    if (
      !event.target.classList.contains('fa-caret-down') &&
      !event.target.classList.contains(
        'ant-select-dropdown-menu-item-active'
      ) &&
      !event.target.classList.contains('ant-select-selection-selected-value') &&
      !event.target.classList.contains(
        'ant-select-dropdown-menu-item-selected'
      ) &&
      divBuscaRef.current &&
      !divBuscaRef.current.contains(event.target)
    )
      mostraBusca();
  };

  const selecionaTurmaCasosEspecificos = () => {
    if (dados.length === 1) {
      setModalidadeFiltroSelecionada(dados[0].codModalidade.toString());
      setDreFiltroSelecionada(dados[0].codDre.toString());
      setUnidadeEscolarFiltroSelecionada(dados[0].codEscola.toString());
      setTurmaFiltroSelecionada(dados[0].codTurma.toString());
      setPeriodoFiltroSelecionado(dados[0].semestre.toString());
      store.dispatch(selecionarTurma(dados));
    }
  };

  const onChangeAutocomplete = () => {
    const texto = inputBuscaRef.current.value;
    setTextoAutocomplete(texto);

    const resultadosAutocomplete = [];
    if (texto.length >= 2) {
      dados
        .filter(dado => {
          return (
            dado.modalidade.toLowerCase().includes(texto) ||
            dado.nomeTurma.toLowerCase().includes(texto) ||
            dado.ue.toLowerCase().includes(texto)
          );
        })
        .map(dado => {
          return resultadosAutocomplete.push(dado);
        });
      setResultadosFiltro(resultadosAutocomplete);
    }
  };

  let selecionado = -1;

  const onKeyDownAutocomplete = event => {
    if (resultadosFiltro && resultadosFiltro.length > 0) {
      const resultados = document.querySelectorAll('.list-group-item');
      if (resultados && resultados.length > 0) {
        if (event.key === 'ArrowUp') {
          if (selecionado > 0) selecionado -= 1;
        } else if (event.key === 'ArrowDown') {
          if (selecionado < resultados.length - 1) selecionado += 1;
        }
        resultados.forEach(resultado =>
          resultado.classList.remove('selecionado')
        );
        if (resultados[selecionado]) {
          resultados[selecionado].classList.add('selecionado');
          inputBuscaRef.current.focus();
        }
      }
    }
  };

  const onSubmitAutocomplete = event => {
    event.preventDefault();
    if (resultadosFiltro) {
      if (resultadosFiltro.length === 1) {
        setDreFiltroSelecionada(resultadosFiltro[0].codDre);
        setUnidadeEscolarFiltroSelecionada(resultadosFiltro[0].codEscola);
        setTurmaFiltroSelecionada(resultadosFiltro[0].codTurma.toString());
        selecionaTurmaAutocomplete(resultadosFiltro[0]);
      } else {
        const selecionado = document.querySelector(
          '.list-group-item.selecionado'
        );
        if (selecionado) {
          const indice = selecionado.getAttribute('tabindex');
          if (indice) {
            const resultado = resultadosFiltro[indice];
            if (resultado) {
              setModalidadeFiltroSelecionada(
                resultado.codModalidade.toString()
              );
              setDreFiltroSelecionada(resultado.codDre);
              setUnidadeEscolarFiltroSelecionada(resultado.codEscola);
              setTurmaFiltroSelecionada(resultado.codTurma.toString());
              selecionaTurmaAutocomplete(resultado);
            }
          }
        }
      }
    }
  };

  const selecionaTurmaAutocomplete = resultado => {
    store.dispatch(selecionarTurma([resultado]));
    setResultadosFiltro([]);
  };

  const onFocusBusca = () => {
    if (toggleBusca) {
      setToggleBusca(false);
      setToggleInputFocus(true);
    }
  };

  const mostraBusca = () => {
    setToggleBusca(!toggleBusca);
    setToggleInputFocus(false);
  };

  const onChangeAnoLetivo = ano => {
    setAnoLetivoFiltroSelecionado(ano);
  };

  const onChangeModalidade = modalidade => {
    setModalidadeFiltroSelecionada(modalidade);
  };

  const onChangePeriodo = periodo => {
    setPeriodoFiltroSelecionado(periodo);
  };

  const onChangeDre = dre => {
    setDreFiltroSelecionada(dre);
  };

  const onChangeUnidadeEscolarFiltro = unidade => {
    setUnidadeEscolarFiltroSelecionada(unidade);
  };

  const onChangeTurma = turma => {
    setTurmaFiltroSelecionada(turma);
  };

  const selecionaTurma = () => {
    const turma = dados.filter(dado => {
      return (
        dado.anoLetivo.toString() === anoLetivoFiltroSelecionado &&
        dado.codModalidade.toString() === modalidadeFiltroSelecionada &&
        dado.codDre.toString() === dreFiltroSelecionada &&
        dado.codEscola.toString() === unidadeEscolarFiltroSelecionada &&
        dado.codTurma.toString() === turmaFiltroSelecionada
      );
    });

    if (turma.length > 0) {
      store.dispatch(selecionarTurma(turma));
      setToggleBusca(false);
      sucesso('Turma selecionada com sucesso!');
    }
  };

  const aplicarFiltro = () => {
    if (
      anoLetivoFiltroSelecionado &&
      modalidadeFiltroSelecionada &&
      dreFiltroSelecionada &&
      unidadeEscolarFiltroSelecionada &&
      turmaFiltroSelecionada
    ) {
      if (modalidadeFiltroSelecionada == 3 && !periodoFiltroSelecionado) {
        erro('É necessário informar todos os dados da turma!');
      } else {
        selecionaTurma();
      }
    } else {
      erro('É necessário informar todos os dados da turma!');
    }
  };

  const removerTurmaSelecionada = () => {
    store.dispatch(removerTurma());
    setTextoAutocomplete();
    setModalidadeFiltroSelecionada();
    setPeriodoFiltroSelecionado();
    setDreFiltroSelecionada();
    setUnidadeEscolarFiltroSelecionada();
    setTurmaFiltroSelecionada();
    setTurmaUeSelecionada();
  };

  return (
    <Container className="position-relative w-100">
      <form className="w-100" onSubmit={onSubmitAutocomplete}>
        <div className="form-group mb-0 w-100 position-relative">
          <Search className="fa fa-search fa-lg bg-transparent position-absolute text-center" />
          <Input
            type="text"
            className="form-control form-control-lg rounded d-flex px-5 border-0 fonte-14"
            placeholder="Pesquisar Turma"
            ref={inputBuscaRef}
            onFocus={onFocusBusca}
            onChange={onChangeAutocomplete}
            onKeyDown={onKeyDownAutocomplete}
            readOnly={!!turmaUeSelecionada}
            value={turmaUeSelecionada || textoAutocomplete}
          />
          {dados.length > 1 && turmaUeSelecionada && (
            <Times
              className="fa fa-times position-absolute"
              onClick={removerTurmaSelecionada}
            />
          )}
          <Caret
            className="fa fa-caret-down rounded-circle position-absolute text-center"
            onClick={mostraBusca}
          />
        </div>
        {resultadosFiltro.length > 0 && (
          <div className="container position-absolute bg-white shadow rounded mt-1 p-0">
            <div className="list-group">
              {resultadosFiltro.map((resultado, indice) => {
                return (
                  <ListItem
                    key={shortid.generate()}
                    className="list-group-item list-group-item-action border-0 rounded-0"
                    onClick={() => selecionaTurmaAutocomplete(resultado)}
                    tabIndex={indice}
                  >
                    {`${resultado.modalidade} - ${resultado.nomeTurma} - ${resultado.tipoEscola} - ${resultado.ue}`}
                  </ListItem>
                );
              })}
            </div>
          </div>
        )}
        {toggleBusca && (
          <div
            ref={divBuscaRef}
            className="container position-absolute bg-white shadow rounded mt-1 px-3 pt-5 pb-1"
          >
            <div className="form-row">
              <Grid cols={3} className="form-group">
                <SelectComponent
                  className="fonte-14"
                  onChange={onChangeAnoLetivo}
                  lista={anosLetivosFiltro}
                  valueOption="ano"
                  valueText="ano"
                  valueSelect={anoLetivoFiltroSelecionado}
                  placeholder="Ano"
                />
              </Grid>
              <Grid
                cols={modalidadeFiltroSelecionada === '3' ? 5 : 9}
                className="form-group"
              >
                <SelectComponent
                  className="fonte-14"
                  onChange={onChangeModalidade}
                  lista={modalidadesFiltro}
                  valueOption="codigo"
                  valueText="modalidade"
                  valueSelect={modalidadeFiltroSelecionada}
                  placeholder="Modalidade"
                />
              </Grid>
              {modalidadeFiltroSelecionada === '3' && (
                <Grid cols={4} className="form-group">
                  <SelectComponent
                    className="fonte-14"
                    onChange={onChangePeriodo}
                    lista={periodosFiltro}
                    valueOption="codigo"
                    valueText="periodo"
                    valueSelect={periodoFiltroSelecionado}
                    placeholder="Período"
                  />
                </Grid>
              )}
            </div>
            <div className="form-group">
              <SelectComponent
                className="fonte-14"
                onChange={onChangeDre}
                lista={dresFiltro}
                valueOption="codigo"
                valueText="dre"
                valueSelect={dreFiltroSelecionada}
                placeholder="Diretoria Regional De Educação (DRE)"
              />
            </div>
            <div className="form-group">
              <SelectComponent
                className="fonte-14"
                onChange={onChangeUnidadeEscolarFiltro}
                lista={unidadesEscolaresFiltro}
                valueOption="codigo"
                valueText="unidade"
                valueSelect={unidadeEscolarFiltroSelecionada}
                placeholder="Unidade Escolar (UE)"
              />
            </div>
            <div className="form-row d-flex justify-content-between">
              <Grid cols={3} className="form-group">
                <SelectComponent
                  className="fonte-14"
                  onChange={onChangeTurma}
                  lista={usuario.turmasUsuario}
                  valueOption="codigo"
                  valueText="turma"
                  valueSelect={turmaFiltroSelecionada}
                  placeholder="Turma"
                  disabled={desabilitarTurma}
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
