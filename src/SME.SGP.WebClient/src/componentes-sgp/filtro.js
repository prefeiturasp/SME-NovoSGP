import React, { useState, useEffect, useRef, useLayoutEffect } from 'react';
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

  const Input = styled.input`
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

  const Search = styled(Icone)`
    left: 0;
    max-height: 23px;
    max-width: 14px;
    padding: 1rem;
    right: 0;
    top: 0;
  `;

  const Times = styled(Icone)`
    right: 50px;
    top: 15px;
  `;

  const Caret = styled(Icone)`
    background: ${Base.CinzaDesabilitado};
    max-height: 36px;
    max-width: 36px;
    padding: 0.7rem 0.9rem;
    right: 5px;
    top: 5px;
    transition: transform 0.3s;
    ${alternarFocoBusca && 'transform: rotate(180deg);'}
  `;

  const ListItem = styled.li`
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

  const usuario = useSelector(state => state.usuario);
  const [turmaUsuarioSelecionada, setTurmaUsuarioSelecionada] = useState(
    usuario.turmaSelecionada
  );

  useEffect(() => {
    if (typeof turmaUsuarioSelecionada === 'object')
      setTurmaUsuarioSelecionada(turmaUsuarioSelecionada.desc);
  }, [turmaUsuarioSelecionada]);

  const aplicarFiltro = () => {
    if (
      anoLetivoSelecionado &&
      modalidadeSelecionada &&
      dreSelecionada &&
      unidadeEscolarSelecionada &&
      turmaSelecionada
    ) {
      const modalidadeDesc = novoModalidades.filter(
        modalidade => modalidade.valor.toString() === modalidadeSelecionada
      );
      const turmaDesc = novoTurmas.filter(
        turma => turma.valor === turmaSelecionada
      );
      const unidadeEscolarDesc = novoUnidadesEscolares.filter(
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

  useLayoutEffect(() => {
    if (!alternarFocoBusca && alternarFocoCampo) campoBuscaRef.current.focus();
    if (alternarFocoBusca)
      document.addEventListener('click', controlaClickFora);
    return () => document.removeEventListener('click', controlaClickFora);
  }, [alternarFocoBusca, alternarFocoCampo]);

  const [textoAutocomplete, setTextoAutocomplete] = useState();
  const [resultadosFiltro, setResultadosFiltro] = useState([]);

  const onChangeAutocomplete = () => {
    const texto = campoBuscaRef.current.value;
    setTextoAutocomplete(texto);

    api.get(`v1/abrangencia/${texto}`).then(resposta => {
      console.log(resposta);
    });

    // const resultadosAutocomplete = [];
    // if (texto.length >= 2) {
    //   dados
    //     .filter(dado => {
    //       return (
    //         dado.modalidade.toLowerCase().includes(texto) ||
    //         dado.nomeTurma.toLowerCase().includes(texto) ||
    //         dado.ue.toLowerCase().includes(texto)
    //       );
    //     })
    //     .map(dado => {
    //       return resultadosAutocomplete.push(dado);
    //     });
    //   setResultadosFiltro(resultadosAutocomplete);
    // }
  };

  const selecionaTurmaAutocomplete = resultado => {
    store.dispatch(selecionarTurma(resultado));
    setResultadosFiltro([]);
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
        setModalidadeSelecionada(resultadosFiltro[0].codModalidade.toString());
        setDreSelecionada(resultadosFiltro[0].codDre);
        setUnidadeEscolarSelecionada(resultadosFiltro[0].codEscola);
        setTimeout(() => {
          setTurmaSelecionada(resultadosFiltro[0].codTurma.toString());
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
              setModalidadeSelecionada(resultado.codModalidade.toString());
              setDreSelecionada(resultado.codDre);
              setUnidadeEscolarSelecionada(resultado.codEscola);
              setTimeout(() => {
                setTurmaSelecionada(resultado.codTurma.toString());
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
  //   setAlternarFocoBusca(false);
  //   sucesso('Turma selecionada com sucesso!');
  // }
  // };

  // const aplicarFiltro = () => {
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
  // };

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
            onFocus={aoFocarBusca}
            onChange={onChangeAutocomplete}
            onKeyDown={onKeyDownAutocomplete}
            readOnly={!!turmaUsuarioSelecionada}
            value={turmaUsuarioSelecionada || textoAutocomplete}
          />
          {turmaUsuarioSelecionada && (
            <Times
              className="fa fa-times position-absolute"
              onClick={removerTurmaSelecionada}
            />
          )}
          <Caret
            className="fa fa-caret-down rounded-circle position-absolute text-center"
            onClick={mostrarEsconderBusca}
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
