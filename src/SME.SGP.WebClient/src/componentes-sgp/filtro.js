import React, { useState, useEffect, createRef } from 'react';
import styled from 'styled-components';
import Grid from '../componentes/grid';
import Button from '../componentes/button';
import { Base, Colors } from '../componentes/colors';
import SelectComponent from '../componentes/select';
import { sucesso } from '../servicos/alertas';

const Filtro = () => {
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
    setModalidades([
      { modalidade: 'EJA' },
      { modalidade: 'Ensino Fundamental' },
    ]);
    setPeriodos([{ periodo: '1º Semestre' }, { periodo: '2º Semestre' }]);
    setDres([{ dre: 'Diretoria Regional de Educação (DRE) Ipiranga' }]);
    setUnidadesEscolares([
      {
        unidade:
          'Unidade Escolar (UE) Luiz Gonzaga do Nascimento Jr - Gonzaguinha',
      },
    ]);
    setTurmas([
      { turma: '1-A' },
      { turma: '1-B' },
      { turma: '1-C' },
      { turma: '1-D' },
    ]);
  }, []);

  const inputBuscaRef = createRef();

  useEffect(() => {
    if (!toggleBusca && toggleInputFocus) inputBuscaRef.current.focus();
  }, [toggleBusca]);

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
          />
          <Caret
            className="fa fa-caret-down rounded-circle position-absolute text-center"
            onClick={mostraBusca}
          />
        </div>
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
