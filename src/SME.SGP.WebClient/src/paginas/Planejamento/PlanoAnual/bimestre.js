import React, { useState, useEffect, useMemo } from 'react';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import Disciplinas from './disciplinas';
import { ListItem, ListItemButton, ListaObjetivos } from './bimestre.css';
import { Button, Colors, Grid, Auditoria, Loader } from '~/componentes';
import Seta from '../../../recursos/Seta.svg';
import Editor from '~/componentes/editor/editor';
import servicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import { erros as mostrarErros, erro } from '~/servicos/alertas';

const Bimestre = ({
  bimestre,
  disciplinas,
  ano,
  ehEja,
  ehMedio,
  disciplinaSemObjetivo,
  selecionarObjetivo,
  onChangeDescricaoObjetivo,
  detalhesDisciplinaObjetivos,
}) => {
  const [objetivosAprendizagem, setObjetivosAprendizagem] = useState([]);
  const [objetivosCarregados, setObjetivosCarregados] = useState(false);
  const [objetivosSelecionados, setObjetivosSelecionados] = useState(
    bimestre.objetivosAprendizagem
  );
  const [disciplinasPreSelecionadas] = useState(
    bimestre.objetivosAprendizagem.map(c => c.componenteCurricularEolId)
  );
  const [descricaoObjetivo, setDescricaoObjetivo] = useState(
    bimestre.descricao
  );
  const [layoutEspecial, setLayoutEspecial] = useState(false);
  const [carregandoDados, setCarregandoDados] = useState(true);

  const getSelecionados = () => {
    if (objetivosAprendizagem && objetivosAprendizagem.length) {
      const listaObjetivosSelecionados = objetivosAprendizagem.filter(
        c => c.selecionado
      );
      return listaObjetivosSelecionados;
    }
    bimestre.objetivosAprendizagem.forEach(item => {
      item.selecionado = true;
    });
    return bimestre.objetivosAprendizagem;
  };

  const onChangeDisciplinasSelecionadas = disciplinasSelecionadas => {
    if (disciplinasSelecionadas && disciplinasSelecionadas.length > 0) {
      setCarregandoDados(true);
      servicoPlanoAnual
        .obterObjetivosPorAnoEComponenteCurricular(ano, disciplinasSelecionadas)
        .then(resposta => {
          if (objetivosSelecionados && objetivosSelecionados.length > 0) {
            resposta.data.forEach(c => {
              const objetivo = objetivosSelecionados.find(o => {
                if (String(o.id) === String(c.id)) {
                  return o;
                }
                return false;
              });
              if (objetivo) {
                c.selecionado = true;
              } else {
                c.selecionado = false;
              }
            });
            objetivosSelecionados.forEach(objetivoSelecionado => {
              const objetivo = resposta.data.find(
                o => o.id === objetivoSelecionado.id
              );
              if (!objetivo) {
                resposta.data.push(objetivoSelecionado);
              }
            });
          }
          setObjetivosAprendizagem(resposta.data);
          setObjetivosCarregados(true);
        })
        .catch(e => {
          mostrarErros(e);
        })
        .finally(() => {
          setCarregandoDados(false);
        });
    } else {
      const objs = getSelecionados();
      setObjetivosAprendizagem(objs);
      setObjetivosSelecionados(objs);
    }
  };

  const selecionaObjetivo = objetivo => {
    const objetivoAprendizagem = objetivo;
    objetivoAprendizagem.selecionado = !objetivoAprendizagem.selecionado;
    setObjetivosAprendizagem([...objetivosAprendizagem]);
    selecionarObjetivo(bimestre.bimestre, objetivoAprendizagem);
  };

  const onChangeDescricaoObjetivos = descricao => {
    setDescricaoObjetivo(descricao);
    onChangeDescricaoObjetivo(bimestre.bimestre, descricao);
  };

  const removerTodosObjetivos = () => {
    objetivosAprendizagem.forEach(c => {
      c.selecionado = false;
    });
    setObjetivosAprendizagem([...objetivosAprendizagem]);
    objetivosSelecionados.forEach(c =>
      selecionarObjetivo(bimestre.bimestre, c)
    );
    setObjetivosSelecionados([]);
  };

  useEffect(() => {
    if (
      detalhesDisciplinaObjetivos &&
      !detalhesDisciplinaObjetivos.possuiObjetivos
    ) {
      erro(
        `Não foram encontrados objetivos para a disciplina ${detalhesDisciplinaObjetivos.nome}`
      );
    }
  }, [detalhesDisciplinaObjetivos]);

  useMemo(() => {
    setLayoutEspecial(ehEja || ehMedio || disciplinaSemObjetivo);
  }, [disciplinaSemObjetivo, ehEja, ehMedio]);

  useMemo(() => {
    const listaObjetivosSelecionados = objetivosAprendizagem.filter(
      c => c.selecionado
    );
    setObjetivosSelecionados(listaObjetivosSelecionados);
  }, [objetivosAprendizagem]);

  useEffect(() => {
    if (
      objetivosCarregados &&
      bimestre.objetivosAprendizagem &&
      bimestre.objetivosAprendizagem.length > 0 &&
      objetivosAprendizagem &&
      objetivosAprendizagem.length > 0
    ) {
      const componentesCurricularesId = bimestre.objetivosAprendizagem.map(
        c => c.id
      );
      const listaObjetivosAprendizagemSelecionados = objetivosAprendizagem.map(
        c => {
          if (componentesCurricularesId.includes(c.id)) {
            c.selecionado = true;
          }
          return c;
        }
      );
      setObjetivosAprendizagem([...listaObjetivosAprendizagemSelecionados]);
    }
  }, [
    bimestre.objetivosAprendizagem,
    objetivosAprendizagem,
    objetivosCarregados,
  ]);

  useEffect(() => {
    setDescricaoObjetivo(bimestre.descricao);
  }, [bimestre.descricao]);

  return (
    <Loader loading={carregandoDados}>
      <div className="row">
        <Grid cols={6} className="m-b-10">
          {!layoutEspecial && (
            <h6 className="d-inline-block font-weight-bold my-0 fonte-14 mb-2">
              Objetivos de Aprendizagem e Desenvolvimento
            </h6>
          )}
          <div className="mt-3">
            <Disciplinas
              disciplinas={disciplinas}
              preSelecionadas={disciplinasPreSelecionadas}
              onChange={onChangeDisciplinasSelecionadas}
              layoutEspecial={layoutEspecial}
              carregandoDisciplinas={carregando =>
                setCarregandoDados(carregando)
              }
            />
          </div>

          {!layoutEspecial && (
            <ListaObjetivos className="mt-4 overflow-auto">
              {objetivosAprendizagem &&
                objetivosAprendizagem.length > 0 &&
                objetivosAprendizagem.map(objetivo => (
                  <ul
                    className="list-group list-group-horizontal mt-3"
                    key={objetivo.codigo}
                  >
                    <ListItemButton
                      className={`list-group-item d-flex align-items-center font-weight-bold fonte-14 ${objetivo.selecionado &&
                        'selecionado'}`}
                      role="button"
                      onClick={() => selecionaObjetivo(objetivo)}
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
                ))}
            </ListaObjetivos>
          )}
        </Grid>
        <Grid cols={layoutEspecial ? 12 : 6}>
          {!layoutEspecial && (
            <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
              Objetivos de Aprendizagem e Desenvolvimento/Objetivos específicos
            </h6>
          )}
          <div
            className="row col-md-12 d-flex"
            role="group"
            aria-label={`${objetivosSelecionados.length} objetivos selecionados`}
          >
            {objetivosSelecionados &&
              objetivosSelecionados.length > 0 &&
              objetivosSelecionados.map(selecionado => {
                return (
                  <Button
                    id={shortid.generate()}
                    key={selecionado.codigo}
                    label={selecionado.codigo}
                    color={Colors.AzulAnakiwa}
                    bold
                    indice={selecionado.id}
                    steady
                    remove
                    className="text-dark mt-3 mr-2 stretched-link"
                    onClick={() => selecionaObjetivo(selecionado)}
                  />
                );
              })}
            {objetivosSelecionados && objetivosSelecionados.length > 1 ? (
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
                border
                className="text-dark mt-3 mr-2 stretched-link"
                onClick={() => removerTodosObjetivos()}
              />
            ) : null}
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
              <Editor
                onChange={onChangeDescricaoObjetivos}
                inicial={descricaoObjetivo}
              />
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
    </Loader>
  );
};

Bimestre.propTypes = {
  bimestre: PropTypes.oneOfType([PropTypes.any]).isRequired,
  disciplinas: PropTypes.oneOfType([PropTypes.any]).isRequired,
  ano: PropTypes.oneOfType([PropTypes.any]).isRequired,
  ehEja: PropTypes.oneOfType([PropTypes.any]).isRequired,
  ehMedio: PropTypes.oneOfType([PropTypes.any]).isRequired,
  disciplinaSemObjetivo: PropTypes.oneOfType([PropTypes.any]).isRequired,
  selecionarObjetivo: PropTypes.oneOfType([PropTypes.any]).isRequired,
  onChangeDescricaoObjetivo: PropTypes.oneOfType([PropTypes.any]).isRequired,
  detalhesDisciplinaObjetivos: PropTypes.oneOfType([PropTypes.any]).isRequired,
};

export default Bimestre;
