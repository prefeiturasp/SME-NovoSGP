import React from 'react';
import { useSelector } from 'react-redux';
import CardCollapse from '~/componentes/cardCollapse';
import ServicoPlanoAula from '~/servicos/Paginas/DiarioClasse/ServicoPlanoAula';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import DadosPlanoAula from './DadosPlanoAula/dadosPlanoAula';

const PlanoAula = () => {
  const componenteCurricular = useSelector(
    state => state.frequenciaPlanoAula.componenteCurricular
  );

  const dataSelecionada = useSelector(
    state => state.frequenciaPlanoAula.dataSelecionada
  );

  const exibirCardCollapsePlanoAula = useSelector(
    state => state.frequenciaPlanoAula.exibirCardCollapsePlanoAula
  );

  const aulaId = useSelector(state => state.frequenciaPlanoAula.aulaId);

  const usuario = useSelector(state => state.usuario);

  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    state => state.filtro.modalidades
  );

  const onClickPlanoAula = () => {
    ServicoPlanoAula.obterPlanoAula();
  };

  return (
    <>
      {componenteCurricular &&
      componenteCurricular.codigoComponenteCurricular &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) &&
      dataSelecionada &&
      aulaId ? (
        <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
          <CardCollapse
            key="plano-aula-collapse"
            onClick={onClickPlanoAula}
            titulo="Plano de aula"
            indice="plano-aula-collapse"
            show={exibirCardCollapsePlanoAula.exibir}
          >
            <DadosPlanoAula />
          </CardCollapse>
        </div>
      ) : (
        ''
      )}
    </>
  );
};

export default PlanoAula;
