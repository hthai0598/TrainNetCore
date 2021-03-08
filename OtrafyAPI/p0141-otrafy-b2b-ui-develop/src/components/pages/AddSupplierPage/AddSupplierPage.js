import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading/PageHeading'
import PageFormWrapper from '../../organisms/PageFormWrapper'
import breadcrumbs from './breadcrumbs'
import AddSupplierForm from './AddSupplierForm'
import { inject, observer } from 'mobx-react'
import { message } from 'antd'

const AddSupplierPage = ({ suppliersStore, tagsStore, usersStore, history }) => {

  useEffect(() => {
    suppliersStore.clearForm()
    tagsStore.clearTags()
    return () => {
      suppliersStore.clearForm()
      tagsStore.clearTags()
    }
  }, [])

  useEffect(() => {
    tagsStore.getAllTags('')
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
        <title>Add new supplier | Otrafy</title>
      </Helmet>
      <PageHeading
        breadcrumbs={breadcrumbs}
        title={'Add new supplier'}/>
      <PageFormWrapper
        form={<AddSupplierForm tagsList={tagsStore.tagsList}/>}
        title={'Add new supplier'}/>
    </MainLayout>
  )
}

export default inject('suppliersStore', 'tagsStore', 'usersStore')(observer(AddSupplierPage))