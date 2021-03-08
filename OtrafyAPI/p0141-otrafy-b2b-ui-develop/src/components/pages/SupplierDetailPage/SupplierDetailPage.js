import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import { inject, observer } from 'mobx-react'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading'
import breadcrumbs from './breadcrumbs'
import { message, Tabs } from 'antd'
import OverviewTab from './OverviewTab/OverviewTab'
import ProductsTab from './ProductsTab'
import DetailTab from './DetailTab'

const { TabPane } = Tabs

const SupplierDetailPage = props => {

  const {
    suppliersStore, formsRequestsStore, usersStore,
    match,
  } = props

  const supplierId = match.params.supplierId

  const handleChangeTab = key => {
    suppliersStore.setActiveTab(key)
    suppliersStore.toggleEditMode(false)
  }

  useEffect(() => {
    formsRequestsStore.updateFormRequestData('selectedSupplierId', supplierId)
    return () => {
      suppliersStore.setActiveTab('1')
    }
  }, [])

  useEffect(() => {
    const role = usersStore.currentUser.role
    if (role) {
      switch (role) {
        case 'buyers':
          break
        default:
          message.error(`You dont have permission to view this page, please contact admin for more information`)
          usersStore.userLogout()
            .then(() => history.push('/'))
      }
    }
  }, [usersStore.currentUser])

  return (
    <MainLayout>
      <Helmet>
        <title>Supplier detail | Otrafy</title>
      </Helmet>
      <PageHeading
        title={'Supplier detail'}
        breadcrumbs={breadcrumbs}/>
      <Tabs
        activeKey={suppliersStore.supplierDetailActiveTab}
        animated={false}
        style={{ padding: '0 15px' }}
        onChange={handleChangeTab}>
        <TabPane tab="OVERVIEW" key="1">
          <OverviewTab/>
        </TabPane>
        <TabPane tab="SUPPLIER DETAIL" key="2">
          <DetailTab/>
        </TabPane>
        <TabPane tab="PRODUCTS" key="3">
          <ProductsTab/>
        </TabPane>
      </Tabs>
    </MainLayout>
  )
}

export default inject('suppliersStore', 'formsRequestsStore', 'usersStore')(observer(SupplierDetailPage))