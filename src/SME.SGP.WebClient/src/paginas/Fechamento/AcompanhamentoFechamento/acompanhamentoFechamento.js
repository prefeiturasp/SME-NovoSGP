import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Divider } from 'antd';

import {
  Base,
  Button,
  Card,
  Colors,
  Label,
  Loader,
  PainelCollapse,
} from '~/componentes';
import { Cabecalho, Paginacao } from '~/componentes-sgp';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';

import {
  setCarregandoAcompanhamentoFechamento,
  setTurmasAcompanhamentoFechamento,
} from '~/redux/modulos/acompanhamentoFechamento/actions';

import {
  ehTurmaInfantil,
  erros,
  history,
  ServicoAcompanhamentoFechamento,
} from '~/servicos';

import { CardStatus } from './CardStatus';
import { Filtros } from './Filtros';

const AcompanhamentoFechamento = () => {
  const [ehInfantil, setEhInfantil] = useState(false);
  const [parametrosFiltro, setParametrosFiltro] = useState();
  const [dadosStatusFechamento, setDadosStatusFechamento] = useState([]);
  const [dadosStatusConsselhoClasse, setDadosStatusConselhoClasse] = useState(
    []
  );
  const [numeroPagina, setNumeroPagina] = useState(1);

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const permissoesTela = { podeIncluir: false, podeAlterar: false };

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const carregandoAcompanhamentoFechamento = useSelector(
    state => state.acompanhamentoFechamento.carregandoAcompanhamentoFechamento
  );

  const turmasAcompanhamentoFechamento = useSelector(
    state => state.acompanhamentoFechamento.turmasAcompanhamentoFechamento
  );

  const numeroRegistros = turmasAcompanhamentoFechamento?.totalRegistros;
  const pageSize = 10;
  const exibiPaginacao = numeroRegistros > pageSize;

  const dispatch = useDispatch();

  const aoClicarBotaoVoltar = () => {
    history.push('/');
  };

  const onChangeFiltros = async (params, paginaAlterada = numeroPagina) => {
    dispatch(setCarregandoAcompanhamentoFechamento(true));
    const retorno = await ServicoAcompanhamentoFechamento.obterTurmas({
      ...params,
      numeroPagina: paginaAlterada,
    })
      .catch(e => erros(e))
      .finally(() => dispatch(setCarregandoAcompanhamentoFechamento(false)));
    if (retorno?.data?.totalRegistros) {
      dispatch(setTurmasAcompanhamentoFechamento(retorno.data));
    }
    if (params) {
      setParametrosFiltro(params);
    }
  };

  useEffect(() => {
    const infantil = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setEhInfantil(infantil);
  }, [turmaSelecionada, permissoesTela, modalidadesFiltroPrincipal]);

  const obterFechamentos = async turmaId => {
    const retorno = await ServicoAcompanhamentoFechamento.obterFechamentos({
      turmaId,
      bimestre: parametrosFiltro.bimestre,
    });

    if (retorno?.data) {
      setDadosStatusFechamento(retorno.data);
    }
  };

  const obterConselhoClasse = async turmaId => {
    const retorno = await ServicoAcompanhamentoFechamento.obterConselhoClasse({
      turmaId,
      bimestre: parametrosFiltro.bimestre,
    });

    if (retorno?.data) {
      setDadosStatusConselhoClasse(retorno.data);
    }
  };

  const onChangeCollapse = turmaId => {
    if (turmaId) {
      dispatch(setCarregandoAcompanhamentoFechamento(true));
      Promise.all([obterFechamentos(turmaId), obterConselhoClasse(turmaId)])
        .catch(e => erros(e))
        .finally(() => dispatch(setCarregandoAcompanhamentoFechamento(false)));
    }
  };

  const onChangePaginacao = pagina => {
    setNumeroPagina(pagina);
    onChangeFiltros(parametrosFiltro, pagina);
  };

  return (
    <Loader loading={carregandoAcompanhamentoFechamento} ignorarTip>
      <AlertaModalidadeInfantil />
      <Cabecalho pagina="Acompanhamento do fechamento" classes="mb-2" />
      <Card>
        <div className="col-md-12 p-0">
          <div className="row mb-2">
            <div className="col-sm-12 d-flex justify-content-end">
              <Button
                id="botao-voltar"
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                onClick={aoClicarBotaoVoltar}
                border
              />
            </div>
          </div>
          <div className="mb-4">
            <Filtros
              onChangeFiltros={onChangeFiltros}
              ehInfantil={ehInfantil}
            />
          </div>
          {!!turmasAcompanhamentoFechamento?.items?.length && (
            <>
              <div className="mb-3 pt-3">
                <Label
                  text="Dados por turma"
                  tamanhoFonte="18"
                  altura="24"
                  className="mb-3"
                />
                <PainelCollapse accordion onChange={onChangeCollapse}>
                  {turmasAcompanhamentoFechamento.items.map(dadosTurmas => (
                    <PainelCollapse.Painel
                      key={dadosTurmas?.turmaId}
                      accordion
                      espacoPadrao
                      corBorda={Base.AzulBordaCollapse}
                      temBorda
                      header={dadosTurmas?.nome}
                    >
                      <>
                        <Label text="Fechamento" className="mb-3" altura="24" />
                        <div className="d-flex justify-content-between">
                          {dadosStatusFechamento?.map(dadosFechamento => (
                            <CardStatus dadosStatus={dadosFechamento} />
                          ))}
                        </div>
                        <Divider style={{ background: Base.CinzaDivisor }} />
                        <Label
                          text="Conselho de classe"
                          className="mb-3"
                          altura="24"
                        />
                        <div className="d-flex justify-content-between">
                          {dadosStatusConsselhoClasse?.map(
                            dadosConselhoClasse => (
                              <CardStatus dadosStatus={dadosConselhoClasse} />
                            )
                          )}
                        </div>
                      </>
                    </PainelCollapse.Painel>
                  ))}
                </PainelCollapse>
              </div>
              {exibiPaginacao && (
                <div className="col-12 d-flex justify-content-center mt-2">
                  <Paginacao
                    numeroRegistros={numeroRegistros}
                    pageSize={pageSize}
                    onChangePaginacao={onChangePaginacao}
                  />
                </div>
              )}
            </>
          )}
        </div>
      </Card>
    </Loader>
  );
};

export default AcompanhamentoFechamento;
