import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading'
import BreadCrumbs from './BreadCrumbs'
import PageFormWrapper from '../../organisms/PageFormWrapper'
import AddProductForm from './AddProductForm'
import { inject, observer } from 'mobx-react'

const AddProductPage = ({ tagsStore }) => {

  useEffect(() => {
    tagsStore.getAllTags('')
  }, [])

  return (
    <MainLayout>
      <Helmet>
        <title>Add new product | Otrafy</title>
      </Helmet>
      <PageHeading
        breadcrumbs={<BreadCrumbs/>}
        title={'Add new product'}/>
      <PageFormWrapper
        form={<AddProductForm tagsList={tagsStore.tagsList}/>}
        title={'Add new product'}/>
    </MainLayout>
  )
}

export default inject('tagsStore')(observer(AddProductPage))