import React, { useEffect } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading'
import breadcrumbs from './breadcrumbs'
import { inject, observer } from 'mobx-react'
import FormCreator from '../../organisms/FormCreator'
import FormButtonGroup from '../../organisms/FormButtonGroup'
import { Button } from 'antd'

const ViewFormPage = props => {

  const {
    history, match,
    formsStore, buyersStore, commonStore,
  } = props

  const formId = match.params.formId

  const handleSaveForm = () => {
    buyersStore.updateFormData(formId, history)
  }

  useEffect(() => {
    formsStore.getFormDetail(formId)
    commonStore.toggleCollapseSidebar(true)
    return () => {
      buyersStore.clearForm()
      formsStore.clearAllFormsData()
    }
  }, [])

  return (
    <MainLayout>
      <Helmet>
        <title>View form detail | Otrafy</title>
      </Helmet>
      <PageHeading
        breadcrumbs={breadcrumbs}
        title={'View form detail'}>
        <FormButtonGroup>
          <Button type={'danger'} ghost onClick={() => history.push(`/all-forms`)}>
            Cancel
          </Button>
          <Button type={'primary'} onClick={handleSaveForm}>
            Save form
          </Button>
        </FormButtonGroup>
      </PageHeading>
      <FormCreator/>
    </MainLayout>
  )
}

export default inject('formsStore', 'buyersStore', 'commonStore')(observer(ViewFormPage))