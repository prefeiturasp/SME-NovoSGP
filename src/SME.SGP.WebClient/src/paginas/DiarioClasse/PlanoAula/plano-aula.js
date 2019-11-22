import React, { useState, useRef, useEffect } from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import styled from 'styled-components';
import TextEditor from '~/componentes/textEditor/component';
import Grid from '~/componentes/grid';
import { Base, Colors } from '~/componentes';
import Button from '~/componentes/button';
import {QuantidadeBotoes, ObjetivosList, ListItem, ListItemButton, Corpo, Descritivo, Badge} from './plano-aula.css';

const PlanoAula = (props) => {
  const [mostrarCardPrincipal, setMostrarCardPrincipal] = useState(false);
  const [quantidadeAulas, setQuantidadeAulas] = useState(0);
  const [planoAula, setPlanoAula] = useState({
    temObjetivos: true,
    selecionaDisciplinas: true,
    objetivosEspecificos: 'teste',
    desenvolvimentoAula: 'teste',
    recuperacaoContinua: 'teste',
    licaoCasa: 'teste'
  })
  const configCabecalho = {
    altura: '44px',
    corBorda: '#4072d6'
  }
  const [objetivosAprendizagem, setObjetivosAprendizagem] = useState([
    {
      id: 1,
      selected: false,
      codigo: 'EF45644',
      descricao: 'Teste de descrição'
    },
    {
      id: 2,
      selected: true,
      codigo: 'EF45645',
      descricao: 'Teste de descrição, teste teste teste teste teste teste teste teste teste teste teste teste teste teste teste'
    },
  ]);
  const textEditorObjetivosRef = useRef(null);
  const textEditorDesenvAulaRef = useRef(null);
  const textEditorRecContinuaRef = useRef(null);
  const textEditorLicaoCasaRef = useRef(null);

  const selecionarObjetivo = id => {
    const index = objetivosAprendizagem.findIndex(a => a.id == id);
    objetivosAprendizagem[index].selected = !objetivosAprendizagem[index].selected;
    setObjetivosAprendizagem([...objetivosAprendizagem]);
  }

  const removerObjetivo = id => {
    const index = objetivosAprendizagem.findIndex(a => a.id == id);
    objetivosAprendizagem[index].selected = false;
    setObjetivosAprendizagem([...objetivosAprendizagem]);
  }

  const removerTodosObjetivos = () => {
    const objetivos = objetivosAprendizagem.map(objetivo => {
      objetivo.selected = false;
      return objetivo;
    })
    setObjetivosAprendizagem([...objetivos]);
  }

  return (
    <Corpo>
      <CardCollapse
        key="plano-aula"
        onClick={() => { setMostrarCardPrincipal(!mostrarCardPrincipal) }}
        titulo={'Plano de aula'}
        indice={'Plano de aula'}
        show={mostrarCardPrincipal}
      >
        <QuantidadeBotoes className="col-md-12">
          <span>Quantidade de aulas: {quantidadeAulas}</span>
        </QuantidadeBotoes>
        <CardCollapse
          key="objetivos-aprendizagem"
          onClick={() => { }}
          titulo={'Objetivos de aprendizagem e meus objetivos (Currículo da Cidade)'}
          indice={'objetivos-aprendizagem'}
          show={true}
          configCabecalho={configCabecalho}
        >
          <div className="row">
            {planoAula.temObjetivos ?
              <Grid cols={6}>
                <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
                  Objetivos de aprendizagem
            </h6>
                <ObjetivosList className="mt-4 overflow-auto">
                  {objetivosAprendizagem.map((objetivo, index) => {
                    return (
                      <ul
                        key={`${objetivo.id}-objetivo`}
                        className="list-group list-group-horizontal mt-3"
                      >
                        <ListItemButton
                          className={`${objetivo.selected ? 'selecionado ' : ''}
                        list-group-item d-flex align-items-center font-weight-bold fonte-14`}
                          role="button"
                          id={objetivo.id}
                          aria-pressed={objetivo.selected ? true : false}
                          onClick={() => selecionarObjetivo(objetivo.id)}
                          onKeyUp={() => selecionarObjetivo(objetivo.id)}
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
                  })}
                </ObjetivosList>
              </Grid>
              : null}
            <Grid cols={planoAula.temObjetivos ? 6 : 12}>
              {planoAula.temObjetivos ?
                <Grid cols={12}>
                  <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
                    Objetivos trabalhados na aula
              </h6>
                  <div className="row col-md-12 d-flex">
                    {objetivosAprendizagem
                      .filter(objetivo => objetivo.selected)
                      .map((selecionado) => {
                        return (
                          <Button
                            key={`Objetivo${selecionado.id}`}
                            label={selecionado.codigo}
                            color={Colors.AzulAnakiwa}
                            bold
                            id={`Objetivo${selecionado.id}`}
                            indice={selecionado.id}
                            disabled={false}
                            steady
                            remove
                            className="text-dark mt-3 mr-2 stretched-link"
                            onClick={() => removerObjetivo(selecionado.id)}
                          />
                        );
                      })}
                    {objetivosAprendizagem.filter(x => x.selected).length >
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
                          onClick={() => removerTodosObjetivos()}
                        />
                      ) : null}
                  </div>
                </Grid>
                : null}
              <Grid cols={12} className="mt-4 d-inline-block">
                <h6 className="font-weight-bold my-0 fonte-14">
                  {planoAula.temObjetivos ? 'Meus objetivos específicos' : 'Objetivos trabalhados'}
                </h6>
                {!planoAula.temObjetivos ?
                  <Descritivo className="d-inline-block my-0 fonte-14">
                    Para este componente curricular é necessário descrever os objetivos de aprendizagem.
                  </Descritivo>
                  : null}
                <fieldset className="mt-3">
                  <form action="">
                    <TextEditor
                      className="form-control"
                      ref={textEditorObjetivosRef}
                      id="textEditor-meus_objetivos"
                      height="135px"
                      alt="Meus objetivos específicos"
                      value={planoAula.objetivosEspecificos}
                    />
                  </form>
                </fieldset>
              </Grid>
            </Grid>
          </div>
        </CardCollapse>

        <CardCollapse
          key="desenv-aula"
          onClick={() => { }}
          titulo={'Desenvolvimento da aula'}
          indice={'desenv-aula'}
          show={false}
          configCabecalho={configCabecalho}
        >
          <fieldset className="mt-3">
            <form action="">
              <TextEditor
                className="form-control"
                id="textEditor-desenv-aula"
                ref={textEditorDesenvAulaRef}
                height="135px"
                alt="Desenvolvimento da aula"
                value={planoAula.desenvolvimentoAula}
              />
            </form>
          </fieldset>
        </CardCollapse>

        <CardCollapse
          key="rec-continua"
          onClick={() => { }}
          titulo={'Recuperação contínua'}
          indice={'rec-continua'}
          show={false}
          configCabecalho={configCabecalho}
        >
          <fieldset className="mt-3">
            <form action="">
              <TextEditor
                className="form-control"
                id="textEditor-rec-continua"
                ref={textEditorRecContinuaRef}
                height="135px"
                alt="Recuperação contínua"
                value={planoAula.recuperacaoContinua}
              />
            </form>
          </fieldset>
        </CardCollapse>

        <CardCollapse
          key="licao-casa"
          onClick={() => { }}
          titulo={'Lição de casa'}
          indice={'licao-casa'}
          show={false}
          configCabecalho={configCabecalho}
        >
          <fieldset className="mt-3">
            <form action="">
              <TextEditor
                className="form-control"
                id="textEditor-licao-casa"
                ref={textEditorLicaoCasaRef}
                height="135px"
                alt="Lição de casa"
                value={planoAula.licaoCasa}
              />
            </form>
          </fieldset>
        </CardCollapse>
      </CardCollapse>
    </Corpo>
  )
};

export default PlanoAula;
