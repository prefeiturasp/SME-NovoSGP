import React, { useState, useEffect, useRef, useLayoutEffect } from 'react';
import { useDispatch } from 'react-redux';
import { Badge, ObjetivosList, ListItemButton, ListItem } from './bimestre.css';
import CardCollapse from '../../../componentes/cardCollapse';
import Grid from '../../../componentes/grid';
import Button from '../../../componentes/button';
import TextEditor from '../../../componentes/textEditor';
import { Colors } from '../../../componentes/colors';
import Seta from '../../../recursos/Seta.svg';
import Servico from '../../../servicos/Paginas/PlanoAnualServices';
import { Salvar } from '../../../redux/modulos/planoAnual/action';
import { erro } from '../../../servicos/alertas';

// Utilizado para importar a função scrollIntoViewIfNeeded para navegadores que não possuem essa funcionalidade.
import '../../../componentes/scrollIntoViewIfNeeded';
import Service from '../../../servicos/Paginas/PlanoAnualServices';

const BimestreComponent = props => {
  const Ano = 1;

  const dispatch = useDispatch();

  const { bimestreDOM, eja } = props;

  const [bimestre, setBimestre] = useState({ ...bimestreDOM });
  const [objetivosSelecionados, setObjetivosSelecionados] = useState([]);
  const textEditorRef = useRef(null);

  const ListRef = useRef(null);

  useLayoutEffect(() => {
    focarObjetivo();
  });

  useEffect(() => {
    bimestre.objetivo = textEditorRef.current.state.value;

    dispatch(Salvar(bimestre.indice, bimestre));
  }, [bimestre]);

  const obterBimestre = () => {
    if (!bimestre.ehExpandido) {
      Service.obterBimestre({ ...bimestre, bimestre: bimestre.indice })
        .then(resposta => {
          if (resposta.data) {
            setBimestre({
              ...resposta.data,
              indice: bimestre.indice,
              objetivo: resposta.data.descricao,
              ehExpandido: true,
              nome: `${bimestre.indice}º ${eja ? 'Semestre' : 'Bimestre'}`
            });
            if (resposta.data.idsObjetivosAprendizagem) {
              setObjetivosSelecionados(resposta.data.objetivosAprendizagem);
            }
          }
        })
        .catch(error => {
          error.response.data.mensagens.forEach(mensagem => erro(mensagem));
        });
    } else {
      setEhExpandido(false);
    }
  };

  const focarObjetivo = () => {
    if (!bimestre.objetivoIdFocado) return;

    const Elem = document.getElementById(bimestre.objetivoIdFocado);
    const listDivObjetivos = ListRef.current;
    Elem.scrollIntoViewIfNeeded(listDivObjetivos);
  };

  const setObjetivoFocado = objetivoId => {
    bimestre.objetivoIdFocado = objetivoId;

    setBimestre({ ...bimestre });
  };

  const getObjetivos = () => {
    if (!bimestre.materias || bimestre.materias.length === 0) {
      bimestre.objetivosAprendizagem = [];
      setBimestre({ ...bimestre });
      return;
    }

    const materiasSelecionadas = bimestre.materias
      .filter(materia => materia.selected)
      .map(x => x.codigo);

    if (!materiasSelecionadas || materiasSelecionadas.length === 0) {
      bimestre.objetivosAprendizagem = [];
      setBimestre({ ...bimestre });
      return;
    }

    setEhExpandido(true);

    Servico.getObjetivoseByDisciplinas(Ano, materiasSelecionadas).then(res => {
      if (
        !bimestre.objetivosAprendizagem ||
        bimestre.objetivosAprendizagem.length === 0
      ) {
        bimestre.objetivosAprendizagem = res;
        setBimestre({ ...bimestre });
      }

      bimestre.objetivosAprendizagem = res;

      const concatenados = bimestre.objetivosAprendizagem.concat(
        res.filter(item => {
          const index = bimestre.objetivosAprendizagem.findIndex(
            x => x.codigo === item.codigo
          );

          return index < 0;
        })
      );

      bimestre.objetivosAprendizagem = concatenados;

      setBimestre({ ...bimestre });
    });
  };

  const setEhExpandido = ehExpandido => {
    bimestre.ehExpandido = ehExpandido;
    setBimestre({ ...bimestre });
  };

  const selecionaMateria = e => {
    const index = e.target.getAttribute('data-index');
    const ariaPressed = e.target.getAttribute('aria-pressed');

    bimestre.materias[index].selected = ariaPressed !== 'true';

    setBimestre({ ...bimestre });

    getObjetivos();
  };

  const selecionaObjetivo = e => {
    const index = e.target.getAttribute('data-index');
    const ariaPressed = e.target.getAttribute('aria-pressed');

    bimestre.objetivosAprendizagem[index].selected = ariaPressed !== 'true';

    setObjetivoFocado(e.target.id);

    setBimestre({ ...bimestre });
  };

  const removeObjetivoSelecionado = e => {
    const indice = bimestre.objetivosAprendizagem.findIndex(
      objetivo => objetivo.id == e.target.id
    );

    if (bimestre.objetivosAprendizagem[indice])
      bimestre.objetivosAprendizagem[indice].selected = false;

    setBimestre({ ...bimestre });
  };

  const onBlurTextEditor = value => {
    setEhExpandido(true);

    bimestre.objetivo = value;

    setBimestre({ ...bimestre });
  };

  return (
    <CardCollapse
      key={bimestre.indice}
      titulo={bimestre.nome}
      indice={`Bimestre${bimestre.indice}`}
      show={bimestre.ehExpandido}
      onClick={obterBimestre}
    >
      <div className="row">
        <Grid cols={6}>
          <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
            Objetivos de aprendizagem
          </h6>
          <div>
            {bimestre.materias && bimestre.materias.length > 0
              ? bimestre.materias.map((materia, indice) => {
                  return (
                    <Badge
                      role="button"
                      onClick={selecionaMateria}
                      aria-pressed={materia.selected && true}
                      id={materia.codigo}
                      data-index={indice}
                      key={materia.codigo}
                      className="badge badge-pill border text-dark bg-white font-weight-light px-2 py-1 mt-3 mr-2"
                    >
                      {materia.materia}
                    </Badge>
                  );
                })
              : null}
          </div>
          <ObjetivosList ref={ListRef} className="mt-4 overflow-auto">
            {bimestre.objetivosAprendizagem &&
            bimestre.objetivosAprendizagem.length > 0
              ? bimestre.objetivosAprendizagem.map((objetivo, indice) => {
                  return (
                    <ul
                      key={`${objetivo.id}Bimestre`}
                      className="list-group list-group-horizontal mt-3"
                    >
                      <ListItemButton
                        className="list-group-item d-flex align-items-center font-weight-bold fonte-14"
                        role="button"
                        id={`${bimestre.indice}Bimestre${objetivo.id}`}
                        aria-pressed={!!objetivo.selected}
                        data-index={indice}
                        onClick={selecionaObjetivo}
                        onKeyUp={selecionaObjetivo}
                      >
                        {objetivo.codigo}
                      </ListItemButton>
                      <ListItem className="list-group-item flex-fill p-2 fonte-12">
                        {objetivo.descricao}
                      </ListItem>
                    </ul>
                  );
                })
              : null}
          </ObjetivosList>
        </Grid>
        <Grid cols={6}>
          <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
            Objetivos de aprendizagem e meus objetivos (Currículo da cidade)
          </h6>
          <div
            role="group"
            aria-label={`${bimestre.objetivosAprendizagem &&
              bimestre.objetivosAprendizagem.length > 0 &&
              bimestre.objetivosAprendizagem.filter(
                objetivo => objetivo.selected
              ).length} objetivos selecionados`}
          >
            {objetivosSelecionados &&
            objetivosSelecionados.length > 0
              ? objetivosSelecionados
                  .map(selecionado => {
                    return (
                      <Button
                        key={selecionado.id}
                        label={selecionado.codigo}
                        color={Colors.AzulAnakiwa}
                        bold
                        id={selecionado.id}
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
                  value={bimestre.objetivo}
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
