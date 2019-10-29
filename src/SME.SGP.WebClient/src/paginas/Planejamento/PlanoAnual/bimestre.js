import React, { useEffect, useRef, useLayoutEffect, useState } from 'react';
import {
  Badge,
  ObjetivosList,
  ListItemButton,
  ListItem,
  H5,
  BoxAuditoria,
} from './bimestre.css';
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
  SelecionarObjetivo,
  SetarDescricao,
  ObterBimestreServidor,
  removerSelecaoTodosObjetivos,
} from '../../../redux/modulos/planoAnual/action';
import Auditoria from '~/componentes/auditoria';

//Utilizado para importar a função scrollIntoViewIfNeeded para navegadores que não possuem essa funcionalidade.
import '../../../componentes/scrollIntoViewIfNeeded';
import modalidade from '~/dtos/modalidade';
import { erro } from '~/servicos/alertas';

const BimestreComponent = props => {
  const dispatch = useDispatch();

  const { indice, disabled, modalidadeEja, disciplinaSelecionada } = props;

  const bimestre = useSelector(store => store.bimestres.bimestres[indice]);

  const { LayoutEspecial } = bimestre;

  const { materias } = bimestre;

  const { focado } = bimestre;

  const objetivos = bimestre.objetivosAprendizagem;

  const [idObjetivoFocado, setIDObjetivoFocado] = useState('0');

  const [estadoAdicionalEditorTexto, setEstadoAdicionalEditorTexto] = useState({
    focado: false,
    ultimoFoco: null,
  });

  const textEditorRef = useRef(null);

  const ListRef = useRef(null);

  const bimestreJaObtidoServidor = bimestre.ehExpandido;

  useLayoutEffect(() => {
    if (!bimestre.setarObjetivo) {
      setarDescricaoFunction(descricaoFunction);
    }
  }, []);

  useEffect(() => {
    obterObjetivos();
  }, [materias]);

  useLayoutEffect(() => {
    focarObjetivo();
  }, [objetivos]);

  const descricaoFunction = () => {
    return textEditorRef.current.state.value;
  };

  const setarDescricaoFunction = descricaoFunction => {
    dispatch(SetarDescricaoFunction(indice, descricaoFunction));
  };

  const obterObjetivos = () => {
    dispatch(ObterObjetivosCall(bimestre));
  };

  const setarDescricao = descricao => {
    setarDescricaoFunction(descricaoFunction);
    dispatch(SetarDescricao(indice, descricao));
  };

  const selecionarMaterias = (index, selecionarMaterias) => {
    dispatch(SelecionarMateria(indice, index, selecionarMaterias));
  };

  const focarObjetivo = () => {
    if (!idObjetivoFocado || idObjetivoFocado === '0') return;

    const Elem = document.getElementById(idObjetivoFocado);

    if (!Elem) return;

    const listDivObjetivos = ListRef.current;
    Elem.scrollIntoViewIfNeeded(listDivObjetivos);
  };

  const setObjetivoFocado = objetivoId => {
    setIDObjetivoFocado(objetivoId);
  };

  const selecionarObjetivo = (index, ariaPressed) => {
    dispatch(SelecionarObjetivo(indice, index, ariaPressed));
  };

  const setEhExpandido = ehExpandido => {
    if (bimestre) dispatch(SalvarEhExpandido(indice, ehExpandido));
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

  const onClickTextEditor = ultimoFoco => {
    if (!bimestre.ehEdicao) {
      setEhExpandido(true);

      setEstadoAdicionalEditorTexto({
        focado: true,
        ultimoFoco,
      });
    }
  };

  const removeObjetivoSelecionado = e => {
    const index = bimestre.objetivosAprendizagem.findIndex(
      objetivo => objetivo.id == e.target.id
    );

    selecionarObjetivo(index, false);
  };

  const removerTodosObjetivoSelecionado = () => {
    dispatch(removerSelecaoTodosObjetivos(indice));
  };

  const onBlurTextEditor = value => {
    if (!bimestre) return;

    setEhExpandido(true);

    setEstadoAdicionalEditorTexto({
      focado: false,
      ultimoFoco: null,
    });

    setarDescricao(value);
    setarDescricaoFunction(descricaoFunction);
  };

  const onClickBimestre = () => {
    if (!disciplinaSelecionada) {
      erro(
        'Não é possivel salvar um plano anual sem selecionar uma disciplina'
      );
      return;
    }

    if (!bimestreJaObtidoServidor)
      dispatch(
        ObterBimestreServidor(bimestre, disciplinaSelecionada, LayoutEspecial)
      );
  };

  return (
    <CardCollapse
      key={indice}
      onClick={onClickBimestre}
      titulo={bimestre.nome}
      indice={`Bimestre${indice}`}
      show={bimestre.ehExpandido}
      disabled={disabled}
      alt={`Card ${indice}º Bimestre`}
    >
      <div className="row">
        <Grid cols={LayoutEspecial ? 12 : 6} className="m-b-10">
          {LayoutEspecial ? null : (
            <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
              Objetivos de aprendizagem
            </h6>
          )}
          <div>
            {bimestre.materias && bimestre.materias.length > 0
              ? bimestre.materias.map((materia, indice) => {
                  return (
                    <Badge
                      role="button"
                      onClick={selecionaMateria}
                      aria-pressed={materia.selecionada && true}
                      id={materia.codigo}
                      data-index={indice}
                      alt={materia.materia}
                      key={materia.codigo}
                      disabled={
                        disabled || LayoutEspecial || !materia.possuiObjetivos
                      }
                      readonly={LayoutEspecial}
                      className={`badge badge-pill border text-dark bg-white font-weight-light px-2 py-1 ${
                        LayoutEspecial ? '' : 'mt-3'
                      } mr-2`}
                    >
                      {materia.materia}
                    </Badge>
                  );
                })
              : null}
          </div>
          {LayoutEspecial ? null : (
            <ObjetivosList ref={ListRef} className="mt-4 overflow-auto">
              {bimestre.objetivosAprendizagem &&
              bimestre.objetivosAprendizagem.length > 0
                ? bimestre.objetivosAprendizagem.map((objetivo, index) => {
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
                          alt={`Codigo do Objetivo : ${objetivo.codigo} `}
                        >
                          {objetivo.codigo}
                        </ListItemButton>
                        <ListItem
                          alt={objetivo.descricao}
                          className="list-group-item flex-fill p-2 fonte-12"
                        >
                          {objetivo.descricao}
                        </ListItem>
                      </ul>
                    );
                  })
                : null}
            </ObjetivosList>
          )}
        </Grid>
        <Grid cols={LayoutEspecial ? 12 : 6}>
          {LayoutEspecial ? null : (
            <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
              Objetivos de aprendizagem e meus objetivos (Currículo da cidade)
            </h6>
          )}
          {LayoutEspecial ? null : (
            <div
              className="row col-md-12 d-flex"
              role="group"
              aria-label={`${bimestre.objetivosAprendizagem &&
                bimestre.objetivosAprendizagem.length > 0 &&
                bimestre.objetivosAprendizagem.filter(
                  objetivo => objetivo.selected
                ).length} objetivos selecionados`}
            >
              {bimestre.objetivosAprendizagem &&
              bimestre.objetivosAprendizagem.length > 0
                ? bimestre.objetivosAprendizagem
                    .filter(objetivo => objetivo.selected)
                    .map(selecionado => {
                      return (
                        <Button
                          key={`Objetivo${selecionado.id}Selecionado${indice}`}
                          label={selecionado.codigo}
                          color={Colors.AzulAnakiwa}
                          bold
                          id={`Objetivo${selecionado.id}Selecionado${indice}Id`}
                          disabled={disabled}
                          steady
                          remove
                          className="text-dark mt-3 mr-2 stretched-link"
                          onClick={removeObjetivoSelecionado}
                        />
                      );
                    })
                : null}
              {bimestre.objetivosAprendizagem &&
              bimestre.objetivosAprendizagem.length > 0 &&
              bimestre.objetivosAprendizagem.filter(x => x.selected).length >
                1 ? (
                <Button
                  key={`removerTodos`}
                  label={`Remover Todos`}
                  color={Colors.CinzaBotao}
                  bold
                  alt="Remover todos os objetivos selecionados"
                  id={`removerTodos`}
                  height="38px"
                  width="92px"
                  fontSize="12px"
                  padding="0px 5px"
                  lineHeight="1.2"
                  steady
                  border
                  className="text-dark mt-3 mr-2 stretched-link"
                  onClick={removerTodosObjetivoSelecionado}
                />
              ) : null}
            </div>
          )}
          <div className="mt-4">
            <h6 className="d-inline-block font-weight-bold my-0 mr-2 fonte-14">
              {modalidadeEja ? 'Planejamento Semestral' : 'Planejamento Anual'}
            </h6>
            <span className="text-secondary font-italic fonte-12">
              Itens autorais do professor
            </span>
            <p className="text-secondary mt-3 fonte-13">
              É importante seguir a seguinte estrutura:
            </p>
            <ul className="list-group list-group-horizontal fonte-10 col-md-12">
              <li className="list-group-item border-right-0 p-r-10 p-l-10 py-1 col-md-2">
                Objetivos
              </li>
              <li className="list-group-item border-left-0 border-right-0 px-0 py-1 col-md-1">
                <img src={Seta} alt="Próximo" />
              </li>
              <li className="list-group-item border-left-0 border-right-0 px-0 py-1 col-md-2">
                Conteúdo
              </li>
              <li className="list-group-item border-left-0 border-right-0 px-0 py-1 col-md-1">
                <img src={Seta} alt="Próximo" />
              </li>
              <li className="list-group-item border-left-0 border-right-0 px-0 py-1 col-md-2">
                Estratégia
              </li>
              <li className="list-group-item border-left-0 border-right-0 px-0 py-1 col-md-1">
                <img src={Seta} alt="Próximo" />
              </li>
              <li className="list-group-item border-left-0 px-0 py-1 col-md-2">
                Avaliação
              </li>
            </ul>
            <fieldset className="mt-3">
              <form action="">
                <TextEditor
                  className="form-control"
                  ref={textEditorRef}
                  id="textEditor"
                  height="135px"
                  alt="Descrição do plano Anual"
                  disabled={disabled}
                  estadoAdicional={estadoAdicionalEditorTexto}
                  onClick={onClickTextEditor}
                  value={bimestre.objetivo}
                  onBlur={onBlurTextEditor}
                />
              </form>
            </fieldset>
            <Grid cols={12} className="p-0">
              <Auditoria
                criadoPor={bimestre.criadoPor}
                criadoEm={bimestre.criadoEm}
                alteradoPor={bimestre.alteradoPor}
                alteradoEm={bimestre.alteradoEm}
                alteradoRf={bimestre.alteradoRF > 0 && bimestre.alteradoRF}
                criadoRf={bimestre.criadoRF > 0 && bimestre.criadoRF}
              />
            </Grid>
          </div>
        </Grid>
      </div>
    </CardCollapse>
  );
};

export default BimestreComponent;
