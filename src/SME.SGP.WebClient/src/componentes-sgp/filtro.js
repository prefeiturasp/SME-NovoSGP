import React, { useState, useEffect, useRef } from 'react';
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
    setmodalidadeFiltroSelecionada,
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

  const Container = styled.div`
    max-width: 571px !important;
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
    &:hover {
      background: ${Base.Roxo} !important;
      color: ${Base.Branco} !important;
    }
  `;

  const inputBuscaRef = useRef();
  const [queryAutocomplete, setQueryAutocomplete] = useState();

  const usuario = useSelector(state => state.usuario);

  useEffect(() => {
    if (usuario.rf.length > 0) {
      const dados = api.get(`v1/professores/${usuario.rf[0]}/turmas`);
      setDados(dados);
    } else {
      setDados([
        {
          ano: 8,
          anoLetivo: 2019,
          codDre: '108600',
          codEscola: '095346',
          codModalidade: 5,
          codTipoEscola: '1',
          codTipoUE: 3,
          codTurma: 2008187,
          dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
          dreAbrev: 'DRE - IP',
          modalidade: 'Fundamental',
          nomeTurma: '8A',
          tipoEscola: 'EMEF ',
          tipoSemestre: 1,
          tipoUE: 'UNIDADE ADMINISTRATIVA',
          ue: 'QUEIROZ FILHO, PROF.',
          ueAbrev: 'QUEIROZ FILHO, PROF.',
        },
        {
          ano: 3,
          anoLetivo: 2019,
          codDre: '108600',
          codEscola: '095346',
          codModalidade: 3,
          codTipoEscola: '1',
          codTipoUE: 3,
          codTurma: 2103083,
          dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
          dreAbrev: 'DRE - IP',
          modalidade: 'EJA',
          nomeTurma: '3D',
          tipoEscola: 'EMEF ',
          tipoSemestre: 2,
          tipoUE: 'UNIDADE ADMINISTRATIVA',
          ue: 'QUEIROZ FILHO, PROF.',
          ueAbrev: 'QUEIROZ FILHO, PROF.',
        },
        {
          ano: 4,
          anoLetivo: 2019,
          codDre: '108600',
          codEscola: '095346',
          codModalidade: 3,
          codTipoEscola: '1',
          codTipoUE: 3,
          codTurma: 2103084,
          dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
          dreAbrev: 'DRE - IP',
          modalidade: 'EJA',
          nomeTurma: '4C',
          tipoEscola: 'EMEF ',
          tipoSemestre: 2,
          tipoUE: 'UNIDADE ADMINISTRATIVA',
          ue: 'QUEIROZ FILHO, PROF.',
          ueAbrev: 'QUEIROZ FILHO, PROF.',
        },
        {
          ano: 4,
          anoLetivo: 2019,
          codDre: '108600',
          codEscola: '095346',
          codModalidade: 3,
          codTipoEscola: '1',
          codTipoUE: 3,
          codTurma: 2103086,
          dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
          dreAbrev: 'DRE - IP',
          modalidade: 'EJA',
          nomeTurma: '4D',
          tipoEscola: 'EMEF ',
          tipoSemestre: 2,
          tipoUE: 'UNIDADE ADMINISTRATIVA',
          ue: 'QUEIROZ FILHO, PROF.',
          ueAbrev: 'QUEIROZ FILHO, PROF.',
        },
        {
          ano: 4,
          anoLetivo: 2019,
          codDre: '108600',
          codEscola: '095346',
          codModalidade: 3,
          codTipoEscola: '1',
          codTipoUE: 3,
          codTurma: 2103089,
          dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
          dreAbrev: 'DRE - IP',
          modalidade: 'EJA',
          nomeTurma: '4E',
          tipoEscola: 'EMEF ',
          tipoSemestre: 2,
          tipoUE: 'UNIDADE ADMINISTRATIVA',
          ue: 'QUEIROZ FILHO, PROF.',
          ueAbrev: 'QUEIROZ FILHO, PROF.',
        },
        {
          ano: 4,
          anoLetivo: 2019,
          codDre: '108600',
          codEscola: '095346',
          codModalidade: 3,
          codTipoEscola: '1',
          codTipoUE: 3,
          codTurma: 2103094,
          dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
          dreAbrev: 'DRE - IP',
          modalidade: 'EJA',
          nomeTurma: '4F',
          tipoEscola: 'EMEF ',
          tipoSemestre: 2,
          tipoUE: 'UNIDADE ADMINISTRATIVA',
          ue: 'QUEIROZ FILHO, PROF.',
          ueAbrev: 'QUEIROZ FILHO, PROF.',
        },
      ]);
    }

    if (usuario.turmaSelecionada.length > 0) {
      const {
        modalidade,
        nomeTurma,
        tipoEscola,
        ue,
      } = usuario.turmaSelecionada[0];
      setTurmaUeSelecionada(
        `${modalidade} - ${nomeTurma} - ${tipoEscola} - ${ue}`
      );
    }
  }, [usuario.turmaSelecionada, usuario.rf]);

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

      if (dado.tipoSemestre === 2 && periodos.length < 2) {
        for (let semestre = 1; semestre <= dado.tipoSemestre; semestre++) {
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
        });
      }
    });

    setAnosLetivosFiltro([...anosLetivos]);
    setAnoLetivoFiltroSelecionado('2019');

    setModalidadesFiltro([...modalidades]);
    setPeriodosFiltro([...periodos]);
    setDresFiltro([...dres]);
    setUnidadesEscolaresFiltro([...unidadesEscolares]);

    const ordenaTurmas = (x, y) => {
      const a = x.turma.toLowerCase();
      const b = y.turma.toLowerCase();

      if (a > b) return 1;
      else if (a < b) return -1;

      return 0;
    };

    store.dispatch(turmasUsuario(turmas.sort(ordenaTurmas)));
  }, [dados]);

  useEffect(() => {
    inputBuscaRef.current.focus();
  }, [queryAutocomplete]);

  useEffect(() => {
    if (!toggleBusca && toggleInputFocus) inputBuscaRef.current.focus();
  }, [toggleBusca, toggleInputFocus]);

  const onChangeAutocomplete = () => {
    const texto = inputBuscaRef.current.value;
    setQueryAutocomplete(texto);

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
    setmodalidadeFiltroSelecionada(modalidade);
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
      selecionaTurma();
    } else {
      erro('Preencha todos os dados da turma!');
    }
  };

  const removerTurmaSelecionada = () => {
    store.dispatch(removerTurma());
    setQueryAutocomplete();
    setmodalidadeFiltroSelecionada();
    setPeriodoFiltroSelecionado();
    setDreFiltroSelecionada();
    setUnidadeEscolarFiltroSelecionada();
    setTurmaFiltroSelecionada();
    setTurmaUeSelecionada();
  };

  return (
    <Container className="position-relative w-100 mx-auto">
      <form className="w-100">
        <div className="form-group mb-0 w-100 position-relative">
          <Search className="fa fa-search fa-lg bg-transparent position-absolute text-center" />
          <Input
            type="text"
            className="form-control form-control-lg rounded d-flex px-5 border-0 fonte-14"
            placeholder="Pesquisar Turma"
            ref={inputBuscaRef}
            onFocus={onFocusBusca}
            onChange={onChangeAutocomplete}
            readOnly={turmaUeSelecionada ? true : false}
            value={turmaUeSelecionada ? turmaUeSelecionada : queryAutocomplete}
          />
          {turmaUeSelecionada && (
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
              {resultadosFiltro.map(resultado => {
                return (
                  <ListItem
                    key={shortid.generate()}
                    className="list-group-item list-group-item-action border-0 rounded-0"
                    onClick={() => selecionaTurmaAutocomplete(resultado)}
                  >
                    {`${resultado.modalidade} - ${resultado.nomeTurma} - ${resultado.tipoEscola} - ${resultado.ue}`}
                  </ListItem>
                );
              })}
            </div>
          </div>
        )}
        {toggleBusca && (
          <div className="container position-absolute bg-white shadow rounded mt-1 px-3 pt-5 pb-1">
            <div className="form-row">
              <Grid cols={3} className="form-group">
                <SelectComponent
                  className="fonte-14"
                  onChange={onChangeAnoLetivo}
                  lista={anosLetivosFiltro}
                  valueOption="ano"
                  label="ano"
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
                  label="modalidade"
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
                    label="periodo"
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
                label="dre"
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
                label="unidade"
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
                  label="turma"
                  valueSelect={turmaFiltroSelecionada}
                  placeholder="Turma"
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
