import React, { useEffect, useState } from 'react'
import { Helmet } from 'react-helmet'
import MainLayout from '../../layout/MainLayout'
import PageHeading from '../../organisms/PageHeading/PageHeading'
import { DatePicker, Table, Menu, Input, Button, message } from 'antd'
import StatisticCard from '../../organisms/StatisticCard'
import ChartBlock from '../../organisms/ChartBlock'
import TableFooter from '../../organisms/TableFooter'
import { inject, observer } from 'mobx-react'
import { toJS } from 'mobx'
import moment from 'moment'
import CustomPagination from '../../organisms/CustomPagination'
import CustomDropdown from '../../organisms/CustomDropdown'
import StatusTag from '../../elements/StatusTag'
import {
  StatisticCardWrapper, MainContentWrapper, TableWrapper, ChartWrapper,
  TableHeading, TableHeadingTitle,
  StyledSearch,
} from './CustomStyled'
// IMG imports
import docIcon from '../../../assets/imgs/doc@2x.png'
import shopIcon from '../../../assets/imgs/shop@2x.png'
import userIcon from '../../../assets/imgs/user@2x.png'
// Chart JS
import { Doughnut, Line } from 'react-chartjs-2'
import 'chartjs-plugin-labels'

const { RangePicker } = DatePicker
const { Search } = Input

const DashboardPage = ({ usersStore, suppliersStore, buyersStore, history }) => {

  const [pageSize, setPageSize] = useState(10)
  const [currentPage, setCurrentPage] = useState(1)
  const [filterValue, setFilterValue] = useState('')

  const lineChartData = {
    labels: ['Week 1, Aug', 'Week 2, Aug', 'Week 3, Aug', 'Week 4, Aug'],
    datasets: [
      {
        label: '1 week',
        data: [18, 43, 22, 35],
        borderColor: 'rgba(233, 30, 99, 1)',
        backgroundColor: 'rgba(233, 30, 99, .1)',
        borderWidth: 1,
      },
      {
        label: '> 2 week',
        data: [10, 22, 41, 15],
        borderColor: 'rgba(255, 122, 0, 1)',
        backgroundColor: 'rgba(255, 122, 0, .1)',
        borderWidth: 1,
      },
    ],
  }
  const lineChartOption = {
    legend: {
      display: false,
    },
    scales: {
      yAxes: [
        {
          ticks: {
            beginAtZero: true,
            stepSize: 10,
          },
        },
      ],
    },
  }
  const doughnutChartData = {
    labels: ['1 week', '> 2 weeks', '2 weeks'],
    datasets: [{
      label: '# of Votes',
      data: [40, 40, 20],
      backgroundColor: [
        '#FF9800',
        '#E94C3B',
        '#3598DC',
      ],
      borderColor: [
        'white',
        'white',
        'white',
      ],
      borderWidth: 1,
    }],
  }
  const doughnutChartOption = {
    title: {
      display: true,
      text: '1992 response',
      fontSize: 14,
      fontFamily: 'Poppins, sans-serif',
      fontStyle: 'normal',
      position: 'bottom',
      lineHeight: 2,
    },
    legend: {
      position: 'right',
      labels: {
        fontSize: 14,
        fontColor: '#666',
        boxWidth: 14,
      },
    },
    plugins: {
      labels: {
        fontSize: 12,
        fontColor: '#fff',
        fontFamily: 'Poppins, sans-serif',
      },
    },
  }

  const handleChangePageSize = ({ key }) => {
    setCurrentPage(1)
    setPageSize(parseInt(key))
  }

  const onChangeDate = (date, dateString) => {
    console.log(date, dateString)
  }

  const handleChangePage = page => {
    let params
    filterValue
      ? params = `?pageSize=${pageSize}&pageNumber=${page}&FilterBy=Name&FilterValue=${encodeURIComponent(filterValue)}`
      : params = `?pageSize=${pageSize}&pageNumber=${page}`
    suppliersStore.getAllSuppliers(params)
    setCurrentPage(page)
  }

  const handleSearch = value => {
    setFilterValue(value)
    setCurrentPage(1)
  }

  const sortByStatus = (a, b) => {
    if (a.status < b.status) {
      return -1
    }
    if (a.status > b.status) {
      return 1
    }
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
      key: 'supplierName',
      render: record => record.name,
    },
    {
      title: 'Request date',
      key: `createdDate`,
      render: record => moment(record.createdDate).format('D MMM YYYY'),
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
        }
        return null
      },
    },
    {
      title: 'Email',
      key: 'email',
      render: record => record.email,
    },
    {
      title: 'Request title',
      key: 'request',
      render: record => {
        const requestTitle = () => {
          if (record.formRequest.length > 0) {
            record.formRequest.slice().sort(sortByStatus)
            return record.formRequest[0].title
          }
          return null
        }
        return (
          <div className={'action-row'}>
            <div className="show">
              {requestTitle()}
            </div>
            <div className="hidden">
              <Button
                onClick={() => history.push(`/suppliers-management/suppliers-detail/${record.id}`)}
                type={'primary'}>
                View supplier detail
              </Button>
            </div>
          </div>
        )
      },
    },
  ]

  let statistic = toJS(buyersStore.statistic)

  useEffect(() => {
    buyersStore.getBuyerStatistic()
  }, [])

  useEffect(() => {
    if (filterValue) {
      let params = `?pageSize=${pageSize}&pageNumber=1&FilterBy=Name&FilterValue=${encodeURIComponent(filterValue)}`
      suppliersStore.getAllSuppliers(params)
    } else {
      suppliersStore.getAllSuppliers(`?pageSize=${pageSize}&pageNumber=1`)
    }
  }, [filterValue, pageSize])

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
        <title>Dashboard | Otrafy</title>
      </Helmet>
      <PageHeading title={'Dashboard'}>
        {/*<RangePicker format={'DD/MM/YYYY'} onChange={e => console.log(e)}/>*/}
      </PageHeading>
      <StatisticCardWrapper>
        <StatisticCard
          heading={'Total suppliers'}
          baseColor={'#FF9800'}
          imgURL={shopIcon}
          statistic={statistic.hasOwnProperty('totalSuppliers') ? statistic.totalSuppliers : 0}/>
        <StatisticCard
          heading={'Pending requests'}
          baseColor={'#2196F3'}
          imgURL={userIcon}
          statistic={statistic.hasOwnProperty('pendingRequest') ? statistic.pendingRequest : 0}/>
        <StatisticCard
          heading={'Total forms'}
          baseColor={'#E91E63'}
          imgURL={docIcon}
          statistic={statistic.hasOwnProperty('totalForm') ? statistic.totalForm : 0}/>
      </StatisticCardWrapper>
      <MainContentWrapper>
        <ChartWrapper>
          <ChartBlock
            // addon={<DatePicker onChange={onChangeDate}/>}
            title={'Response rate'}
            chart={<Doughnut data={doughnutChartData} options={doughnutChartOption}/>}/>
          <ChartBlock
            // addon={<DatePicker onChange={onChangeDate}/>}
            title={'Completed request'}
            chart={<Line data={lineChartData} options={lineChartOption}/>}/>
        </ChartWrapper>
        <TableWrapper>
          <TableHeading>
            <TableHeadingTitle>Action required</TableHeadingTitle>
            <StyledSearch onSearch={value => handleSearch(value)}
                          placeholder={'Search by name of supplier'}/>
            {/*<DatePicker onChange={onChangeDate} style={{ marginLeft: 10 }}/>*/}
          </TableHeading>
          <Table pagination={false} rowKey={record => record.id}
                 columns={columns} dataSource={suppliersStore.suppliersList}/>
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
      </MainContentWrapper>
    </MainLayout>
  )
}

export default inject('usersStore', 'suppliersStore', 'buyersStore')(observer(DashboardPage))