import React, { useState, useEffect } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import { Link } from 'react-router-dom'
import PageHeading from '../../organisms/PageHeading'
import { inject, observer } from 'mobx-react'
import moment from 'moment'
import {
  SearchBar, TableWrapper,
} from './CustomStyled'
import { Table, Menu, message } from 'antd'
import TableFooter from '../../organisms/TableFooter'
import StatusTag from '../../elements/StatusTag'
import CustomDropdown from '../../organisms/CustomDropdown'
import CustomPagination from '../../organisms/CustomPagination'

const RequestManagementPage = ({ history, formsRequestsStore, usersStore }) => {

  const [pageSize, setPageSize] = useState(10)
  const [currentPage, setCurrentPage] = useState(1)
  const [filterValue, setFilterValue] = useState('')

  const handleChangePageSize = ({ key }) => {
    setCurrentPage(1)
    setPageSize(parseInt(key))
  }

  const handleChangePage = page => {
    let params
    filterValue
      ? params = `?pageSize=${pageSize}&pageNumber=${page}&FilterBy=Name&FilterValue=${encodeURIComponent(filterValue)}`
      : params = `?pageSize=${pageSize}&pageNumber=${page}`
    formsRequestsStore.getFormsRequestForSupplier(params)
    setCurrentPage(page)
  }

  const handleSearch = value => {
    setFilterValue(value)
    setCurrentPage(1)
  }

  const dropdownMenu = (
    <Menu onClick={handleChangePageSize}>
      <Menu.Item key="10">
        Showing <strong>10</strong> of <strong>{formsRequestsStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="20">
        Showing <strong>20</strong> of <strong>{formsRequestsStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="50">
        Showing <strong>50</strong> of <strong>{formsRequestsStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
    </Menu>
  )

  const columns = [
    {
      title: 'Request title',
      key: 'title',
      render: record =>
        <Link to={`/request-management/request-detail/${record.id}`}>{record.title}</Link>
      ,
    },
    {
      title: 'Request description',
      key: 'description',
      render: record => record.description,
    },
    {
      title: 'Company name',
      key: 'companyName',
      render: record => record.companyName,
    },
    {
      title: 'Date created',
      key: 'createdDate',
      render: record => moment(record.createdDate).format('D MMM YYYY'),
    },
    {
      title: 'Date updated',
      key: 'updatedDate',
      render: record => moment(record.updatedDate).format('D MMM YYYY'),
    },
    {
      title: 'Product name',
      key: 'productName',
      render: record => record.productName,
    },
    {
      title: 'Status',
      key: 'status',
      render: record => {
        switch (record.status) {
          case 1:
            return <StatusTag mainColor={'yellow'}>Pending</StatusTag>
          case 2:
            return <StatusTag mainColor={'green'}>Completed</StatusTag>
          case 3:
            return <StatusTag mainColor={'red'}>Rejected</StatusTag>
          case 4:
            return <StatusTag mainColor={'red'}>Approved</StatusTag>
        }
      },
    },
  ]

  useEffect(() => {
    if (filterValue) {
      formsRequestsStore.getFormsRequestForSupplier(`?pageSize=${pageSize}&pageNumber=1&FilterBy=title&FilterValue=${filterValue}`)
    } else {
      formsRequestsStore.getFormsRequestForSupplier(`?pageSize=${pageSize}&pageNumber=1`)
    }
  }, [filterValue, pageSize])

  useEffect(() => {
    const role = usersStore.currentUser.role
    if (role) {
      switch (role) {
        case 'suppliers':
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
        <title>Request management | Otrafy</title>
      </Helmet>
      <PageHeading
        title={'Request management'}>
        <SearchBar
          onSearch={value => handleSearch(value)}
          placeholder={'Search by request title'}
        />
      </PageHeading>
      <TableWrapper>
        <Table
          rowKey={record => record.id}
          pagination={false} columns={columns}
          dataSource={formsRequestsStore.formsRequestsList}/>
        <TableFooter>
          <CustomDropdown
            dropdownMenu={dropdownMenu}
            total={formsRequestsStore.paging.totalRecordCount}
            pageSize={pageSize}
          />
          <CustomPagination
            total={formsRequestsStore.paging.totalRecordCount}
            pageCount={formsRequestsStore.paging.pageCount}
            current={currentPage}
            pageSize={pageSize} hideOnSinglePage={true}
            onChange={page => handleChangePage(page)}
          />
        </TableFooter>
      </TableWrapper>
    </MainLayout>
  )
}

export default inject('formsRequestsStore', 'usersStore')(observer(RequestManagementPage))