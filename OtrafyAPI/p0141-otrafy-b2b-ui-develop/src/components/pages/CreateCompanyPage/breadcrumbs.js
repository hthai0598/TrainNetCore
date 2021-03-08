import React from 'react'
import { Breadcrumb } from 'antd'
import { Link } from 'react-router-dom'

const breadcrumbs = (
  <Breadcrumb>
    <Breadcrumb.Item>
      <Link to={'/'}>
        Home
      </Link>
    </Breadcrumb.Item>
    <Breadcrumb.Item>
      <Link to={'/company-management'}>
        Company management
      </Link>
    </Breadcrumb.Item>
    <Breadcrumb.Item>
      Create new company
    </Breadcrumb.Item>
  </Breadcrumb>
)

export default breadcrumbs