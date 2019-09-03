import React, { useState, useEffect } from 'react';
import { Badge, ObjetivosList, ListItemButton, ListItem } from './bimestre.css';
import CardCollapse from '../../../componentes/cardCollapse';
import Grid from '../../../componentes/grid';
import Button from '../../../componentes/button';
import TextEditor from '../../../componentes/textEditor';
import { Colors } from '../../../componentes/colors';
import Seta from '../../../recursos/Seta.svg';
import Servico from '../../../servicos/Paginas/PlanoAnualServices';

// import { Container } from './styles';

const BimestreComponent = (props) => {

    const Ano = 1;

    const { bimestreDOM } = props;

    const [ehExpandido, setEhExpandido] = useState(false);

    const [objetivos, setObjetivos] = useState({});

    const [bimestre, setBimestre] = useState({});

    const [materias, setMaterias] = useState([]);


    useEffect(() => {

        setBimestre({ ...bimestreDOM });
        setMaterias([...bimestreDOM.materias])

    }, [])

    useEffect(() => {

        getObjetivos();

    }, [materias])

    const getObjetivos = () => {

        if (!materias)
            return;

        const materiasSelecionadas = materias.filter(materia => materia.selected).map(x => x.codigo);

        if (materiasSelecionadas.length > 0) {

            Servico.getObjetivoseByDisciplinas(Ano, materiasSelecionadas)
                .then(res => {

                    const concatenados = objetivos.concat(res.filter((item) => {

                        const index = objetivos.findIndex(x => x.codigo === item.codigo);

                        return index < 0;
                    }));

                    setObjetivos([...concatenados]);
                    setEhExpandido(true);

                });
        }
        else {
            setObjetivos([]);
        }
    }

    const selecionaMateria = e => {

        materias[materias.findIndex(materia => materia.codigo == e.target.id)].selected = e.target.getAttribute('aria-pressed') !== 'true';

        setMaterias([...materias]);
    };

    const selecionaObjetivo = e => {

        objetivos[
            objetivos.findIndex(objetivo => objetivo.codigo === e.target.id)
        ].selected = e.target.getAttribute('aria-pressed') !== 'true';

        setObjetivos([...objetivos]);
    };

    const removeObjetivoSelecionado = e => {

        const indice = objetivos.findIndex(
            objetivo => objetivo.id == e.target.id
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

    return (

        <CardCollapse
            key={bimestre.indice}
            titulo={bimestre.nome}
            indice={`Bimestre${bimestre.indice}`}
            show={ehExpandido}
        >
            <div className="row">
                <Grid cols={6}>
                    <h6 className="d-inline-block font-weight-bold my-0 fonte-14">
                        Objetivos de aprendizagem
                    </h6>
                    <div>
                        {materias && materias.length > 0
                            ? materias.map(materia => {
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
                    <ObjetivosList className="mt-4 overflow-auto">
                        {objetivos && objetivos.length > 0
                            ? objetivos.map(objetivo => {
                                return (
                                    <ul
                                        key={objetivo.id}
                                        className="list-group list-group-horizontal mt-3"
                                    >
                                        <ListItemButton
                                            className="list-group-item d-flex align-items-center font-weight-bold fonte-14"
                                            role="button"
                                            id={objetivo.id}
                                            aria-pressed={objetivo.selected && true}
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


export default BimestreComponent;