import React, { useState, useEffect, useMemo } from 'react';
import Disciplinas from './disciplinas';
import { ListItem, ListItemButton, ListaObjetivos } from './bimestre.css';
import { Button, Colors, Grid } from '~/componentes';
import Seta from '../../../recursos/Seta.svg';
import Editor from '~/componentes/editor/editor';
import servicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import { erros } from '~/servicos/alertas';

const Bimestre = ({ bimestre, disciplinas, regencia, ano, onChange }) => {
  const [objetivosAprendizagem, setObjetivosAprendizagem] = useState([]);
  const [objetivosCarregados, setObjetivosCarregados] = useState(false);
  const [objetivosSelecionados, setObjetivosSelecionados] = useState(
    bimestre.objetivosAprendizagem
  );
  const [disciplinasPreSelecionadas, setDisciplinasPreSelecionadas] = useState(
    bimestre.objetivosAprendizagem.map(c => c.componenteCurricularEolId)
  );
  const [descricaoObjetivo, setDescricaoObjetivo] = useState(
    bimestre.descricao
  );

  const onChangeDisciplinasSelecionadas = disciplinasSelecionadas => {
    if (disciplinasSelecionadas && disciplinasSelecionadas.length > 0) {
      if (!regencia)
        servicoPlanoAnual
          .obterObjetivosPorAnoEComponenteCurricular(
            ano,
            disciplinasSelecionadas
          )
          .then(resposta => {
            setObjetivosAprendizagem(resposta.data);
            setObjetivosCarregados(true);
          })
          .catch(e => erros(e));
    } else setObjetivosAprendizagem([]);
  };

  const selecionaObjetivo = objetivo => {
    const objetivoAprendizagem = objetivo;
    objetivoAprendizagem.selecionado = !objetivoAprendizagem.selecionado;
    setObjetivosAprendizagem([...objetivosAprendizagem]);
  };

  const onChangeDescricaoObjetivos = descricao => {
    setDescricaoObjetivo(descricao);
  };

  const removerTodosObjetivos = () => {
    objetivosAprendizagem.forEach(c => {
      c.selecionado = false;
    });
    setObjetivosAprendizagem([...objetivosAprendizagem]);
  };

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
  }, [objetivosCarregados]);

  useEffect(() => {
    if (objetivosSelecionados && objetivosSelecionados.length > 0) {
      const bimestreAtual = {
        ...bimestre,
        objetivosAprendizagem: objetivosSelecionados,
        descricao: descricaoObjetivo,
      };
      onChange(bimestreAtual);
    }
  }, [bimestre, descricaoObjetivo, objetivosSelecionados]);

  return (
    <div className="row">
      <Grid cols={6} className="m-b-10">
        <h6 className="d-inline-block font-weight-bold my-0 fonte-14 mb-2">
          Objetivos de aprendizagem
        </h6>
        <div>
          <Disciplinas
            disciplinas={disciplinas}
            preSelecionadas={disciplinasPreSelecionadas}
            onChange={onChangeDisciplinasSelecionadas}
          />
        </div>
        <ListaObjetivos className="mt-4 overflow-auto">
          {objetivosAprendizagem &&
            objetivosAprendizagem.length > 0 &&
            objetivosAprendizagem.map((objetivo, index) => (
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
      </Grid>
      <Grid cols={6}>
        <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
          Objetivos de aprendizagem e meus objetivos (Currículo da cidade)
        </h6>
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
        </div>
      </Grid>
    </div>
  );
};
export default Bimestre;
