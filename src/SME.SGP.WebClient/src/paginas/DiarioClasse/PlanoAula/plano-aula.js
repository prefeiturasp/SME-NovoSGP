import { Switch } from 'antd';
import React, { useEffect, useRef, useState } from 'react';
import { Colors } from '~/componentes';
import Button from '~/componentes/button';
import CardCollapse from '~/componentes/cardCollapse';
import Grid from '~/componentes/grid';
import TextEditor from '~/componentes/textEditor';
import { Badge, Corpo, Descritivo, HabilitaObjetivos, ListItem, ListItemButton, ObjetivosList, QuantidadeBotoes } from './plano-aula.css';
import api from '~/servicos/api';
import { useSelector } from 'react-redux';
import modalidade from '~/dtos/modalidade';

const PlanoAula = (props) => {
  const { planoAula, ehRegencia, listaMaterias, disciplinaIdSelecionada, dataAula, ehProfessorCj } = props;

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;
  const [mostrarCardPrincipal, setMostrarCardPrincipal] = useState(true);
  const [informaObjetivos, setInformaObjetivos] = useState(true);
  const [materias, setMaterias] = useState([...listaMaterias]);
  const configCabecalho = {
    altura: '44px',
    corBorda: '#4072d6'
  }
  const [objetivosAprendizagem, setObjetivosAprendizagem] = useState([]);
  const textEditorObjetivosRef = useRef(null);
  const textEditorDesenvAulaRef = useRef(null);
  const textEditorRecContinuaRef = useRef(null);
  const textEditorLicaoCasaRef = useRef(null);
  const ehEja =
    turmaSelecionada && turmaSelecionada.modalidade === String(modalidade.EJA)
      ? true
      : false;

  useEffect(() => {
    setMaterias(listaMaterias)
  }, [listaMaterias])

  const selecionarObjetivo = id => {
    const index = objetivosAprendizagem.findIndex(a => a.id == id);
    const idxObjSelec = planoAula.objetivosAprendizagemAula.findIndex(a => a.id == id)
    objetivosAprendizagem[index].selected = !objetivosAprendizagem[index].selected;
    if(idxObjSelec < 0){
      planoAula.objetivosAprendizagemAula.push(objetivosAprendizagem[index])
    }else{
      planoAula.objetivosAprendizagemAula.splice(idxObjSelec, 1);
      planoAula.objetivosAprendizagemAula = [...planoAula.objetivosAprendizagemAula]
    }
    setObjetivosAprendizagem([...objetivosAprendizagem]);
  }

  const removerObjetivo = id => {
    const index = objetivosAprendizagem.findIndex(a => a.id == id);
    const idxObjSelec = planoAula.objetivosAprendizagemAula.findIndex(a => a.id == id)
    objetivosAprendizagem[index].selected = false;
    planoAula.objetivosAprendizagemAula.splice(idxObjSelec, 1);
    planoAula.objetivosAprendizagemAula = [...planoAula.objetivosAprendizagemAula]
    setObjetivosAprendizagem([...objetivosAprendizagem]);
  }

  const removerTodosObjetivos = () => {
    const objetivos = objetivosAprendizagem.map(objetivo => {
      objetivo.selected = false;
      return objetivo;
    })
    planoAula.objetivosAprendizagemAula.splice(0, planoAula.objetivosAprendizagemAula.length);
    planoAula.objetivosAprendizagemAula = [...planoAula.objetivosAprendizagemAula]
    setObjetivosAprendizagem([...objetivos]);
  }

  const selecionarMateria = async id => {
    const index = materias.findIndex(a => a.id === id);
    const materia = materias[index];
    materia.selecionada = !materia.selecionada;
    if (materia.selecionada) {
      const objetivos = await
        api.get(`v1/objetivos-aprendizagem/objetivos/turmas/${turmaId}/componentes/${disciplinaIdSelecionada}/disciplinas/${id}?dataAula=${dataAula}`);
      if (objetivos && objetivos.data && objetivos.data.length > 0) {
        materia.objetivos = objetivos.data;
        materia.objetivos.forEach(objetivo => {
          const idx = objetivosAprendizagem.findIndex(obj => obj.id === objetivo.id);
          if (idx < 0) {
            objetivosAprendizagem.push(objetivo);
          }
        })
      }
    } else {
      if (objetivosAprendizagem && objetivosAprendizagem.length > 0) {
        materia.objetivos.forEach(objetivo => {
          const idx = objetivosAprendizagem.findIndex(obj => obj.codigo === objetivo.codigo);
          if (!objetivosAprendizagem[idx].selected) {
            objetivosAprendizagem.splice(idx, 1);
          }
        });
      }
    }
    setMaterias([...materias]);
  }

  const habilitarDesabilitarObjetivos = () => {
    setInformaObjetivos(!informaObjetivos);
    planoAula.temObjetivos = !informaObjetivos;
  }

  const onBlurMeusObjetivos = value => {
    planoAula.descricao = value;
  }

  const onBlurDesenvolvimentoAula = async value => {
    planoAula.desenvolvimentoAula = await value;
  }

  const onBlurRecuperacaoContinua = value => {
    planoAula.recuperacaoAula = value;
  }

  const onBlurLicaoCasa = value => {
    planoAula.licaoCasa = value;
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
        <QuantidadeBotoes className="col-md-12" hidden={ehProfessorCj || ehEja}>
          <span>Quantidade de aulas: {planoAula.qtdAulas}</span>
        </QuantidadeBotoes>
        <HabilitaObjetivos className="row d-inline-block col-md-12" hidden={!ehProfessorCj || ehEja}>
          <label>Objetivos de aprendizagem</label>
          <Switch
            onChange={() => habilitarDesabilitarObjetivos()}
            checked={informaObjetivos}
            size="default"
            className="mr-2"
          />
        </HabilitaObjetivos>
        <CardCollapse
          key="objetivos-aprendizagem"
          onClick={() => { }}
          titulo={'Objetivos de aprendizagem e meus objetivos (Currículo da Cidade)'}
          indice={'objetivos-aprendizagem'}
          show={true}
          configCabecalho={configCabecalho}
        >
          <div className="row">
            {planoAula.temObjetivos && !ehEja ?
              <Grid cols={6}>
                <h6 className="d-inline-block font-weight-bold my-0 fonte-14 w-100">
                  Objetivos de aprendizagem
                </h6>
                {ehRegencia ?
                  materias.map((materia) => {
                    return (
                      <Badge
                        role="button"
                        onClick={() => selecionarMateria(materia.id)}
                        id={materia.id}
                        alt={materia.descricao}
                        key={materia.id}
                        className={`badge badge-pill border text-dark bg-white font-weight-light px-2 py-1 mr-2
                      ${materia.selecionada ? ' badge-selecionado' : ''}`}
                      >
                        {materia.descricao}
                      </Badge>
                    );
                  })
                  : null}
                <ObjetivosList className="mt-4 overflow-auto">
                  {objetivosAprendizagem.map((objetivo, index) => {
                    return (
                      <ul
                        key={`${objetivo.id}-objetivo`}
                        className="list-group list-group-horizontal mt-3"
                      >
                        <ListItemButton
                          className={`${objetivo.selected ? 'objetivo-selecionado ' : ''}
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
            <Grid cols={planoAula.temObjetivos && !ehEja ? 6 : 12}>
              {planoAula.temObjetivos && !ehEja ?
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
                  {planoAula.temObjetivos && !ehEja ? 'Meus objetivos específicos' : 'Objetivos trabalhados'}
                </h6>
                {!planoAula.temObjetivos && !ehEja ?
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
                      value={planoAula.descricao}
                      onBlur={onBlurMeusObjetivos}
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
                onBlur={onBlurDesenvolvimentoAula}
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
                value={planoAula.recuperacaoAula}
                onBlur={onBlurRecuperacaoContinua}
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
                onBlur={onBlurLicaoCasa}
              />
            </form>
          </fieldset>
        </CardCollapse>
      </CardCollapse>
    </Corpo>
  )
};

export default PlanoAula;
