import React, { useState, useEffect } from 'react';
import { Corpo, Titulo, TituloAno } from './aulaDadaAulaPrevista.css';
import Grid from '~/componentes/grid';
import Card from '~/componentes/card';
import Button from '~/componentes/button';
import SelectComponent from '~/componentes/select';
import { useSelector } from 'react-redux';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import { Colors } from '~/componentes';

const AulaDadaAulaPrevista = () => {
  const usuario = useSelector(store => store.usuario);
  const turmaSelecionada = usuario.turmaSelecionada;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;
  const anoLetivo = turmaSelecionada ? turmaSelecionada.anoLetivo : 0;
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [disciplinaIdSelecionada, setDisciplinaIdSelecionada] = useState(
    undefined
  );

  useEffect(() => {
    const obterDisciplinas = async () => {
      const disciplinas = await ServicoDisciplina.obterDisciplinasPorTurma(
        turmaId
      );
      setListaDisciplinas(disciplinas.data);
      if (disciplinas.data && disciplinas.data.length == 1) {
        const disciplina = disciplinas.data[0];
        setDisciplinaSelecionada(disciplina);
        setDisciplinaIdSelecionada(
          String(disciplina.codigoComponenteCurricular)
        );
        setDesabilitarDisciplina(true);
      }
    };
    if (turmaId) {
      obterDisciplinas(turmaId);
    }
  }, [turmaSelecionada.turma]);

  const onChangeDisciplinas = disciplinaId => {
  }

  const onClickVoltar = () => {

  }

  const onClickCancelar = () => {

  }

  const onClickSalvar = () => {

  }

  return (
    <>
      <Grid cols={12} className="p-0">
        <Titulo>
          Aula prevista X Aula dada
          <TituloAno>
            {' '}
            {` / ${anoLetivo ? anoLetivo : new Date().getFullYear()}`}{' '}
          </TituloAno>{' '}
        </Titulo>{' '}
      </Grid>{' '}
      <Card>

        <div className="col-md-12">
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
              <SelectComponent
                id="disciplina"
                name="disciplinaId"
                lista={listaDisciplinas}
                valueOption="codigoComponenteCurricular"
                valueText="nome"
                valueSelect={disciplinaIdSelecionada}
                onChange={onChangeDisciplinas}
                placeholder="Disciplina"
                disabled={desabilitarDisciplina}
              />
            </div>
            <div className="col-sm-12 col-lg-8 col-md-8 d-flex justify-content-end pb-4">
              <Button
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickVoltar}
              />
              <Button
                label="Cancelar"
                color={Colors.Roxo}
                border
                className="mr-2"
                onClick={onClickCancelar}
              />
              <Button
                label="Salvar"
                color={Colors.Roxo}
                border
                bold
                className="mr-2"
                onClick={onClickSalvar}
              />
            </div>
          </div>
        </div>
      </Card>
    </>
  );
}

export default AulaDadaAulaPrevista;
