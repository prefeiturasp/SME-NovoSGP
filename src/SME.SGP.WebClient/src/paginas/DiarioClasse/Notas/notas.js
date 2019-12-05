import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import api from '~/servicos/api';

const Notas = () => {

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;

  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);

  useEffect(() => {
    const obterDisciplinas = async () => {
      const url = `v1/calendarios/frequencias/turmas/${turmaId}/disciplinas`;
      const disciplinas = await api.get(url);

      setListaDisciplinas(disciplinas.data);
      if (disciplinas.data && disciplinas.data.length == 1) {
        const disciplina = disciplinas.data[0];
        setDisciplinaSelecionada(String(disciplina.codigoComponenteCurricular));
        setDesabilitarDisciplina(true);
      }
    };

    if (turmaId) {
      setDisciplinaSelecionada(undefined);
      obterDisciplinas();
    } else {
      // TODO - Resetar tela
      setListaDisciplinas([]);
      setDesabilitarDisciplina(false);
      setDisciplinaSelecionada(undefined);
    }
  }, [turmaSelecionada.turma]);

  const onClickVoltar = ()=> {
    console.log('onClickVoltar');
  }

  const onClickCancelar = ()=> {
    console.log('onClickCancelar');
  }

  const onClickSalvar = ()=> {
    console.log('onClickSalvar');
  }

  const onChangeDisciplinas =  disciplinaId => {
    setDisciplinaSelecionada(disciplinaId);
  };

  return (
    <>
      <Cabecalho pagina="Lançamento de notas" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
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
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
              <SelectComponent
                id="disciplina"
                name="disciplinaId"
                lista={listaDisciplinas}
                valueOption="codigoComponenteCurricular"
                valueText="nome"
                valueSelect={disciplinaSelecionada}
                onChange={onChangeDisciplinas}
                placeholder="Disciplina"
                disabled={desabilitarDisciplina}
              />
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default Notas;
