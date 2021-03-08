import React, { useEffect, useState } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import { Table, Button, Menu, message } from 'antd'
import PageHeading from '../../organisms/PageHeading/PageHeading'
import AdvanceFilterTrigger from './AdvanceFilterTrigger'
import AdvanceFilterZone from './AdvanceFilterZone'
import NormalTag from '../../elements/NormalTag'
import { inject, observer } from 'mobx-react'
import {
  TableWrapper, InputGroup, StyledSearch, ActionRow,
} from './CustomStyled'
// Icons
import addIcon from '../../../assets/icons/user-add@2x.png'
import suppliersStore from '../../../stores/suppliersStore'
import TableFooter from '../../organisms/TableFooter'
import CustomPagination from '../../organisms/CustomPagination'
import CustomDropdown from '../../organisms/CustomDropdown'
import StatusTag from '../../elements/StatusTag'

const SuppliersManagementPage = props => {

  const {
    usersStore, commonStore, suppliersStore, formsRequestsStore, formsStore,
    history,
  } = props

  const [showAdvanceFilter, setShowAdvanceFilter] = useState(false)
  const [pageSize, setPageSize] = useState(10)
  const [currentPage, setCurrentPage] = useState(1)
  const [filterValue, setFilterValue] = useState('')

  const createRequestHandler = () => {
    history.push(`/suppliers-management/create-new-request`)
  }

  const addSupplierHandler = () => {
    history.push(`/suppliers-management/add-new-supplier`)
  }

  const handleSearch = value => {
    setFilterValue(value)
    setCurrentPage(1)
  }

  const handleChangePage = page => {
    let queryParams
    filterValue
      ? queryParams = `?pageSize=${pageSize}&pageNumber=${page}&FilterBy=Name&FilterValue=${encodeURIComponent(filterValue)}`
      : queryParams = `?pageSize=${pageSize}&pageNumber=${page}`
    suppliersStore.getAllSuppliers(queryParams)
    setCurrentPage(page)
  }

  const handleChangePageSize = ({ key }) => {
    setCurrentPage(1)
    setPageSize(parseInt(key))
  }

  const handleSendRequest = supplierId => {
    formsRequestsStore.updateFormRequestData('selectedSupplierId', supplierId)
    createRequestHandler()
  }

  const sortByStatus = (a, b) => {
    if (a.status < b.status) return -1
    if (a.status > b.status) return 1
    return 0
  }

  const dropdownMenu = (
    <Menu onClick={handleChangePageSize}>
      <Menu.Item key="10">
        Showing <strong>10</strong> of <strong>{suppliersStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="20">
        Showing <strong>20</strong> of <strong>{suppliersStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="50">
        Showing <strong>50</strong> of <strong>{suppliersStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
    </Menu>
  )

  const columns = [
    {
      title: `Supplier's name`,
      dataIndex: 'name',
      key: 'name',
    },
    {
      title: 'Status',
      key: 'status',
      render: record => {
        if (record.formRequest.length > 0) {
          record.formRequest.slice().sort(sortByStatus)
          switch (record.formRequest[0].status) {
            case 1:
              return <StatusTag mainColor={'yellow'}>Pending</StatusTag>
            case 2:
              return <StatusTag mainColor={'yellow'}>In progress</StatusTag>
            case 3:
              return <StatusTag mainColor={'green'}>Completed</StatusTag>
            case 4:
              return <StatusTag mainColor={'blue'}>Approved</StatusTag>
            case 5:
              return <StatusTag mainColor={'red'}>Rejected</StatusTag>
            default:
              return null
          }
        } else {
          return null
        }
      },
    },
    {
      title: 'Email',
      key: 'email',
      render: record => record.email,
    },
    {
      title: 'Tags',
      key: 'tags',
      render: record => record.tags
        ? <NormalTag tags={record.tags}/>
        : null,
    },
    {
      title: 'Company name',
      dataIndex: 'companyName',
      key: 'companyName',
    },
    {
      title: 'Request title',
      key: 'form',
      render: record => {
        if (record.formRequest.length > 0) {
          record.formRequest.slice().sort(sortByStatus)
          return record.formRequest[0].title
        } else {
          return null
        }
      },
    },
    {
      title: 'Products',
      key: 'product',
      render: record => {
        return (
          <div className={'action-row'}>
            <div className="show">
              {record.products}
            </div>
            <div className="hidden">
              <ActionRow>
                <Button onClick={() => handleSendRequest(record.id)}>
                  Send request
                </Button>
                <Button
                  onClick={() => history.push(`/suppliers-management/suppliers-detail/${record.id}`)}
                  type={'primary'}
                  style={{ marginLeft: 10 }}>
                  Detail
                </Button>
              </ActionRow>
            </div>
          </div>
        )
      },
    },
  ]

  const btnStyle = {
    display: 'inline-flex',
    alignItems: 'center',
    justifyContent: 'center',
    marginLeft: 10,
  }

  useEffect(() => {
    if (filterValue) {
      let params = `?pageSize=${pageSize}&pageNumber=1&FilterBy=Name&FilterValue=${encodeURIComponent(filterValue)}`
      suppliersStore.getAllSuppliers(params)
    } else {
      suppliersStore.getAllSuppliers(`?pageSize=${pageSize}&pageNumber=1`)
    }
  }, [filterValue, pageSize])

  useEffect(() => {
    let params = `?pageSize=${pageSize}&pageNumber=1`
    suppliersStore.getAllSuppliers(params)
  }, [])

  useEffect(() => {
    if (usersStore.currentUser.role) {
      switch (usersStore.currentUser.role) {
        case 'buyers':
          break
        default:
          history.push('/')
      }
    }
  }, [usersStore.currentUser])

  useEffect(() => {
    formsStore.getFormsList('')
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
        <title>Suppliers | Otrafy</title>
      </Helmet>
      <PageHeading title={'Suppliers management'}>
        <InputGroup>
          <StyledSearch
            placeholder="Search by name of company"
            onSearch={value => handleSearch(value)}/>
          <AdvanceFilterTrigger
            active={showAdvanceFilter}
            theme={commonStore.appTheme}
            onClick={() => setShowAdvanceFilter(!showAdvanceFilter)}/>
          <Button style={btnStyle} onClick={createRequestHandler}>
            <span style={{ marginRight: 5, fontSize: 16 }}>+</span>
            Create new request
          </Button>
          <Button
            onClick={addSupplierHandler}
            style={btnStyle}
            type={'primary'}>
            <img src={addIcon} alt="Add users" style={{
              height: 13.3,
              marginRight: 10,
            }}/>
            Add new supplier
          </Button>
        </InputGroup>
      </PageHeading>
      {
        !showAdvanceFilter ? null :
          <AdvanceFilterZone
            formsList={formsStore.formsList}
            onClose={() => setShowAdvanceFilter(false)}/>
      }
      <TableWrapper>
        <Table
          rowKey={record => record.id}
          pagination={false}
          dataSource={suppliersStore.suppliersList}
          columns={columns}/>
        <TableFooter>
          <CustomDropdown
            dropdownMenu={dropdownMenu}
            pageSize={pageSize}
            total={suppliersStore.paging.totalRecordCount}/>
          <CustomPagination
            total={suppliersStore.paging.totalRecordCount}
            pageCount={suppliersStore.paging.pageCount}
            current={currentPage}
            pageSize={pageSize} hideOnSinglePage={true}
            onChange={page => handleChangePage(page)}/>
        </TableFooter>
      </TableWrapper>
    </MainLayout>
  )
}

export default inject(
  'usersStore', 'commonStore', 'suppliersStore', 'formsRequestsStore', 'formsStore',
)(observer(SuppliersManagementPage))