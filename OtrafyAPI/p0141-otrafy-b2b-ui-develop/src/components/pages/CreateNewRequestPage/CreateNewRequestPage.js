import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading'
import breadcrumbs from './breadcrumbs'
import PageFormWrapper from '../../organisms/PageFormWrapper'
import CreateNewRequestForm from './CreateNewRequestForm'
import { inject, observer } from 'mobx-react'
import { message } from 'antd'

const CreateNewRequestPage = props => {

  const {
    productsStore, formsStore, suppliersStore, formsRequestsStore, usersStore,
    history,
  } = props

  useEffect(() => {
    suppliersStore.getAllSuppliers('')
    formsStore.getFormsList('')
  }, [])

  useEffect(() => {
    if (formsRequestsStore.formRequestData.selectedSupplierId) {
      productsStore.getAllProducts(`?supplierId=${formsRequestsStore.formRequestData.selectedSupplierId}`)
    }
  }, [formsRequestsStore.formRequestData.selectedSupplierId])

  useEffect(() => {
    return () => {
      formsRequestsStore.clearFormRequestData()
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
        <title>Create new request | Otrafy</title>
      </Helmet>
      <PageHeading title={'Create new request'} breadcrumbs={breadcrumbs}/>
      <PageFormWrapper
        form={<CreateNewRequestForm
          suppliersList={suppliersStore.suppliersList}
          productsList={productsStore.productsList}
          formsList={formsStore.formsList}/>}
        title={'Create new request'}/>
    </MainLayout>
  )
}

export default inject('productsStore', 'formsStore', 'suppliersStore', 'formsRequestsStore', 'usersStore')(observer(CreateNewRequestPage))