import React, { useEffect, useRef, useLayoutEffect } from 'react';
import { Badge, ObjetivosList, ListItemButton, ListItem } from './bimestre.css';
import CardCollapse from '../../../componentes/cardCollapse';
import Grid from '../../../componentes/grid';
import Button from '../../../componentes/button';
import TextEditor from '../../../componentes/textEditor';
import { Colors } from '../../../componentes/colors';
import Seta from '../../../recursos/Seta.svg';
import { useDispatch, useSelector } from 'react-redux';
import {
  ObterObjetivosCall,
  SalvarEhExpandido,
  SelecionarMateria,
  SetarDescricaoFunction,
  SalvarObjetivos,
  DefinirObjetivoFocado,
  SelecionarObjetivo,
  SetarDescricao,
  ObterBimestreServidor,
} from '../../../redux/modulos/planoAnual/action';

//Utilizado para importar a função scrollIntoViewIfNeeded para navegadores que não possuem essa funcionalidade.
import '../../../componentes/scrollIntoViewIfNeeded';

const BimestreComponent = props => {
  const dispatch = useDispatch();

  const { indice, disabled } = props;

  const bimestres = useSelector(store => store.bimestres.bimestres);

  const materias = bimestres[indice].materias;

  const textEditorRef = useRef(null);

  const ListRef = useRef(null);

  useLayoutEffect(() => {
    focarObjetivo();

    if (!bimestres[indice].setarObjetivo) {
      setarDescricaoFunction(descricaoFunction);
    }
  });

  useEffect(() => {
    obterObjetivos();
  }, [materias]);

  const descricaoFunction = () => {
    return textEditorRef.current.state.value;
  };

  const setarDescricaoFunction = descricaoFunction => {
    dispatch(SetarDescricaoFunction(indice, descricaoFunction));
  };

  const obterObjetivos = () => {
    dispatch(ObterObjetivosCall(bimestres[indice]));
  };

  const setarDescricao = descricao => {
    dispatch(SetarDescricao(indice, descricao));
  };

  const selecionarMaterias = (index, selecionarMaterias) => {
    dispatch(SelecionarMateria(indice, index, selecionarMaterias));
  };

  const focarObjetivo = () => {
    if (
      !bimestres[indice].objetivoIdFocado ||
      bimestres[indice].objetivoIdFocado === 0
    )
      return;

    const Elem = document.getElementById(bimestres[indice].objetivoIdFocado);
    const listDivObjetivos = ListRef.current;
    Elem.scrollIntoViewIfNeeded(listDivObjetivos);

    DefinirObjetivoFocado(indice, 0);
  };

  const setObjetivoFocado = objetivoId => {
    dispatch(DefinirObjetivoFocado(indice, objetivoId));
  };

  const selecionarObjetivo = (index, ariaPressed) => {
    dispatch(SelecionarObjetivo(indice, index, ariaPressed));
  };

  const setEhExpandido = ehExpandido => {
    dispatch(SalvarEhExpandido(indice, ehExpandido));
  };

  const selecionaMateria = async e => {
    const index = e.target.getAttribute('data-index');
    const ariaPressed = e.target.getAttribute('aria-pressed') !== 'true';

    setEhExpandido(true);

    selecionarMaterias(index, ariaPressed);
  };

  const selecionaObjetivo = e => {
    const index = e.target.getAttribute('data-index');
    const ariaPressed = e.target.getAttribute('aria-pressed') !== 'true';

    setObjetivoFocado(e.target.id);

    selecionarObjetivo(index, ariaPressed);
  };

  const onClickTextEditor = () => {
    setEhExpandido(true);
  };

  const removeObjetivoSelecionado = e => {
    const index = bimestres[indice].objetivosAprendizagem.findIndex(
      objetivo => objetivo.id == e.target.id
    );

    selecionarObjetivo(index, false);
  };

  const onBlurTextEditor = value => {
    setEhExpandido(true);

    setarDescricao(value);
  };

  const onClickBimestre = () => {
    dispatch(ObterBimestreServidor(bimestres[indice]));
  };

  return (
    <CardCollapse
      key={indice}
      onClick={onClickBimestre}
      titulo={bimestres[indice].nome}
      indice={`Bimestre${indice}`}
      show={bimestres[indice].ehExpandido}
      disabled={disabled}
    >
      <div className="row">
        <Grid cols={6}>
          <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
            Objetivos de aprendizagem
          </h6>
          <div>
            {bimestres[indice].materias && bimestres[indice].materias.length > 0
              ? bimestres[indice].materias.map((materia, indice) => {
                  return (
                    <Badge
                      role="button"
                      onClick={selecionaMateria}
                      aria-pressed={materia.selected && true}
                      id={materia.codigo}
                      data-index={indice}
                      key={materia.codigo}
                      disabled={disabled}
                      className="badge badge-pill border text-dark bg-white font-weight-light px-2 py-1 mt-3 mr-2"
                    >
                      {materia.materia}
                    </Badge>
                  );
                })
              : null}
          </div>
          <ObjetivosList ref={ListRef} className="mt-4 overflow-auto">
            {bimestres[indice].objetivosAprendizagem &&
            bimestres[indice].objetivosAprendizagem.length > 0
              ? bimestres[indice].objetivosAprendizagem.map(
                  (objetivo, index) => {
                    return (
                      <ul
                        key={`${objetivo.id}Bimestre${index}`}
                        className="list-group list-group-horizontal mt-3"
                      >
                        <ListItemButton
                          className="list-group-item d-flex align-items-center font-weight-bold fonte-14"
                          role="button"
                          id={`${indice}Bimestre${objetivo.id}`}
                          aria-pressed={objetivo.selected ? true : false}
                          data-index={index}
                          onClick={selecionaObjetivo}
                          onKeyUp={selecionaObjetivo}
                          disabled={disabled}
                        >
                          {objetivo.codigo}
                        </ListItemButton>
                        <ListItem className="list-group-item flex-fill p-2 fonte-12">
                          {objetivo.descricao}
                        </ListItem>
                      </ul>
                    );
                  }
                )
              : null}
          </ObjetivosList>
        </Grid>
        <Grid cols={6}>
          <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
            Objetivos de aprendizagem e meus objetivos (Currículo da cidade)
          </h6>
          <div
            role="group"
            aria-label={`${bimestres[indice].objetivosAprendizagem &&
              bimestres[indice].objetivosAprendizagem.length > 0 &&
              bimestres[indice].objetivosAprendizagem.filter(
                objetivo => objetivo.selected
              ).length} objetivos selecionados`}
          >
            {bimestres[indice].objetivosAprendizagem &&
            bimestres[indice].objetivosAprendizagem.length > 0
              ? bimestres[indice].objetivosAprendizagem
                  .filter(objetivo => objetivo.selected)
                  .map(selecionado => {
                    return (
                      <Button
                        key={selecionado.id}
                        label={selecionado.codigo}
                        color={Colors.AzulAnakiwa}
                        bold
                        id={selecionado.id}
                        disabled={disabled}
                        steady
                        remove
                        className="text-dark mt-3 mr-2 stretched-link"
                        onClick={removeObjetivoSelecionado}
                      />
                    );
                  })
              : null}
          </div>
          <div className="mt-4">
            <h6 className="d-inline-block font-weight-bold my-0 mr-2 fonte-14">
              Planejamento Anual
            </h6>
            <span className="text-secondary font-italic fonte-12">
              Itens autorais do professor
            </span>
            <p className="text-secondary mt-3 fonte-13">
              É importante seguir a seguinte estrutura:
            </p>
            <ul className="list-group list-group-horizontal fonte-10">
              <li className="list-group-item border-right-0 py-1">Objetivos</li>
              <li className="list-group-item border-left-0 border-right-0 px-0 py-1">
                <img src={Seta} alt="Próximo" />
              </li>
              <li className="list-group-item border-left-0 border-right-0 py-1">
                Conteúdo
              </li>
              <li className="list-group-item border-left-0 border-right-0 px-0 py-1">
                <img src={Seta} alt="Próximo" />
              </li>
              <li className="list-group-item border-left-0 border-right-0 py-1">
                Estratégia
              </li>
              <li className="list-group-item border-left-0 border-right-0 px-0 py-1">
                <img src={Seta} alt="Próximo" />
              </li>
              <li className="list-group-item border-left-0 py-1">Avaliação</li>
            </ul>
            <fieldset className="mt-3">
              <form action="">
                <TextEditor
                  className="form-control"
                  ref={textEditorRef}
                  id="textEditor"
                  height="135px"
                  height="135px"
                  disabled={disabled}
                  onClick={onClickTextEditor}
                  value={bimestres[indice].objetivo}
                  onBlur={onBlurTextEditor}
                />
              </form>
            </fieldset>
          </div>
        </Grid>
      </div>
    </CardCollapse>
  );
};

export default BimestreComponent;
