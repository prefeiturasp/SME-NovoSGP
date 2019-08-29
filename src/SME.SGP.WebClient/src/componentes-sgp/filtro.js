import React, { useState, useEffect, useRef } from 'react';
import styled from 'styled-components';
import Grid from '../componentes/grid';
import Button from '../componentes/button';
import { Base, Colors } from '../componentes/colors';
import SelectComponent from '../componentes/select';
import { sucesso } from '../servicos/alertas';

const Filtro = () => {
  const [dadosProfessor] = useState([
    {
      Modalidade: 'EJA',
      CodModalidade: 3,
      Dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      DreAbrev: 'DRE - IP',
      UE: 'QUEIROZ FILHO, PROF.',
      UEAbrev: 'QUEIROZ FILHO, PROF.',
      NomeTurma: '3C',
      Ano: 3,
    },
    {
      Modalidade: 'EJA',
      CodModalidade: 3,
      Dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      DreAbrev: 'DRE - IP',
      UE: 'QUEIROZ FILHO, PROF.',
      UEAbrev: 'QUEIROZ FILHO, PROF.',
      NomeTurma: '3D',
      Ano: 3,
    },
    {
      Modalidade: 'EJA',
      CodModalidade: 3,
      Dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      DreAbrev: 'DRE - IP',
      UE: 'QUEIROZ FILHO, PROF.',
      UEAbrev: 'QUEIROZ FILHO, PROF.',
      NomeTurma: '4C',
      Ano: 4,
    },
    {
      Modalidade: 'EJA',
      CodModalidade: 3,
      Dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      DreAbrev: 'DRE - IP',
      UE: 'QUEIROZ FILHO, PROF.',
      UEAbrev: 'QUEIROZ FILHO, PROF.',
      NomeTurma: '4D',
      Ano: 4,
    },
    {
      Modalidade: 'EJA',
      CodModalidade: 3,
      Dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      DreAbrev: 'DRE - IP',
      UE: 'QUEIROZ FILHO, PROF.',
      UEAbrev: 'QUEIROZ FILHO, PROF.',
      NomeTurma: '4F',
      Ano: 4,
    },
    {
      Modalidade: 'Fundamental',
      CodModalidade: 5,
      Dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      DreAbrev: 'DRE - IP',
      UE: 'QUEIROZ FILHO, PROF.',
      UEAbrev: 'QUEIROZ FILHO, PROF.',
      NomeTurma: '8A',
      Ano: 8,
    },
    {
      Modalidade: 'Fundamental',
      CodModalidade: 5,
      Dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      DreAbrev: 'DRE - IP',
      UE: 'QUEIROZ FILHO, PROF.',
      UEAbrev: 'QUEIROZ FILHO, PROF.',
      NomeTurma: '8B',
      Ano: 8,
    },
    {
      Modalidade: 'Fundamental',
      CodModalidade: 5,
      Dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      DreAbrev: 'DRE - IP',
      UE: 'QUEIROZ FILHO, PROF.',
      UEAbrev: 'QUEIROZ FILHO, PROF.',
      NomeTurma: '8C',
      Ano: 8,
    },
  ]);

  const [anosLetivos, setAnosLetivos] = useState([]);
  const [anoLetivoSelecionado, setAnoLetivoSelecionado] = useState();

  const [modalidades, setModalidades] = useState([]);
  const [modalidadeSelecionada, setModalidadeSelecionada] = useState();

  const [periodos, setPeriodos] = useState([]);
  const [periodoSelecionado, setPeriodoSelecionado] = useState();

  const [dres, setDres] = useState([{ dre: '' }]);
  const [dreSelecionado, setDreSelecionado] = useState();

  const [unidadesEscolares, setUnidadesEscolares] = useState();
  const [unidadeEscolarSelecionada, setUnidadeEscolarSelecionada] = useState();

  const [turmas, setTurmas] = useState();
  const [turmaSelecionada, setTurmaSelecionada] = useState();

  const [resultadosFiltro, setResultadosFiltro] = useState([]);

  const [toggleInputFocus, setToggleInputFocus] = useState(false);
  const [toggleBusca, setToggleBusca] = useState(false);

  const Container = styled.div`
    max-width: 571px !important;
  `;

  const Input = styled.input`
    background: ${Base.CinzaFundo} !important;
    height: 45px !important;
    &:focus {
      background: ${Base.Branco} !important;
      box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075) !important;
      color ${Base.Preto} !important;
    }
  `;

  const Icon = styled.i`
    color: ${Base.CinzaMako} !important;
    cursor: pointer !important;
  `;

  const Search = styled(Icon)`
    left: 0;
    max-height: 23px;
    max-width: 14px;
    padding: 1rem !important;
    right: 0;
    top: 0;
  `;

  const Caret = styled(Icon)`
    background: ${Base.CinzaDesabilitado} !important;
    max-height: 36px;
    max-width: 36px;
    padding: 0.7rem 0.9rem !important;
    right: 5px !important;
    top: 5px !important;
    ${toggleBusca && 'transform: rotate(180deg) !important;'}
  `;

  useEffect(() => {
    setAnosLetivos([{ ano: '2019' }]);
    setAnoLetivoSelecionado('2019');

    const modalidadesList = [];
    const dresList = [];
    const unidadesEscolaresList = [];
    const turmasList = [];

    dadosProfessor.forEach(dado => {
      if (
        modalidadesList.findIndex(
          modalidade => modalidade.codigo === dado.CodModalidade
        ) < 0
      ) {
        modalidadesList.push({
          codigo: dado.CodModalidade,
          modalidade: dado.Modalidade,
        });
      }

      if (dresList.findIndex(dre => dre.abrev === dado.DreAbrev) < 0) {
        dresList.push({
          abrev: dado.DreAbrev,
          dre: dado.Dre,
        });
      }

      if (
        unidadesEscolaresList.findIndex(
          unidade => unidade.abrev === dado.UEAbrev
        ) < 0
      ) {
        unidadesEscolaresList.push({
          abrev: dado.UEAbrev,
          unidade: dado.UE,
        });
      }

      if (turmasList.findIndex(turma => turma.turma === dado.NomeTurma) < 0) {
        turmasList.push({
          ano: dado.Ano,
          turma: dado.NomeTurma,
        });
      }
    });

    setModalidades([...modalidadesList]);
    setPeriodos([{ periodo: '1º Semestre' }, { periodo: '2º Semestre' }]);
    setDres([...dresList]);
    setUnidadesEscolares([...unidadesEscolaresList]);
    setTurmas([...turmasList]);
  }, []);

  const inputBuscaRef = useRef();

  useEffect(() => {
    if (!toggleBusca && toggleInputFocus) inputBuscaRef.current.focus();
  }, [toggleBusca, toggleInputFocus]);

  const onKeyUpAutocomplete = () => {
    const texto = inputBuscaRef.current.value;
    const resultadosAutocomplete = [];
    if (texto.length >= 2) {
      dadosProfessor
        .filter(dado => {
          return dado.UE.toLowerCase().includes(texto);
        })
        .forEach(dado => {
          resultadosAutocomplete.push(dado);
        });
      setResultadosFiltro(resultadosAutocomplete);
      setToggleInputFocus(true);
    }
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
    setAnoLetivoSelecionado(ano);
  };

  const onChangeModalidade = modalidade => {
    setModalidadeSelecionada(modalidade);
  };

  const onChangePeriodo = periodo => {
    setPeriodoSelecionado(periodo);
  };

  const onChangeDre = dre => {
    setDreSelecionado(dre);
  };

  const onChangeUnidadeEscolar = unidade => {
    setUnidadeEscolarSelecionada(unidade);
  };

  const onChangeTurma = turma => {
    setTurmaSelecionada(turma);
  };

  const aplicarFiltro = () => {
    setToggleBusca(false);
    sucesso('Suas escolhas foram salvas com sucesso!');
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
            onKeyUp={onKeyUpAutocomplete}
          />
          <Caret
            className="fa fa-caret-down rounded-circle position-absolute text-center"
            onClick={mostraBusca}
          />
        </div>
        {resultadosFiltro.length > 0 && (
          <div className="container position-absolute bg-white shadow rounded mt-1 p-2">
            <div className="list-group">
              {resultadosFiltro.map(resultado => {
                return (
                  <li className="list-group-item list-group-item-action border-0">{`${resultado.NomeTurma} ${resultado.UE}`}</li>
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
                  lista={anosLetivos}
                  valueOption="ano"
                  label="ano"
                  valueSelect={anoLetivoSelecionado}
                  placeholder="Ano"
                />
              </Grid>
              <Grid
                cols={modalidadeSelecionada === 'EJA' ? 5 : 9}
                className="form-group"
              >
                <SelectComponent
                  className="fonte-14"
                  onChange={onChangeModalidade}
                  lista={modalidades}
                  valueOption="modalidade"
                  label="modalidade"
                  valueSelect={modalidadeSelecionada}
                  placeholder="Modalidade"
                />
              </Grid>
              {modalidadeSelecionada === 'EJA' && (
                <Grid cols={4} className="form-group">
                  <SelectComponent
                    className="fonte-14"
                    onChange={onChangePeriodo}
                    lista={periodos}
                    valueOption="periodo"
                    label="periodo"
                    valueSelect={periodoSelecionado}
                  />
                </Grid>
              )}
            </div>
            <div className="form-group">
              <SelectComponent
                className="fonte-14"
                onChange={onChangeDre}
                lista={dres}
                valueOption="dre"
                label="dre"
                valueSelect={dreSelecionado}
                placeholder="Diretoria Regional De Educação (DRE)"
              />
            </div>
            <div className="form-group">
              <SelectComponent
                className="fonte-14"
                onChange={onChangeUnidadeEscolar}
                lista={unidadesEscolares}
                valueOption="unidade"
                label="unidade"
                valueSelect={unidadeEscolarSelecionada}
                placeholder="Unidade Escolar (UE)"
              />
            </div>
            <div className="form-row d-flex justify-content-between">
              <Grid cols={3} className="form-group">
                <SelectComponent
                  className="fonte-14"
                  onChange={onChangeTurma}
                  lista={turmas}
                  valueOption="turma"
                  label="turma"
                  valueSelect={turmaSelecionada}
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
