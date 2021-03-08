import React from 'react'
import Sidebar from './Sidebar'
import { inject, observer } from 'mobx-react'
import { Link } from 'react-router-dom'
import {
  NormalLayoutWrapper, SidebarWrapper, MainContentWrapper,
} from './CustomLayoutStyled'
import LoadingSpinner from '../elements/LoadingSpinner'
import { Drawer } from 'antd'
import MediaQuery from 'react-responsive'
import TabletHeaderBar from './TabletHeaderBar'
import NavigationMenu from './NavigationMenu'
// Icons
import logo from '../../assets/icons/otrafy-logo-black@2x.png'

const MainLayout = props => {

  const {
    children,
    commonStore, buyersStore, companiesStore, formsRequestsStore,
    formsStore, productsStore, suppliersStore, tagsStore, usersStore,
  } = props

  const checkAppLoading = () => {
    return !!(
      commonStore.isLoading ||
      buyersStore.isLoading ||
      companiesStore.isLoading ||
      formsRequestsStore.isLoading ||
      formsStore.isLoading ||
      productsStore.isLoading ||
      suppliersStore.isLoading ||
      tagsStore.isLoading ||
      usersStore.isLoading
    )
  }

  return (
    <NormalLayoutWrapper>
      <MediaQuery minWidth={1025}>
        <SidebarWrapper style={{
          width: commonStore.isSidebarCollapse ? 70 : 232,
        }}>
          <Sidebar/>
        </SidebarWrapper>
      </MediaQuery>
      <MediaQuery maxWidth={1024}>
        <TabletHeaderBar/>
        <Drawer
          closable width={250}
          onClose={() => commonStore.toggleDrawer(false)}
          placement={'left'}
          visible={commonStore.isDrawerVisible}>
          <Link to={'/'} style={{ padding: '25px 22px', display: 'block' }}>
            <img src={logo} alt="Otrafy" width={94}/>
          </Link>
          <NavigationMenu role={usersStore.currentUser.role}/>
        </Drawer>
      </MediaQuery>
      <MainContentWrapper style={{
        width: commonStore.isSidebarCollapse ? 'calc(100% - 70px)' : 'calc(100% - 232px)',
      }}>
        {children}
      </MainContentWrapper>
      {
        checkAppLoading()
          ? <LoadingSpinner theme={commonStore.appTheme} type={'page'}/>
          : null
      }
    </NormalLayoutWrapper>
  )
}

export default inject(
  'commonStore', 'buyersStore', 'companiesStore', 'formsRequestsStore',
  'formsStore', 'productsStore', 'suppliersStore', 'tagsStore', 'usersStore',
)(observer(MainLayout))