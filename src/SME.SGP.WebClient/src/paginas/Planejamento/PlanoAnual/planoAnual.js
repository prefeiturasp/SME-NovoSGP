import React, { useState, useRef } from 'react';
import shortid from 'shortid';
import styled from 'styled-components';
import CardCollapse from '../../../componentes/cardCollapse';
import Grid from '../../../componentes/grid';
import Button from '../../../componentes/button';
import TextEditor from '../../../componentes/textEditor';
import { Colors, Base } from '../../../componentes/colors';
import Seta from '../../../recursos/Seta.svg';
import { confirmacao } from '../../../servicos/alertas';

export default function PlanoAnual() {

  const textEditorRef = useRef(null);

  const [bimestres, setBimestres] = useState([
    { nome: '1º Bimestre', materias: [] },
    { nome: '2º Bimestre', materias: [] },
    {
      nome: '3º Bimestre',
      materias: [
        { materia: 'Ciências' },
        { materia: 'História' },
        { materia: 'Geografia' },
      ],
      objetivo:
        'In semper mi vitae nulla bibendum, ut dictum magna dictum. Morbi sodales rutrum turpis, sit amet fringilla orci rutrum sit amet. Nulla tristique dictum neque, ac placerat urna aliquam non. Sed commodo tellus ac hendrerit mollis. Mauris et congue nulla.',
    },
    { nome: '4º Bimestre', materias: [] },
  ]);

  const [objetivos, setObjetivos] = useState([
    {
      id: 1623,
      year: 'third',
      code: 'EF03EF01',
      selected: false,
      description:
        'Vivenciar/experimentar/fruir brincadeiras e jogos do contexto familiar/comunitário, incluindo os de matrizes africanas e indígenas, prezando pelo trabalho coletivo e pelo protagonismo e relacionando os elementos comuns a essas brincadeiras.',
      curricular_component_id: 3,
      created_at: '2019-01-09T18:54:00.495Z',
      updated_at: '2019-01-09T18:54:00.495Z',
    },
    {
      id: 1624,
      year: 'third',
      code: 'EF03EF02',
      selected: true,
      description:
        'Planejar e utilizar estratégias para resolver desafios de brincadeiras e jogos do contexto familiar/comunitário, incluindo os de matrizes africanas e indígenas, com base no reconhecimento das características dessas práticas.',
      curricular_component_id: 3,
      created_at: '2019-01-09T18:54:34.206Z',
      updated_at: '2019-01-09T18:54:34.206Z',
    },
    {
      id: 1625,
      year: 'third',
      code: 'EF03EF03',
      selected: false,
      description:
        'Descrever, por meio de múltiplas linguagens (corporal, oral e escrita e audiovisual), as brincadeiras e jogos regionais e populares de matrizes africanas e indígenas, explicando suas características e a importância desse patrimônio histórico-cultural na preservação das diferentes culturas.',
      curricular_component_id: 3,
      created_at: '2019-01-09T18:55:13.424Z',
      updated_at: '2019-01-10T18:47:34.270Z',
    },
    {
      id: 1626,
      year: 'third',
      code: 'EF03EF04',
      selected: true,
      description:
        'Recriar, individual e coletivamente, brincadeiras e jogos do contexto familiar/comunitário, incluindo os de matrizes africanas e indígenas e demais práticas corporais tematizadas na escola, adequandoas aos espaços.',
      curricular_component_id: 3,
      created_at: '2019-01-09T18:55:53.435Z',
      updated_at: '2019-01-09T18:55:53.435Z',
    },
  ]);

  const Badge = styled.button`
    &:last-child {
      margin-right: 0 !important;
    }

    &[aria-pressed='true'] {
      background: ${Base.CinzaBadge} !important;
      border-color: ${Base.CinzaBadge} !important;
    }
  `;

  const ListItem = styled.li`
    border-color: ${Base.AzulAnakiwa} !important;
  `;

  const ListItemButton = styled(ListItem)`
    cursor: pointer;

    &[aria-pressed='true'] {
      background: ${Base.AzulAnakiwa} !important;
    }
  `;

  const selecionaMateria = e => {
    e.target.setAttribute(
      'aria-pressed',
      e.target.getAttribute('aria-pressed') !== 'true'
    );
  };

  const onChangeTextEditor = (value, reference) => {

    //Use reference.current.props.propsPai para obter as propriedades do objeto pai, podendo passar N outras propriedades personalizadas
    //Esse metodo vai servir pra quando você quiser alterar o estado do componente para edição. Não dependa do valor daqui para atualizar o estado do editor de texto
    //Para usar o estado do componete use textEditorRef.current.state.value ele fornecerá o valor mais atual do componente textEditor, e mais confiavel do que defender do blur
  }

  const selecionaObjetivo = e => {
    e.persist();

    objetivos[
      objetivos.findIndex(objetivo => objetivo.code === e.target.innerHTML)
    ].selected = e.target.getAttribute('aria-pressed') !== 'true';

    setObjetivos([...objetivos]);
  };

  const removeObjetivoSelecionado = e => {
    e.persist();

    const indice = objetivos.findIndex(
      objetivo => objetivo.code === e.target.innerText
    );

    if (objetivos[indice]) objetivos[indice].selected = false;

    setObjetivos([...objetivos]);
  };

  const confirmarCancelamento = () => { };

  const cancelarAlteracoes = () => {
    confirmacao(
      'Atenção',
      `Você não salvou as informações
      preenchidas. Deseja realmente cancelar as alterações?`,
      confirmarCancelamento,
      () => true
    );
  };

  return (
    <>
      <Grid cols={12}>
        <h1>Plano Anual</h1>
      </Grid>
      <Grid cols={6} className="d-flex justify-content-start mb-3">
        <Button
          label="Migrar Conteúdo"
          icon="share-square"
          color={Colors.Azul}
          border
          disabled
        />
      </Grid>
      <Grid cols={6} className="d-flex justify-content-end mb-3">
        <Button
          label="Voltar"
          icon="arrow-left"
          color={Colors.Azul}
          border
          className="mr-3"
        />
        <Button
          label="Cancelar"
          color={Colors.Roxo}
          border
          bold
          className="mr-3"
          onClick={cancelarAlteracoes}
        />
        <Button label="Salvar" color={Colors.Roxo} border bold disabled />
      </Grid>
      <Grid cols={12}>
        {bimestres && bimestres.length > 0
          ? bimestres.map(bimestre => {
            const indice = shortid.generate().replace(/[0-9]/g, '');
            return (
              <CardCollapse
                key={indice}
                titulo={bimestre.nome}
                indice={indice}
                show={bimestre.nome === '3º Bimestre' && true}
              >
                <div className="row">
                  <Grid cols={6}>
                    <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
                      Objetivos de aprendizagem
                      </h6>
                    <div>
                      {bimestre.materias && bimestre.materias.length > 0
                        ? bimestre.materias.map(materia => {
                          return (
                            <Badge
                              role="button"
                              onClick={selecionaMateria}
                              aria-pressed={false}
                              key={shortid.generate()}
                              className="badge badge-pill border text-dark bg-white font-weight-light px-2 py-1 mt-3 mr-2"
                            >
                              {materia.materia}
                            </Badge>
                          );
                        })
                        : null}
                    </div>
                    <div className="mt-4">
                      {objetivos.length > 0
                        ? objetivos.map(objetivo => {
                          return (
                            <ul
                              key={shortid.generate()}
                              className="list-group list-group-horizontal mt-3"
                            >
                              <ListItemButton
                                className="list-group-item d-flex align-items-center font-weight-bold fonte-14"
                                role="button"
                                aria-pressed={objetivo.selected && true}
                                onClick={selecionaObjetivo}
                                onKeyUp={selecionaObjetivo}
                              >
                                {objetivo.code}
                              </ListItemButton>
                              <ListItem className="list-group-item flex-fill p-2 fonte-12">
                                {objetivo.description}
                              </ListItem>
                            </ul>
                          );
                        })
                        : null}
                    </div>
                  </Grid>
                  <Grid cols={6}>
                    <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
                      Objetivos de aprendizagem e meus objetivos (Currículo da
                      cidade)
                      </h6>
                    <div
                      role="group"
                      aria-label={`${objetivos.length > 0 &&
                        objetivos.filter(objetivo => objetivo.selected)
                          .length} objetivos selecionados`}
                    >
                      {objetivos.length > 0
                        ? objetivos
                          .filter(objetivo => objetivo.selected)
                          .map(selecionado => {
                            return (
                              <Button
                                key={shortid.generate()}
                                label={selecionado.code}
                                color={Colors.AzulAnakiwa}
                                bold
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
                        <li className="list-group-item border-right-0 py-1">
                          Objetivos
                          </li>
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
                        <li className="list-group-item border-left-0 py-1">
                          Avaliação
                          </li>
                      </ul>
                      <fieldset className="mt-3">
                        <form action="">
                          <TextEditor ref={textEditorRef} id="textEditor" bimestre={bimestre.nome} height="135px" maxHeight="135px" onBlur={onChangeTextEditor} value={bimestre.objetivo} />
                        </form>
                      </fieldset>
                    </div>
                  </Grid>
                </div>
              </CardCollapse>
            );
          })
          : null}
      </Grid>
    </>
  );
}
