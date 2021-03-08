import React, { useEffect, useState } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import { Button, Icon, Table, Menu, message } from 'antd'
import PageHeading from '../../organisms/PageHeading'
import moment from 'moment'
import {
  Toolsbox,
  StyledSearch,
  TableWrapper,
} from './CustomStyled'
import NormalTag from '../../elements/NormalTag'
import TableFooter from '../../organisms/TableFooter'
import CustomPagination from '../../organisms/CustomPagination'
import { inject, observer } from 'mobx-react'
import CustomDropdown from '../../organisms/CustomDropdown'

const AllFormsPage = ({ history, formsStore, usersStore }) => {

  const [pageSize, setPageSize] = useState(10)
  const [currentPage, setCurrentPage] = useState(1)
  const [filterValue, setFilterValue] = useState('')

  const handleChangePage = page => {
    let queryParams
    filterValue
      ? queryParams = `?pageSize=${pageSize}&pageNumber=${page}&FilterBy=Name&FilterValue=${encodeURIComponent(filterValue)}`
      : queryParams = `?pageSize=${pageSize}&pageNumber=${page}`
    formsStore.getFormsList(queryParams)
    setCurrentPage(page)
  }

  const createFormHandler = () => {
    history.push('/all-forms/create-new-form')
  }

  const handleChangePageSize = ({ key }) => {
    setCurrentPage(1)
    setPageSize(parseInt(key))
  }

  const handleSearch = value => {
    setFilterValue(value)
    setCurrentPage(1)
  }

  const dropdownMenu = (
    <Menu onClick={handleChangePageSize}>
      <Menu.Item key="10">
        Showing <strong>10</strong> of <strong>{formsStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="20">
        Showing <strong>20</strong> of <strong>{formsStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="50">
        Showing <strong>50</strong> of <strong>{formsStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
    </Menu>
  )

  const columns = [
    {
      title: 'Form name',
      key: 'name',
      render: record => record.name,
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
      title: 'Tags',
      key: 'tags',
      render: record => <NormalTag tags={record.tags}/>,
    },
    {
      title: 'Action',
      align: 'center',
      key: 'action',
      render: record =>
        <div style={{ display: 'flex', justifyContent: 'center' }}>
          <Button style={{ height: 40, margin: '0 5px' }}
                  onClick={() => history.push(`/all-forms/edit-form-detail/${record.id}`)}>
            Edit
          </Button>
          <Button type={'primary'}
                  onClick={() => history.push(`/all-forms/view-form-detail/${record.id}`)}
                  style={{ height: 40, margin: '0 5px' }}>
            View form
          </Button>
        </div>,
    },
  ]

  useEffect(() => {
    formsStore.setDefaultPaging()
    return () => {
      setFilterValue('')
      setPageSize(9)
      setCurrentPage(1)
    }
  }, [])

  useEffect(() => {
    if (filterValue) {
      let params = `?pageSize=${pageSize}&pageNumber=1&FilterBy=name&FilterValue=${encodeURIComponent(filterValue)}`
      formsStore.getFormsList(params)
    } else {
      formsStore.getFormsList(`?pageSize=${pageSize}&pageNumber=1`)
    }
  }, [filterValue])

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
        <title>All Forms | Otrafy</title>
      </Helmet>
      <PageHeading title={'All forms'}>
        <Toolsbox>
          <StyledSearch
            onSearch={value => handleSearch(value)}
            placeholder={'Search by form name'}/>
          <Button type={'primary'} onClick={createFormHandler}>
            <Icon type="plus"/>
            Create new form
          </Button>
        </Toolsbox>
      </PageHeading>
      <TableWrapper>
        <Table
          pagination={false}
          rowKey={record => record.id}
          columns={columns}
          dataSource={formsStore.formsList}/>
        <TableFooter>
          <CustomDropdown
            dropdownMenu={dropdownMenu}
            pageSize={pageSize}
            total={formsStore.paging.totalRecordCount}/>
          <CustomPagination
            total={formsStore.paging.totalRecordCount}
            pageCount={formsStore.paging.pageCount}
            current={currentPage}
            pageSize={pageSize} hideOnSinglePage={true}
            onChange={page => handleChangePage(page)}/>
        </TableFooter>
      </TableWrapper>
    </MainLayout>
  )
}

export default inject('formsStore', 'usersStore')(observer(AllFormsPage))