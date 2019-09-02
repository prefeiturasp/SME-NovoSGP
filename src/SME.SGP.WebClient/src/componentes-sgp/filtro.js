import React, { useState, useEffect, useRef } from 'react';
// eslint-disable-next-line import/no-extraneous-dependencies
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import shortid from 'shortid';
import { store } from '../redux';
import {
  turmasUusario,
  selecionarTurma,
} from '../redux/modulos/usuario/actions';
import Grid from '../componentes/grid';
import Button from '../componentes/button';
import { Base, Colors } from '../componentes/colors';
import SelectComponent from '../componentes/select';
import { sucesso, erro } from '../servicos/alertas';

const Filtro = () => {
  const [dadosProfessor] = useState([
    {
      ano: 5,
      anoLetivo: 2019,
      codModalidade: 5,
      dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      dreAbrev: 'DRE - IP',
      modalidade: 'Fundamental',
      nomeTurma: '5A',
      tipoSemestre: 1,
      tipoUE: 'UNIDADE ADMINISTRATIVA',
      codTipoUE: 3,
      ue: 'PRUDENTE DE MORAIS, PRES.',
      ueAbrev: 'PRUDENTE DE MORAIS, PRES.',
      tipoEscola: 'EMEF ',
      codTipoEscola: '1',
    },
    {
      ano: 5,
      anoLetivo: 2019,
      codModalidade: 5,
      dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      dreAbrev: 'DRE - IP',
      modalidade: 'Fundamental',
      nomeTurma: '5B',
      tipoSemestre: 1,
      tipoUE: 'UNIDADE ADMINISTRATIVA',
      codTipoUE: 3,
      ue: 'PRUDENTE DE MORAIS, PRES.',
      ueAbrev: 'PRUDENTE DE MORAIS, PRES.',
      tipoEscola: 'EMEF ',
      codTipoEscola: '1',
    },
    {
      ano: 5,
      anoLetivo: 2019,
      codModalidade: 5,
      dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      dreAbrev: 'DRE - IP',
      modalidade: 'Fundamental',
      nomeTurma: '5C',
      tipoSemestre: 1,
      tipoUE: 'UNIDADE ADMINISTRATIVA',
      codTipoUE: 3,
      ue: 'PRUDENTE DE MORAIS, PRES.',
      ueAbrev: 'PRUDENTE DE MORAIS, PRES.',
      tipoEscola: 'EMEF ',
      codTipoEscola: '1',
    },
    {
      ano: 6,
      anoLetivo: 2019,
      codModalidade: 5,
      dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      dreAbrev: 'DRE - IP',
      modalidade: 'Fundamental',
      nomeTurma: '6A',
      tipoSemestre: 1,
      tipoUE: 'UNIDADE ADMINISTRATIVA',
      codTipoUE: 3,
      ue: 'PRUDENTE DE MORAIS, PRES.',
      ueAbrev: 'PRUDENTE DE MORAIS, PRES.',
      tipoEscola: 'EMEF ',
      codTipoEscola: '1',
    },
    {
      ano: 6,
      anoLetivo: 2019,
      codModalidade: 5,
      dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      dreAbrev: 'DRE - IP',
      modalidade: 'Fundamental',
      nomeTurma: '6B',
      tipoSemestre: 1,
      tipoUE: 'UNIDADE ADMINISTRATIVA',
      codTipoUE: 3,
      ue: 'PRUDENTE DE MORAIS, PRES.',
      ueAbrev: 'PRUDENTE DE MORAIS, PRES.',
      tipoEscola: 'EMEF ',
      codTipoEscola: '1',
    },
    {
      ano: 7,
      anoLetivo: 2019,
      codModalidade: 5,
      dre: 'DIRETORIA REGIONAL DE EDUCACAO IPIRANGA',
      dreAbrev: 'DRE - IP',
      modalidade: 'Fundamental',
      nomeTurma: '7A',
      tipoSemestre: 1,
      tipoUE: 'UNIDADE ADMINISTRATIVA',
      codTipoUE: 3,
      ue: 'PRUDENTE DE MORAIS, PRES.',
      ueAbrev: 'PRUDENTE DE MORAIS, PRES.',
      tipoEscola: 'EMEF ',
      codTipoEscola: '1',
    },
  ]);

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

  const [periodos, setPeriodos] = useState([]);
  const [periodoSelecionado, setPeriodoSelecionado] = useState();

  const [dresFiltro, setDresFiltro] = useState([]);
  const [dreFiltroSelecionada, setDreFiltroSelecionada] = useState();

  const [unidadesEscolaresFiltro, setUnidadesEscolaresFiltro] = useState();
  const [
    unidadeEscolarFiltroSelecionada,
    setUnidadeEscolarFiltroSelecionada,
  ] = useState();

  const [
    nomeTurmaFiltroSelecionada,
    setNomeTurmaFiltroSelecionada,
  ] = useState();

  const [resultadosFiltro, setResultadosFiltro] = useState([]);

  const [toggleInputFocus, setToggleInputFocus] = useState(false);
  const [toggleBusca, setToggleBusca] = useState(false);

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

  const Caret = styled(Icon)`
    background: ${Base.CinzaDesabilitado} !important;
    max-height: 36px !important;
    max-width: 36px !important;
    padding: 0.7rem 0.9rem !important;
    right: 5px !important;
    top: 5px !important;
    ${toggleBusca && 'transform: rotate(180deg) !important;'}
  `;

  const inputBuscaRef = useRef();
  // const [turmaEscolaSelecionada, setTurmaEscolaSelecionada] = useState();

  useEffect(() => {
    const anosLetivos = [];
    const modalidades = [];
    const dres = [];
    const unidadesEscolares = [];
    const turmas = [];

    dadosProfessor.forEach(dado => {
      if (anosLetivos.findIndex(ano => ano.ano === dado.anoLetivo) < 0) {
        anosLetivos.push({ ano: dado.anoLetivo });
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

      if (dres.findIndex(dre => dre.abrev === dado.dreAbrev) < 0) {
        dres.push({
          abrev: dado.dreAbrev,
          dre: dado.dre,
        });
      }

      if (
        unidadesEscolares.findIndex(unidade => unidade.abrev === dado.ueAbrev) <
        0
      ) {
        unidadesEscolares.push({
          abrev: dado.ueAbrev,
          unidade: dado.ue,
        });
      }

      if (turmas.findIndex(turma => turma.turma === dado.nomeTurma) < 0) {
        turmas.push({
          ano: dado.ano,
          turma: dado.nomeTurma,
        });
      }
    });

    setAnosLetivosFiltro([...anosLetivos]);
    setAnoLetivoFiltroSelecionado('2019');
    setModalidadesFiltro([...modalidades]);
    setPeriodos([{ periodo: '1º Semestre' }, { periodo: '2º Semestre' }]);
    setDresFiltro([...dres]);
    setUnidadesEscolaresFiltro([...unidadesEscolares]);
    store.dispatch(turmasUusario(turmas));
  }, []);

  const usuario = useSelector(state => state.usuario);

  const turmaEscolaSelecionada =
    usuario.turmaSelecionada.length > 0
      ? `${usuario.turmaSelecionada[0].modalidade} - ${usuario.turmaSelecionada[0].nomeTurma} - ${usuario.turmaSelecionada[0].tipoEscola} - ${usuario.turmaSelecionada[0].ue}`
      : '';

  useEffect(() => {
    if (!toggleBusca && toggleInputFocus) inputBuscaRef.current.focus();
  }, [toggleBusca, toggleInputFocus]);

  const onChangeAutocomplete = () => {
    const texto = inputBuscaRef.current.value;
    const resultadosAutocomplete = [];
    if (texto.length >= 2) {
      dadosProfessor
        .filter(dado => {
          return dado.ue.toLowerCase().includes(texto);
        })
        .forEach(dado => {
          resultadosAutocomplete.push(dado);
        });
      setResultadosFiltro(resultadosAutocomplete);
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
    setAnoLetivoFiltroSelecionado(ano);
  };

  const onChangeModalidade = modalidade => {
    setmodalidadeFiltroSelecionada(modalidade);
  };

  const onChangePeriodo = periodo => {
    setPeriodoSelecionado(periodo);
  };

  const onChangeDre = dre => {
    setDreFiltroSelecionada(dre);
  };

  const onChangeUnidadeEscolarFiltro = unidade => {
    setUnidadeEscolarFiltroSelecionada(unidade);
  };

  const onChangeTurma = turma => {
    setNomeTurmaFiltroSelecionada(turma);
  };

  const selecionaTurma = () => {
    const turma = dadosProfessor.filter(dado => {
      return (
        dado.anoLetivo.toString() === anoLetivoFiltroSelecionado &&
        dado.codModalidade.toString() === modalidadeFiltroSelecionada &&
        dado.dreAbrev === dreFiltroSelecionada &&
        dado.nomeTurma === nomeTurmaFiltroSelecionada &&
        dado.ue === unidadeEscolarFiltroSelecionada
      );
    });

    if (turma.length > 0) {
      store.dispatch(selecionarTurma(turma));
      setToggleBusca(false);
      sucesso('Suas escolhas foram salvas com sucesso!');
    }
  };

  const aplicarFiltro = () => {
    if (
      anoLetivoFiltroSelecionado &&
      modalidadeFiltroSelecionada &&
      dreFiltroSelecionada &&
      nomeTurmaFiltroSelecionada &&
      unidadeEscolarFiltroSelecionada
    ) {
      selecionaTurma();
    } else {
      erro('Você precisa selecionar todas as informações!');
    }
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
            value={turmaEscolaSelecionada}
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
                  <li
                    key={shortid.generate()}
                    className="list-group-item list-group-item-action border-0"
                  >{`${resultado.NomeTurma} ${resultado.UE}`}</li>
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
                cols={modalidadeFiltroSelecionada === 'EJA' ? 5 : 9}
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
              {modalidadeFiltroSelecionada === 3 && (
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
                lista={dresFiltro}
                valueOption="abrev"
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
                valueOption="unidade"
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
                  lista={usuario.turmasUusario}
                  valueOption="turma"
                  label="turma"
                  valueSelect={nomeTurmaFiltroSelecionada}
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
