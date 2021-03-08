import React, { useState, useEffect } from 'react'
import { inject, observer } from 'mobx-react'
import { withRouter } from 'react-router-dom'
import { Button, Icon, Table, Menu } from 'antd'
import {
  TableWrapper, TableHeading, StyledSearch, ModalLink,
} from './ProductsTabStyled'
import TableFooter from '../../../organisms/TableFooter'
import NormalTag from '../../../elements/NormalTag'
import CustomDropdown from '../../../organisms/CustomDropdown'
import CustomPagination from '../../../organisms/CustomPagination'
import ProductDetailModal from './ProductDetailModal'

const ProductsTab = ({ productsStore, commonStore, formsRequestsStore, history, match }) => {

  const [pageSize, setPageSize] = useState(10)
  const [currentPage, setCurrentPage] = useState(1)
  const [filterValue, setFilterValue] = useState('')
  const [showModal, setShowModal] = useState(false)

  const supplierId = match.params.supplierId

  const handleChangePageSize = ({ key }) => {
    setCurrentPage(1)
    setPageSize(parseInt(key))
  }

  const handleChangePage = page => {
    let params
    filterValue
      ? params = `?supplierId=${supplierId}&pageSize=${pageSize}&pageNumber=${page}&FilterBy=name&&FilterBy=tags&FilterValue=${encodeURIComponent(filterValue)}&FilterValue=${encodeURIComponent(filterValue)}`
      : params = `?supplierId=${supplierId}&pageSize=${pageSize}&pageNumber=${page}`
    productsStore.getAllProducts(params)
    setCurrentPage(page)
  }

  const handleSearch = value => {
    setFilterValue(value)
    setCurrentPage(1)
  }

  const handleClickOnProduct = productId => {
    formsRequestsStore.updateFormRequestData('selectedProductId', productId)
    productsStore.getProductById(productId)
    setShowModal(true)
  }

  const handleCloseModal = () => {
    formsRequestsStore.updateFormRequestData('selectedProductId', undefined)
    setShowModal(false)
    setTimeout(() => {
      productsStore.toggleEditMode(false)
    }, 300)
  }

  const tableColumns = [
    {
      title: 'Product name',
      key: 'name',
      render: record =>
        <ModalLink
          color={commonStore.appTheme.solidColor}
          onClick={() => handleClickOnProduct(record.id)}>
          {record.name}
        </ModalLink>,
    },
    {
      title: 'Product ID',
      key: 'code',
      render: record => record.code,
    },
    {
      title: 'Tags',
      key: 'tags',
      render: record => <NormalTag tags={record.tags}/>,
    },
    {
      title: 'Grade',
      key: 'grade',
      render: record => record.grade,
    },
    {
      title: 'Description',
      key: 'description',
      render: record => record.description,
    },
  ]

  const dropdownMenu = (
    <Menu onClick={handleChangePageSize}>
      <Menu.Item key="10">
        Showing <strong>10</strong> of <strong>{productsStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="20">
        Showing <strong>20</strong> of <strong>{productsStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
      <Menu.Item key="50">
        Showing <strong>50</strong> of <strong>{productsStore.paging.totalRecordCount}</strong> items
      </Menu.Item>
    </Menu>
  )

  useEffect(() => {
    if (filterValue) {
      let queryParams = `?supplierId=${supplierId}&pageSize=${pageSize}&pageNumber=1&FilterBy=tags&&FilterBy=name&FilterValue=${encodeURIComponent(filterValue)}`
      productsStore.getAllProducts(queryParams)
    } else {
      productsStore.getAllProducts(`?supplierId=${supplierId}&pageSize=${pageSize}&pageNumber=1`)
    }
  }, [filterValue, pageSize, productsStore.editMode])

  return (
    <TableWrapper>
      <TableHeading>
        <div className="title">List of products</div>
        <div className="action">
          <StyledSearch
            placeholder={'Search by product name or tag'}
            onSearch={value => handleSearch(value)}/>
          <Button
            onClick={() => history.push(`/suppliers-management/create-new-request`)}
            style={{ marginLeft: 10 }}>
            <Icon type="plus"/> Create new request
          </Button>
          <Button type={'primary'} style={{ marginLeft: 10 }}
                  onClick={() => history.push(`/suppliers-management/suppliers-detail/${supplierId}/add-new-product`)}>
            <Icon type="plus"/> Add new product
          </Button>
        </div>
      </TableHeading>
      <Table
        pagination={false}
        rowKey={record => record.id}
        columns={tableColumns}
        dataSource={productsStore.productsList}/>
      <TableFooter>
        <CustomDropdown
          dropdownMenu={dropdownMenu}
          pageSize={pageSize}
          total={productsStore.paging.totalRecordCount}/>
        <CustomPagination
          total={productsStore.paging.totalRecordCount}
          pageCount={productsStore.paging.pageCount}
          current={currentPage}
          pageSize={pageSize} hideOnSinglePage={true}
          onChange={page => handleChangePage(page)}/>
      </TableFooter>
      <ProductDetailModal
        handleClose={handleCloseModal}
        visible={showModal}/>
    </TableWrapper>
  )
}

export default withRouter(inject('productsStore', 'commonStore', 'formsRequestsStore')(observer(ProductsTab)))