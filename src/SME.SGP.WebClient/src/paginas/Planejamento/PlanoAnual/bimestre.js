import React, { useState, useEffect } from 'react';
import shortid from 'shortid';
import styled from 'styled-components';
import CardCollapse from '../../../componentes/cardCollapse';
import Grid from '../../../componentes/grid';
import Button from '../../../componentes/button';
import TextEditor from '../../../componentes/textEditor';
import { Colors, Base } from '../../../componentes/colors';
import Seta from '../../../recursos/Seta.svg';

// import { Container } from './styles';

export default function Bimestre(props) {

    const { bimestreDOM } = props;

    const [objetivos, setObjetivos] = useState(null);

    const [bimestre, setBimestre] = useState(bimestreDOM);

    const [materias, setMaterias] = useState(bimestre.materias);

    useEffect(() => {

        if (materias.length > 0)
            console.log(materias.filter(materia => materia.selected));
        else
            setObjetivos([]);

    }, [materias])

    useEffect(() => {

        setMaterias([...bimestre.materias]);

    }, [bimestre])

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
        e.persist();

        bimestre.materias[bimestre.materias.findIndex(materia => materia.codigo == e.target.id)].selected = e.target.getAttribute('aria-pressed') !== 'true';

        setBimestre({ ...bimestre });
    };

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

    const toolbarOptions = [
        ['bold', 'italic', 'underline'],
        [{ list: 'bullet' }, { list: 'ordered' }],
    ];

    const modules = {
        toolbar: toolbarOptions,
    };


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
                                        aria-pressed={materia.selected && true}
                                        id={materia.codigo}
                                        key={materia.codigo}
                                        className="badge badge-pill border text-dark bg-white font-weight-light px-2 py-1 mt-3 mr-2"
                                    >
                                        {materia.materia}
                                    </Badge>
                                );
                            })
                            : null}
                    </div>
                    <div className="mt-4">
                        {objetivos && objetivos.length > 0
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
                        aria-label={`${objetivos && objetivos.length > 0 &&
                            objetivos.filter(objetivo => objetivo.selected)
                                .length} objetivos selecionados`}
                    >
                        {objetivos && objetivos.length > 0
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
                                <TextEditor
                                    className="form-control"
                                    modules={modules}
                                    height={135}
                                    value={bimestre.objetivo}
                                />
                            </form>
                        </fieldset>
                    </div>
                </Grid>
            </div>
        </CardCollapse>
    );
}
