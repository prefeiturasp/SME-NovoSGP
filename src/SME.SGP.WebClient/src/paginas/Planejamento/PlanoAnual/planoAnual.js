import React, { useEffect } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Salvar } from '../../../redux/modulos/planoAnual/action';
import Grid from '../../../componentes/grid';
import Button from '../../../componentes/button';
import { Colors, Base } from '../../../componentes/colors';
import _ from "lodash";
import Card from '../../../componentes/card';
import Bimestre from './bimestre';
import { confirmacao } from '../../../servicos/alertas';
import Service from '../../../servicos/Paginas/PlanoAnualServices';

export default function PlanoAnual() {

  const qtdBimestres = 4;

  const RF = 6082840;

  const Turma = 1982186;

  const bimestres = useSelector(store => store.bimestres.bimestres);
  const dispatch = useDispatch();

  useEffect(() => {

    Service.getMateriasProfessor(RF, Turma)
      .then((res) => {
        ObtenhaBimestres(_.cloneDeep(res));
      });

  }, [])

  const ehEja = false;

  const ObtenhaNomebimestre = (index) => `${index}º ${ehEja ? "Semestre" : "Bimestre"}`

  const confirmarCancelamento = () => { };

  const ObtenhaBimestres = (materias) => {

    for (let i = 1; i <= qtdBimestres; i++) {

      const Nome = ObtenhaNomebimestre(i);

      const objetivo = 'In semper mi vitae nulla bibendum, ut dictum magna dictum. Morbi sodales rutrum turpis, sit amet fringilla orci rutrum sit amet. Nulla tristique dictum neque, ac placerat urna aliquam non. Sed commodo tellus ac hendrerit mollis. Mauris et congue nulla.';

      const bimestre = {
        indice: i,
        nome: Nome,
        materias: _.cloneDeep(materias),
        objetivo: objetivo
      };

      dispatch(Salvar(i, bimestre));
    }
  }

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
    <Card>
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
        {
          bimestres ? bimestres.map(bim => {
            return (<Bimestre key={bim.indice} bimestreDOM={bim} />);
          }) : null
        }
      </Grid>
    </Card>
  );
}
