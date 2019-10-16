import React, { useState, useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import shortid from 'shortid';
import { store } from '../redux';
import { selecionarTurma } from '../redux/modulos/usuario/actions';
import Grid from '../componentes/grid';
import Button from '../componentes/button';
import { Base, Colors } from '../componentes/colors';
import SelectComponent from '../componentes/select';
import api from '../servicos/api';
// import modalidade from '~/dtos/modalidade';
// import ServicoFiltro from '~/servicos/Componentes/ServicoFiltro';

const Filtro = () => {
  const dadosUsuario = useSelector(state => state.usuario.dadosUsuario);
  const [dados] = useState(dadosUsuario);

  const [] = useState([]);

  const [] = useState([]);
  const [, setModalidadeFiltroSelecionada] = useState();

  const [] = useState([]);
  const [] = useState();

  const [] = useState([]);
  const [, setDreFiltroSelecionada] = useState();

  const [] = useState([]);
  const [, setUnidadeEscolarFiltroSelecionada] = useState();

  const [, setTurmaFiltroSelecionada] = useState();

  const [resultadosFiltro, setResultadosFiltro] = useState([]);

  const [, setToggleInputFocus] = useState(false);
  const [toggleBusca, setToggleBusca] = useState(false);

  const [turmaUeSelecionada] = useState();

  const [] = useState(true);

  const Container = styled.div`
    width: 568px !important;
    z-index: 100;
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

  const Icone = styled.i`
    color: ${Base.CinzaMako} !important;
    cursor: pointer !important;
  `;

  const Search = styled(Icone)`
    left: 0 !important;
    max-height: 23px !important;
    max-width: 14px !important;
    padding: 1rem !important;
    right: 0 !important;
    top: 0 !important;
  `;

  const Times = styled(Icone)`
    right: 50px !important;
    top: 15px !important;
  `;

  const Caret = styled(Icone)`
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

  // Novo Filtro

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

  const [novoAnosLetivos, setNovoAnosLetivos] = useState([]);
  const [anoLetivoSelecionado, setAnoLetivoSelecionado] = useState();

  useEffect(() => {
    const anosLetivos = [];
    api.get('v1/abrangencia/anos-letivos').then(resposta => {
      if (resposta.data) {
        resposta.data.forEach(ano => {
          anosLetivos.push({ desc: ano, valor: ano });
        });
        setNovoAnosLetivos(anosLetivos);
        setCampoAnoLetivoDesabilitado(false);
      }
    });
  }, []);

  useEffect(() => {
    if (novoAnosLetivos && novoAnosLetivos.length === 1)
      setAnoLetivoSelecionado(novoAnosLetivos[0].valor);
  }, [novoAnosLetivos]);

  const [novoModalidades, setNovoModalidades] = useState([]);
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
          setNovoModalidades(modalidades);
          setCampoModalidadeDesabilitado(false);
        }
      });
    } else {
      setModalidadeSelecionada();
      setCampoModalidadeDesabilitado(true);
    }
  }, [anoLetivoSelecionado]);

  useEffect(() => {
    if (novoModalidades && novoModalidades.length === 1)
      setModalidadeSelecionada(novoModalidades[0].valor);
  }, [novoModalidades]);

  const [novoPeriodos, setNovoPeriodos] = useState([]);
  const [periodoSelecionado, setPeriodoSelecionado] = useState();

  const [novoDres, setNovoDres] = useState([]);
  const [dreSelecionada, setDreSelecionada] = useState();

  useEffect(() => {
    if (modalidadeSelecionada) {
      const periodos = [];
      api.get('v1/abrangencia/semestres').then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(periodo => {
            periodos.push({ desc: periodo, valor: periodo });
          });
          setNovoPeriodos(periodos);
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
          setNovoDres(dres);
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
    if (novoDres && novoDres.length === 1) setDreSelecionada(novoDres[0].valor);
  }, [novoDres]);

  const [novoUnidadesEscolares, setNovoUnidadesEscolares] = useState([]);
  const [unidadeEscolarSelecionada, setUnidadeEscolarSelecionada] = useState();

  useEffect(() => {
    if (dreSelecionada) {
      const unidades = [];
      api.get(`v1/abrangencia/dres/${dreSelecionada}/ues`).then(resposta => {
        if (resposta.data) {
          resposta.data.forEach(unidade => {
            unidades.push({ desc: unidade.nome, valor: unidade.codigo });
          });
          setNovoUnidadesEscolares(unidades);
          setCampoUnidadeEscolarDesabilitado(false);
        }
      });
    } else {
      setUnidadeEscolarSelecionada();
      setCampoUnidadeEscolarDesabilitado(true);
    }
  }, [dreSelecionada]);

  useEffect(() => {
    if (novoUnidadesEscolares && novoUnidadesEscolares.length === 1)
      setUnidadeEscolarSelecionada(novoUnidadesEscolares[0].valor);
  }, [novoUnidadesEscolares]);

  const [novoTurmas, setNovoTurmas] = useState([]);
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
            setNovoTurmas(turmas);
            setCampoTurmaDesabilitado(false);
          }
        });
    } else {
      setTurmaSelecionada();
      setCampoTurmaDesabilitado(true);
    }
  }, [unidadeEscolarSelecionada]);

  useEffect(() => {
    if (novoTurmas && novoTurmas.length === 1)
      setTurmaSelecionada(novoTurmas[0].valor);
  }, [novoTurmas]);

  // Novo Filtro

  const [textoAutocomplete, setTextoAutocomplete] = useState();

  // const setAnoAtual = ano => {
  // setAnoLetivoFiltroSelecionado(`${ano}`);
  // store.dispatch(filtroAtual({ anoLetivo: `${ano}` }));
  // };

  // useEffect(() => {
  //   if (modalidadesFiltro.length === 1)
  //     setModalidadeFiltroSelecionada(modalidadesFiltro[0].codigo.toString());
  //   if (dresFiltro.length === 1)
  //     setDreFiltroSelecionada(dresFiltro[0].codigo.toString());
  //   if (unidadesEscolaresFiltro.length === 1)
  //     setUnidadeEscolarFiltroSelecionada(
  //       unidadesEscolaresFiltro[0].codigo.toString()
  //     );
  //   if (turmasUsuarioLista.length === 1)
  //     setTurmaFiltroSelecionada(turmasUsuarioLista[0].codigo.toString());
  // }, [
  //   modalidadesFiltro,
  //   dresFiltro,
  //   unidadesEscolaresFiltro,
  //   turmasUsuarioLista,
  // ]);

  // useEffect(() => {
  //   campoBuscaRef.current.focus();
  //   if (!textoAutocomplete) setResultadosFiltro([]);
  // }, [textoAutocomplete]);

  // useLayoutEffect(() => {
  //   if (!toggleBusca && toggleInputFocus) campoBuscaRef.current.focus();
  //   if (toggleBusca) document.addEventListener('click', handleClickFora);
  //   return () => document.removeEventListener('click', handleClickFora);
  // }, [toggleBusca, toggleInputFocus]);

  // useEffect(() => {
  //   const dres = [];
  //   const unidadesEscolares = [];
  //   const turmas = [];

  //   dados
  //     .filter(dado => {
  //       if (modalidadeFiltroSelecionada)
  //         return dado.codModalidade.toString() === modalidadeFiltroSelecionada;
  //       return true;
  //     })
  //     .filter(dado => {
  //       if (periodoFiltroSelecionado)
  //         return dado.semestre.toString() === periodoFiltroSelecionado;
  //       return true;
  //     })
  //     .filter(dado => {
  //       if (dreFiltroSelecionada)
  //         return dado.codDre.toString() === dreFiltroSelecionada;
  //       return true;
  //     })
  //     .filter(dado => {
  //       if (unidadeEscolarFiltroSelecionada)
  //         return dado.codEscola.toString() === unidadeEscolarFiltroSelecionada;
  //       return true;
  //     })
  //     .forEach(dado => {
  //       if (dres.findIndex(dre => dre.codigo === dado.codDre) < 0) {
  //         dres.push({
  //           codigo: dado.codDre,
  //           dre: dado.dre,
  //         });
  //       }
  //       if (
  //         unidadesEscolares.findIndex(
  //           unidade => unidade.codigo === dado.codEscola
  //         ) < 0
  //       ) {
  //         unidadesEscolares.push({
  //           codigo: dado.codEscola,
  //           unidade: dado.ue,
  //         });
  //       }
  //       if (turmas.findIndex(turma => turma.codigo === dado.codTurma) < 0) {
  //         turmas.push({
  //           codigo: dado.codTurma,
  //           ano: dado.ano,
  //           turma: dado.nomeTurma,
  //           codEscola: dado.codEscola,
  //         });
  //       }
  //     });

  //   if (dres.filter(dre => dre.codigo === dreFiltroSelecionada).length === 0)
  //     setDreFiltroSelecionada();
  //   setDresFiltro([...dres]);

  //   if (
  //     unidadesEscolares.filter(
  //       unidade => unidade.codigo === unidadeEscolarFiltroSelecionada
  //     ).length === 0
  //   )
  //     setUnidadeEscolarFiltroSelecionada();
  //   setUnidadesEscolaresFiltro([...unidadesEscolares]);

  //   if (
  //     turmas.filter(turma => turma.codigo === turmaFiltroSelecionada).length ===
  //     0
  //   )
  //     setTurmaFiltroSelecionada();
  //   store.dispatch(turmasUsuario(turmas.sort(ordenaTurmas)));
  // }, [
  //   modalidadeFiltroSelecionada,
  //   periodoFiltroSelecionado,
  //   dreFiltroSelecionada,
  //   unidadeEscolarFiltroSelecionada,
  // ]);

  // useEffect(() => {
  //   if (modalidadeFiltroSelecionada) {
  //     if (
  //       modalidade.EJA == modalidadeFiltroSelecionada &&
  //       !periodoFiltroSelecionado
  //     ) {
  //       setDesabilitarTurma(true);
  //     } else {
  //       setDesabilitarTurma(false);
  //     }
  //   } else {
  //     setDesabilitarTurma(true);
  //   }
  // }, [modalidadeFiltroSelecionada, turmaFiltroSelecionada]);

  // useEffect(() => {
  //   if (
  //     modalidadeFiltroSelecionada &&
  //     periodoFiltroSelecionado &&
  //     modalidade.EJA == modalidadeFiltroSelecionada
  //   ) {
  //     setDesabilitarTurma(false);
  //   } else {
  //     setDesabilitarTurma(true);
  //   }
  // }, [periodoFiltroSelecionado]);

  // const handleClickFora = event => {
  //   if (
  //     !event.target.classList.contains('fa-caret-down') &&
  //     !event.target.classList.contains(
  //       'ant-select-dropdown-menu-item-active'
  //     ) &&
  //     !event.target.classList.contains('ant-select-selection-selected-value') &&
  //     !event.target.classList.contains(
  //       'ant-select-dropdown-menu-item-selected'
  //     ) &&
  //     divBuscaRef.current &&
  //     !divBuscaRef.current.contains(event.target)
  //   )
  //     mostraBusca();
  // };

  // const selecionaTurmaCasosEspecificos = () => {
  //   if (dados.length === 1) {
  //     setModalidadeFiltroSelecionada(dados[0].codModalidade.toString());
  //     setDreFiltroSelecionada(dados[0].codDre.toString());
  //     setUnidadeEscolarFiltroSelecionada(dados[0].codEscola.toString());
  //     setTurmaFiltroSelecionada(dados[0].codTurma.toString());
  //     setPeriodoFiltroSelecionado(dados[0].semestre.toString());
  //     store.dispatch(selecionarTurma(dados));
  //   }
  // };

  const onChangeAutocomplete = () => {
    const texto = campoBuscaRef.current.value;
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
          campoBuscaRef.current.focus();
        }
      }
    }
  };

  const onSubmitAutocomplete = event => {
    event.preventDefault();
    if (resultadosFiltro) {
      if (resultadosFiltro.length === 1) {
        setModalidadeFiltroSelecionada(
          resultadosFiltro[0].codModalidade.toString()
        );
        setDreFiltroSelecionada(resultadosFiltro[0].codDre);
        setUnidadeEscolarFiltroSelecionada(resultadosFiltro[0].codEscola);
        setTimeout(() => {
          setTurmaFiltroSelecionada(resultadosFiltro[0].codTurma.toString());
        }, 1000);
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
              setTimeout(() => {
                setTurmaFiltroSelecionada(resultado.codTurma.toString());
              }, 1000);
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

  // const selecionaTurma = () => {
  // const turma = dados.filter(dado => {
  //   return (
  //     dado.anoLetivo.toString() === anoLetivoFiltroSelecionado &&
  //     dado.codModalidade.toString() === modalidadeFiltroSelecionada &&
  //     dado.codDre.toString() === dreFiltroSelecionada &&
  //     dado.codEscola.toString() === unidadeEscolarFiltroSelecionada &&
  //     dado.codTurma.toString() === turmaFiltroSelecionada
  //   );
  // });

  // if (turma.length > 0) {
  //   store.dispatch(selecionarTurma(turma));
  //   setToggleBusca(false);
  //   sucesso('Turma selecionada com sucesso!');
  // }
  // };

  const aplicarFiltro = () => {
    //   if (
    //     // anoLetivoFiltroSelecionado &&
    //     modalidadeFiltroSelecionada &&
    //     dreFiltroSelecionada &&
    //     unidadeEscolarFiltroSelecionada &&
    //     turmaFiltroSelecionada
    //   ) {
    //     if (modalidadeFiltroSelecionada == 3 && !periodoFiltroSelecionado) {
    //       erro('É necessário informar todos os dados da turma!');
    //     } else {
    //       selecionaTurma();
    //     }
    //   } else {
    //     erro('É necessário informar todos os dados da turma!');
    //   }
  };

  const removerTurmaSelecionada = () => {
    // store.dispatch(removerTurma());
    // setTextoAutocomplete();
    // setModalidadeFiltroSelecionada();
    // setPeriodoFiltroSelecionado();
    // setDreFiltroSelecionada();
    // setUnidadeEscolarFiltroSelecionada();
    // setTurmaFiltroSelecionada();
    // setTurmaUeSelecionada();
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
            ref={campoBuscaRef}
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
                  onChange={aoTrocarAnoLetivo}
                  lista={novoAnosLetivos}
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
                  lista={novoModalidades}
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
                    lista={novoPeriodos}
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
                lista={novoDres}
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
                lista={novoUnidadesEscolares}
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
                  lista={novoTurmas}
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
