import React, { useEffect, useState } from 'react'
import { Helmet } from 'react-helmet'
import { Link } from 'react-router-dom'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading/PageHeading'
import { Table, Icon, Menu, message } from 'antd'
import { inject, observer } from 'mobx-react'
import { Button } from 'antd'
import defaultCompanyAvatar from '../../../assets/dummy/user-avatar@2x.png'
import { apiUrl } from '../../../config'
import {
  Toolsbox,
  StyledSearch,
  TableWrapper,
  CompanyAvatar,
} from './CustomStyled'
import TableFooter from '../../organisms/TableFooter'
import CustomPagination from '../../organisms/CustomPagination'
import CustomDropdown from '../../organisms/CustomDropdown'

const CompanyManagementPage = ({ companiesStore, usersStore, history }) => {

  const [pageSize, setPageSize] = useState(10)
  const [currentPage, setCurrentPage] = useState(1)
  const [filterValue, setFilterValue] = useState('')

  const handleChangePageSize = ({ key }) => {
    setCurrentPage(1)
    setPageSize(parseInt(key))
  }

  const handleChangePage = page => {
    let queryParams
    filterValue
      ? queryParams = `?pageSize=${pageSize}&pageNumber=${page}&FilterBy=Name&FilterValue=${encodeURIComponent(filterValue)}`
      : queryParams = `?pageSize=${pageSize}&pageNumber=${page}`
    companiesStore.getCompanyList(queryParams)
    setCurrentPage(page)
  }

  const handleSearch = value => {
    setFilterValue(value)
    setCurrentPage(1)
  }

  const addCompanyHandler = () => {
    history.push('/company-management/create-new')
  }

  const dropdownMenu = (
    <Menu onClick={handleChangePageSize}>
      <Menu.Item key="10">
        Showing <strong>10</strong> of <strong>{companiesStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="20">
        Showing <strong>20</strong> of <strong>{companiesStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="50">
        Showing <strong>50</strong> of <strong>{companiesStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
    </Menu>
  )

  const tableConfig = [
    {
      title: () => {
        return (
          <React.Fragment>
            <span>Company name</span><br/>
            <small>{companiesStore.paging.totalRecordCount} companies</small>
          </React.Fragment>
        )
      },
      key: 'name',
      render: record => {
        return (
          <Link to={`/company-management/${record.companyId}`} style={{
            display: 'flex',
            alignItems: 'center',
          }}>
            <CompanyAvatar active={record.isActive}>
              <img src={defaultCompanyAvatar} alt={record.name}/>
            </CompanyAvatar>
            <span>{record.name}</span>
          </Link>
        )
      },
    },
    {
      title: () => {
        return (
          <React.Fragment>
            <span>Buyers</span><br/>
            <small>(created / maximum)</small>
          </React.Fragment>
        )
      },
      key: 'buyers',
      render: record =>
        <span style={{ color: '#2196F3', backgroundColor: '#e9f5fe', padding: '4px 15px', borderRadius: 25 }}>
          {record.totalBuyersCreated}/{record.maxNumberBuyersAllowed}
        </span>,
    },
    {
      title: () => {
        return (
          <React.Fragment>
            <span>Suppliers</span><br/>
            <small>(invited / maximum)</small>
          </React.Fragment>
        )
      },
      key: 'suppliers',
      render: record =>
        <span style={{ color: '#FF9800', backgroundColor: '#fff5e6', padding: '4px 15px', borderRadius: 25 }}>
          {record.totalSuppliersInvited}/{record.maxNumberSuppliersAllowed}
        </span>,
    },
    {
      title: () => {
        return (
          <React.Fragment>
            <span>Forms</span><br/>
            <small>(created / maximum)</small>
          </React.Fragment>
        )
      },
      key: 'forms',
      render: record =>
        <span style={{ color: '#F44336', backgroundColor: '#feedeb', padding: '4px 15px', borderRadius: 25 }}>
          {record.totalFormsCreated}/{record.maxNumberFormsAllowed}
        </span>,
    },
  ]

  useEffect(() => {
    if (filterValue) {
      let queryParams = `?pageSize=${pageSize}&pageNumber=1&FilterBy=Name&FilterValue=${encodeURIComponent(filterValue)}`
      companiesStore.getCompanyList(queryParams)
    } else {
      companiesStore.getCompanyList(`?pageSize=${pageSize}&pageNumber=1`)
    }
  }, [filterValue])

  useEffect(() => {
    companiesStore.setDefaultPaging()
  }, [])

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
        <title>Company management | Otrafy</title>
      </Helmet>
      <PageHeading title={'Company management'}>
        <Toolsbox>
          <StyledSearch
            placeholder="Search by name of company"
            onSearch={value => handleSearch(value)}/>
          <Button type={'primary'} onClick={() => addCompanyHandler()}>
            <Icon type="plus"/> Add new company
          </Button>
        </Toolsbox>
      </PageHeading>
      <TableWrapper>
        <Table rowKey={record => record.companyId} pagination={false}
               columns={tableConfig} dataSource={companiesStore.companiesList}/>
        <TableFooter>
          <CustomDropdown
            dropdownMenu={dropdownMenu}
            pageSize={pageSize}
            total={companiesStore.paging.totalRecordCount}/>
          <CustomPagination
            total={companiesStore.paging.totalRecordCount}
            pageCount={companiesStore.paging.pageCount}
            current={currentPage}
            pageSize={pageSize} hideOnSinglePage={true}
            onChange={page => handleChangePage(page)}>
          </CustomPagination>
        </TableFooter>
      </TableWrapper>
    </MainLayout>
  )
}

export default inject('companiesStore', 'usersStore')(observer(CompanyManagementPage))