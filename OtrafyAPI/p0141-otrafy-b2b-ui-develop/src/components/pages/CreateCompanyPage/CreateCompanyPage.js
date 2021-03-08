import React, { useEffect } from 'react'
import { inject, observer } from 'mobx-react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading/PageHeading'
import breadcrumbs from './breadcrumbs'
import PageFormWrapper from '../../organisms/PageFormWrapper'
import CreateCompany from './CreateCompany'
import { message } from 'antd'

const CreateCompanyPage = ({ usersStore, companiesStore, commonStore, history }) => {

  useEffect(() => {
    const role = usersStore.currentUser.role
    if (role) {
      switch (role) {
        case 'administrator':
          break
        default:
          history.push('/')
      }
    }
  }, [usersStore.currentUser])

  const onCreateCompany = values => {
    const submitValue = {
      name: values.name.trim(),
      address: values.address,
      email: values.email,
      phone: values.phone,
      website: values.website,
      isActive: values.isActive ? values.isActive : true,
      maxNumberBuyersAllowed: values.maxNumberBuyersAllowed,
      maxNumberSuppliersAllowed: values.maxNumberSuppliersAllowed,
      maxNumberFormsAllowed: values.maxNumberFormsAllowed,
    }
    companiesStore.createCompany(submitValue, history)
  }

  useEffect(() => {
    const role = usersStore.currentUser.role
    if (role) {
      switch (role) {
        case 'administrator':
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
        <title>Create new company | Otrafy</title>
      </Helmet>
      <PageHeading
        breadcrumbs={breadcrumbs}
        title={'Create new company'}/>
      <PageFormWrapper
        form={<CreateCompany onSubmit={val => onCreateCompany(val)}/>}
        theme={commonStore.appTheme}
        title={'Create new company'}/>
    </MainLayout>
  )
}

export default inject('usersStore', 'companiesStore', 'commonStore')(observer(CreateCompanyPage))