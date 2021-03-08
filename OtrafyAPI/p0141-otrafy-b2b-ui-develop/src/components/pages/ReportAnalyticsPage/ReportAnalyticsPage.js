import React from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading'

const ReportAnalyticsPage = ({ history }) => {

  return (
    <MainLayout>
      <Helmet>
        <title>Report & Analytics | Otrafy</title>
      </Helmet>
      <PageHeading title={'Reports & Analytics'}>
        Tools here
      </PageHeading>
    </MainLayout>
  )
}

export default ReportAnalyticsPage