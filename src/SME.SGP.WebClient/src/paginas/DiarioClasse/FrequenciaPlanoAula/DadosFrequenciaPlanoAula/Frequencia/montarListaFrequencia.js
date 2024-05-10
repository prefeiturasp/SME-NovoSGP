import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Auditoria } from '~/componentes';
import ListaFrequencia from '~/componentes-sgp/ListaFrequencia/listaFrequencia';
import Ordenacao from '~/componentes-sgp/Ordenacao/ordenacao';
import CardCollapse from '~/componentes/cardCollapse';
import { RotasDto } from '~/dtos';
import {
  setExibirCardCollapseFrequencia,
  setListaDadosFrequencia,
  setModoEdicaoFrequencia,
} from '~/redux/modulos/frequenciaPlanoAula/actions';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const MontarListaFrequencia = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(state => state.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.FREQUENCIA_PLANO_AULA];

  const { turmaSelecionada } = usuario;

  const listaDadosFrequencia = useSelector(
    state => state.frequenciaPlanoAula.listaDadosFrequencia
  );

  const componenteCurricular = useSelector(
    state => state.frequenciaPlanoAula.componenteCurricular
  );

  const dataSelecionada = useSelector(
    state => state.frequenciaPlanoAula.dataSelecionada
  );

  const modalidadesFiltroPrincipal = useSelector(
    state => state.filtro.modalidades
  );

  const somenteConsulta = useSelector(
    state => state.frequenciaPlanoAula.somenteConsulta
  );

  const aulaId = useSelector(state => state.frequenciaPlanoAula.aulaId);

  const exibirCardCollapseFrequencia = useSelector(
    state => state.frequenciaPlanoAula.exibirCardCollapseFrequencia
  );

  const onClickFrequencia = () => {
    const permiteRegistroFrequencia = !listaDadosFrequencia.desabilitado;
    const frequenciaId = listaDadosFrequencia.id;

    const desabilitarCampos =
      frequenciaId > 0
        ? somenteConsulta || !permissoesTela.podeAlterar
        : somenteConsulta || !permissoesTela.podeIncluir;

    if (!desabilitarCampos && permiteRegistroFrequencia) {
      let temAulas = false;
      if (
        listaDadosFrequencia &&
        listaDadosFrequencia.listaFrequencia &&
        listaDadosFrequencia.listaFrequencia.length
      ) {
        const aulas = listaDadosFrequencia.listaFrequencia.filter(
          item => item.aulas && item.aulas.length
        );
        temAulas = !!(aulas && aulas.length);
      }

      if (listaDadosFrequencia.temPeriodoAberto && temAulas && !frequenciaId) {
        dispatch(setModoEdicaoFrequencia(true));
      }
    }
    dispatch(setExibirCardCollapseFrequencia(!exibirCardCollapseFrequencia));
  };

  const onChangeFrequencia = () => {
    dispatch(setModoEdicaoFrequencia(true));
  };

  const atualizarValoresAlterados = dados => {
    listaDadosFrequencia.listaFrequencia = dados;
    dispatch(setListaDadosFrequencia(listaDadosFrequencia));
  };

  return (
    <>
      {componenteCurricular &&
      componenteCurricular.codigoComponenteCurricular &&
      dataSelecionada &&
      aulaId ? (
        <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
          <CardCollapse
            key="frequencia-collapse"
            onClick={onClickFrequencia}
            titulo="Frequência"
            indice="frequencia-collapse"
            alt="card-collapse-frequencia"
            show={exibirCardCollapseFrequencia}
          >
            {listaDadosFrequencia &&
            listaDadosFrequencia.listaFrequencia &&
            listaDadosFrequencia.listaFrequencia.length ? (
              <>
                <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                  <Ordenacao
                    conteudoParaOrdenar={listaDadosFrequencia.listaFrequencia}
                    ordenarColunaNumero="numeroAlunoChamada"
                    ordenarColunaTexto="nomeAluno"
                    retornoOrdenado={retorno => {
                      const dados = { ...listaDadosFrequencia };
                      dados.listaFrequencia = retorno;
                      dispatch(setListaDadosFrequencia(dados));
                    }}
                  />
                  <ListaFrequencia
                    dataSource={listaDadosFrequencia.listaFrequencia}
                    frequenciaId={listaDadosFrequencia.id}
                    onChangeFrequencia={onChangeFrequencia}
                    permissoesTela={permissoesTela}
                    temPeriodoAberto={listaDadosFrequencia.temPeriodoAberto}
                    ehInfantil={ehTurmaInfantil(
                      modalidadesFiltroPrincipal,
                      turmaSelecionada
                    )}
                    aulaId={aulaId}
                    componenteCurricularId={
                      componenteCurricular.codigoComponenteCurricular
                    }
                    setDataSource={atualizarValoresAlterados}
                  />
                </div>
                {listaDadosFrequencia && listaDadosFrequencia.criadoEm ? (
                  <Auditoria
                    className="mt-2"
                    criadoEm={listaDadosFrequencia.criadoEm}
                    criadoPor={listaDadosFrequencia.criadoPor}
                    alteradoPor={listaDadosFrequencia.alteradoPor}
                    alteradoEm={listaDadosFrequencia.alteradoEm}
                  />
                ) : (
                  ''
                )}
              </>
            ) : null}
          </CardCollapse>
        </div>
      ) : (
        ''
      )}
    </>
  );
};

export default MontarListaFrequencia;
