import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Card } from '~/componentes';
import AlertaNaoPermiteTurmaInfantil from '~/componentes-sgp/AlertaNaoPermiteTurmaInfantil/alertaNaoPermiteTurmaInfantil';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import RotasDto from '~/dtos/rotasDto';
import { limparDadosPlanoAnual } from '~/redux/modulos/anual/actions';
import { obterDescricaoNomeMenu } from '~/servicos/servico-navegacao';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import BotoesAcoesPlanoAnual from './DadosPlanoAnual/BotoesAcoes/botoesAcoesPlanoAnual';
import ComponenteCurricularPlanoAnual from './DadosPlanoAnual/ComponenteCurricular/componenteCurricularPlanoAnual';
import DadosPlanoAnual from './DadosPlanoAnual/dadosPlanoAnual';
import { ContainerPlanoAnual } from './planoAnual.css';

const PlanoAnual = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const [turmaInfantil, setTurmaInfantil] = useState(false);

  const onClickSalvar = () => {};
  const onClickCancelar = () => {};

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosPlanoAnual());
  }, [dispatch]);

  useEffect(() => {
    resetarInfomacoes();
  }, [turmaSelecionada, resetarInfomacoes]);

  useEffect(() => {
    const infantil = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setTurmaInfantil(infantil);
  }, [modalidadesFiltroPrincipal, turmaSelecionada]);

  useEffect(() => {
    if (turmaInfantil) {
      resetarInfomacoes();
    }
  }, [turmaInfantil, resetarInfomacoes]);

  return (
    <>
      {!turmaSelecionada.turma && !turmaInfantil ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'plano-anual-selecione-turma',
            mensagem: 'Você precisa escolher uma turma.',
            estiloTitulo: { fontSize: '18px' },
          }}
          className="mb-2"
        />
      ) : null}
      <AlertaNaoPermiteTurmaInfantil />
      <ContainerPlanoAnual>
        <Cabecalho
          pagina={obterDescricaoNomeMenu(
            RotasDto.PLANO_ANUAL,
            modalidadesFiltroPrincipal,
            turmaSelecionada
          )}
        />
        <Card>
          <div className="col-md-12">
            <div className="row">
              <div className="col-md-12 d-flex justify-content-end pb-4">
                <BotoesAcoesPlanoAnual
                  onClickCancelar={onClickCancelar}
                  onClickSalvar={onClickSalvar}
                  ehTurmaInfantil={turmaInfantil}
                />
              </div>
            </div>
          </div>
          <div className="col-md-12">
            <div className="row">
              <div className="col-sm-12 col-md-12 col-lg-6 col-xl-4 mb-2">
                <ComponenteCurricularPlanoAnual />
              </div>
            </div>
          </div>
          <div className="col-md-12">
            {!turmaInfantil && turmaSelecionada && turmaSelecionada.turma ? (
              <DadosPlanoAnual />
            ) : (
              ''
            )}
          </div>
        </Card>
      </ContainerPlanoAnual>
    </>
  );
};

export default PlanoAnual;
