import { Switch } from 'antd';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import { Colors, Auditoria, Loader } from '~/componentes';
import Button from '~/componentes/button';
import CardCollapse from '~/componentes/cardCollapse';
import Grid from '~/componentes/grid';
import Editor from '~/componentes/editor/editor';
import {
  Badge,
  Corpo,
  Descritivo,
  HabilitaObjetivos,
  ListItem,
  ListItemButton,
  ObjetivosList,
  QuantidadeBotoes,
} from './plano-aula.css';
import api from '~/servicos/api';
import { store } from '~/redux';

// Componentes
import ModalCopiarConteudo from './componentes/ModalCopiarConteudo';
import RotasDto from '~/dtos/rotasDto';
import history from '~/servicos/history';
import { selecionaDia } from '~/redux/modulos/calendarioProfessor/actions';
import { RegistroMigrado } from '~/componentes-sgp/registro-migrado';

const PlanoAula = props => {
  const {
    planoAula,
    listaMaterias,
    carregandoMaterias,
    disciplinaIdSelecionada,
    dataAula,
    ehProfessorCj,
    ehEja,
    setModoEdicao,
    permissoesTela,
    somenteConsulta,
    ehMedio,
    temObjetivos,
    setTemObjetivos,
    expandido,
    auditoria,
    temAvaliacao,
    ehRegencia,
    onClick,
  } = props;

  const [desabilitarCampos, setDesabilitarCampos] = useState(false);
  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;
  const [mostrarCardPrincipal, setMostrarCardPrincipal] = useState(false);
  const [mostrarModalCopiarConteudo, setMostrarModalCopiarConteudo] = useState(
    false
  );
  const [materias, setMaterias] = useState([...listaMaterias]);
  const setModoEdicaoPlano = ehEdicao => {
    setModoEdicao(ehEdicao);
  };
  const habilitaDesabilitaObjetivos = temObj => {
    setTemObjetivos(temObj);
    if (!temObj && objetivosAprendizagem.length > 0) {
      setModoEdicaoPlano(true);
    }
    setEscolhaHabilitaObjetivos(temObj);
  };
  const configCabecalho = {
    altura: '44px',
    corBorda: '#4072d6',
  };
  const [objetivosAprendizagem, setObjetivosAprendizagem] = useState(
    planoAula.objetivosAprendizagemAula
  );
  const [habilitaEscolhaObjetivos, setEscolhaHabilitaObjetivos] = useState(
    false
  );
  const [carregandoObjetivos, setCarregandoObjetivos] = useState(false);
  const [
    carregandoObjetivosSelecionados,
    setCarregandoObjetivosSelecionados,
  ] = useState(false);

  useEffect(() => {
    const verificaHabilitarDesabilitarCampos = () => {
      if (planoAula && planoAula.id > 0) {
        setDesabilitarCampos(!permissoesTela.podeAlterar || somenteConsulta);
      } else {
        setDesabilitarCampos(!permissoesTela.podeIncluir || somenteConsulta);
      }
    };
    verificaHabilitarDesabilitarCampos();
  }, [permissoesTela, planoAula, somenteConsulta]);

  useEffect(() => {
    setEscolhaHabilitaObjetivos(planoAula.objetivosAprendizagemAula.length > 0);
    setObjetivosAprendizagem([...planoAula.objetivosAprendizagemAula]);
    setTimeout(() => {
      setCarregandoObjetivosSelecionados(false);
    }, 1000);
  }, [planoAula.objetivosAprendizagemAula]);

  useEffect(() => {
    setMaterias(listaMaterias);
  }, [listaMaterias]);

  const setObjetivos = objetivos => {
    planoAula.objetivosAprendizagemAula = [...objetivos];
    setObjetivosAprendizagem([...objetivos]);
  };

  const selecionarObjetivo = id => {
    setCarregandoObjetivosSelecionados(true);
    setModoEdicaoPlano(true);
    const index = objetivosAprendizagem.findIndex(
      a => a.id.toString() === id.toString()
    );
    objetivosAprendizagem[index].selected = !objetivosAprendizagem[index]
      .selected;
    setObjetivos(objetivosAprendizagem);
  };

  const removerObjetivo = id => {
    setCarregandoObjetivosSelecionados(true);
    setModoEdicaoPlano(true);
    const index = objetivosAprendizagem.findIndex(
      a => a.id.toString() === id.toString()
    );
    objetivosAprendizagem[index].selected = false;
    setObjetivos(objetivosAprendizagem);
  };

  const removerTodosObjetivos = () => {
    setCarregandoObjetivosSelecionados(true);
    setModoEdicaoPlano(true);
    const objetivos = objetivosAprendizagem.map(objetivo => {
      objetivo.selected = false;
      return objetivo;
    });
    setObjetivos(objetivos);
  };

  const selecionarMateria = async id => {
    setCarregandoObjetivos(true);
    const index = materias.findIndex(a => a.id === id);
    const materia = materias[index];
    //materia.selecionada = !materia.selecionada;
    materias.forEach(m => {
      m.selecionada = m.id === id ? !m.selecionada : false;
    });
    if (materia.selecionada) {
      removerObjetivosNaoSelecionados();
      const objetivos = await api.get(
        `v1/objetivos-aprendizagem/objetivos/turmas/${turmaId}/componentes/${disciplinaIdSelecionada}/disciplinas/${id}?dataAula=${dataAula}&regencia=${ehRegencia}`
      );
      if (objetivos && objetivos.data && objetivos.data.length > 0) {
        materia.objetivos = objetivos.data;
        let novosObjetivos = [];
        materia.objetivos.forEach(objetivo => {
          const idx = objetivosAprendizagem.findIndex(
            obj => obj.id === objetivo.id
          );
          if (idx < 0) {
            novosObjetivos.push(objetivo);
          }
        });
        setObjetivosAprendizagem(novosObjetivos.concat(objetivosAprendizagem));
      }
    } else {
      removerObjetivosNaoSelecionados();
    }
    setMaterias([...materias]);
    setCarregandoObjetivos(false);
  };

  const removerObjetivosNaoSelecionados = () => {
    let objetivosRemover = [];
    objetivosAprendizagem.forEach(objetivo => {
      if (!objetivo.selected) {
        objetivosRemover.push(objetivo);
      }
    });
    objetivosRemover.forEach(obj => {
      objetivosAprendizagem.splice(objetivosAprendizagem.indexOf(obj), 1);
    });
  };

  const onBlurMeusObjetivos = value => {
    if (value !== planoAula.descricao) {
      setModoEdicaoPlano(true);
    }
    planoAula.descricao = value;
  };

  const onBlurDesenvolvimentoAula = async value => {
    if (value !== planoAula.desenvolvimentoAula) {
      setModoEdicaoPlano(true);
    }
    planoAula.desenvolvimentoAula = await value;
  };

  const onBlurRecuperacaoContinua = value => {
    if (value !== planoAula.recuperacaoAula) {
      setModoEdicaoPlano(true);
    }
    planoAula.recuperacaoAula = value;
  };

  const onBlurLicaoCasa = value => {
    if (value !== planoAula.licaoCasa) {
      setModoEdicaoPlano(true);
    }
    planoAula.licaoCasa = value;
  };

  const layoutComObjetivos = () => {
    const naoEhEjaEMedio = !ehEja && !ehMedio;
    const resultado = !ehProfessorCj
      ? temObjetivos && naoEhEjaEMedio
      : naoEhEjaEMedio && habilitaEscolhaObjetivos;
    return resultado && !planoAula.migrado;
  };

  const aoClicarBotaoNovaAvaliacao = () => {
    store.dispatch(selecionaDia(dataAula));
    history.push(`${RotasDto.CADASTRO_DE_AVALIACAO}/novo`);
  };

  return (
    <Corpo>
      <CardCollapse
        key="plano-aula"
        onClick={() => {
          onClick();
          setMostrarCardPrincipal(!mostrarCardPrincipal);
        }}
        titulo="Plano de aula"
        indice="Plano de aula"
        show={mostrarCardPrincipal}
      >
        <Loader loading={mostrarCardPrincipal && carregandoMaterias}>
          <QuantidadeBotoes className="col-md-12">
            <span>Quantidade de aulas: {planoAula.qtdAulas}</span>
            {!temAvaliacao ? (
              <Button
                id={shortid.generate()}
                label="Nova Avaliação"
                color={Colors.Roxo}
                className="ml-auto mr-3"
                onClick={aoClicarBotaoNovaAvaliacao}
              />
            ) : null}
            <Button
              id={shortid.generate()}
              label="Copiar Conteúdo"
              icon="clipboard"
              color={Colors.Azul}
              border
              className="btnGroupItem"
              onClick={() => setMostrarModalCopiarConteudo(true)}
              disabled={!planoAula.id}
            />
            {planoAula.migrado && (
              <RegistroMigrado className="ml-3 align-self-center float-right">
                Registro Migrado
              </RegistroMigrado>
            )}
          </QuantidadeBotoes>
          <HabilitaObjetivos
            className="row d-inline-block col-md-12"
            hidden={!ehProfessorCj || ehEja || ehMedio}
          >
            <label>Objetivos de Aprendizagem e Desenvolvimento</label>
            <Switch
              onChange={() => habilitaDesabilitaObjetivos(!temObjetivos)}
              checked={habilitaEscolhaObjetivos}
              size="default"
              className="mr-2"
              disabled={
                desabilitarCampos ||
                (ehProfessorCj && !planoAula.possuiPlanoAnual)
              }
            />
          </HabilitaObjetivos>
          <CardCollapse
            key="objetivos-aprendizagem"
            onClick={() => {}}
            titulo="Objetivos de Aprendizagem e Desenvolvimento e meus objetivos (Currículo da Cidade)"
            indice="objetivos-aprendizagem"
            show
            configCabecalho={configCabecalho}
          >
            <div className="row">
              {layoutComObjetivos() ? (
                <Grid cols={6}>
                  <h6 className="d-inline-block font-weight-bold my-0 fonte-13 w-100">
                    Objetivos de Aprendizagem e Desenvolvimento
                  </h6>
                  {temObjetivos
                    ? materias.map(materia => {
                        return (
                          <Badge
                            role="button"
                            disabled={desabilitarCampos}
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

                  <Loader loading={carregandoObjetivos}>
                    <ObjetivosList className="mt-4 overflow-auto">
                      {objetivosAprendizagem.map(objetivo => {
                        return (
                          <ul
                            key={`${objetivo.id}-objetivo`}
                            className="list-group list-group-horizontal mt-3"
                          >
                            <ListItemButton
                              className={`${
                                objetivo.selected ? 'objetivo-selecionado ' : ''
                              } list-group-item d-flex align-items-center font-weight-bold fonte-14`}
                              role="button"
                              id={objetivo.id}
                              aria-pressed={!!objetivo.selected}
                              onClick={() => selecionarObjetivo(objetivo.id)}
                              onKeyUp={() => selecionarObjetivo(objetivo.id)}
                              alt={`Codigo do Objetivo : ${objetivo.codigo} `}
                              disabled={desabilitarCampos}
                            >
                              {objetivo.codigo}
                            </ListItemButton>
                            <ListItem
                              disabled={desabilitarCampos}
                              alt={objetivo.descricao}
                              className="list-group-item flex-fill p-2 fonte-12"
                            >
                              {objetivo.descricao}
                            </ListItem>
                          </ul>
                        );
                      })}
                    </ObjetivosList>
                  </Loader>
                </Grid>
              ) : null}
              <Grid cols={layoutComObjetivos() ? 6 : 12}>
                {layoutComObjetivos() ? (
                  <Loader loading={carregandoObjetivosSelecionados}>
                    <Grid cols={12}>
                      <h6 className="d-inline-block font-weight-bold my-0 fonte-13">
                        Objetivos de Aprendizagem e Desenvolvimento trabalhados
                        na aula
                      </h6>
                      <div className="row col-md-12 d-flex">
                        {objetivosAprendizagem
                          .filter(objetivo => objetivo.selected)
                          .map(selecionado => {
                            return (
                              <Button
                                key={`Objetivo${selecionado.id}`}
                                label={selecionado.codigo}
                                color={Colors.AzulAnakiwa}
                                bold
                                id={`Objetivo${selecionado.id}`}
                                indice={selecionado.id}
                                steady
                                remove
                                disabled={desabilitarCampos}
                                className="text-dark mt-3 mr-2 stretched-link"
                                onClick={() => removerObjetivo(selecionado.id)}
                              />
                            );
                          })}
                        {objetivosAprendizagem.filter(x => x.selected).length >
                        1 ? (
                          <Button
                            key="removerTodos"
                            label="Remover Todos"
                            color={Colors.CinzaBotao}
                            bold
                            alt="Remover todos os objetivos selecionados"
                            id="removerTodos"
                            height="38px"
                            width="92px"
                            fontSize="12px"
                            padding="0px 5px"
                            lineHeight="1.2"
                            steady
                            disabled={desabilitarCampos}
                            border
                            className="text-dark mt-3 mr-2 stretched-link"
                            onClick={() => removerTodosObjetivos()}
                          />
                        ) : null}
                      </div>
                    </Grid>
                  </Loader>
                ) : null}
                <Grid cols={12} className="mt-4 d-inline-block">
                  <h6 className="font-weight-bold my-0 fonte-13">
                    {layoutComObjetivos()
                      ? 'Objetivos específicos para a aula'
                      : 'Objetivos trabalhados'}
                  </h6>
                  {!layoutComObjetivos() ? (
                    <Descritivo className="d-inline-block my-0 fonte-14">
                      Para este componente curricular é necessário descrever os
                      objetivos de aprendizagem.
                    </Descritivo>
                  ) : null}
                  <fieldset className="mt-3">
                    <Editor
                      onChange={onBlurMeusObjetivos}
                      inicial={planoAula.descricao}
                    />
                  </fieldset>
                </Grid>
              </Grid>
            </div>
          </CardCollapse>

          <CardCollapse
            key="desenv-aula"
            onClick={() => {}}
            titulo="Desenvolvimento da aula"
            indice="desenv-aula"
            show
            configCabecalho={configCabecalho}
          >
            <fieldset className="mt-3">
              <Editor
                onChange={onBlurDesenvolvimentoAula}
                inicial={planoAula.desenvolvimentoAula}
              />
            </fieldset>
          </CardCollapse>

          <CardCollapse
            key="rec-continua"
            onClick={() => {}}
            titulo="Recuperação contínua"
            indice="rec-continua"
            show={false}
            configCabecalho={configCabecalho}
          >
            <fieldset className="mt-3">
              <Editor
                onChange={onBlurRecuperacaoContinua}
                inicial={planoAula.recuperacaoAula}
              />
            </fieldset>
          </CardCollapse>

          <CardCollapse
            key="licao-casa"
            onClick={() => {}}
            titulo="Lição de casa"
            indice="licao-casa"
            show={false}
            configCabecalho={configCabecalho}
          >
            <fieldset className="mt-3">
              <Editor
                onChange={onBlurLicaoCasa}
                inicial={planoAula.licaoCasa}
              />
            </fieldset>
          </CardCollapse>
          {planoAula.id > 0 && auditoria ? (
            <Auditoria
              className="mt-2"
              criadoEm={auditoria.criadoEm}
              criadoPor={auditoria.criadoPor}
              alteradoPor={auditoria.alteradoPor}
              alteradoEm={auditoria.alteradoEm}
            />
          ) : (
            ''
          )}
        </Loader>
      </CardCollapse>
      <ModalCopiarConteudo
        show={mostrarModalCopiarConteudo}
        onClose={() => setMostrarModalCopiarConteudo(false)}
        disciplina={disciplinaIdSelecionada}
        planoAula={planoAula}
      />
    </Corpo>
  );
};

export default PlanoAula;
