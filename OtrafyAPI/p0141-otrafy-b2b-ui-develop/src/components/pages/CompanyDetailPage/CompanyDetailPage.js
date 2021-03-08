import React, { useState, useEffect, Fragment } from 'react'
import { Helmet } from 'react-helmet'
import { Link } from 'react-router-dom'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading/PageHeading'
import breadcrumbs from './breadcrumbs'
import { Icon, Input, Table, Menu, Button, message } from 'antd'
import CompanyDetailCard from './CompanyDetailCard'
import { inject, observer } from 'mobx-react'
import CompanyDetailCardEdit from './CompanyDetailCardEdit'
import {
  MainContent,
  InfoCardWrapper,
  TableWrapper,
  TableHeading,
} from './CustomStyled'
import ActiveStatusIndicator from '../../elements/ActiveStatusIndicator'
import TableFooter from '../../organisms/TableFooter'
import CustomPagination from '../../organisms/CustomPagination'
import NormalTag from '../../elements/NormalTag'
import CustomDropdown from '../../organisms/CustomDropdown'

const { Search } = Input

const CompanyDetailPage = ({ companiesStore, usersStore, buyersStore, history, match }) => {

  const [pageSize, setPageSize] = useState(10)
  const [currentPage, setCurrentPage] = useState(1)
  const [filterValue, setFilterValue] = useState('')

  const companyId = match.params.companyId

  const handleChangePageSize = ({ key }) => {
    setCurrentPage(1)
    setPageSize(parseInt(key))
  }

  const updateCompanyInfo = (companyId, values) => {
    companiesStore.updateCompanyInfo(companyId, values)
  }

  const toggleEditMode = () => {
    companiesStore.setEditMode(true)
  }

  const resendInvitation = buyerId => {
    buyersStore.resendInvitation(buyerId)
  }

  const editInvitationDetail = buyerId => {
    history.push(`/company-management/buyer-detail/${buyerId}`)
  }

  const handleChangePage = page => {
    let params
    filterValue
      ? params = `?pageSize=${pageSize}&pageNumber=${page}&CompanyId=${companyId}&FilterBy=Name&&FilterBy=username&FilterValue=${encodeURIComponent(filterValue)}`
      : params = `?pageSize=${pageSize}&pageNumber=${page}&CompanyId=${companyId}`
    buyersStore.getCurrentBuyerList(params)
    setCurrentPage(page)
  }

  const handleSearch = value => {
    setFilterValue(value)
    setCurrentPage(1)
  }

  const dropdownMenu = (
    <Menu onClick={handleChangePageSize}>
      <Menu.Item key="10">
        Showing <strong>10</strong> of <strong>{buyersStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="20">
        Showing <strong>20</strong> of <strong>{buyersStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="50">
        Showing <strong>50</strong> of <strong>{buyersStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
    </Menu>
  )

  const columns = [
    {
      title: `Name`,
      key: 'name',
      render: record => record.name,
    },
    {
      title: `Username`,
      key: 'username',
      render: record => record.username,
    },
    {
      title: `Email address`,
      key: 'email',
      render: record => record.email,
    },
    {
      title: `Permission`,
      key: 'permission',
      render: record =>
        <NormalTag tags={record.permission.map(p => {
          switch (p) {
            case 0:
              return 'Run report'
            case 1:
              return 'View all suppliers'
            case 2:
              return 'Create form template'
            case 3:
              return 'Create new supplier'
          }
        })}/>,
    },
    {
      title: `Job title`,
      key: 'jobTitle',
      render: record => record.jobTitle,
    },
    {
      title: `Status`,
      key: 'status',
      render: record => {
        return (
          <div className={'action-row'}>
            <div className={'show'}>
              <ActiveStatusIndicator status={record.isActive} type={'inline'}/>
            </div>
            <div className={'hidden'}>
              {
                record.isActive
                  ? <Button type={'primary'} onClick={() => editInvitationDetail(record.id)}
                            style={{ marginRight: 10 }}>Detail</Button>
                  : <Fragment>
                    <Button type={'primary'} onClick={() => editInvitationDetail(record.id)}
                            style={{ marginRight: 10 }}>Detail</Button>
                    <Button onClick={() => resendInvitation(record.id)}>Resend</Button>
                  </Fragment>
              }
            </div>
          </div>
        )
      },
    },
  ]

  useEffect(() => {
    companiesStore.getCurrentCompanyView(companyId, history)
  }, [])

  useEffect(() => {
    companiesStore.setEditMode(false)
  }, [])

  useEffect(() => {
    if (filterValue) {
      let params = `?pageSize=${pageSize}&pageNumber=1&CompanyId=${companyId}&FilterBy=Name&&FilterBy=username&FilterValue=${encodeURIComponent(filterValue)}&FilterValue=${encodeURIComponent(filterValue)}`
      buyersStore.getCurrentBuyerList(params)
    } else {
      buyersStore.getCurrentBuyerList(`?pageSize=${pageSize}&pageNumber=1&CompanyId=${companyId}`)
    }
  }, [filterValue, pageSize])

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
        <title>Company Detail | Otrafy</title>
      </Helmet>
      <PageHeading
        breadcrumbs={breadcrumbs}
        title={'Company Detail'}/>
      <MainContent>
        <InfoCardWrapper>
          {
            companiesStore.editMode
              ? <CompanyDetailCardEdit onSubmit={(companyId, values) => updateCompanyInfo(companyId, values)}
                                       onCancel={() => companiesStore.setEditMode(false)}
                                       companyData={companiesStore.currentCompanyView}/>
              : <CompanyDetailCard onToggleEditMode={toggleEditMode}/>
          }
        </InfoCardWrapper>
        <TableWrapper>
          <TableHeading>
            <div className={'info'}>
              <span>Buyers created: {companiesStore.currentCompanyView.totalBuyersCreated}</span>
              <span>Maximum: {companiesStore.currentCompanyView.maxNumberBuyersAllowed}</span>
            </div>
            <div className={'action'}>
              <Search
                size={'large'}
                placeholder="Search by name or username"
                onSearch={value => handleSearch(value)}/>
              <Link to={`/company-management/${companiesStore.currentCompanyView.companyId}/create-buyer`}>
                <Button type={'primary'} block={false} style={{ height: 40 }}>
                  <Icon type="plus"/>
                  Invite new buyer
                </Button>
              </Link>
            </div>
          </TableHeading>
          <Table
            rowKey={record => record.id}
            className={'buyer-list-table'}
            pagination={false} columns={columns}
            dataSource={buyersStore.buyerList}/>
          <TableFooter>
            <CustomDropdown
              dropdownMenu={dropdownMenu}
              pageSize={pageSize}
              total={buyersStore.paging.totalRecordCount}/>
            <CustomPagination
              total={buyersStore.paging.totalRecordCount}
              pageCount={buyersStore.paging.pageCount}
              current={currentPage}
              pageSize={pageSize} hideOnSinglePage={true}
              onChange={page => handleChangePage(page)}/>
          </TableFooter>
        </TableWrapper>
      </MainContent>
    </MainLayout>
  )
}


export default inject('companiesStore', 'usersStore', 'buyersStore')(observer(CompanyDetailPage))