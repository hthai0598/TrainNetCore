import React from 'react'
import { withRouter } from 'react-router-dom'
import { Breadcrumb } from 'antd'
import { Link } from 'react-router-dom'

const BreadCrumbs = ({ match }) => {
  return (
    <Breadcrumb>
      <Breadcrumb.Item>
        <Link to={`/`}>
          Home
        </Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to={`/suppliers-management`}>
          Supplier management
        </Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to={`/suppliers-management/suppliers-detail/${match.params.supplierId}`}>
          Supplier detail
        </Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        Add new product
      </Breadcrumb.Item>
    </Breadcrumb>
  )
}

export default withRouter(BreadCrumbs)