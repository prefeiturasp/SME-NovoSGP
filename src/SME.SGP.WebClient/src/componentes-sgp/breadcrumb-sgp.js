import React, { useEffect, useState } from 'react';
import { Breadcrumb } from 'antd';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { Base } from '../componentes/colors';
import styled from 'styled-components';

const BreadcrumbSgp = () => {
    const BreadcrumbBody = styled.div`
        margin: 0 0 5px 15px;

        i{
            margin-right: 10px;
            margin-left: 10px;
        }
    `;

    const NavegacaoStore = useSelector(store => store.navegacao);

    const routes = new Map();
    routes.set('/', { breadcrumbName: 'Home', parentName: null });
    routes.set('/planejamento/plano-ciclo', { breadcrumbName: 'Plano de Ciclo', parentName: 'Planejamento' });
    routes.set('/planejamento/plano-anual', { breadcrumbName: 'Plano Anual', parentName: 'Planejamento' });

    const [itens, setItens] = useState([]);
    const [paginaAtual, setPaginaAtual] = useState('');

    useEffect(() => { carregaBreadcrumbs(NavegacaoStore.activeRoute); }, [NavegacaoStore.activeRoute]);

    const carregaBreadcrumbs = (route) => {
        console.log('mudou')
        const item = routes.get(route);
        if (item) {
            setPaginaAtual(item.breadcrumbName);
            const newItens = itens;
            newItens.push({ breadcrumbName: item.breadcrumbName, parentName: item.parentName, path: route })
            setItens(newItens);
        }
    }

    return (
        <BreadcrumbBody>
            {itens.map(item => {
                return (
                    <Breadcrumb.Item key={item.path} separator="">
                        <Link to={item.path}>{item.breadcrumbName}</Link>
                        <i style={{ color: Base.Roxo }} className='fas fa-chevron-circle-right' />
                    </Breadcrumb.Item>
                );
            })}
            {/* <Breadcrumb.Item separator="">
                <span>{paginaAtual}</span>
            </Breadcrumb.Item> */}
        </BreadcrumbBody>
    );
}

export default BreadcrumbSgp;