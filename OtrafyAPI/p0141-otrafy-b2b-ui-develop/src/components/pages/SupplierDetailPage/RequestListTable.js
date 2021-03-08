import React, { useEffect, useState, Fragment } from 'react'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import moment from 'moment'
import StatusTag from '../../elements/StatusTag'
import { Button, Icon, Menu, Table } from 'antd'
import TableFooter from '../../organisms/TableFooter'
import CustomDropdown from '../../organisms/CustomDropdown'
import CustomPagination from '../../organisms/CustomPagination'
import { StyledSearch, TableHeading, TableWrapper } from './CustomStyled'

const RequestListTable = ({ formsRequestsStore, match, history }) => {

  const [pageSize, setPageSize] = useState(10)
  const [currentPage, setCurrentPage] = useState(1)
  const [filterValue, setFilterValue] = useState('')
  const supplierId = match.params.supplierId

  const handleChangePageSize = ({ key }) => {
    setCurrentPage(1)
    setPageSize(parseInt(key))
  }

  const handleChangePage = page => {
    let params
    filterValue
      ? params = `?pageSize=${pageSize}&pageNumber=${page}&supplierId=${supplierId}&FilterBy=Name&FilterValue=${encodeURIComponent(filterValue)}`
      : params = `?pageSize=${pageSize}&pageNumber=${page}&supplierId=${supplierId}`
    formsRequestsStore.getFormsRequestsList(params)
    setCurrentPage(page)
  }

  const handleSearch = value => {
    setFilterValue(value)
    setCurrentPage(1)
  }

  const handleDeleteRequest = formRequestId => {
    formsRequestsStore.deleteRequest(supplierId, formRequestId)
  }

  const tableColumns = [
    {
      title: 'Request title',
      key: 'requestTitle',
      render: record => record.title,
    },
    {
      title: 'Status',
      key: 'status',
      render: record => {
        switch (record.status) {
          case 1:
            return <StatusTag mainColor={'yellow'}>Pending</StatusTag>
          case 2:
            return <StatusTag mainColor={'yellow'}>In progress</StatusTag>
          case 3:
            return <StatusTag mainColor={'green'}>Ready to review</StatusTag>
          case 4:
            return <StatusTag mainColor={'blue'}>Approved</StatusTag>
          case 5:
            return <StatusTag mainColor={'red'}>Rejected</StatusTag>
          default:
            return null
        }
      },
    },
    {
      title: 'Date sent',
      key: 'dateSent',
      render: record => moment(record.dateSent).format('MMMM D, YYYY'),
    },
    {
      title: 'Date updated',
      key: 'dateUpdated',
      render: record => moment(record.dateUpdated).format('MMMM D, YYYY'),
    },
    {
      title: 'Buyer in charge',
      key: 'buyerInCharge',
      render: record => {
        const buttonGroup = () => {
          switch (record.status) {
            case 1: // Pending
              return (
                <Fragment>
                  <Button
                    onClick={() => handleDeleteRequest(record.id)}
                    type={'danger'} ghost>
                    Delete
                  </Button>
                  <Button type={'primary'} style={{ marginLeft: 5 }}>
                    Remind
                  </Button>
                </Fragment>
              )
            case 2: // In progress
              return (
                <Fragment>
                  <Button type={'primary'}>
                    View
                  </Button>
                </Fragment>
              )
            case 3: // Ready to review
              return (
                <Fragment>
                  <Button type={'primary'}>
                    Review
                  </Button>
                </Fragment>
              )
            case 4: // Approved
              return (
                <Fragment>
                  <Button>
                    View
                  </Button>
                  <Button type={'primary'}>
                    Download
                  </Button>
                </Fragment>
              )
            case 5: // Rejected
              return (
                <Fragment>
                  <Button type={'primary'}>
                    Review
                  </Button>
                </Fragment>
              )
          }
        }
        return (
          <div className={'action-row'}>
            <div className="show">
              {record.buyerInCharge}
            </div>
            <div className="hidden">
              {buttonGroup()}
            </div>
          </div>
        )
      },
    },
  ]

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

  useEffect(() => {
    if (filterValue) {
      formsRequestsStore.getFormsRequestsList(`?pageSize=${pageSize}&pageNumber=1&supplierId=${supplierId}&FilterBy=title&FilterValue=${filterValue}`)
    } else {
      formsRequestsStore.getFormsRequestsList(`?pageSize=${pageSize}&pageNumber=1&supplierId=${supplierId}`)
    }
  }, [filterValue, pageSize])

  return (
    <TableWrapper>
      <TableHeading>
        <div className="info">
          Filled forms instead of Action required
        </div>
        <div className="action">
          <StyledSearch
            placeholder={'Search by request title'}
            onSearch={value => handleSearch(value)}/>
          <Button type={'primary'}
                  onClick={() => history.push('/suppliers-management/create-new-request')}
                  style={{ marginLeft: 10 }}>
            <Icon type="plus"/> New request
          </Button>
        </div>
      </TableHeading>
      <Table
        rowKey={record => record.id}
        className={'request-list-table'}
        pagination={false} columns={tableColumns}
        dataSource={formsRequestsStore.formsRequestsList}/>
      <TableFooter>
        <CustomDropdown
          dropdownMenu={dropdownMenu}
          pageSize={pageSize}
          total={formsRequestsStore.paging.totalRecordCount}/>
        <CustomPagination
          total={formsRequestsStore.paging.totalRecordCount}
          pageCount={formsRequestsStore.paging.pageCount}
          current={currentPage}
          pageSize={pageSize} hideOnSinglePage={true}
          onChange={page => handleChangePage(page)}/>
      </TableFooter>
    </TableWrapper>
  )

}

export default withRouter(inject('formsRequestsStore', 'suppliersStore', 'tagsStore')(observer(RequestListTable)))